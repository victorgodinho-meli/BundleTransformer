﻿using System.Configuration;

namespace BundleTransformer.UglifyJs.Configuration
{
	/// <summary>
	/// Configuration settings of compression
	/// </summary>
	public sealed class CompressionSettings : ConfigurationElement
	{
		/// <summary>
		/// Gets or sets a flag for whether to enable support of <code>@ngInject</code> annotations
		/// </summary>
		[ConfigurationProperty("angular", DefaultValue = false)]
		public bool Angular
		{
			get { return (bool)this["angular"]; }
			set { this["angular"] = value; }
		}

		/// <summary>
		/// Gets or sets a flag for whether to enable various optimizations for boolean
		/// context (for example, <code>!!a ? b : c → a ? b : c</code>)
		/// </summary>
		[ConfigurationProperty("booleans", DefaultValue = true)]
		public bool Booleans
		{
			get { return (bool)this["booleans"]; }
			set { this["booleans"] = value; }
		}

		/// <summary>
		/// Gets or sets a flag for whether to small optimization for sequences
		/// (for example: transform <code>x, x</code> into <code>x</code>
		/// and <code>x = something(), x</code> into <code>x = something()</code>)
		/// </summary>
		[ConfigurationProperty("cascade", DefaultValue = true)]
		public bool Cascade
		{
			get { return (bool)this["cascade"]; }
			set { this["cascade"] = value; }
		}

		/// <summary>
		/// Gets or sets a flag for whether to collapse single-use <code>var</code> and
		/// <code>const</code> definitions when possible
		/// </summary>
		[ConfigurationProperty("collapseVars", DefaultValue = true)]
		public bool CollapseVars
		{
			get { return (bool)this["collapseVars"]; }
			set { this["collapseVars"] = value; }
		}

		/// <summary>
		/// Gets or sets a flag for whether to apply certain optimizations
		/// to binary nodes, attempts to negate binary nodes, etc.
		/// </summary>
		[ConfigurationProperty("comparisons", DefaultValue = true)]
		public bool Comparisons
		{
			get { return (bool) this["comparisons"]; }
			set { this["comparisons"] = value; }
		}

		/// <summary>
		/// Gets or sets a flag for whether to compress code
		/// </summary>
		[ConfigurationProperty("compress", DefaultValue = true)]
		public bool Compress
		{
			get { return (bool) this["compress"]; }
			set { this["compress"] = value; }
		}

		/// <summary>
		/// Gets or sets a flag for whether to apply optimizations
		/// for <code>if</code>-s and conditional expressions
		/// </summary>
		[ConfigurationProperty("conditionals", DefaultValue = true)]
		public bool Conditionals
		{
			get { return (bool) this["conditionals"]; }
			set { this["conditionals"] = value; }
		}

		/// <summary>
		/// Gets or sets a flag for whether to remove unreachable code
		/// </summary>
		[ConfigurationProperty("deadCode", DefaultValue = true)]
		public bool DeadCode
		{
			get { return (bool) this["deadCode"]; }
			set { this["deadCode"] = value; }
		}

		/// <summary>
		/// Gets or sets a flag for whether to discard calls to <code>console.*</code> functions
		/// </summary>
		[ConfigurationProperty("dropConsole", DefaultValue = false)]
		public bool DropConsole
		{
			get { return (bool)this["dropConsole"]; }
			set { this["dropConsole"] = value; }
		}

		/// <summary>
		/// Gets or sets a flag for whether to remove <code>debugger;</code> statements
		/// </summary>
		[ConfigurationProperty("dropDebugger", DefaultValue = true)]
		public bool DropDebugger
		{
			get { return (bool) this["dropDebugger"]; }
			set { this["dropDebugger"] = value; }
		}

		/// <summary>
		/// Gets or sets a flag for whether to attempt to evaluate constant expressions
		/// </summary>
		[ConfigurationProperty("evaluate", DefaultValue = true)]
		public bool Evaluate
		{
			get { return (bool)this["evaluate"]; }
			set { this["evaluate"] = value; }
		}

