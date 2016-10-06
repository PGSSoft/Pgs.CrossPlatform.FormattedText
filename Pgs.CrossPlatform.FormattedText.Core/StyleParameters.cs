namespace Pgs.CrossPlatform.FormattedText.Core
{
    internal class StyleParameters
    {
        public string Tag { get; set; }
        
        public int StartIndex { get; set; }

        public int Length { get; set; }

        public StyleParameters(string tag, int startI, int length)
        {
            Tag = tag;
            StartIndex = startI;
            Length = length;
        }
    }
}
