using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SnapCarHire.Views
{

    public class HomePageMasterMenuItem : INotifyPropertyChanged
    {
        public HomePageMasterMenuItem()
        {
            TargetType = typeof(HomePageMasterMenuItem);
        }
        public int Id { get; set; }
        public string Title { get; set; }
        // public string IconSource { get; set; }

        public bool _isSelected { get; set; }
        public bool isSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                OnPropertyChanged("isSelected");
            }
        }

        public ImageSource IconSource { get; set; }
        public ImageSource _IconSource { get; set; }
        public ImageSource IconSource1
        {
            get
            {
                return _IconSource;
            }
            set
            {
                _IconSource = value;
                OnPropertyChanged("IconSource1");
            }
        }
        public Color _BgColor { get; set; }
        public Color BgColor
        {
            get
            {
                return _BgColor;
            }
            set
            {
                _BgColor = value;
                OnPropertyChanged("BgColor");
            }
        }


        public Color _txtColor { get; set; } = Color.White;
        public Color txtColor
        {
            get
            {
                return _txtColor;
            }
            set
            {
                _txtColor = value;
                OnPropertyChanged("txtColor");
            }
        }

        public Type TargetType { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}