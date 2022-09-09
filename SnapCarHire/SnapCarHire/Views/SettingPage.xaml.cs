//using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SnapCarHire.Views
{
    public partial class SettingPage : ContentPage
    {
        public SettingPage()
        {
            InitializeComponent();
            if ((int)App.Current.Properties["AppTheme"] == 1)
            {
                lCheck.Value = true;
            }
            else if ((int)App.Current.Properties["AppTheme"] == 2)
            {
               dCheck.Value=true;
            }
            else
            {
                sCheck.Value = true;
            }


        }

        private void btnBack_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }


        private void themegroup_SelectedItemChanged(object sender, EventArgs e)
        {
            var selectedValue = themegroup.SelectedItem.ToString();
            if (selectedValue == "2")
            {
                App.Current.Properties["AppTheme"] = 2;
                Application.Current.UserAppTheme = OSAppTheme.Dark;
            }
            else if (selectedValue == "1")
            {
                App.Current.Properties["AppTheme"] = 1;
                Application.Current.UserAppTheme = OSAppTheme.Light;
            }
            else
            {
                App.Current.Properties["AppTheme"] = 3;
                Application.Current.UserAppTheme = OSAppTheme.Unspecified;
            }
        }

        void RadioButton_CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            App.Current.Properties["AppTheme"] = 2;
            Application.Current.UserAppTheme = OSAppTheme.Dark;
        }

        void RadioButton_CheckedChanged_1(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            App.Current.Properties["AppTheme"] = 1;
            Application.Current.UserAppTheme = OSAppTheme.Light;
        }

        void RadioButton_CheckedChanged_2(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            App.Current.Properties["AppTheme"] = 3;
            Application.Current.UserAppTheme = OSAppTheme.Unspecified;
        }
    }
}