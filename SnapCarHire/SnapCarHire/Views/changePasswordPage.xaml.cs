using SnapCarHire.Popups;
using SnapCarHireController;
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
    public partial class changePasswordPage : ContentPage
    {
        string token;
        int customerId;
        CustomerController customoerController;
        public changePasswordPage()
        {
            InitializeComponent();
            customoerController = new CustomerController();
            token = Application.Current.Properties["currentToken"].ToString();
            customerId = (int)Application.Current.Properties["CustomerId"];
        }

        async void updateBtn_Clicked(System.Object sender, System.EventArgs e)
        {
            int id = 0;
            if (string.IsNullOrEmpty(OldPassEntry.Text))
            {
                await PopupNavigation.Instance.PushAsync(new Error_popup("Please enter your old password."));
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
                    try
                    {
                        busy = true;
                        await PopupNavigation.Instance.PushAsync(new LoadingPopup("Loading.."));
                        await Task.Run(() =>
                        {
                            id = customoerController.changePassword(customerId, OldPassEntry.Text, newPassEntry.Text, token);
                        });
                    }
                    finally
                    {
                        busy = false;
                        if (PopupNavigation.Instance.PopupStack.Count == 1)
                        {
                            await PopupNavigation.Instance.PopAllAsync();
                        }
                        else if (PopupNavigation.Instance.PopupStack.Count > 1)
                        {
                            if (PopupNavigation.Instance.PopupStack[PopupNavigation.Instance.PopupStack.Count - 1].GetType() != typeof(ErrorWithClosePagePopup))
                            {
                                await PopupNavigation.Instance.PopAllAsync();
                            }
                        }

                        if (id != 0)
                        {
                            if (id == -1)
                            {
                                await PopupNavigation.Instance.PushAsync(new Error_popup("Please check your Old password."));
                            }
                            else if (id == 1)
                            {
                                await PopupNavigation.Instance.PushAsync(new SuccessPopUp("Successfully changed your password. Please login to continue.", 7));
                            }
                            else
                            {
                                await PopupNavigation.Instance.PushAsync(new Error_popup("Something went wrong. Please try again. "));
                            }
                        }
                        else
                        {
                            await PopupNavigation.Instance.PushAsync(new Error_popup("Something went wrong. Please try again. "));
                        }
                    }
                }
            }
        }

        void btnBack_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}
