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
            FormatParser.Instance.Initalize(new List<FormatTag>() {
                new FormatTag("b", B), // first arg: tag name ex. <b>something</b>, where "b" is name
                new FormatTag("i", I), // first arg: tag name ex. <b>something</b>, where "b" is name
            }, throwOnConfigLackchar, tagStartChar, tagEndChar);
        }

        public static void B(object obj, object sourceControl, int i1, int i2)
        {
			((NSMutableAttributedString)obj).SetAttributes(new UIStringAttributes { Font = UIFont.BoldSystemFontOfSize(((UILabel)sourceControl).Font.PointSize + 1), ForegroundColor = UIColor.Black }, new NSRange(i1, i2-i1));
		}

        public static void I(object obj, object sourceControl, int i1, int i2)
        {
            ((NSMutableAttributedString)obj).SetAttributes(new UIStringAttributes { Font = UIFont.ItalicSystemFontOfSize(((UILabel)sourceControl).Font.PointSize + 1) }, new NSRange(i1, i2 - i1));
        }

    }
}