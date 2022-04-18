using SnapCarHire.Popups;
using SnapCarHireController;
using SnapCarHireModel;
using SnapCarHireModel.AccessModels;
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
    public partial class WelcomPage : ContentPage
    {
        GetClientDetailsForMobileResponse getClientDetailsForMobile;
        public WelcomPage()
        {
            InitializeComponent();
            var assembly = typeof(WelcomPage);
            getClientDetailsForMobile = null;
            logoImage.Source = ImageSource.FromResource("SnapCarHire.Assets.logo_high_resolution_white-1.png", assembly);
            if (!App.Current.Properties.ContainsKey("CustomerId"))
            {
                App.Current.Properties.Add("CustomerId", 0);
            }

            var loginTap = new TapGestureRecognizer();
            loginTap.Tapped += async (s, e) =>
            {
                if ((int)App.Current.Properties["CustomerId"] == 0)
                {
                    await Navigation.PushAsync(new LoginPage());

                }
                else
                {
                    IsBusy = false;
                    if (!IsBusy)
                    {

                        
                        IsBusy = true;
                        await HomeBtn.FadeTo(0,100, Easing.SinInOut);
                        //await loginBtnFrame.FadeTo(0, 100, Easing.SinInOut);
                        try
                        {
                            if (Navigation.NavigationStack[Navigation.NavigationStack.Count - 1].GetType() != typeof(HomePage))
                            {
                                await Navigation.PushAsync(new HomePage());
                            }
                               
                        }
                        finally
                        {
                            IsBusy = false;
                            await HomeBtn.FadeTo(1, 1000);
                            //await loginBtnFrame.FadeTo(1, 1000);

                        }
                    } 
                }
            };
            //loginBtnFrame.GestureRecognizers.Add(loginTap);
            HomeBtn.GestureRecognizers.Add(loginTap);

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
            }


            int c = Navigation.NavigationStack.Count;
            if (c > 0)
            {
                for (var counter = 1; counter < c-1; counter++)
                {
                    Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
                }
            }
        }

        private async void BooknowBtn_Clicked(object sender, EventArgs e)
        {
            ApiToken apiToken = null;
            bool busy = false;
            if (!busy)
            {
                try
                {

                    busy = true;
                    BooknowBtn.IsVisible = false;
                    bookNowLoader.IsVisible = true;
                    bookNowSpinner.IsRunning = true;
                    
                    await Task.Run(async () =>
                    {
                        GetClientSecretTokenRequest getClientSecretTokenRequest = new GetClientSecretTokenRequest();
                        getClientSecretTokenRequest.ClientId = Constants.ClientId;
                        ApiController apiController = new ApiController();
                        GetClientSecretTokenResponse clientSecretTokenResponse = null;
                        try
                        {
                            clientSecretTokenResponse=apiController.GetClientSecretToken(getClientSecretTokenRequest);
                        }
                        catch (Exception ex)
                        {
                            await PopupNavigation.Instance.PushAsync(new Error_popup(ex.Message));
                        }
                        if(clientSecretTokenResponse!= null)
                        {
                            GetAccessTokenRequest tokenRequest = new GetAccessTokenRequest();
                            tokenRequest.client_id = clientSecretTokenResponse.apiConsumerId;
                            tokenRequest.client_secret = clientSecretTokenResponse.apiConsumerSecret;
                            tokenRequest.grant_type = "client_credentials";

                            try
                            {
                                apiToken = apiController.GetAccessToken(tokenRequest);
                            }
                            catch (Exception ex)
                            {
                                await PopupNavigation.Instance.PushAsync(new Error_popup(ex.Message));
                            }
                            if (apiToken != null)
                            {
                                string _token = apiToken.access_token;
                                CommonController commonController = new CommonController();

                                try
                                {
                                    getClientDetailsForMobile = commonController.GetClientDetailsForMobile(_token);
                                }
                                catch (Exception ex)
                                {
                                    await PopupNavigation.Instance.PushAsync(new Error_popup(ex.Message));
                                }


                                if(getClientDetailsForMobile != null)
                                {
                                    if(getClientDetailsForMobile.admin!= null)
                                    {
                                        Constants.admin = getClientDetailsForMobile.admin;
                                    }
                                }

                                if (App.Current.Properties.ContainsKey("currentToken"))
                                {
                                    App.Current.Properties["currentToken"] = _token;
                                }
                                else
                                {
                                    App.Current.Properties.Add("currentToken", _token);
                                }
                            }
                                

                        }


                    });
                }
                
                finally
                {
                    if (apiToken!= null) {
                        ReservationView reservation = new ReservationView();
                        //await Navigation.PushAsync(new VehicleDetailPage(reservation));
                    }
                    
                    busy = false;
                    BooknowBtn.IsVisible = true;
                    bookNowLoader.IsVisible = false;
                    bookNowSpinner.IsRunning = false;
                }
            }
        }

        protected override bool OnBackButtonPressed()
        {
            if (PopupNavigation.Instance.PopupStack.Count > 0) { return true; }
            else if (Navigation.NavigationStack.Count > 2)
            {
                return true;
            }
            // Always return true because this method is not asynchronous.
            // We must handle the action ourselves: see above.
            return false;
        }

        private async void HomeBtn_Clicked(object sender, EventArgs e)
        {
            try
            {
                if ((int)App.Current.Properties["CustomerId"] == 0)
                {
                    await Navigation.PushAsync(new LoginPage());

                }
                else
                {
                    IsBusy = false;
                    if (!IsBusy)
                    {

                        // HomeBtn.IsVisible = false;
                        //  LoginLoader.IsVisible = true;
                        // LoginSpinner.IsRunning = true;
                        IsBusy = true;

                        try
                        {
                            if (Navigation.NavigationStack[Navigation.NavigationStack.Count - 1].GetType() != typeof(HomePage))
                            {
                                await Navigation.PushAsync(new HomePage());
                            }

                        }
                        finally
                        {
                            IsBusy = false;

                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                //  HomeBtn.IsVisible = false;
                //  LoginLoader.IsVisible = true;
                // LoginSpinner.IsRunning = true;
            }
        }

        private void btnSignup_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RegisterPage());
        }



        private void forgetPasswordBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ForgetPasswordPage());
        }

        //private void HomeBtn_Clicked(object sender, EventArgs e)
        //{
        //    if ((int)App.Current.Properties["CustomerId"] == 0)
        //    {
        //        PopupNavigation.Instance.PushAsync(new AskForLogin());

        //    }
        //    else
        //    {
        //        Navigation.PushAsync(new HomePage());
        //    }
        //}
    }
}