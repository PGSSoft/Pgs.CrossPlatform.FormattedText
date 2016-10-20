using System.Collections.Generic;
using Android.Graphics;
using System.Threading.Tasks;
using Android.Text;
using Android.Text.Style;
using Pgs.CrossPlatform.FormattedText.Core;

public static class FormatConfig
{
    public static void Init(char tagStartChar = '<', char tagEndChar = '>')
    {
        var t = new Task(() =>
        {
            FormatParser.Instance.Initalize(new List<FormatTag>() {
                new FormatTag("b", B), // first arg: tag name ex. <b>something</b>, where "b" is name
				new FormatTag("i", I),  // second arg: tag method
				new FormatTag("u", U),
                new FormatTag("h1", H1),
                new FormatTag("h2", H2),
                new FormatTag("h3", H3),
                new FormatTag("h4", H4),
                new FormatTag("h5", H5),
                new FormatTag("h6", H6),
            }, tagStartChar, tagEndChar);
        });
        t.Start();
    }

    public static void B(object obj, int i1, int i2)
    {
        // Typeface font = Typeface.CreateFromAsset(Forms.Context.ApplicationContext.Assets, "Fonts/FontName.ttf");
        // First use Bold than apply custom font! - only way to make it work
        ((SpannableStringBuilder)obj).SetSpan(new TextAppearanceSpan("", TypefaceStyle.Bold, -1, null, null), i1, i2, SpanTypes.ExclusiveInclusive);
        // ((SpannableStringBuilder)obj).SetSpan(new CustomTypefaceSpan("FontName", font), i1, i2, SpanTypes.ExclusiveInclusive); // apply custom font
    }
    public static void I(object obj, int i1, int i2) => ((SpannableStringBuilder)obj).SetSpan(new TextAppearanceSpan("", TypefaceStyle.Italic, -1, null, null), i1, i2, SpanTypes.ExclusiveInclusive);
    
    public static void U(object obj, int i1, int i2) => ((SpannableStringBuilder)obj).SetSpan(new UnderlineSpan(), i1, i2, SpanTypes.ExclusiveInclusive);
    

    public static void H1(object obj, int i1, int i2) => ((SpannableStringBuilder)obj).SetSpan(new TextAppearanceSpan("", TypefaceStyle.Bold, 100, null, null), i1, i2, SpanTypes.ExclusiveInclusive);

    public static void H2(object obj, int i1, int i2) => ((SpannableStringBuilder)obj).SetSpan(new TextAppearanceSpan("", TypefaceStyle.Bold, 85, null, null), i1, i2, SpanTypes.ExclusiveInclusive);

    public static void H3(object obj, int i1, int i2) => ((SpannableStringBuilder)obj).SetSpan(new TextAppearanceSpan("", TypefaceStyle.Bold, 70, null, null), i1, i2, SpanTypes.ExclusiveInclusive);

    public static void H4(object obj, int i1, int i2) => ((SpannableStringBuilder)obj).SetSpan(new TextAppearanceSpan("", TypefaceStyle.Bold, 55, null, null), i1, i2, SpanTypes.ExclusiveInclusive);

    public static void H5(object obj, int i1, int i2) => ((SpannableStringBuilder)obj).SetSpan(new TextAppearanceSpan("", TypefaceStyle.Bold, 40, null, null), i1, i2, SpanTypes.ExclusiveInclusive);

    public static void H6(object obj, int i1, int i2) => ((SpannableStringBuilder)obj).SetSpan(new TextAppearanceSpan("", TypefaceStyle.Bold, 30, null, null), i1, i2, SpanTypes.ExclusiveInclusive);

}