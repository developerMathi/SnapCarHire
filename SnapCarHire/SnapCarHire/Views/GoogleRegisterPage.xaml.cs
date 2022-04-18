using SnapCarHire.AuthHelpers;
using SnapCarHire.Popups;
using SnapCarHireModel;
using Rg.Plugins.Popup.Services;
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
    public partial class GoogleRegisterPage : ContentPage
    {
        User user;
        CustomerReview customer;
        public GoogleRegisterPage(User user)
        {
            InitializeComponent();
            this.user = user;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (PopupNavigation.Instance.PopupStack.Count > 0)
            {
                if (PopupNavigation.Instance.PopupStack[PopupNavigation.Instance.PopupStack.Count - 1].GetType() == typeof(ErrorWithClosePagePopup))
                {
                    await PopupNavigation.Instance.PopAllAsync();
                }
                if (user != null)
                {
                    EmailLabel.Text = user.Email;
                    if (user.Name != null)
                    {
                        FirstNameEntry.Text = user.Name;
                        profileImage.Source = ImageSource.FromUri(new Uri(user.Picture)); ;
                    }
                }
            }

        }

        private void regiseterNxtBtn_Clicked(object sender, EventArgs e)
        {

        }

        private void DateOfBithEntry_Focused(object sender, FocusEventArgs e)
        {

        }

        private void countryPicker_Unfocused(object sender, FocusEventArgs e)
        {

        }
    }
}