#if !_NuGetRelease_
using System.Collections.Generic;
using System.Linq;
using CoreText;
using Foundation;
using Pgs.CrossPlatform.FormattedText.Core;
using UIKit;

namespace Pgs.CrossPlatform.FormattedText.iOS
{
    public static class FormatConfig
    {
        public static void Init(bool throwOnConfigLackchar, char tagStartChar = '<', char tagEndChar = '>')
        {
            FormatParser.Instance.Initalize(new List<FormatTag>() {
                new FormatTag("b", B), // first arg: tag name ex. <b>something</b>, where "b" is name
                new FormatTag("i", I), // first arg: tag name ex. <b>something</b>, where "b" is name
            }, throwOnConfigLackchar, tagStartChar, tagEndChar);
        }

        public static void B(object obj, object sourceControl, int i1, int i2)
        {
            ApplyFontStyle(obj, sourceControl, i1, i2, UIFontDescriptorSymbolicTraits.Bold);
        }

        private static void ApplyFontStyle(object obj, object sourceControl, int i1, int i2, UIFontDescriptorSymbolicTraits traitsToApply)
        {
            var mutableStr = ((NSMutableAttributedString) obj);

            var existingRange = new NSRange(i1, i2 - i1);
            var existingAttributes = mutableStr.GetAttributes(i1, out existingRange);

            if (existingAttributes.Any() && existingRange.Equals(new NSRange(i1, i2 - i1)))
            {
                foreach (var attribute in existingAttributes)
                {
                    if (attribute.Key.Equals(UIStringAttributeKey.Font))
                    {
                        var value = (UIFont) attribute.Value;
                        var traits = value.FontDescriptor.Traits;

                        if (traits.SymbolicTrait != null)
                            mutableStr.AddAttribute(UIStringAttributeKey.Font,
                                CustomizeFont(sourceControl, traits.SymbolicTrait.Value | traitsToApply),
                                new NSRange(i1, i2 - i1));
                    }
                }
            }
            else
            {
                mutableStr.AddAttribute(UIStringAttributeKey.Font,
                    CustomizeFont(sourceControl, traitsToApply), new NSRange(i1, i2 - i1));
            }
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

    }
}
#endif