#if !_NuGetRelease_
using System.Collections.Generic;
using Android.Graphics;
using System.Threading.Tasks;
using Android.Text;
using Android.Text.Style;
using Pgs.CrossPlatform.FormattedText.Core;

namespace Pgs.CrossPlatform.FormattedText.Droid
{
    public static class FormatConfig
    {
        public static void Init(char tagStartChar = '<', char tagEndChar = '>')
        {
            var t = new Task(() =>
            {
                FormatParser.Instance.Initalize(new List<FormatTag>() {
                    new FormatTag("b", B), // first arg: tag name ex. <b>something</b>, where "b" is name
                    new FormatTag("i", I)  // second arg: tag method
                }, tagStartChar, tagEndChar);
            });
            t.Start();
        }

        public static void B(object obj, int i1, int i2)
        {
            // Typeface font = Typeface.CreateFromAsset(Forms.Context.ApplicationContext.Assets, "Fonts/FontName.ttf");
            // First use Bold than apply custom font! - only way to make it work
            ((SpannableStringBuilder)obj).SetSpan(new TextAppearanceSpan("", TypefaceStyle.Bold, -1, null, null), i1, i2, SpanTypes.ExclusiveInclusive);
            // ((SpannableStringBuilder)obj).SetSpan(new CustomTypefaceSpan("FontName", font), i1, i2, SpanTypes.ExclusiveInclusive);
        }

        public static void I(object obj, int i1, int i2)
        {
            ((SpannableStringBuilder)obj).SetSpan(new TextAppearanceSpan("", TypefaceStyle.Italic, -1, null, null), i1, i2, SpanTypes.ExclusiveInclusive);
        }

    }
}
#endif