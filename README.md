# Pgs.CrossPlatform.FormattedText
Cross Platform C# Xamarin Formatted Text Label control enabling to add own tags [b][/b] or similar to HTML tags &lt;b>&lt;b>&lt;H1>&lt;/H2>  to format text in any size and font

We are going to publish NuGet packages:
- Pgs.CrossPlatform.FormattedText.Libs,
- Pgs.CrossPlatform.FormattedText.Templated. (temporary name)

How to use(applied mainly to .Libs):
  * add to MainActivity.cs at the very start of OnCreate:
```C#
new ParserGeneratorConfig().Init('[', ']'); // class name will be changed probably  
```
  * adjust config by extending ParserGeneratorConfig(name will be changed probably) class
    following pattern for configuration(this ex. is ParserGeneratorConfig):
```C#
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
            // Typeface font = Typeface.CreateFromAsset(Forms.Context.ApplicationContext.Assets, "Fonts/FONT_NAME.ttf");
            // First use Bold than apply custom font! - only way to make it work
            ((SpannableStringBuilder)obj).SetSpan(new TextAppearanceSpan("", TypefaceStyle.Bold, -1, null, null), i1, i2, SpanTypes.ExclusiveInclusive);
           // ((SpannableStringBuilder)obj).SetSpan(new CustomTypefaceSpan("FONT_NAME", font), i1, i2, SpanTypes.ExclusiveInclusive);
        }

        public static void I(object obj, int i1, int i2)
        {
            ((SpannableStringBuilder)obj).SetSpan(new TextAppearanceSpan("", TypefaceStyle.Italic, -1, null, null), i1, i2, SpanTypes.ExclusiveInclusive);
        }
```  
  * use control within xaml like:
```C#
xmlns:ft="clr-namespace:Pgs.CrossPlatform.FormattedText;assembly=Pgs.CrossPlatform.FormattedText" // namespace
// later in content
    <ft:FormattedLabel Text="zzzzzzzzzzzWelcome [i]W[b]elc[/b]om[b]e[/b][/i] [b]t[i]o[/i][/b] [b][i]Xama[/i]rin[/b] [b][i]Forms[/i][/b]!"
             x:Name="formated"
             VerticalOptions="Center"
             HorizontalOptions="Center" />
```  
