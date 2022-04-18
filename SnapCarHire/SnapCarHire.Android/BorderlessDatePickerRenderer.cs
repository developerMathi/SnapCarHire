using Android.Graphics;
using Android.Widget;
using SnapCarHire.Droid;
using SnapCarHire.Renders;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(BorderlessDatePicker), typeof(BorderlessDatePickerRenderer))]

namespace SnapCarHire.Droid
{
    [Obsolete]
    public class BorderlessDatePickerRenderer : DatePickerRenderer
    {
        public static void Init() { }
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.DatePicker> e)
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

                BorderlessDatePicker element = Element as BorderlessDatePicker;
                if (!string.IsNullOrWhiteSpace(element.Placeholder))
                {
                    Control.Text = element.Placeholder;
                }
                this.Control.TextChanged += (sender, arg) =>
                {
                    var selectedDate = arg.Text.ToString();
                    if (selectedDate == element.Placeholder)
                    {
                        Control.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }

                };

            }

        }


    }
}