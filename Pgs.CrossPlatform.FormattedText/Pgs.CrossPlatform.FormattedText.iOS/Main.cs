﻿using UIKit;

namespace Pgs.CrossPlatform.FormattedText.iOS
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
#if !_NuGetRelease_
            FormatConfig.Init(false, '[', ']'); // comment when building for NuGet
#endif
            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, "AppDelegate");
        }
    }
}
