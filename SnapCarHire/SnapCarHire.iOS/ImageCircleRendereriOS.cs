using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Foundation;
using SnapCarHire.iOS;
using SnapCarHire.Renders;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ImageCircle), typeof(ImageCircleRendereriOS))]
namespace SnapCarHire.iOS
{
    public class ImageCircleRendereriOS : ImageRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);
            if (Control == null)
                return;

            CreateCircle();
        }

        private void CreateCircle()
        {
            try
            {
                double min = Math.Min(Element.Width, Element.Height);
                Control.Layer.CornerRadius = (float)(min / 2.0);
                Control.Layer.MasksToBounds = false;
                //Control.Layer.BorderColor = Color.White.ToCGColor();
                // Control.Layer.BorderWidth = 3;
                Control.ClipsToBounds = true;
            }
            catch (Exception ex)
            {
                // Debug.WriteLine("Unable to create circle image: " + ex)
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == VisualElement.HeightProperty.PropertyName ||
                e.PropertyName == VisualElement.WidthProperty.PropertyName)
            {
                CreateCircle();
            }
        }
    }
}