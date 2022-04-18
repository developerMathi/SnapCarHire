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
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SnapCarHire.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Productor : ContentPage
    {

        private ApiToken apiToken;
        private GetClientDetailsForMobileResponse getClientDetailsForMobile;
        string _token;
        bool IsUpdatesAvailable;
        string currentVersion;

        public Productor()
        {
            InitializeComponent();
            IsUpdatesAvailable = false;

            if (!App.Current.Properties.ContainsKey("CustomerId"))
            {
                App.Current.Properties.Add("CustomerId", 0);
            }

            if (!App.Current.Properties.ContainsKey("InquiryID"))
            {
                App.Current.Properties.Add("InquiryID", 0);
            }

            if (!App.Current.Properties.ContainsKey("AppTheme"))
            {
                App.Current.Properties.Add("AppTheme", 0);
            }
            currentVersion = VersionTracking.CurrentVersion;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();            //getAndSaveClientSecrets();

            if ((int)App.Current.Properties["AppTheme"] == 1)
            {
                Application.Current.UserAppTheme = OSAppTheme.Light;
            }
            else if ((int)App.Current.Properties["AppTheme"] == 2)
            {
                Application.Current.UserAppTheme = OSAppTheme.Dark;
            }
            else
            {
                Application.Current.UserAppTheme = OSAppTheme.Unspecified;
            }




            bool busy = false;
            if (!busy)
            {
                busy = true;
                await Task.WhenAny<bool>
                    (
                    logoImage.TranslateTo(0, -100, 200)
                    );// Move image up
                loadingIndicatior.IsVisible = true;
                try
                {
                    GetClientSecretTokenRequest getClientSecretTokenRequest = new GetClientSecretTokenRequest();
                    getClientSecretTokenRequest.ClientId = Constants.ClientId;
                    ApiController apiController = new ApiController();
                    GetClientSecretTokenResponse clientSecretTokenResponse = null;
                    try
                    {
                        await Task.Run(() =>
                        {
                            clientSecretTokenResponse = apiController.GetClientSecretToken(getClientSecretTokenRequest);
                        });
                    }
                    catch (Exception ex)
                    {
                        await PopupNavigation.Instance.PushAsync(new Error_popup(ex.Message));
                    }
                    if (clientSecretTokenResponse != null)
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
                            apiToken = null;
                        }
                        if (apiToken != null)
                        {
                            _token = apiToken.access_token;
                            CommonController commonController = new CommonController();

                            try
                            {
                                await Task.Run(() =>
                                {
                                    getClientDetailsForMobile = commonController.GetClientDetailsForMobile(_token);
                                });
                            }
                            catch (Exception ex)
                            {
                                await PopupNavigation.Instance.PushAsync(new Error_popup(ex.Message));
                            }


                            if (getClientDetailsForMobile != null)
                            {
                                if (getClientDetailsForMobile.admin != null)
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
                }
                finally
                {
                    busy = false;
                    loadingIndicatior.IsRunning = false;

                }

            }

            if (_token != null)
            {
                //IsUpdatesAvailable = checkForApplicationUpdates(currentVersion);
                if (!IsUpdatesAvailable)
                {
                    if ((int)App.Current.Properties["CustomerId"] == 0)
                    {

                        if ((bool)App.Current.Properties["IsOnborded"])
                        {
                            await Navigation.PushAsync(new LoginPage());
                        }
                        else
                        {
                            await Navigation.PushAsync(new IntialLoading());
                        }
                            
                        //MainPage = new NavigationPage(new WelcomPage());
                        //MainPage = new NavigationPage(new enterConfirmationCodePage());
                    }
                    else
                    {
                        await Navigation.PushAsync(new HomePage());
                        //MainPage = new NavigationPage(new HomePage());
                    }
                }
                else
                {
                    //Want to retuen page that reqire the app to update
                    //await Navigation.PushAsync(new UpdateApplication());
                }
            }
            else
            {
                //Want to return No network page
                //await Navigation.PushAsync(new NoNetwork());
            }
        }

        private async void getAndSaveClientSecrets()
        {

        }
    }
}