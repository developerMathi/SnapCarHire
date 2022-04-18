using System;
using Android.Content;
using Android.Graphics;
using SnapCarHire.Droid;
using SnapCarHire.Renders;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;


[assembly: ExportRenderer(typeof(ImageCircle), typeof(ImageCircleRendererDroid))]

namespace SnapCarHire.Droid
{
    public class ImageCircleRendererDroid : ImageRenderer
    {
        public ImageCircleRendererDroid(Context context) : base(context)
        {
        }

        protected override bool DrawChild(Canvas canvas, global::Android.Views.View child, long drawingTime)
        {
            try
            {
                var radius = Math.Min(Width, Height) / 2;
                var strokeWidth = 10;
                radius -= strokeWidth / 2;

                //Create path to clip
                var path = new Path();
                path.AddCircle(Width / 2, Height / 2, radius, Path.Direction.Ccw);
                canvas.Save();
                canvas.ClipPath(path);

                var result = base.DrawChild(canvas, child, drawingTime);

                canvas.Restore();

                // Create path for circle border
                path = new Path();
                path.AddCircle(Width / 2, Height / 2, radius, Path.Direction.Ccw);

                var paint = new Paint();
                paint.AntiAlias = true;
                // paint.StrokeWidth = 5;
                paint.SetStyle(Paint.Style.Stroke);
                paint.Color = global::Android.Graphics.Color.Transparent;

                canvas.DrawPath(path, paint);

                //Properly dispose
                paint.Dispose();
                path.Dispose();
                return result;
            }
            catch (Exception ex)
            {
                // Debug.WriteLine("Unable to create circle image: " + ex);
            }

            return base.DrawChild(canvas, child, drawingTime);
        }
    }
}