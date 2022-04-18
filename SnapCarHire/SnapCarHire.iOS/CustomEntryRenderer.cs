using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using SnapCarHire.Renders;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(EntryRenderer))]

namespace SnapCarHire.iOS
{
    public class MyEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.BorderStyle = UITextBorderStyle.None;
                Control.Layer.CornerRadius = 10;
                //Control.TextColor = UIColor.White;
            }
        }
    }
}