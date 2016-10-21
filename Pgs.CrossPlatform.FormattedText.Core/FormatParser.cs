using Android;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Pgs.CrossPlatform.FormattedText.Core
{
    /// <summary>
    /// Entry class to SpanParserGenerator - this class is singleton - call its instance by Instance property
    /// </summary>
    public sealed class FormatParser
    {
        private FormatParser() { }

        static FormatParser() { }

        private static readonly FormatParser _instance = new FormatParser();
        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static FormatParser Instance => _instance;

        private bool _isInitialized = false;

        private bool _throwOnConfigLack = true;

        public readonly Dictionary<string, TagStylingMethod> FormatConfig = new Dictionary<string, TagStylingMethod>();

        /// <summary>
        /// Gets or sets the tag start character.
        /// </summary>
        /// <value>
        /// The tag start character.
        /// </value>
        public char TagStartChar { get; set; }
        /// <summary>
        /// Gets or sets the tag end character.
        /// </summary>
        /// <value>
        /// The tag end character.
        /// </value>
        public char TagEndChar { get; set; }

        /// <summary>
        /// Generates parser from given config.
        /// </summary>
        /// <param name="spansConfig">List of SpanTag - config for parser generator to generate specific parser</param>
        /// <param name="throwOnConfigLack">if set to <c>true</c> [throw exception on configuration lack].</param>
        /// <param name="tagStartChar">The tag start character.</param>
        /// <param name="tagEndChar">The tag end character.</param>
        public void Initalize(IEnumerable<FormatTag> spansConfig, bool throwOnConfigLack, char tagStartChar = '<', char tagEndChar = '>')
        {
            foreach (var spanTag in spansConfig)
            {
                try
                {
                    FormatConfig.Add(spanTag.Tag, spanTag.MethodToCall);
                }
                catch (Exception ex)
                {
#if DEBUG
                    Debugger.Break();
#endif
                    throw;
                }
            }

            TagStartChar = tagStartChar;
            TagEndChar = tagEndChar;
            _throwOnConfigLack = throwOnConfigLack;
            _isInitialized = true;
        }

        /// <summary>
        /// Parses the text and output specific object of formated text.
        /// ex. SpannableStringBuilder for Android
        /// </summary>
        /// <typeparam name="OutT">Type of formated text requested, currently only SpannableStringBuilder is explicitly supported(tested).</typeparam>
        /// <param name="text">The text to format.</param>
        /// <returns>
        /// Specific object of formated text
        /// </returns>
        /// <exception cref="System.InvalidOperationException">FormatParser must be initialized before using!</exception>
        public OutT Parse<OutT>(string text)
        {
            if (!_isInitialized)
                throw new InvalidOperationException("FormatParser must be initialized before using!");

            var styleParams = new HashSet<FormatParameters>();

            InnerParser.Parse(styleParams, ref text, TagStartChar, TagEndChar);

            var spannableString = (OutT)Activator.CreateInstance(typeof(OutT), text);

            foreach (var styleParam in styleParams)
            {
                // Apply styles
                var endIndex = styleParam.StartIndex + styleParam.Length;
                try
                {
                    FormatConfig[styleParam.Tag].Invoke(spannableString, styleParam.StartIndex, endIndex);
                }
                catch (KeyNotFoundException)
                {
                    if (_throwOnConfigLack)
                        throw new ArgumentOutOfRangeException($"Could not find config for tag: {styleParam.Tag}");
                }
            }
            return spannableString;
        }
    }
}
