using SnapCarHire.Utilties;
using SnapCarHireController;
using SnapCarHireModel.AccessModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SnapCarHire.Views
{
    public partial class HomePageMaster : ContentPage
    {
        public ListView ListView;
        GetCustomerPortalDetailsMobileRequest portalDetailsMobileRequest;
        GetCustomerPortalDetailsMobileResponse PortalDetailsMobileResponse;
        string token;
        int customerId;
        CustomerController customoerController;

        public HomePageMaster()
        {
            InitializeComponent();
            //Common.mMasterPage.Master = this;
            customoerController = new CustomerController();
            token = Application.Current.Properties["currentToken"].ToString();
            customerId = (int)Application.Current.Properties["CustomerId"];
            portalDetailsMobileRequest = new GetCustomerPortalDetailsMobileRequest();
            portalDetailsMobileRequest.customerId = customerId;
            PortalDetailsMobileResponse = null;

            BindingContext = new HomePageMasterViewModel();
            if(Constants.cutomerAuthContext!= null){
                welcomeText.Text = "Welcome " + Constants.cutomerAuthContext.FirstName;
            }
            ListView = MenuItemsListView;


            var homeTab = new TapGestureRecognizer();
            homeTab.Tapped += (s, e) =>
            {
                Navigation.PushAsync(new HomePageDetail());
            };
            //HomeBtn.GestureRecognizers.Add(homeTab);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (Constants.customerDetails != null)
            {
                welcomeText.Text = "Welcome " + Constants.customerDetails.FirstName;

                if (Constants.customerDetails.CustomerId != (int)Application.Current.Properties["CustomerId"])
                {
                    getCustomerRevieAndUpdateImage();
                }
                else
                {
                    if (Constants.customerDetails.CustomerImages.Count > 0)
                    {
                        if (Constants.customerDetails.CustomerImages[Constants.customerDetails.CustomerImages.Count - 1].Base64 != null)
                        {
                            byte[] Base64Stream = Convert.FromBase64String(Constants.customerDetails.CustomerImages[Constants.customerDetails.CustomerImages.Count - 1].Base64);
                            profileImage.Source = ImageSource.FromStream(() => new MemoryStream(Base64Stream));
                        }
                    }
                }
            }
            else
            {
                getCustomerRevieAndUpdateImage();
            }
        }

        private void getCustomerRevieAndUpdateImage()
        {
            PortalDetailsMobileResponse = getCustomerDetailsWithProfilePic(portalDetailsMobileRequest, token);

            if (PortalDetailsMobileResponse != null)
            {
                if (PortalDetailsMobileResponse.customerReview != null)
                {
                    Constants.customerDetails = PortalDetailsMobileResponse.customerReview;
                    welcomeText.Text = "Welcome " + Constants.customerDetails.FirstName;
                    if (PortalDetailsMobileResponse.customerReview.CustomerImages.Count > 0)
                    {
                        if (PortalDetailsMobileResponse.customerReview.CustomerImages[PortalDetailsMobileResponse.customerReview.CustomerImages.Count - 1].Base64 != null)
                        {
                            byte[] Base64Stream = Convert.FromBase64String(PortalDetailsMobileResponse.customerReview.CustomerImages[PortalDetailsMobileResponse.customerReview.CustomerImages.Count - 1].Base64);
                            profileImage.Source = ImageSource.FromStream(() => new MemoryStream(Base64Stream));
                        }
                    }
                }
            }

        }

        private GetCustomerPortalDetailsMobileResponse getCustomerDetailsWithProfilePic(GetCustomerPortalDetailsMobileRequest portalDetailsMobileRequest, string token)
        {
            GetCustomerPortalDetailsMobileResponse response = new GetCustomerPortalDetailsMobileResponse();
            try
            {
                response = customoerController.getCustomerDetailsWithProfilePic(portalDetailsMobileRequest, token);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        class HomePageMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<HomePageMasterMenuItem> MenuItems { get; set; }

            public HomePageMasterViewModel()
            {
                MenuItems = new ObservableCollection<HomePageMasterMenuItem>(new[]
                {

                    new HomePageMasterMenuItem { Id = 0,BgColor = Color.Transparent,IconSource=ImageSource.FromResource("SnapCarHire.Assets.iconWhiteDashboard.png"),  Title = "Dashboard" },
                    new HomePageMasterMenuItem { Id = 1,BgColor = Color.Transparent,IconSource=ImageSource.FromResource("SnapCarHire.Assets.iconWhiteCar.png"), Title = "Book Now" },
                    new HomePageMasterMenuItem { Id = 3,BgColor = Color.Transparent,IconSource=ImageSource.FromResource("SnapCarHire.Assets.iconWhiteBooking.png"), Title = "My Reservations" },
                    new HomePageMasterMenuItem { Id = 2,BgColor = Color.Transparent,IconSource=ImageSource.FromResource("SnapCarHire.Assets.iconWhiteRental.png"), Title = "My Rentals " },
                    new HomePageMasterMenuItem { Id = 4,BgColor = Color.Transparent,IconSource=ImageSource.FromResource("SnapCarHire.Assets.iconWhiteUser.png"), Title = "My Profile" },
                    new HomePageMasterMenuItem { Id = 6,BgColor = Color.Transparent,IconSource=ImageSource.FromResource("SnapCarHire.Assets.iconWhiteSetting.png"), Title = "Settings" },
                    new HomePageMasterMenuItem { Id = 5,BgColor = Color.Transparent,IconSource=ImageSource.FromResource("SnapCarHire.Assets.iconWhiteLogout.png"), Title = "Log out" },
                   // new HomePageMasterMenuItem { Id = 2, Title = "Upcoming reservation " },
                   // new HomePageMasterMenuItem { Id = 3, Title = "My Rentals" },
                   //new HomePageMasterMenuItem { Id = 3,BgColor = Color.Transparent,IconSource=ImageSource.FromResource("SnapCarHire.Assets.iconWhiteHelp.png"), Title = "Help" },

                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }

        private void MenuItemsListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item =MenuItemsListView.SelectedItem  as HomePageMasterMenuItem;
            if (item == null)
                return;
            else if (item.Id == 0)
            {
                Navigation.PushModalAsync(new HomePage());
            }
            else if (item.Id == 1)
            {
                Navigation.PushModalAsync(new BookNow());

            }
            else if (item.Id == 2)
            {
                Navigation.PushModalAsync(new MyRentals());
            }
            else if (item.Id == 3)
            {
                Navigation.PushModalAsync(new UpcomingReservations());
            }
            else if (item.Id == 4)
            {
                Navigation.PushModalAsync(new MyProfile());
            }
            else if (item.Id == 6)
            {
                Navigation.PushModalAsync(new SettingPage());
            }
            else if (item.Id == 5)
            {
                App.Current.Properties["CustomerId"] = 0;
                App.Current.Properties["InquiryID"] = 0;
                Constants.cutomerAuthContext = null;
                var pageOne = new LoginPage();
                NavigationPage.SetHasNavigationBar(pageOne, false);
                NavigationPage mypage = new NavigationPage(pageOne);
                Application.Current.MainPage = mypage;
            }
        }
    }
}