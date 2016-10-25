using Foundation;
using Pgs.CrossPlatform.FormattedText;
using Pgs.CrossPlatform.FormattedText.Core;
using Pgs.CrossPlatform.FormattedText.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(FormattedLabel), typeof(FormattedLabelRenderer))]
namespace Pgs.CrossPlatform.FormattedText.iOS
{
    public class FormattedLabelRenderer : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
			Control.AttributedText = FormatParser.Instance.Parse<NSMutableAttributedString>(Control.Text, Control);
        }
    }
}