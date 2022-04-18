using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace SnapCarHire.Converters
{
    public class statustoColourConverter : IValueConverter
    {
        public Color New { get; set; }
        public Color Open { get; set; }
        public Color CheckOut { get; set; }
        public Color NoShow { get; set; }
        public Color Canceled { get; set; }
        public Color Quote { get; set; }
        public Color noStatus { get; set; }
       

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int statId = int.Parse(value.ToString());
            if (statId == 7)
            {
                return New;
            }
            else if (statId == 2)
            {
                return Open;
            }
            else if (statId == 3)
            {
                return CheckOut;
            }
            else if (statId == 4)
            {
                return NoShow;
            }
            else if (statId == 5)
            {
                return Canceled;
            }
            else if (statId == 6)
            {
                return Quote;
            }
            else
            {
                return noStatus;
            }
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
