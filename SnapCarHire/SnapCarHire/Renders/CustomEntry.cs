using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SnapCarHire.Renders
{
    public class CustomEntry : Entry
    {
        private string _mask = "";
        public string Mask
        {
            get => _mask;
            set
            {
                _mask = value;
            }
        }
        private void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            var entry = sender as Entry;
            var text = entry.Text;

            if (!string.IsNullOrWhiteSpace(Mask))

                // 1. Adding the MaxLength
                if (text.Length == _mask.Length)
                    entry.MaxLength = _mask.Length;

            // 2. Evaluating if the user is removing test
            if ((args.OldTextValue == null) || (args.OldTextValue.Length <= args.NewTextValue.Length))

                // 3. Evaluating mask positions
                for (int i = Mask.Length; i >= text.Length; i--)
                {
                    if (Mask[(text.Length - 1)] != 'X')
                    {
                        text = text.Insert((text.Length - 1), Mask[(text.Length - 1)].ToString());
                    }
                }
            entry.Text = text;
        }
    }


}
