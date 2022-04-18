using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SnapCarHire.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LogoutPage : ContentPage
    {
        public LogoutPage()
        {
            InitializeComponent();
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            bool logout = await DisplayAlert("Alert", "Are you sure want to logout", "Yes", "No");
            if (logout)
            {
                App.Current.Properties["CustomerId"] = 0;
                Constants.cutomerAuthContext = null;
                await Navigation.PushAsync(new WelcomPage());
            }
            else
            {
                await Navigation.PushAsync(new HomeTabbedPage());
            }
        }
    }
}