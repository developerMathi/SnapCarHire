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
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SnapCarHire.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class enterConfirmationCodePage : ContentPage
    {
        ConfirmEmailAddressRequest request;
        CheckConfirmEmailAddressResponse response;
        string token;
        int customerId;
        private emailConfirmationType confirmationType;
        private int fromVal;

        public enterConfirmationCodePage(int customerId)
        {

            InitializeComponent();
            request = new ConfirmEmailAddressRequest();
            response = null;
            token = App.Current.Properties["currentToken"].ToString();
            emailAddressLabel.Text = Constants.cutomerAuthContext.CustomerEmail;
            this.customerId = customerId;
            this.confirmationType = emailConfirmationType.LogIn;
            this.fromVal = 0;
        }

        public enterConfirmationCodePage(int customerId, emailConfirmationType type)
        {
            InitializeComponent();
            request = new ConfirmEmailAddressRequest();
            response = null;
            token = App.Current.Properties["currentToken"].ToString();
            emailAddressLabel.Text = Constants.cutomerAuthContext.CustomerEmail;
            this.customerId = customerId;
            this.confirmationType = emailConfirmationType.LogIn;

            this.confirmationType = type;
            this.fromVal = 0;

        }

        public enterConfirmationCodePage(int customerId, emailConfirmationType type, int fromVal) : this(customerId, type)
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

        private async void ConfirmBtn_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (ContactNoEntry.Text == null)
                {
                    // Use default vibration length
                    Vibration.Vibrate();
                    //codeFrame.BorderColor = Color.Red;
                    codeFrameshakeAnimation();
                }
                else
                {
                    string code = ContactNoEntry.Text.Replace(" ", "").Trim();
                    if (code.Length < 6)
                    {
                        Vibration.Vibrate();
                        //codeFrame.BorderColor = Color.Red;
                        codeFrameshakeAnimation();
                        ContactNoEntry.Text = null;
                    }
                    else
                    {
                        request.ClientId = Constants.ClientId;
                        request.CustomerId = Constants.cutomerAuthContext.CustomerId;
                        request.Email = Constants.cutomerAuthContext.CustomerEmail;
                        request.ConfirmationCode = code;

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
                                        response = checkConfirmEmailAddress(request);
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
                                if (response != null)
                                {
                                    if (response.refId == 1)
                                    {
                                        if (App.Current.Properties.ContainsKey("CustomerId"))
                                        {
                                            App.Current.Properties["CustomerId"] = customerId;
                                        }
                                        else
                                        {
                                            App.Current.Properties.Add("CustomerId", customerId);
                                        }
                                        if (fromVal == 1)
                                        {
                                            await Navigation.PushModalAsync(new EmailonfirmSuccess(confirmationType, fromVal));
                                        }
                                        else
                                        {
                                            await Navigation.PushModalAsync(new EmailonfirmSuccess(confirmationType));
                                        }

                                    }
                                    else if (response.refId == 2)
                                    {
                                        await PopupNavigation.Instance.PushAsync(new Error_popup("Sorry, your confirmation code was expired. Please resend code. "));
                                    }
                                    else
                                    {
                                        await PopupNavigation.Instance.PushAsync(new Error_popup("Invalid confirmation code"));

                                    }
                                }
                                else
                                {
                                    await PopupNavigation.Instance.PushAsync(new Error_popup("Something went wrong, Please try again."));
                                }
                            }
                        }
                    }
                }



            }
            catch (FeatureNotSupportedException ex)
            {
                // Feature not supported on device
            }
            catch (Exception ex)
            {
                // Other error has occurred.
            }
            //Navigation.PushAsync(new EmailonfirmSuccess());
        }

        private CheckConfirmEmailAddressResponse checkConfirmEmailAddress(ConfirmEmailAddressRequest request)
        {
            CheckConfirmEmailAddressResponse response = new CheckConfirmEmailAddressResponse();
            LoginController controller = new LoginController();

            response = controller.checkConfirmEmailAddress(request, token);
            return response;
        }

        private void titleBackBtn_Clicked(object sender, EventArgs e)
        {
            if (Navigation.ModalStack.Count > 2)
            {
                if (Navigation.ModalStack[Navigation.ModalStack.Count - 2] is OtherInformationPage)
                {
                    //Do nothing
                }
                else
                {
                    Navigation.PopModalAsync();
                }

            }
            else
            {
                Navigation.PopModalAsync();
            }

        }

        private async void codeFrameshakeAnimation()
        {
            uint timeout = 50;

            await codeFrame.TranslateTo(-15, 0, timeout);

            await codeFrame.TranslateTo(15, 0, timeout);

            await codeFrame.TranslateTo(-10, 0, timeout);

            await codeFrame.TranslateTo(10, 0, timeout);

            await codeFrame.TranslateTo(-5, 0, timeout);

            await codeFrame.TranslateTo(5, 0, timeout);

            codeFrame.TranslationX = 0;
        }

        private void ContactNoEntry_Focused(object sender, FocusEventArgs e)
        {
            //codeFrame.BorderColor = Color.Black;
        }

        private void resendLable_Tapped(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new ConfirmEmailRequest(customerId, emailConfirmationType.Register));
        }
        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            if (Navigation.ModalStack.Count > 2)
            {
                if (Navigation.ModalStack[Navigation.ModalStack.Count - 2] is OtherInformationPage)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            else
            {
                return false;
            }

        }
    }
}