# Pgs.CrossPlatform.FormattedText
Cross Platform C# Xamarin Formatted Text Label control enabling to add own tags [b][/b] or similar to HTML tags &lt;b>&lt;b>&lt;H1>&lt;/H2>  to format text in any size and font

We are going to publish NuGet packages:
- Pgs.CrossPlatform.FormattedText.Libs,
- Pgs.CrossPlatform.FormattedText.Templated. (temporary name)

How to use(applied mainly to .Libs):
  * add to MainActivity.cs at the very start of OnCreate:
```C#
FormatConfig.Init('[', ']'); // class name will be changed probably  
```
  * adjust config in FormatConfig.cs
  * use control within xaml like:
```C#
xmlns:ft="clr-namespace:Pgs.CrossPlatform.FormattedText;assembly=Pgs.CrossPlatform.FormattedText" // namespace
// later in content
    <ft:FormattedLabel Text="zzzzzzzzzzzWelcome [i]W[b]elc[/b]om[b]e[/b][/i] [b]t[i]o[/i][/b] [b][i]Xama[/i]rin[/b] [b][i]Forms[/i][/b]!"
             x:Name="formated"
             VerticalOptions="Center"
             HorizontalOptions="Center" />
```  
