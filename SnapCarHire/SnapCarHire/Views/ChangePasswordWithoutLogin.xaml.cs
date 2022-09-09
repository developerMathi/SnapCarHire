using SnapCarHire.Popups;
using SnapCarHireController;
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
    public partial class ChangePasswordWithoutLogin : ContentPage
    {
        private string email;
        string token;
        private CutomerAuthContext cutomerAuthContext;
        int CPId;

        public ChangePasswordWithoutLogin(string email)
        {
            InitializeComponent();
            token = App.Current.Properties["currentToken"].ToString();
            this.email = email;
            emailEntry.Text = email;
            CPId = 0;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (PopupNavigation.PopupStack.Count > 0)
            {
                PopupNavigation.Instance.PopAllAsync();
            }
        }

        async void updateBtn_Clicked(System.Object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(OldPassEntry.Text))
            {
                await PopupNavigation.Instance.PushAsync(new Error_popup("Please enter your current password."));
            }
            else if (string.IsNullOrEmpty(newPassEntry.Text))
            {
                await PopupNavigation.Instance.PushAsync(new Error_popup("Please enter a new password."));
            }
            else if (newPassEntry.Text.Length < 6)
            {
                await PopupNavigation.Instance.PushAsync(new Error_popup("Password must be atleast with 6 characters."));
            }
            else if (!(newPassEntry.Text == confPassEntry.Text))
            {
                await PopupNavigation.Instance.PushAsync(new Error_popup("Passwords do not match. Please re - enter your password"));
                confPassEntry.Text = null;
            }
            else
            {
                bool busy = false;
                if (!busy)
                {
                    await PopupNavigation.Instance.PushAsync(new LoadingPopup(""));
                    try
                    {
                        busy = true;

                        await Task.Run(() =>
                        {
                            CPId = ChangePassword();

                        });
                    }
                    finally
                    {
                        busy = false;
                        if (PopupNavigation.Instance.PopupStack.Count > 0)
                        {
                            await PopupNavigation.Instance.PopAllAsync();
                        }
                        if (CPId > 0)
                        {
                            await PopupNavigation.Instance.PushAsync(new SuccessPopUp("Your password changed successfully", 6));
                        }
                        else if (CPId == -1)
                        {
                            OldPassEntry.Text = null;
                            await PopupNavigation.Instance.PushAsync(new Error_popup("Please check your current password."));
                        }
                        else
                        {
                            await PopupNavigation.Instance.PushAsync(new Error_popup("Something went wrong. Please try again."));
                        }
                    }
                }
            }
        }

        private int ChangePassword()
        {
            CustomerLogin loginCustomer = new CustomerLogin();
            loginCustomer.email = email;
            loginCustomer.Password = OldPassEntry.Text;
            loginCustomer.clientId = Constants.ClientId;
            loginCustomer.ClientTime = DateTime.Now;

            LoginController loginController = new LoginController();
            cutomerAuthContext = loginController.CheckLogin(loginCustomer, token);

            if (cutomerAuthContext != null)
            {
                if (cutomerAuthContext.CustomerId > 0)
                {
                    CustomerController customoerController = new CustomerController();
                    CPId = customoerController.changePassword(cutomerAuthContext.CustomerId, OldPassEntry.Text, newPassEntry.Text, token);
                }
                else
                {
                    CPId = -1;
                }
            }
            else
            {
                CPId = -1;
            }
            return CPId;

        }

        void passDeatilBtn_Tapped(System.Object sender, System.EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new DetailPopUp("Temporary  password", "You should have received a temporary password via email. Copy it here. If you did not receive it, click ‘Resend Email’.", 1));
        }


    }
}
