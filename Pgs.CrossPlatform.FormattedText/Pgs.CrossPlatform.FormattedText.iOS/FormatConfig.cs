#if !_NuGetRelease_
using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using Pgs.CrossPlatform.FormattedText.Core;
using UIKit;

namespace Pgs.CrossPlatform.FormattedText.iOS
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
                new FormatTag("b", B), // first arg: tag name ex. <b>something</b>, where "b" is name
                new FormatTag("i", I),
                new FormatTag("backGreen", BackGreen),
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

        public static void BackGreen(object obj, object sourceControl, int i1, int i2)
        {
            ((NSMutableAttributedString)obj).SetAttributes(new UIStringAttributes { BackgroundColor = UIColor.Green}, new NSRange(i1, i2));
        }

        public static void B(object obj, object sourceControl, int i1, int i2)
        {
            ApplyFontStyle(obj, sourceControl, i1, i2, UIFontDescriptorSymbolicTraits.Bold);
        }
        
        public static void I(object obj, object sourceControl, int i1, int i2)
        {
            ApplyFontStyle(obj, sourceControl, i1, i2, UIFontDescriptorSymbolicTraits.Italic);
        }

        private static UIFont CustomizeFont(object srcControl, UIFontDescriptorSymbolicTraits attr)
        {
            var control = (UILabel)srcControl;
            var font = UIFont.FromDescriptor(control.Font.FontDescriptor.CreateWithTraits(attr), control.Font.PointSize);
            return font;
        }

        private static void ApplyFontStyle(object obj, object sourceControl, int i1, int i2, UIFontDescriptorSymbolicTraits traitsToApply)
        {
            var mutableStr = ((NSMutableAttributedString)obj);

            var existingRange = new NSRange(i1, i2);
            var existingAttributes = mutableStr.GetAttributes(i1, out existingRange);

            if (existingAttributes.Any() && existingRange.Equals(new NSRange(i1, i2)))
            {
                foreach (var attribute in existingAttributes)
                {
                    if (attribute.Key.Equals(UIStringAttributeKey.Font))
                    {
                        var value = (UIFont)attribute.Value;
                        var traits = value.FontDescriptor.Traits;

                        if (traits.SymbolicTrait != null)
                            mutableStr.AddAttribute(UIStringAttributeKey.Font,
                                CustomizeFont(sourceControl, traits.SymbolicTrait.Value | traitsToApply),
                                new NSRange(i1, i2));
                    }
                }
            }
            else
            {
                mutableStr.AddAttribute(UIStringAttributeKey.Font,
                    CustomizeFont(sourceControl, traitsToApply), new NSRange(i1, i2));
            }
        }
    }
}
#endif