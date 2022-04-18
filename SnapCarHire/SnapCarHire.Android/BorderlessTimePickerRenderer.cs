using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Widget;
using SnapCarHire.Droid;
using SnapCarHire.Renders;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(BorderlessTimePicker), typeof(BorderlessTimePickerRenderer))]

namespace SnapCarHire.Droid
{
    [Obsolete]
    public class BorderlessTimePickerRenderer : TimePickerRenderer
    {
        public static void Init() { }
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.TimePicker> e)
        {
            base.OnElementChanged(e);

            TimePickerDialogIntervals timePickerDlg = new TimePickerDialogIntervals(this.Context, new EventHandler<TimePickerDialogIntervals.TimeSetEventArgs>(UpdateDuration),
            Element.Time.Hours, Element.Time.Minutes, false);

            //if (e.OldElement == null)
            //{
            //    //Control.Background = null;
            //    //string fontFamily = e.NewElement?.FontFamily;
            //    //if (!string.IsNullOrEmpty(fontFamily))
            //    //{
            //    //    var label = (TextView)Control; // for example
            //    //    Typeface font = Typeface.CreateFromAsset(Forms.Context.Assets, fontFamily + ".otf");
            //    //    label.Typeface = font;
            //    //}
            //    //var layoutParams = new MarginLayoutParams(Control.LayoutParameters);
            //    //layoutParams.SetMargins(0, 0, 0, 0);
            //    //LayoutParameters = layoutParams;
            //    //Control.LayoutParameters = layoutParams;
            //    //Control.SetPadding(0, 0, 0, 0);
            //    //SetPadding(0, 0, 0, 0);

            //    //BorderlessTimePicker element = Element as BorderlessTimePicker;
            //    //if (!string.IsNullOrWhiteSpace(element.Placeholder))
            //    //{
            //    //    Control.Text = element.Placeholder;
            //    //}
            //    //this.Control.TextChanged += (sender, arg) =>
            //    //{
            //    //    var selectedTime = arg.Text.ToString();
            //    //    if (selectedTime == element.Placeholder)
            //    //    {
            //    //        Control.Text = DateTime.Now.TimeOfDay.ToString();
            //    //    }
            //    //};
            //    var control = new EditText(this.Context);
            //    control.Focusable = false;
            //    control.FocusableInTouchMode = false;
            //    control.Clickable = false;
            //    control.Click += (sender, ea) => timePickerDlg.Show();
            //    control.Text = Element.Time.Hours.ToString("00") + ":" + Element.Time.Minutes.ToString("00");
            //    var layoutParams = new MarginLayoutParams(Control.LayoutParameters);
            //    layoutParams.SetMargins(0, 0, 0, 0);
            //    LayoutParameters = layoutParams;
            //    control.LayoutParameters = layoutParams;
            //    control.SetPadding(0, 0, 0, 0);
            //    SetPadding(0, 0, 0, 0);

            //    string fontFamily = e.NewElement?.FontFamily;
            //    if (!string.IsNullOrEmpty(fontFamily))
            //    {
            //        var label = (TextView)Control; // for example
            //        Typeface font = Typeface.CreateFromAsset(Forms.Context.Assets, fontFamily + ".otf");
            //        label.Typeface = font;
            //    }

            //    SetNativeControl(control);
            //}

            void UpdateDuration(object sender, Android.App.TimePickerDialog.TimeSetEventArgs e)
            {
                int min = e.Minute;
                if (e.Minute > 60)
                {
                    min = e.Minute % 60;
                    min = 60 - min;
                }
                Element.Time = new TimeSpan(e.HourOfDay, min, 0);
                if (Element.Time.Hours < 12)
                {
                    Control.Text = Element.Time.Hours.ToString("00") + ":" + Element.Time.Minutes.ToString("00") + " AM";
                }
                else if (Element.Time.Hours == 12)
                {
                    Control.Text = Element.Time.Hours.ToString("00") + ":" + Element.Time.Minutes.ToString("00") + " PM";
                }
                else
                {
                    Control.Text = (Element.Time.Hours - 12).ToString("00") + ":" + Element.Time.Minutes.ToString("00") + "PM";
                }

            }

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
                Control.Click += (sender, ea) => timePickerDlg.Show();
                SetPadding(0, 0, 0, 0);

                BorderlessTimePicker element = Element as BorderlessTimePicker;
                if (!string.IsNullOrWhiteSpace(element.Placeholder))
                {
                    Control.Text = element.Placeholder;
                }
                this.Control.TextChanged += (sender, arg) =>
                {
                    var selectedTime = arg.Text.ToString();
                    if (selectedTime == element.Placeholder)
                    {
                        Control.Text = DateTime.Now.TimeOfDay.ToString();
                    }
                };
            }
        }
        public class TimePickerDialogIntervals : TimePickerDialog
        {
            public const int TimePickerInterval = 15;
            private bool _ignoreEvent = false;

            public TimePickerDialogIntervals(Context context, EventHandler<TimePickerDialog.TimeSetEventArgs> callBack, int hourOfDay, int minute, bool is24HourView)
                : base(context, (sender, e) =>
                {
                    callBack(sender, new TimePickerDialog.TimeSetEventArgs(e.HourOfDay, e.Minute * TimePickerInterval));
                }, hourOfDay, minute / TimePickerInterval, is24HourView)
            {
            }

            public override void OnTimeChanged(Android.Widget.TimePicker view, int hourOfDay, int minute)
            {
                base.OnTimeChanged(view, hourOfDay, minute);

                if (_ignoreEvent) return;

                if (minute % TimePickerInterval != 0)
                {
                    int minuteFloor = minute - (minute % TimePickerInterval);
                    minute = minuteFloor + (minute == minuteFloor + 1 ? TimePickerInterval : 0);
                    if (minute == 60)
                        minute = 0;
                    _ignoreEvent = true;
                    view.CurrentMinute = (Java.Lang.Integer)minute;
                    _ignoreEvent = false;
                }
            }
        }
    }

}