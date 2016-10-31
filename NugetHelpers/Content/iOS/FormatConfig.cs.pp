using System.Collections.Generic;
using Foundation;
using Pgs.CrossPlatform.FormattedText.Core;
using UIKit;

namespace $rootnamespace$
{
    public static class FormatConfig
    {
        public static void Init(bool throwOnConfigLackchar, char tagStartChar = '<', char tagEndChar = '>')
        {
            var sectionConfig = new Dictionary<string, object>()
            {
                {"b", true}, // for bold text true will be passed to IntersectedStyle
                {"i", true},
                {"backGreen", UIColor.Green},
            };

            FormatParser.Instance.Initalize(new List<FormatTag>() {
                new FormatTag("InterStyle", IntersectedStyle), 
            }, throwOnConfigLackchar, tagStartChar, tagEndChar, sectionConfig);
        }

        private static void IntersectedStyle(object obj, object interConfig, int i1, int i2)
        {
            var configs = (Dictionary<string, object>) interConfig; // <UILabel, bool, bool, UIColor, UIColor>
            object toSkipAllTheseOuts = new object();

            var mutaStr = (NSMutableAttributedString) obj;
            var control = configs["sourceControl"];
            
            var traits = UIFontDescriptorSymbolicTraits.ClassUnknown;
            if (configs.TryGetValue("b", out toSkipAllTheseOuts))
                traits = UIFontDescriptorSymbolicTraits.Bold;
            if (configs.TryGetValue("i", out toSkipAllTheseOuts))
                traits = traits | UIFontDescriptorSymbolicTraits.Italic;

            if (configs.TryGetValue("backGreen", out toSkipAllTheseOuts))
                mutaStr.AddAttribute(UIStringAttributeKey.BackgroundColor, (UIColor)configs["backGreen"], new NSRange(i1, i2));

            if (configs.TryGetValue("?", out toSkipAllTheseOuts))
                mutaStr.AddAttribute(UIStringAttributeKey.ForegroundColor, (UIColor)configs["?"], new NSRange(i1, i2));

            var font = UIFont.FromDescriptor(((UILabel)control).Font.FontDescriptor.CreateWithTraits(traits), ((UILabel)control).Font.PointSize);
            
            mutaStr.AddAttribute(UIStringAttributeKey.Font, font, new NSRange(i1, i2));
        }
    }
}