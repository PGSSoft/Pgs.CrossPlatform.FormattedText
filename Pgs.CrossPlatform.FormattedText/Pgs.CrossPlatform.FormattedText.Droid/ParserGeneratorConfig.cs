using System.Collections.Generic;
using Android.Graphics;
using System.Threading.Tasks;
using Android.Text;
using Android.Text.Style;
using Pgs.CrossPlatform.FormattedText.Core;

namespace Pgs.CrossPlatform.FormattedText.Droid
{
    public class ParserGeneratorConfig
    {
        /// <summary>
        /// When overriding, call base at very end of this method
        /// </summary>
        public virtual async void Init(char tagStartChar = '<', char tagEndChar = '>')
        {
            var t = new Task(() =>
            {
                SpanParser.Instance.Initalize(new List<SpanTag>() {
                    new SpanTag("b", B),
                    new SpanTag("i", I)
                }, tagStartChar, tagEndChar);
            });
            t.Start();
        }

        public static void B(object obj, int i1, int i2)
        {
           // Typeface font = Typeface.CreateFromAsset(Forms.Context.ApplicationContext.Assets, "Fonts/SCRIPTIN.ttf");
            // First use Bold than apply custom font! - only way to make it work
            ((SpannableStringBuilder)obj).SetSpan(new TextAppearanceSpan("", TypefaceStyle.Bold, -1, null, null), i1, i2, SpanTypes.ExclusiveInclusive);
           // ((SpannableStringBuilder)obj).SetSpan(new CustomTypefaceSpan("SCRIPTIN", font), i1, i2, SpanTypes.ExclusiveInclusive);
        }

        public static void I(object obj, int i1, int i2)
        {
            ((SpannableStringBuilder)obj).SetSpan(new TextAppearanceSpan("", TypefaceStyle.Italic, -1, null, null), i1, i2, SpanTypes.ExclusiveInclusive);
        }

    }
}