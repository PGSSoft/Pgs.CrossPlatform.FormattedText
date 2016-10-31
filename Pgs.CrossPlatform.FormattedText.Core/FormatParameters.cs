using System.Collections.Generic;
using System.Diagnostics;

namespace Pgs.CrossPlatform.FormattedText.Core
{
    [DebuggerDisplay("{Tag} StartI: {StartIndex} Length: {Length}")]
    internal class FormatParameters
    {
        public string Tag { get; set; }
        
        public int StartIndex { get; set; }

        public int Length { get; set; }

        /// <summary>
        /// Used only for section configs
        /// </summary>
        /// <value>
        /// The source styles.
        /// </value>
        public List<string> SrcStyles { get; set; }

        public FormatParameters(string tag, int startI, int length, List<string> srcStyles = null)
        {
            Tag = tag;
            StartIndex = startI;
            Length = length;
            SrcStyles = srcStyles;
        }
    }
}
