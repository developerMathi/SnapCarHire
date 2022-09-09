using SnapCarHire.AuthHelpers;
using SnapCarHire.Popups;
using SnapCarHire.ViewModels;
using SnapCarHireController;
using SnapCarHireModel;
using SnapCarHireModel.AccessModels;
using SnapCarHireModel.Constants;
using Newtonsoft.Json;
using Plugin.FacebookClient;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace SnapCarHire.Views
{
    public partial class LoginPage : ContentPage
    {
        Account account;
        AccountStore store;

        CutomerAuthContext cutomerAuthContext = null;
        GetCustomerPortalDetailsMobileRequest portalDetailsMobileRequest;
        GetCustomerPortalDetailsMobileResponse PortalDetailsMobileResponse;
        string token;
        private int fromVal;

        public LoginPage()
        {
            InitializeComponent();
            //BindingContext = new SocialLoginPageViewModel();
            store = AccountStore.Create();
            var assembly = typeof(LoginPage);
            portalDetailsMobileRequest = new GetCustomerPortalDetailsMobileRequest();
            PortalDetailsMobileResponse = null;
            // logoImage.Source = ImageSource.FromResource("MaxVonGrafKftMobile.Assets.logo_high_resolution_white-1.png", assembly);
            //emailIcon.Source = ImageSource.FromResource("MaxVonGrafKftMobile.Assets.emailIcon.png", assembly);
            // passwordIcon.Source = ImageSource.FromResource("MaxVonGrafKftMobile.Assets.passwordIcon.png", assembly);
            //LoginButton.ImageSource= ImageSource.FromResource("MaxVonGrafKftMobile.Assets.LoginIcon.png", assembly);

            if (PopupNavigation.Instance.PopupStack.Count > 0)
            {
                PopupNavigation.Instance.PopAllAsync();
            }


            var forgetPassword_tab = new TapGestureRecognizer();
            forgetPassword_tab.Tapped += (s, e) =>
            {
                Navigation.PushModalAsync(new ForgetPasswordPage());
            };
            forgetPasswordLabel.GestureRecognizers.Add(forgetPassword_tab);
            fromVal = 0;
        }

        public LoginPage(int fromVal)
        {
            InitializeComponent();
            //BindingContext = new SocialLoginPageViewModel();
            store = AccountStore.Create();
            var assembly = typeof(LoginPage);
            portalDetailsMobileRequest = new GetCustomerPortalDetailsMobileRequest();
            PortalDetailsMobileResponse = null;
            // logoImage.Source = ImageSource.FromResource("MaxVonGrafKftMobile.Assets.logo_high_resolution_white-1.png", assembly);
            //emailIcon.Source = ImageSource.FromResource("MaxVonGrafKftMobile.Assets.emailIcon.png", assembly);
            // passwordIcon.Source = ImageSource.FromResource("MaxVonGrafKftMobile.Assets.passwordIcon.png", assembly);
            //LoginButton.ImageSource= ImageSource.FromResource("MaxVonGrafKftMobile.Assets.LoginIcon.png", assembly);

            if (PopupNavigation.Instance.PopupStack.Count > 0)
            {
                PopupNavigation.Instance.PopAllAsync();
            }


            var forgetPassword_tab = new TapGestureRecognizer();
            forgetPassword_tab.Tapped += (s, e) =>
            {
                Navigation.PushModalAsync(new ForgetPasswordPage());
            };
            forgetPasswordLabel.GestureRecognizers.Add(forgetPassword_tab);
            this.fromVal = fromVal;

          
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            startAnimation();

           

                if (PopupNavigation.Instance.PopupStack.Count > 0)
            {
                if (PopupNavigation.Instance.PopupStack[PopupNavigation.Instance.PopupStack.Count - 1].GetType() == typeof(ErrorWithClosePagePopup))
                {
                    await PopupNavigation.Instance.PopAllAsync();
                }
                else if (PopupNavigation.Instance.PopupStack[PopupNavigation.Instance.PopupStack.Count - 1].GetType() == typeof(SavedSuccessfullyPopup))
                {
                    await PopupNavigation.Instance.PopAllAsync();
                }
            }

        }

        private void startAnimation()
        {
            Device.StartTimer(TimeSpan.FromSeconds(2), () => {
                BackImage.FadeTo(0.5, 500);
                BackImage.FadeTo(1, 500);
                return true;
            });
        }

        private async void LoginButton_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(loginEmailAddress.Text))
            {
                string emailen = loginEmailAddress.Text.Replace(" ", "");
                loginEmailAddress.Text = emailen;
            }

            if (string.IsNullOrEmpty(loginEmailAddress.Text))
            {
                //errorLabel.Text = "Please enter a email address";
                //errorLabel.IsVisible = true;
            }
            else if (!new EmailAddressAttribute().IsValid(loginEmailAddress.Text))
            {
                //errorLabel.Text = "Email is not in a valid format.Please try again";
                //errorLabel.IsVisible = true;
            }
            else if (string.IsNullOrEmpty(loginPassword.Text))
            {
                //errorLabel.Text = "Please enter a valid password";
                //errorLabel.IsVisible = true;
            }
            else
            {
                bool busy = false;
                if (!busy)
                {
                    try
                    {
                        busy = true;
                        LoginButton.IsVisible = false;
                        loginSpinnerFrame.IsVisible = true;
                        loginSpinner.IsRunning = true;
                        await Task.Run(() =>
                        {
                            GetClientSecretTokenRequest getClientSecretTokenRequest = new GetClientSecretTokenRequest();
                            getClientSecretTokenRequest.ClientId = Constants.ClientId;
                            ApiController apiController = new ApiController();
                            GetClientSecretTokenResponse clientSecretTokenResponse = apiController.GetClientSecretToken(getClientSecretTokenRequest);

                            GetAccessTokenRequest tokenRequest = new GetAccessTokenRequest();
                            tokenRequest.client_id = clientSecretTokenResponse.apiConsumerId;
                            tokenRequest.client_secret = clientSecretTokenResponse.apiConsumerSecret;
                            tokenRequest.grant_type = "client_credentials";
                            ApiToken apiToken = apiController.GetAccessToken(tokenRequest);
                            token = apiToken.access_token;

                            CustomerLogin loginCustomer = new CustomerLogin();
                            loginCustomer.email = loginEmailAddress.Text;
                            loginCustomer.Password = loginPassword.Text;
                            loginCustomer.clientId = Constants.ClientId;
                            loginCustomer.ClientTime = DateTime.Now;


                            LoginController loginController = new LoginController();
                            cutomerAuthContext = loginController.CheckLogin(loginCustomer, token);
                            if (cutomerAuthContext.CustomerId > 0)
                            {
                                App.Current.Properties["IsOnborded"] = true;
                                portalDetailsMobileRequest.customerId = cutomerAuthContext.CustomerId;
                                updateConstantCustomerDetails(portalDetailsMobileRequest);

                            }
                        });
                    }
                    finally
                    {
                        busy = false;
                        loginSpinner.IsRunning = false;
                        loginSpinnerFrame.IsVisible = false;

                        LoginButton.IsVisible = true;
                        if (cutomerAuthContext.CustomerId > 0)
                        {
                            Constants.IsRegisteredandNotLogin = false;
                            Constants.cutomerAuthContext = cutomerAuthContext;
                            if (App.Current.Properties.ContainsKey("currentToken"))
                            {
                                App.Current.Properties["currentToken"] = token;
                            }
                            else
                            {
                                App.Current.Properties.Add("currentToken", token);
                            }





                            if (cutomerAuthContext.IsEmailConfirmed)
                            //if (!cutomerAuthContext.IsEmailConfirmed)
                            {
                                if (App.Current.Properties.ContainsKey("CustomerId"))
                                {
                                    App.Current.Properties["CustomerId"] = cutomerAuthContext.CustomerId;
                                }
                                else
                                {
                                    App.Current.Properties.Add("CustomerId", cutomerAuthContext.CustomerId);
                                }

                                Type type = typeof(WelcomPage);

                                //if (Navigation.NavigationStack[Navigation.NavigationStack.Count - 2].GetType() == type)
                                //{
                                //    await Navigation.PushAsync(new HomePage());
                                //}
                                //else if (Navigation.NavigationStack[Navigation.NavigationStack.Count - 2].GetType() == typeof(OtherInformationPage))
                                //{
                                //    if (Navigation.NavigationStack[Navigation.NavigationStack.Count - 3].GetType() == type)
                                //    {
                                //        await Navigation.PushAsync(new HomePage());
                                //    }
                                //    else
                                //    {
                                //        Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
                                //        await Navigation.PushAsync(new HomePage());
                                //    }

                                //}

                                //else
                                //{
                                //if (Navigation.NavigationStack.Count < 3)
                                //{
                                //    await Navigation.PushAsync(new HomePage());
                                //}
                                //else
                                //{
                                //    await Navigation.PopAsync();
                                //}
                                if (fromVal == 1)
                                {
                                    await Navigation.PopModalAsync();
                                }
                                else
                                {
                                    await Navigation.PushAsync(new HomePage());
                                }

                                //}
                            }
                            else
                            {
                                if (fromVal == 1)
                                {
                                    await Navigation.PushModalAsync(new ConfirmEmailRequest(cutomerAuthContext.CustomerId, emailConfirmationType.LogIn, fromVal));
                                }
                                else
                                {
                                    await Navigation.PushModalAsync(new ConfirmEmailRequest(cutomerAuthContext.CustomerId,emailConfirmationType.LogIn));
                                }

                                //gobackAsync();
                            }


                        }
                        else
                        {
                            await PopupNavigation.Instance.PushAsync(new Error_popup("Login failed. Please try again."));
                            loginPassword.Text = null;
                            //errorLabel.IsVisible = false;
                        }

                    }
                }
            }
        }

        public async void gobackAsync()
        {

            int totalModals = Application.Current.MainPage.Navigation.ModalStack.Count;

            //i set currModal = 1 here to back to page 2, If you wan to go back to 3, you can set currModal = 2, etc...
            // remember to add flase in PopModalAsync to aviod the animation.

            for (int currModal = 1; currModal < totalModals; currModal++)
            {
                await Application.Current.MainPage.Navigation.PopModalAsync(false);
            }
        }

        private void updateConstantCustomerDetails(GetCustomerPortalDetailsMobileRequest portalDetailsMobileRequest)
        {
            GetCustomerPortalDetailsMobileResponse response = new GetCustomerPortalDetailsMobileResponse();
            CustomerController customoerController = new CustomerController();
            try
            {
                response = customoerController.getCustomerDetailsWithProfilePic(portalDetailsMobileRequest, token);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (response.customerReview != null)
            {
                Constants.customerDetails = response.customerReview;
            }
        }

        private void SignUpBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new RegisterPage());
        }

        //protected override bool OnBackButtonPressed()
        //{
        //    if (PopupNavigation.Instance.PopupStack.Count > 0) { return true; }
        //    if (Navigation.NavigationStack.Count > 1)
        //    {
        //        if (Navigation.NavigationStack[Navigation.NavigationStack.Count - 2].GetType() == typeof(WelcomPage))
        //        {
        //            return false;
        //        }
        //        else if (Navigation.NavigationStack[Navigation.NavigationStack.Count - 2].GetType() == typeof(SummaryOfChargesPage))
        //        {
        //            return false;
        //        }
        //        else
        //        {
        //            int c = Navigation.NavigationStack.Count;
        //            for (var counter = 1; counter < c - 1; counter++)
        //            {
        //                Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
        //            }
        //            return false;
        //        }
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}



        private void googleBtn_Clicked(object sender, EventArgs e)
        {
            string clientId = null;
            string redirectUri = null;

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    clientId = GoogleConstants.iOSClientId;
                    redirectUri = GoogleConstants.iOSRedirectUrl;
                    break;

                case Device.Android:
                    clientId = GoogleConstants.AndroidClientId;
                    redirectUri = GoogleConstants.AndroidRedirectUrl;
                    break;
            }

            account = store.FindAccountsForService(GoogleConstants.AppName).FirstOrDefault();

            var authenticator = new OAuth2Authenticator(
                clientId,
                null,
                GoogleConstants.Scope,
                new Uri(GoogleConstants.AuthorizeUrl),
                new Uri(redirectUri),
                new Uri(GoogleConstants.AccessTokenUrl),
                null,
                true);

            authenticator.Completed += OnAuthCompleted;
            authenticator.Error += OnAuthError;

            AuthenticationState.Authenticator = authenticator;

            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
            presenter.Login(authenticator);
        }

        private async void OnAuthCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            var authenticator = sender as OAuth2Authenticator;
            if (authenticator != null)
            {
                authenticator.Completed -= OnAuthCompleted;
                authenticator.Error -= OnAuthError;
            }

            User user = null;
            if (e.IsAuthenticated)
            {
                // If the user is authenticated, request their basic user data from Google
                // UserInfoUrl = https://www.googleapis.com/oauth2/v2/userinfo
                var request = new OAuth2Request("GET", new Uri(GoogleConstants.UserInfoUrl), null, e.Account);
                var response = await request.GetResponseAsync();
                if (response != null)
                {
                    // Deserialize the data and store it in the account store
                    // The users email address will be used to identify data in SimpleDB
                    string userJson = await response.GetResponseTextAsync();
                    user = JsonConvert.DeserializeObject<User>(userJson);
                }

                if (user != null)
                {
                    App.Current.MainPage = new NavigationPage(new GoogleRegisterPage(user));

                }

                //await store.SaveAsync(account = e.Account, AppConstant.Constants.AppName);
                //await DisplayAlert("Email address", user.Email, "OK");
            }
        }

        private void OnAuthError(object sender, AuthenticatorErrorEventArgs e)
        {
            var authenticator = sender as OAuth2Authenticator;
            if (authenticator != null)
            {
                authenticator.Completed -= OnAuthCompleted;
                authenticator.Error -= OnAuthError;
            }

            Debug.WriteLine("Authentication error: " + e.Message);
        }

        private async void fbBtn_Clicked(object sender, EventArgs e)
        {
            FacebookResponse<string> response = await CrossFacebookClient.Current.RequestUserDataAsync(new string[] { "email", "first_name", "gender", "last_name", "birthday" }, new string[] { "email", "user_birthday" });
        }

        private void passwordvisible_Clicked(object sender, EventArgs e)
        {
            if (loginPassword.IsPassword == true)
            {
                loginPassword.IsPassword = false;
            }
            else
            {
                loginPassword.IsPassword = true;
            }
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            //Device.StartTimer(TimeSpan.FromSeconds(2), () => {
            //    BackImage.FadeTo(0.5, 500);
            //    BackImage.FadeTo(1, 500);
            //    return true;
            //});
            var parentAnimation = new Animation();
            var animation = new Animation(v => BackImage.Scale = v, 1, 1.2,Easing.SpringOut);
            var animation2 = new Animation(v => BackImage.Scale = v, 1.2, 1, Easing.SpringIn);

            parentAnimation.Add(0, 0.5, animation);
            parentAnimation.Add(0.5, 1, animation2);

            parentAnimation.Commit(this, "ScaleIt", length: 6000, easing: Easing.Linear,
                finished: (v, c) => BackImage.Scale=1, repeat: () => true);
        }
    }
}