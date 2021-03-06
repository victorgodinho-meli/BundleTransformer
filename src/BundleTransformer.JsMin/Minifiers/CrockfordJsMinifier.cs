﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AdvancedStringBuilder;
using DouglasCrockford.JsMin;

using BundleTransformer.Core.Assets;
using BundleTransformer.Core.Minifiers;
using CoreStrings = BundleTransformer.Core.Resources.Strings;

namespace BundleTransformer.JsMin.Minifiers
{
	/// <summary>
	/// Minifier, which produces minifiction of JS code
	/// by using C# port of Douglas Crockford's JSMin
	/// </summary>
	public sealed class CrockfordJsMinifier : IMinifier
	{
		/// <summary>
		/// Name of minifier
		/// </summary>
		const string MINIFIER_NAME = "JSMin Minifier";

		/// <summary>
		/// Name of code type
		/// </summary>
		const string CODE_TYPE = "JS";


		/// <summary>
		/// Produces a code minifiction of JS asset by using C# port of
		/// Douglas Crockford's JSMin
		/// </summary>
		/// <param name="asset">JS asset</param>
		/// <returns>JS asset with minified text content</returns>
		public IAsset Minify(IAsset asset)
		{
			if (asset == null)
			{
				throw new ArgumentNullException(
					nameof(asset),
					string.Format(CoreStrings.Common_ArgumentIsNull, nameof(asset))
				);
			}

			if (asset.Minified)
			{
				return asset;
			}

			var jsMin = new JsMinifier();

			InnerMinify(asset, jsMin);

			return asset;
		}

		/// <summary>
		/// Produces a code minifiction of JS assets by using C# port of
		/// Douglas Crockford's JSMin
		/// </summary>
		/// <param name="assets">Set of JS assets</param>
		/// <returns>Set of JS assets with minified text content</returns>
		public IList<IAsset> Minify(IList<IAsset> assets)
		{
			if (assets == null)
			{
				throw new ArgumentNullException(
					nameof(assets),
					string.Format(CoreStrings.Common_ArgumentIsNull, nameof(assets))
				);
			}

			if (assets.Count == 0)
			{
				return assets;
			}

			var assetsToProcessing = assets.Where(a => a.IsScript && !a.Minified).ToList();
			if (assetsToProcessing.Count == 0)
			{
				return assets;
			}

			var jsMin = new JsMinifier();

			foreach (var asset in assetsToProcessing)
			{
				InnerMinify(asset, jsMin);
			}

			return assets;
		}

		private void InnerMinify(IAsset asset, JsMinifier jsMin)
		{
			string content = asset.Content;
			string newContent;
			string assetUrl = asset.Url;

			var stringBuilderPool = StringBuilderPool.Shared;
			StringBuilder contentBuilder = stringBuilderPool.Rent(content.Length);

			try
			{
				jsMin.Minify(content, contentBuilder);
				newContent = contentBuilder.ToString();
			}
			catch (JsMinificationException e)
			{
				throw new AssetMinificationException(
					string.Format(CoreStrings.Minifiers_MinificationSyntaxError,
						CODE_TYPE, assetUrl, MINIFIER_NAME, e.Message));
			}
			catch (Exception e)
			{
				throw new AssetMinificationException(
					string.Format(CoreStrings.Minifiers_MinificationFailed,
						CODE_TYPE, assetUrl, MINIFIER_NAME, e.Message));
			}
			finally
			{
				stringBuilderPool.Return(contentBuilder);
			}

			asset.Content = newContent;
			asset.Minified = true;
		}
	}
}