# Pgs.CrossPlatform.FormattedText
Cross Platform C# Xamarin Formatted Text Label control enabling to add own tags [b][/b] or similar to HTML tags &lt;b>&lt;b>&lt;H1>&lt;/H2>  to format text in any size and font

Android and iOS are currently supported

NuGet:
https://www.nuget.org/packages/Pgs.CrossPlatform.FormattedText/

  * Droid: add to MainActivity.cs at the very start of OnCreate:
  
    iOS: add to Main.cs at the very start of Main:
```C#
// TOP OF OnCreate/Main
FormatConfig.Init(true, '[', ']'); // shoudl throw exceptions is config is lacking and chars that starts and ends tag
```
  * adjust config in FormatConfig.cs
  * use control within xaml like:
```C#
// namespace
    xmlns:ft="clr-namespace:Pgs.CrossPlatform.FormattedText;assembly=Pgs.CrossPlatform.FormattedText" 
// later in content
    <ft:FormattedLabel Text="Welcome [i]W[b]elc[/b]om[b]e[/b][/i] [b]t[i]o[/i][/b] [b][i]Xama[/i]rin[/b] [b][i]Forms[/i][/b]!"
        x:Name="formated"
        VerticalOptions="Center"
        HorizontalOptions="Center" />
```  
