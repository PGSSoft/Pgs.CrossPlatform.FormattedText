namespace Pgs.CrossPlatform.FormattedText.Core
{
    internal class FormatParameters
    {
        public string Tag { get; set; }
        
        public int StartIndex { get; set; }

        public int Length { get; set; }

        public FormatParameters(string tag, int startI, int length)
        {
            Tag = tag;
            StartIndex = startI;
            Length = length;
        }
    }
}
