using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SnapCarHire.Renders
{
    public class BorderlessTimePicker : TimePicker
    {
        public static readonly BindableProperty EnterTextProperty = BindableProperty.Create(propertyName: "Placeholder", returnType: typeof(string), declaringType: typeof(BorderlessDatePicker), defaultValue: default(string));
        public string Placeholder
        {
            get;
            set;
        }
    }
}