		/// <summary>
		/// Gets or sets a string representation of the object
		/// (comma-separated list of values of the form SYMBOL[=value])
		/// with properties named after symbols to replace
		/// (except where symbol has properly declared by a <code>var</code>
		/// declaration or use as function parameter or similar) and the values
		/// representing the AST replacement value
		/// </summary>
		[ConfigurationProperty("globalDefinitions", DefaultValue = "")]
		public string GlobalDefinitions
		{
			get { return (string)this["globalDefinitions"]; }
			set { this["globalDefinitions"] = value; }
		}

		/// <summary>
		/// Gets or sets a flag for whether to hoist function declarations
		/// </summary>
		[ConfigurationProperty("hoistFunctions", DefaultValue = true)]
		public bool HoistFunctions
		{
			get { return (bool)this["hoistFunctions"]; }
			set { this["hoistFunctions"] = value; }
		}

		/// <summary>
		/// Gets or sets a flag for whether to hoist <code>var</code> declarations
		/// </summary>
		[ConfigurationProperty("hoistVars", DefaultValue = false)]
		public bool HoistVars
		{
			get { return (bool)this["hoistVars"]; }
			set { this["hoistVars"] = value; }
		}

		/// <summary>
		/// Gets or sets a flag for whether to enable optimizations for if/return
		/// and if/continue
		/// </summary>
		[ConfigurationProperty("ifReturn", DefaultValue = true)]
		public bool IfReturn
		{
			get { return (bool)this["ifReturn"]; }
			set { this["ifReturn"] = value; }
		}

		/// <summary>
		/// Gets or sets a flag for whether to join consecutive <code>var</code> statements
		/// </summary>
		[ConfigurationProperty("joinVars", DefaultValue = true)]
		public bool JoinVars
		{
			get { return (bool)this["joinVars"]; }
			set { this["joinVars"] = value; }
		}

		/// <summary>
		/// Gets or sets a flag for whether to prevent the compressor from discarding
		/// unused function arguments
		/// </summary>
		[ConfigurationProperty("keepFunctionArgs", DefaultValue = true)]
		public bool KeepFunctionArgs
		{
			get { return (bool)this["keepFunctionArgs"]; }
			set { this["keepFunctionArgs"] = value; }
		}

		/// <summary>
		/// Gets or sets a flag for whether to prevent <code>Infinity</code> from being compressed
		/// into <code>1/0</code>, which may cause performance issues on Chrome
		/// </summary>
		[ConfigurationProperty("keepInfinity", DefaultValue = false)]
		public bool KeepInfinity
		{
			get { return (bool)this["keepInfinity"]; }
			set { this["keepInfinity"] = value; }
		}

		/// <summary>
		/// Gets or sets a flag for whether to enable optimizations for <code>do</code>, <code>while</code>
		/// and <code>for</code> loops when we can statically determine the condition
		/// </summary>
		[ConfigurationProperty("loops", DefaultValue = true)]
		public bool Loops
		{
			get { return (bool)this["loops"]; }
			set { this["loops"] = value; }
		}

		/// <summary>
		/// Gets or sets a flag for whether to negate IIFEs
		/// </summary>
		[ConfigurationProperty("negateIife", DefaultValue = true)]
		public bool NegateIife
		{
			get { return (bool)this["negateIife"]; }
			set { this["negateIife"] = value; }
		}

		/// <summary>
		/// Gets or sets a number of times to run compress
		/// </summary>
		[ConfigurationProperty("passes", DefaultValue = 1)]
		[IntegerValidator(MinValue = 1, MaxValue = int.MaxValue, ExcludeRange = false)]
		public int Passes
		{
			get { return (int)this["passes"]; }
			set { this["passes"] = value; }
		}

		/// <summary>
		/// Gets or sets a flag for whether to rewrite property access using
		/// the dot notation (for example, <code>foo["bar"] → foo.bar</code>)
		/// </summary>
		[ConfigurationProperty("propertiesDotNotation", DefaultValue = true)]
		public bool PropertiesDotNotation
		{
			get { return (bool) this["propertiesDotNotation"]; }
			set { this["propertiesDotNotation"] = value; }
		}

