﻿namespace BundleTransformer.Autoprefixer.AutoPrefixers
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Reflection;
	using System.Text;
	using System.Text.RegularExpressions;

	using JavaScriptEngineSwitcher.Core;
	using JavaScriptEngineSwitcher.Core.Helpers;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;

	using Core.Utilities;
	using CoreStrings = Core.Resources.Strings;

	using Resources;

	/// <summary>
	/// CSS-autoprefixer
	/// </summary>
	internal sealed class CssAutoprefixer : IDisposable
	{
		/// <summary>
		/// Namespace for resources
		/// </summary>
		private const string RESOURCES_NAMESPACE = "BundleTransformer.Autoprefixer.Resources";

		/// <summary>
		/// Name of file, which contains a Autoprefixer library
		/// </summary>
		private const string AUTOPREFIXER_LIBRARY_FILE_NAME = "autoprefixer-combined.min.js";

		/// <summary>
		/// Name of file, which contains a Autoprefixer helper
		/// </summary>
		private const string AUTOPREFIXER_HELPER_FILE_NAME = "autoprefixerHelper.min.js";

		/// <summary>
		/// Name of directory, which contains a Autoprefixer country statistics
		/// </summary>
		private const string AUTOPREFIXER_COUNTRY_STATISTICS_DIRECTORY_NAME = "CountryStatistics";

		/// <summary>
		/// Template of function call, which is responsible for setup the country statistics
		/// </summary>
		private const string AUTOPREFIXER_SETUP_COUNTRY_STATISTICS_FUNCTION_CALL_TEMPLATE =
			@"autoprefixerHelper.setupCountryStatistics({0});";

		/// <summary>
		/// Template of function call, which is responsible for autoprefixing
		/// </summary>
		private const string AUTOPREFIXER_PROCESS_FUNCTION_CALL_TEMPLATE = @"autoprefixerHelper.process({0}, {1});";

		/// <summary>
		/// Regular expression for working with country conditional expressions
		/// </summary>
		private static readonly Regex _countryExpressionRegex = new Regex(@"^> (?:\d+\.?\d*)% in (?<countryCode>\w\w)$");

		/// <summary>
		/// Default autoprefixing options
		/// </summary>
		private readonly AutoprefixingOptions _defaultOptions;

		/// <summary>
		/// String representation of the default autoprefixing options
		/// </summary>
		private readonly string _defaultOptionsString;

		/// <summary>
		/// JS engine
		/// </summary>
		private IJsEngine _jsEngine;

		/// <summary>
		/// Synchronizer of autoprefixing
		/// </summary>
		private readonly object _autoprefixingSynchronizer = new object();

		/// <summary>
		/// Flag that CSS-autoprefixer is initialized
		/// </summary>
		private bool _initialized;

		/// <summary>
		/// Flag that object is destroyed
		/// </summary>
		private bool _disposed;


		/// <summary>
		/// Constructs a instance of CSS-autoprefixer
		/// </summary>
		/// <param name="createJsEngineInstance">Delegate that creates an instance of JavaScript engine</param>
		public CssAutoprefixer(Func<IJsEngine> createJsEngineInstance)
			: this(createJsEngineInstance, null)
		{ }

		/// <summary>
		/// Constructs a instance of CSS-autoprefixer
		/// </summary>
		/// <param name="createJsEngineInstance">Delegate that creates an instance of JavaScript engine</param>
		/// <param name="defaultOptions">Default autoprefixing options</param>
		public CssAutoprefixer(Func<IJsEngine> createJsEngineInstance, AutoprefixingOptions defaultOptions)
		{
			_jsEngine = createJsEngineInstance();

			_defaultOptions = defaultOptions ?? new AutoprefixingOptions();
			_defaultOptionsString = ConvertAutoprefixingOptionsToJson(_defaultOptions).ToString();
		}


		/// <summary>
		/// Initializes CSS-autoprefixer
		/// </summary>
		private void Initialize()
		{
			if (!_initialized)
			{
				Type type = GetType();

				_jsEngine.ExecuteResource(RESOURCES_NAMESPACE + "." + AUTOPREFIXER_LIBRARY_FILE_NAME, type);
				_jsEngine.ExecuteResource(RESOURCES_NAMESPACE + "." + AUTOPREFIXER_HELPER_FILE_NAME, type);

				IDictionary<string, JObject> commonCountryStatistics = GetCountryStatistics(_defaultOptions.Browsers);
				if (commonCountryStatistics != null)
				{
					_jsEngine.Execute(string.Format(
						AUTOPREFIXER_SETUP_COUNTRY_STATISTICS_FUNCTION_CALL_TEMPLATE,
						JsonConvert.SerializeObject(commonCountryStatistics))
					);
				}

				_initialized = true;
			}
		}

		/// <summary>
		/// Actualizes a vendor prefixes in CSS-code by using Andrey Sitnik's Autoprefixer
		/// </summary>
		/// <param name="content">Text content of CSS-asset</param>
		/// <param name="path">Path to CSS-asset</param>
		/// <param name="options">Autoprefixing options</param>
		/// <returns>Processed text content of CSS-asset</returns>
		public string Process(string content, string path, AutoprefixingOptions options = null)
		{
			string newContent;
			string currentOptionsString;
			IDictionary<string, JObject> countryStatistics = null;

			if (options != null)
			{
				currentOptionsString = ConvertAutoprefixingOptionsToJson(options).ToString();
				countryStatistics = GetCountryStatistics(options.Browsers);
			}
			else
			{
				currentOptionsString = _defaultOptionsString;
			}

			lock (_autoprefixingSynchronizer)
			{
				Initialize();

				if (countryStatistics != null)
				{
					_jsEngine.Execute(string.Format(
						AUTOPREFIXER_SETUP_COUNTRY_STATISTICS_FUNCTION_CALL_TEMPLATE,
						JsonConvert.SerializeObject(countryStatistics))
					);
				}

				try
				{
					var result = _jsEngine.Evaluate<string>(
						string.Format(AUTOPREFIXER_PROCESS_FUNCTION_CALL_TEMPLATE,
							JsonConvert.SerializeObject(content), currentOptionsString));

					var json = JObject.Parse(result);

					var errors = json["errors"] != null ? json["errors"] as JArray : null;
					if (errors != null && errors.Count > 0)
					{
						throw new CssAutoprefixingException(FormatErrorDetails(errors[0], content, path));
					}

					newContent = json.Value<string>("processedCode");
				}
				catch (JsRuntimeException e)
				{
					throw new CssAutoprefixingException(JsRuntimeErrorHelpers.Format(e));
				}
			}

			return newContent;
		}

		/// <summary>
		/// Converts a autoprefixing options to JSON
		/// </summary>
		/// <param name="options">Autoprefixing options</param>
		/// <returns>Autoprefixing options in JSON format</returns>
		private static JObject ConvertAutoprefixingOptionsToJson(AutoprefixingOptions options)
		{
			var optionsJson = new JObject(
				new JProperty("browsers", new JArray(options.Browsers)),
				new JProperty("cascade", options.Cascade),
				new JProperty("remove", options.Remove),
				new JProperty("add", options.Add)
			);

			return optionsJson;
		}

		/// <summary>
		/// Gets a country statistics by conditional expressions
		/// </summary>
		/// <param name="expressions">List of browser conditional expressions</param>
		/// <returns>Country statistics</returns>
		private static IDictionary<string, JObject> GetCountryStatistics(IList<string> expressions)
		{
			IDictionary<string, JObject> statistics = null;

			if (expressions.Count > 0)
			{
				statistics = new Dictionary<string, JObject>();

				foreach (string expression in expressions)
				{
					Match countryExpressionMatch = _countryExpressionRegex.Match(expression);
					if (countryExpressionMatch.Success)
					{
						string countryCode = countryExpressionMatch.Groups["countryCode"].Value.ToUpperInvariant();

						if (!statistics.ContainsKey(countryCode))
						{
							JObject statisticsForCountry = GetStatisticsForCountry(countryCode);
							statistics.Add(countryCode, statisticsForCountry);
						}
					}
				}

				if (statistics.Count == 0)
				{
					statistics = null;
				}
			}

			return statistics;
		}

		/// <summary>
		/// Gets a statistics for country
		/// </summary>
		/// <param name="countryCode">Two-letter country code</param>
		/// <returns>Statistics for country</returns>
		private static JObject GetStatisticsForCountry(string countryCode)
		{
			JObject statistics;
			string resourceName = RESOURCES_NAMESPACE + "." +
				AUTOPREFIXER_COUNTRY_STATISTICS_DIRECTORY_NAME + "." +
				countryCode + ".json"
				;

			try
			{
				string resourceContent = Utils.GetResourceAsString(resourceName, Assembly.GetExecutingAssembly());
				statistics = JObject.Parse(resourceContent);
			}
			catch (NullReferenceException)
			{
				throw new CssAutoprefixingException(
					string.Format(Strings.PostProcessors_CountryStatisticsNotFound, countryCode));
			}

			return statistics;
		}

		/// <summary>
		/// Generates a detailed error message
		/// </summary>
		/// <param name="errorDetails">Error details</param>
		/// <param name="sourceCode">Source code</param>
		/// <param name="currentFilePath">Path to current CSS-file</param>
		/// <returns>Detailed error message</returns>
		private static string FormatErrorDetails(JToken errorDetails, string sourceCode,
			string currentFilePath)
		{
			var message = errorDetails.Value<string>("message");
			string file = currentFilePath;
			var lineNumber = errorDetails.Value<int>("lineNumber");
			var columnNumber = errorDetails.Value<int>("columnNumber");
			string sourceFragment = SourceCodeNavigator.GetSourceFragment(sourceCode,
				new SourceCodeNodeCoordinates(lineNumber, columnNumber));

			var errorMessage = new StringBuilder();
			errorMessage.AppendFormatLine("{0}: {1}", CoreStrings.ErrorDetails_Message, message);
			if (!string.IsNullOrWhiteSpace(file))
			{
				errorMessage.AppendFormatLine("{0}: {1}", CoreStrings.ErrorDetails_File, file);
			}
			if (lineNumber > 0)
			{
				errorMessage.AppendFormatLine("{0}: {1}", CoreStrings.ErrorDetails_LineNumber,
					lineNumber.ToString(CultureInfo.InvariantCulture));
			}
			if (columnNumber > 0)
			{
				errorMessage.AppendFormatLine("{0}: {1}", CoreStrings.ErrorDetails_ColumnNumber,
					columnNumber.ToString(CultureInfo.InvariantCulture));
			}
			if (!string.IsNullOrWhiteSpace(sourceFragment))
			{
				errorMessage.AppendFormatLine("{1}:{0}{0}{2}", Environment.NewLine,
					CoreStrings.ErrorDetails_SourceError, sourceFragment);
			}

			return errorMessage.ToString();
		}

		/// <summary>
		/// Destroys object
		/// </summary>
		public void Dispose()
		{
			if (!_disposed)
			{
				_disposed = true;

				if (_jsEngine != null)
				{
					_jsEngine.Dispose();
					_jsEngine = null;
				}
			}
		}
	}
}