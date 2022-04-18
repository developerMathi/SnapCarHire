using MaxVonGrafKftMobileModel.AccessModels;
using SnapCarHire.Popups;
using SnapCarHireController;
using SnapCarHireModel.Constants;
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
    public partial class ConfirmEmailRequest : ContentPage
    {
        ConfirmEmailAddressRequest confirmEmailAddressRequest;
        ConfirmEmailAddressResponse emailAddressResponse;
        string token;
        int customerId;
        private emailConfirmationType confirmationType;
        private int fromVal;

        public ConfirmEmailRequest(int customerId, emailConfirmationType register)
        {
            InitializeComponent();
            confirmEmailAddressRequest = new ConfirmEmailAddressRequest();
            emailAddressResponse = null;
            yourEmaillabel.Text = Constants.cutomerAuthContext.CustomerEmail;
            token = App.Current.Properties["currentToken"].ToString();
            this.customerId = customerId;
            this.confirmationType = register;
            this.fromVal = 0;
        }

        public ConfirmEmailRequest(int customerId, emailConfirmationType register, int fromVal) : this(customerId, register)
        {
            this.fromVal = fromVal;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (PopupNavigation.Instance.PopupStack.Count > 0)
            {
                PopupNavigation.Instance.PopAllAsync();
            }
        }

        private void titleBackBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private async void sentOTPBtn_Clicked(object sender, EventArgs e)
        {
            //code with mail request API call
            confirmEmailAddressRequest.ClientId = Constants.ClientId;
            confirmEmailAddressRequest.CustomerId = Constants.cutomerAuthContext.CustomerId;
            confirmEmailAddressRequest.Email = Constants.cutomerAuthContext.CustomerEmail;
            confirmEmailAddressRequest.CustomerId = Constants.cutomerAuthContext.CustomerId;

            bool busy = false;
            if (!busy)
            {
                try
                {
                    busy = true;
                    await PopupNavigation.Instance.PushAsync(new LoadingPopup("Loading details..."));

                    await Task.Run(async () =>
                    {
                        try
                        {
                            emailAddressResponse = ConfirmEmailAddress(confirmEmailAddressRequest);
                        }
                        catch (Exception ex)
                        {
                            await PopupNavigation.Instance.PushAsync(new Error_popup(ex.Message));
                        }
                    });
                }
                finally
                {
                    busy = false;
                    if (PopupNavigation.Instance.PopupStack.Count == 1)
                    {
                        await PopupNavigation.Instance.PopAllAsync();
                    }
                    if (PopupNavigation.Instance.PopupStack.Count > 1)
                    {
                        if (PopupNavigation.Instance.PopupStack[PopupNavigation.Instance.PopupStack.Count - 1].GetType() != typeof(ErrorWithClosePagePopup))
                        {
                            await PopupNavigation.Instance.PopAllAsync();
                        }
                    }

                    if (emailAddressResponse != null)
                    {
                        if (emailAddressResponse.message != null)
                        {
                            if (emailAddressResponse.message.ErrorCode == "200")
                            {
                                if (fromVal == 1)
                                {
                                    await Navigation.PushModalAsync(new enterConfirmationCodePage(customerId, confirmationType, fromVal));
                                }
                                else
                                {
                                    await Navigation.PushModalAsync(new enterConfirmationCodePage(customerId, confirmationType));
                                }

                            }
                            else
                            {
                                await PopupNavigation.Instance.PushAsync(new Error_popup(emailAddressResponse.message.ErrorMessage));
                            }
                        }
                        else
                        {
                            await PopupNavigation.Instance.PushAsync(new Error_popup("Sorry, Something went wrong, Please try again."));
                        }

                    }
                    else
                    {
                        await PopupNavigation.Instance.PushAsync(new Error_popup("Sorry, Something went wrong, Please try again."));
                    }
                }
            }
        }

        private ConfirmEmailAddressResponse ConfirmEmailAddress(ConfirmEmailAddressRequest confirmEmailAddressRequest)
        {
            ConfirmEmailAddressResponse response = null;
            LoginController controller = new LoginController();

            response = controller.ConfirmEmailAddress(confirmEmailAddressRequest, token);
            return response;

        }
    }
}