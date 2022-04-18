using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Text;
using Android.Widget;
using SnapCarHire.Droid;
using SnapCarHire.Renders;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]

namespace SnapCarHire.Droid
{
    [Obsolete]
    public class CustomEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                string fontFamily = e.NewElement?.FontFamily;
                if (!string.IsNullOrEmpty(fontFamily))
                {
                    var label = (TextView)Control; // for example
                    Typeface font = Typeface.CreateFromAsset(Forms.Context.Assets, fontFamily + ".otf");
                    label.Typeface = font;
                }
                GradientDrawable gd = new GradientDrawable();
                gd.SetColor(global::Android.Graphics.Color.Transparent);
                this.Control.SetBackgroundDrawable(gd);
                this.Control.SetRawInputType(InputTypes.TextFlagNoSuggestions);
                //Control.SetHintTextColor(ColorStateList.ValueOf(global::Android.Graphics.Color.White));
            }
        }
    }
}