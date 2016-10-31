using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;

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

        private Dictionary<string, object> _sectionedConfig;

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
        /// <param name="sectionedConfig">The sectioned configuration - if not null, sectioned parsing will be used(USE ON iOS!).</param>
        public void Initalize(IEnumerable<FormatTag> spansConfig, bool throwOnConfigLack, char tagStartChar = '<', char tagEndChar = '>', Dictionary<string, object> sectionedConfig = null)
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
            _sectionedConfig = sectionedConfig;
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
        public OutT Parse<OutT>(string text, object sourceControl)
        {
            if (!_isInitialized)
                throw new InvalidOperationException("FormatParser must be initialized before using!");

            var styleParams = new HashSet<FormatParameters>();

            InnerParser.Parse(styleParams, ref text, TagStartChar, TagEndChar);

            var spannableString = (OutT)Activator.CreateInstance(typeof(OutT), text);

            if (_sectionedConfig != null)
            {
                IOSExecutor(sourceControl, styleParams, spannableString);
            }
            else
            {
                Executor(sourceControl, styleParams, spannableString);
            }

            return spannableString;
        }

        private void Executor<OutT>(object sourceControl, HashSet<FormatParameters> styleParams, OutT spannableString)
        {
            foreach (var styleParam in styleParams)
            {
                // Apply styles
                var endIndex = styleParam.StartIndex + styleParam.Length;
                try
                {
                    FormatConfig[styleParam.Tag].Invoke(spannableString, sourceControl, styleParam.StartIndex, endIndex);
                }
                catch (KeyNotFoundException)
                {
                    if (_throwOnConfigLack)
                        throw new ArgumentOutOfRangeException($"Could not find config for tag: {styleParam.Tag}");
                }
            }
        }

        private void IOSExecutor<OutT>(object sourceControl, HashSet<FormatParameters> styleParams, OutT spannableString)
        {
            var sorted = styleParams.OrderBy(x => x.StartIndex).ThenBy(x => x.Length);
            for (int i = 0; i < sorted.Count(); i++)
            {
                var current = sorted.ElementAt(i);

                var startIndexOfFirstItemOfNextRange = sorted.FirstOrDefault(x => x.StartIndex > current.StartIndex + current.Length)?.StartIndex ?? Int32.MaxValue ;
                var currentRange = sorted.Where(x => x.StartIndex >= current.StartIndex && x.StartIndex < startIndexOfFirstItemOfNextRange).ToList();

                if (currentRange.All(x => x == current))
                {
                    try
                    {
                        FormatConfig["InterStyle"].Invoke(spannableString, new Dictionary<string, object>() { { "sourceControl", sourceControl } }, current.StartIndex, current.Length);
                    }
                    catch (KeyNotFoundException)
                    {
                        if (_throwOnConfigLack)
                            throw new ArgumentOutOfRangeException($"Could not find config for tag: {current.Tag}");
                    }
                    continue;
                }

                i += currentRange.Count - 1;

                var sections = new List<FormatParameters>();
                var lastFormatersState = new List<bool>(currentRange.Select(x => false));
                var currentSectionLength = 0;
                for (int j = 0; j < currentRange.Max(x => x.Length); j++)
                {
                    var newFormatersState = new List<bool>(currentRange.Select(x => x.StartIndex - current.StartIndex == j || x.StartIndex - current.StartIndex + x.Length == j));

                    bool isNewSection = false;
                    for (int k = 0; k < lastFormatersState.Count; k++)
                    {
                        if (newFormatersState[k])
                        {
                            lastFormatersState[k] = !lastFormatersState[k];
                            isNewSection = true;
                        }
                    }
                    currentSectionLength = CreateNewSection(isNewSection, sections, currentSectionLength, lastFormatersState, currentRange, current, j);
                    currentSectionLength++;
                }
                sections.Last().Length = currentSectionLength;
                SectionApplyStyle(sourceControl, spannableString, sections);
            }
        }

        private int CreateNewSection(bool isNewSection, List<FormatParameters> sections, int currentSectionLength, List<bool> lastFormatersState, List<FormatParameters> currentRange, FormatParameters current, int j)
        {
            if (isNewSection)
            {
                if (sections.Any())
                {
                    sections.Last().Length = currentSectionLength;
                    currentSectionLength = 0;
                }

                var sectionElements = new List<string>();
                for (int k = 0; k < lastFormatersState.Count; k++)
                {
                    if (lastFormatersState[k])
                        sectionElements.Add(currentRange[k].Tag);
                }

                sections.Add(new FormatParameters("InterStyle", current.StartIndex + j, 0, sectionElements));
            }
            return currentSectionLength;
        }
        
        private void SectionApplyStyle<OutT>(object sourceControl, OutT spannableString, List<FormatParameters> sections)
        {
            try
            {
                foreach (var currentSection in sections)
                {
                    var sectionStylesDictionary = _sectionedConfig.Keys.Intersect(currentSection.SrcStyles).ToDictionary(k => k, k => _sectionedConfig[k]);
                    sectionStylesDictionary.Add("sourceControl", sourceControl);

                    FormatConfig["InterStyle"].Invoke(spannableString, sectionStylesDictionary, currentSection.StartIndex, currentSection.Length);
                }
                // var currentSection = sections.Last();
            }
            catch (KeyNotFoundException ex)
            {
                throw new ArgumentOutOfRangeException($"iOS must have InterStyle tag configured!");
            }
        }
    }
}
