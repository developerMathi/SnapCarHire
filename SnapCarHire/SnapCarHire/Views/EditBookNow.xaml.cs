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
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditBookNow : ContentPage
    {
        private string _token;
        //List<LocationModel> locationModels;
        GetAllLocationForClientIDForMobileResponse locationResponse;
        List<string> locationNames;
        ReservationView reservationView;
        private ReservationViewModel reservationData;


       

        public EditBookNow(ReservationViewModel reservationData)
        {
            InitializeComponent();
            locationResponse = null;
            reservationView = new ReservationView();
            this.reservationData = reservationData;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var assembly = typeof(BookNow);

            if (PopupNavigation.Instance.PopupStack.Count > 0)
            {
                if (PopupNavigation.Instance.PopupStack[PopupNavigation.Instance.PopupStack.Count - 1].GetType() == typeof(ErrorWithClosePagePopup))
                {
                    await PopupNavigation.Instance.PopAllAsync();
                }
            }


            _token = App.Current.Properties["currentToken"].ToString();

            if ((int)App.Current.Properties["CustomerId"] == 0)
            {
                loginIcon.IconImageSource = ImageSource.FromResource("SnapCarHire.Assets.LogInTool.png", assembly);
            }
            else
            {
                loginIcon.IconImageSource = ImageSource.FromResource("SnapCarHire.Assets.logOutTool.png", assembly);
            }
            GetAllLocationForClientIDForMobileRequest locationRequest = new GetAllLocationForClientIDForMobileRequest();
            locationRequest.ClientID = Constants.ClientId;


            bool busy = false;
            if (!busy)
            {
                try
                {
                    busy = true;
                    await PopupNavigation.Instance.PushAsync(new LoadingPopup("Creating book now page..."));

                    await Task.Run(async () =>
                    {
                        try
                        {

                            locationResponse = getAllLocationsByClientId(locationRequest, _token);
                        }
                        catch (Exception ex)
                        {
                            await PopupNavigation.Instance.PushAsync(new ErrorWithClosePagePopup(ex.Message));
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
                    else if (PopupNavigation.Instance.PopupStack.Count > 1)
                    {
                        if (PopupNavigation.Instance.PopupStack[PopupNavigation.Instance.PopupStack.Count - 1].GetType() != typeof(ErrorWithClosePagePopup))
                        {
                            await PopupNavigation.Instance.PopAllAsync();
                        }
                    }

                }
                if (locationResponse != null)
                {
                    locationNames = new List<string>();
                    foreach (LocationModel l in locationResponse.locationList)
                    {
                        locationNames.Add(l.LocationName);
                    }
                    pickUpLocation.ItemsSource = locationNames;
                    dropOffLocation.ItemsSource = locationNames;
                }
                else
                {
                    await Navigation.PopAsync();
                }


                pickUpDate.MinimumDate = DateTime.Now;
                pickUpDate.Date = DateTime.Now;
                pickUpTime.Time = new TimeSpan(9, 0, 0);
                dropOffDate.Date = DateTime.Now.AddDays(1);
                dropOffDate.MinimumDate = DateTime.Now;
                dropOffTime.Time = new TimeSpan(9, 0, 0);


                pickUpDate.Date = DateTime.Parse(reservationData.Reservationview.StartDateStr);
                dropOffDate.Date = DateTime.Parse(reservationData.Reservationview.EndDateStr);
                pickUpLocation.SelectedItem = reservationData.Reservationview.StartLocationName;
                dropOffLocation.SelectedItem = reservationData.Reservationview.EndLocationName;
                pickUpTime.Time = DateTime.Parse(reservationData.Reservationview.StartDateStr).TimeOfDay;
                dropOffTime.Time = DateTime.Parse(reservationData.Reservationview.EndDateStr).TimeOfDay;
            }
        }

        private GetAllLocationForClientIDForMobileResponse getAllLocationsByClientId(GetAllLocationForClientIDForMobileRequest locationRequest, string _token)
        {
            CommonController commoncontroller = new CommonController();
            GetAllLocationForClientIDForMobileResponse locations = null;
            try

            {
                locations = commoncontroller.getAllLocationsByClientId(locationRequest, _token);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return locations;
        }

        private int getLocationIdByName(string v)
        {
            int locationId = 0;
            foreach (LocationModel l in locationResponse.locationList)
            {
                if (l.LocationName == v)
                {
                    locationId = (int)l.LocationId;
                }
            }
            return locationId;
        }

        //public List<LocationModel> GetAllLocation()
        //{
        //    CommonController commoncontroller = new CommonController();
        //    List<LocationModel> locations = commoncontroller.GetAllLocation(_token);
        //    return locations;
        //}

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            var assembly = typeof(BookNow);
            if ((int)App.Current.Properties["CustomerId"] == 0)
            {

                loginIcon.IconImageSource = ImageSource.FromResource("SnapCarHire.Assets.logOutTool.png", assembly);
                await Navigation.PushAsync(new LoginPage());

            }
            else
            {
                bool logout = await DisplayAlert("Alert", "Are you sure want to logout", "Yes", "No");
                if (logout)
                {
                    App.Current.Properties["CustomerId"] = 0;
                    loginIcon.IconImageSource = ImageSource.FromResource("SnapCarHire.Assets.LogInTool.png", assembly);
                    await Navigation.PushAsync(new WelcomPage());
                }
            }

        }

        protected override bool OnBackButtonPressed()
        {
            if (PopupNavigation.Instance.PopupStack.Count > 0) { return true; }
            // Always return true because this method is not asynchronous.
            // We must handle the action ourselves: see above.
            return false;
        }

        private void PickUpDate_Unfocused(object sender, FocusEventArgs e)
        {
            if (pickUpDate.Date != null)
            {
                dropOffDate.Date = pickUpDate.Date.AddDays(1);
                dropOffDate.MinimumDate = pickUpDate.Date;
            }
        }

        private void PickUpDate_Unfocused_1(object sender, FocusEventArgs e)
        {

        }

        private void backBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void pickUpLocation_Unfocused(object sender, FocusEventArgs e)
        {
            if (pickUpLocation.SelectedIndex != -1)
            {
                dropOffLocation.SelectedIndex = pickUpLocation.SelectedIndex;
            }
        }

        private void bookNowNxtBtn_Clicked(object sender, EventArgs e)
        {
            DateTime pickupDateTime = pickUpDate.Date + pickUpTime.Time;
            DateTime DropOffDateTime = dropOffDate.Date + dropOffTime.Time;

            int result = DateTime.Compare(pickupDateTime, DropOffDateTime);
            if (pickupDateTime < DateTime.Now)
            {
                PopupNavigation.Instance.PushAsync(new Error_popup("Plese check your pick-up time."));
            }
            else if (result > 0 || result == 0)
            {
                PopupNavigation.Instance.PushAsync(new Error_popup("Invalid drop-off time"));
            }
            else if (pickUpLocation.SelectedIndex == -1)
            {
                PopupNavigation.Instance.PushAsync(new Error_popup("Please select a pick-up location"));
            }
            else if (dropOffLocation.SelectedIndex == -1)
            {
                PopupNavigation.Instance.PushAsync(new Error_popup("Please select a drop-off location"));
            }
            else
            {
                reservationView.ReserveId = reservationData.Reservationview.ReserveId;
                reservationView.StartLocationName = pickUpLocation.SelectedItem.ToString();
                reservationView.EndLocationName = dropOffLocation.SelectedItem.ToString();
                reservationView.StartLocationId = getLocationIdByName(pickUpLocation.SelectedItem.ToString());
                reservationView.EndLocationId = getLocationIdByName(dropOffLocation.SelectedItem.ToString());
                reservationView.StartDate = pickUpDate.Date + pickUpTime.Time;
                reservationView.EndDate = dropOffDate.Date + dropOffTime.Time;
                reservationView.StartDateStr = ((DateTime)reservationView.StartDate).ToString("MM/dd/yyyy hh:mm tt").Replace("-", "/");
                reservationView.EndDateStr = ((DateTime)reservationView.EndDate).ToString("MM/dd/yyyy hh:mm tt").Replace("-", "/");

                DateTime sDate = pickUpDate.Date + pickUpTime.Time;
                DateTime eDate = dropOffDate.Date + dropOffTime.Time;
                //reservationView.TotalDays =(int)((eDate - sDate).TotalDays);
                reservationView.ClientId = Constants.ClientId;
                Navigation.PushAsync(new EditVehicleDetail(reservationView,reservationData));
            }
        }
    }
}