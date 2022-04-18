using Android.Graphics;
using Android.Widget;
using SnapCarHire.Droid;
using SnapCarHire.Renders;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(BorderlessPicker), typeof(BorderlessPickerRenderer))]

namespace SnapCarHire.Droid
{
    [Obsolete]
    public class BorderlessPickerRenderer : PickerRenderer
    {
        public static void Init() { }
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                Control.Background = null;
                string fontFamily = e.NewElement?.FontFamily;
                if (!string.IsNullOrEmpty(fontFamily))
                {
                    var label = (TextView)Control; // for example
                    Typeface font = Typeface.CreateFromAsset(Forms.Context.Assets, fontFamily + ".otf");
                    label.Typeface = font;
                }
                var layoutParams = new MarginLayoutParams(Control.LayoutParameters);
                layoutParams.SetMargins(0, 0, 0, 0);
                LayoutParameters = layoutParams;
                Control.LayoutParameters = layoutParams;
                Control.SetPadding(0, 0, 0, 0);
                SetPadding(0, 0, 0, 0);
            }
        }
    }
}