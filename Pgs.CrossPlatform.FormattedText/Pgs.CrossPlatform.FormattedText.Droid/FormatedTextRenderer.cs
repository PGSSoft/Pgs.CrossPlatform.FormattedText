using System;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Android.Graphics;
using Android.Text;
using Android.Text.Style;

using System.ComponentModel;
using Pgs.CrossPlatform.FormattedText;
using Pgs.CrossPlatform.FormattedText.Core;
using Pgs.CrossPlatform.FormattedText.Droid;

[assembly: ExportRenderer(typeof(FormattedLabel), typeof(FormatedTextRenderer))]
namespace Pgs.CrossPlatform.FormattedText.Droid
{
    public class FormatedTextRenderer : LabelRenderer
    {
        private bool _isInitialized = false;

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            
            if (e.PropertyName == "Text" || !_isInitialized)
            {
                if (Control == null)
                    return;

                _isInitialized = true;
                              
                Control.SetText(
                    SpanParser.Instance.Parse<SpannableStringBuilder>(Control.Text), 
                    TextView.BufferType.Spannable);
            }
        }
       
    }
}