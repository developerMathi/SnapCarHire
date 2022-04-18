using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SnapCarHire.Renders
{
    public class MaskedBehavior : Behavior<Entry>
    {
        private string _mask = "";
        public string Mask
        {
            get => _mask;
            set
            {
                _mask = value;
                SetPositions();
            }
        }

        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(entry);
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(entry);
        }

        IDictionary<int, char> _positions;

        void SetPositions()
        {
            if (string.IsNullOrEmpty(Mask))
            {
                _positions = null;
                return;
            }

            var list = new Dictionary<int, char>();
            for (var i = 0; i < Mask.Length; i++)
                if (Mask[i] != 'X')
                    list.Add(i, Mask[i]);

            _positions = list;
        }

        private void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            var entry = sender as Entry;

            var text = entry.Text;

            if (string.IsNullOrWhiteSpace(text) || _positions == null)
                return;

            if (text.Length > _mask.Length)
            {
                entry.Text = text.Remove(text.Length - 1);
                return;
            }

            foreach (var position in _positions)
                if (text.Length >= position.Key + 1)
                {
                    var value = position.Value.ToString();
                    if (text.Substring(position.Key, 1) != value)
                        text = text.Insert(position.Key, value);
                }

            if (entry.Text != text)
                entry.Text = text;


            //var entry = sender as Entry;
            //var text = entry.Text;

            //if (!string.IsNullOrWhiteSpace(Mask))

            //    // 1. Adding the MaxLength
            //    if (text.Length == _mask.Length)
            //        entry.MaxLength = _mask.Length;

            //// 2. Evaluating if the user is removing test
            //if ((args.OldTextValue == null) || (args.OldTextValue.Length <= args.NewTextValue.Length))

            //    // 3. Evaluating mask positions
            //    for (int i = Mask.Length; i >= text.Length; i--)
            //    {
            //        if (Mask[(text.Length - 1)] != 'X')
            //        {
            //            text = text.Insert((text.Length - 1), Mask[(text.Length - 1)].ToString());
            //        }
            //    }
            //entry.Text = text;
            //entry.Keyboard = Keyboard.Telephone;
        }


    }
}
