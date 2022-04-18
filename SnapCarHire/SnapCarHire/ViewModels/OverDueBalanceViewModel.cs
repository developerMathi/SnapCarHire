using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace SnapCarHire.ViewModels
{
    public class OverDueBalanceViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string balance = String.Empty;
        private bool stackVisible = false;

        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        public string _balance
        {
            get
            {
                return this.balance;
            }

            set
            {
                if (value != this.balance)
                {
                    this.balance = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool _stackVisible
        {
            get
            {
                return this._stackVisible;
            }

            set
            {
                if (value != this.stackVisible)
                {
                    this.stackVisible = value;
                    NotifyPropertyChanged();
                }
            }
        }
    }
}