		/// <summary>
		/// Gets or sets a flag for whether to UglifyJS will assume
		/// that object property access (e.g. <code>foo.bar</code> or <code>foo["bar"]</code>)
		/// doesn't have any side effects
		/// </summary>
		[ConfigurationProperty("pureGetters", DefaultValue = false)]
		public bool PureGetters
		{
			get { return (bool)this["pureGetters"]; }
			set { this["pureGetters"] = value; }
		}

		/// <summary>
		/// Gets or sets a string representation of the functions list,
		/// that can be safely removed if their return value is not used
		/// </summary>
		[ConfigurationProperty("pureFunctions", DefaultValue = "")]
		public string PureFunctions
		{
			get { return (string)this["pureFunctions"]; }
			set { this["pureFunctions"] = value; }
		}

		/// <summary>
		/// Gets or sets a flag for whether to improve optimization on variables assigned
		/// with and used as constant values
		/// </summary>
		[ConfigurationProperty("reduceVars", DefaultValue = true)]
		public bool ReduceVars
		{
			get { return (bool)this["reduceVars"]; }
			set { this["reduceVars"] = value; }
		}

		/// <summary>
		/// Gets or sets a flag for whether to join consecutive simple
		/// statements using the comma operator
		/// </summary>
		[ConfigurationProperty("sequences", DefaultValue = true)]
		public bool Sequences
		{
			get { return (bool) this["sequences"]; }
			set { this["sequences"] = value; }
		}

		/// <summary>
		/// Gets or sets a flag for whether to drop unreferenced functions and variables in
		/// the toplevel scope
		/// </summary>
		[ConfigurationProperty("topLevel", DefaultValue = false)]
		public bool TopLevel
		{
			get { return (bool)this["topLevel"]; }
			set { this["topLevel"] = value; }
		}

		/// <summary>
		/// Gets or sets a comma-separated list of toplevel functions and variables to exclude
		/// from <code>unused</code> removal
		/// </summary>
		[ConfigurationProperty("topRetain", DefaultValue = "")]
		public string TopRetain
		{
			get { return (string)this["topRetain"]; }
			set { this["topRetain"] = value; }
		}

		/// <summary>
		/// Gets or sets a flag for whether to apply "unsafe" transformations
		/// </summary>
		[ConfigurationProperty("unsafe", DefaultValue = false)]
		public bool Unsafe
		{
			get { return (bool)this["unsafe"]; }
			set { this["unsafe"] = value; }
		}

		/// <summary>
		/// Gets or sets a flag for whether to optimize numerical expressions like <code>2 * x * 3</code>
		/// into <code>6 * x</code>, which may give imprecise floating point results
		/// </summary>
		[ConfigurationProperty("unsafeMath", DefaultValue = false)]
		public bool UnsafeMath
		{
			get { return (bool)this["unsafeMath"]; }
			set { this["unsafeMath"] = value; }
		}

		/// <summary>
		/// Gets or sets a flag for whether to optimize expressions like
		/// <code>Array.prototype.slice.call(a)</code> into <code>[].slice.call(a)</code>
		/// </summary>
		[ConfigurationProperty("unsafeProto", DefaultValue = false)]
		public bool UnsafeProto
		{
			get { return (bool)this["unsafeProto"]; }
			set { this["unsafeProto"] = value; }
		}

		/// <summary>
		/// Gets or sets a flag for whether to enable substitutions of variables with <code>RegExp</code>
		/// values the same way as if they are constants
		/// </summary>
		[ConfigurationProperty("unsafeRegExp", DefaultValue = false)]
		public bool UnsafeRegExp
		{
			get { return (bool)this["unsafeRegExp"]; }
			set { this["unsafeRegExp"] = value; }
		}

		/// <summary>
		/// Gets or sets a flag for whether to drop unreferenced functions and variables
		/// </summary>
		[ConfigurationProperty("unused", DefaultValue = true)]
		public bool Unused
		{
			get { return (bool)this["unused"]; }
			set { this["unused"] = value; }
		}
	}
}