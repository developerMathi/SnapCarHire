using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SnapCarHire.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Xamarin.Forms.Button), typeof(ButtonCustomRenderer))]
namespace SnapCarHire.Droid
{
    public class ButtonCustomRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {

            base.OnElementChanged(e);
            string fontDamily = e.NewElement?.FontFamily;
            if (!string.IsNullOrEmpty(fontDamily))
            {
                var label = (TextView)Control; // for example
#pragma warning disable CS0618 // 'Forms.Context' is obsolete: 'Context is obsolete as of version 2.5. Please use a local context instead.'
                Typeface font = Typeface.CreateFromAsset(Forms.Context.Assets, fontDamily + ".otf");
#pragma warning restore CS0618 // 'Forms.Context' is obsolete: 'Context is obsolete as of version 2.5. Please use a local context instead.'
                label.Typeface = font;
                var button = this.Control;
                button.SetAllCaps(false);
            }
        }
    }
}