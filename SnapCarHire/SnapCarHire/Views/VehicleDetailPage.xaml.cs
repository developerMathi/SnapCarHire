using MaxVonGrafKftMobileModel;
using SnapCarHire.Popups;
using SnapCarHireController;
using SnapCarHireModel;
using SnapCarHireModel.AccessModels;
using SnapCarHireModel.Constants;
using Newtonsoft.Json;
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
    public partial class VehicleDetailPage : ContentPage
    {
        string token;
        int customerId;
        public static VehicleFilterSearch VehicleFilter;




        List<VehicleTypeResult> vehicleResults;
        //      GetVehicleDetailsMobileListResponse vehicleResponse;
        private ReservationView reservationView;
        ReservationConfigurationVehicleSearch search;

        GetReservationConfigurationMobileRequest vehicleMobileRequest;
        GetReservationConfigurationResponse vehicleMobileResponse;
        List<VehicleViewByTypeForMobile> forlistViewItemSource;
        List<VehicleViewByTypeForMobile> forlistViewItemSourceWithFiter;

        VehicleFilterMenus vehicleFilterMenus;
        List<string> vehicletypeList;
        string type;



        public VehicleDetailPage()
        {

            InitializeComponent();
            this.reservationView = new ReservationView();
            search = new ReservationConfigurationVehicleSearch();
            vehicleMobileRequest = new GetReservationConfigurationMobileRequest();
            vehicleMobileResponse = null;
            search.ClientId = Constants.ClientId;
            //search.LocationId = (int)reservationView.StartLocationId;
            //search.CheckInLocationId = (int)reservationView.EndLocationId;
            search.IsOnline = true;
            //search.StartDate = (DateTime)reservationView.StartDate;
            //search.EndDate = (DateTime)reservationView.EndDate;

            search.StartDate = DateTime.Now;
            search.EndDate = DateTime.Now.AddDays(3);

            search.VehicleCategoryId = 0;
            search.VehicleMakeID = 0;
            search.ModelId = 0;
            search.NumberOfSeats = 0;
            search.RentalPeriod = 0;
            search.VehicleId = 0;
            //lblLocation.Text = reservationView.StartLocationName;
            //startDateLabel.Text = ((DateTime)reservationView.StartDate).ToString("ddd,MM/dd/yyyy");
            //endDateLabel.Text = ((DateTime)reservationView.EndDate).ToString("ddd,MM/dd/yyyy");
            //startTimeLabel.Text = ((DateTime)reservationView.StartDate).ToString("hh:mm tt");
            //endTimeLabel.Text = ((DateTime)reservationView.EndDate).ToString("hh:mm tt");
            vehicletypeList = new List<string>();
            VehicleFilter = new VehicleFilterSearch();


            vehicleMobileRequest.search = search;
            vehicleFilterMenus = new VehicleFilterMenus();
        }




        public VehicleDetailPage(ReservationView reservationView, string v)
        {

            InitializeComponent();
            this.reservationView = reservationView;
            search = new ReservationConfigurationVehicleSearch();
            vehicleMobileRequest = new GetReservationConfigurationMobileRequest();
            vehicleMobileResponse = null;
            search.ClientId = Constants.ClientId;
            search.LocationId = (int)reservationView.StartLocationId;
            search.CheckInLocationId = (int)reservationView.EndLocationId;
            search.IsOnline = true;
            search.StartDate = (DateTime)reservationView.StartDate;
            search.EndDate = (DateTime)reservationView.EndDate;



            search.VehicleCategoryId = 0;
            search.VehicleMakeID = 0;
            search.ModelId = 0;
            search.NumberOfSeats = 0;
            search.RentalPeriod = 0;
            search.VehicleId = 0;
            lblLocation.Text = reservationView.StartLocationName;
            startDateLabel.Text = ((DateTime)reservationView.StartDate).ToString("MM/dd/yyyy");
            endDateLabel.Text = ((DateTime)reservationView.EndDate).ToString("MM/dd/yyyy");
            startTimeLabel.Text = ((DateTime)reservationView.StartDate).ToString("hh:mm tt");
            endTimeLabel.Text = ((DateTime)reservationView.EndDate).ToString("hh:mm tt");
            vehicletypeList = new List<string>();
            VehicleFilter = new VehicleFilterSearch();

            vehicleFilterMenus = new VehicleFilterMenus();
            vehicleMobileRequest.search = search;
            type = v;
        }

        protected override async void OnAppearing()
        {
            customerId = (int)App.Current.Properties["CustomerId"];
            base.OnAppearing();
            var assembly = typeof(BookNow);

            token = App.Current.Properties["currentToken"].ToString();
            customerId = (int)App.Current.Properties["CustomerId"];
            forlistViewItemSource = new List<VehicleViewByTypeForMobile>();

            MessagingCenter.Subscribe<FilterPopup>(this, "FilterUpdated", sender =>
            {
                refreshVehicleList();

            });




            if (PopupNavigation.Instance.PopupStack.Count > 0)
            {
                if (PopupNavigation.Instance.PopupStack[PopupNavigation.Instance.PopupStack.Count - 1].GetType() == typeof(ErrorWithClosePagePopup))
                {
                    await PopupNavigation.Instance.PopAllAsync();
                }
            }


            bool busy = false;
            if (!busy)
            {
                try
                {
                    busy = true;
                    await PopupNavigation.Instance.PushAsync(new LoadingPopup("Getting vehicles informations..."));

                    await Task.Run(() =>
                    {

                        try
                        {
                            vehicleMobileResponse = getVehicleTypesMobileNew(vehicleMobileRequest, token);
                            //vehicleResults = getVehicleTypes(token);
                            //vehicleResponse= getVehicleTypesMobile(token);


                        }
                        catch (Exception ex)
                        {
                            PopupNavigation.Instance.PushAsync(new ErrorWithClosePagePopup(ex.Message));
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
                }
                if (vehicleMobileResponse != null)
                {
                    if (vehicleMobileResponse.message.ErrorCode == "200")
                    {
                        if (vehicleMobileResponse.listVehicle.Count > 0)
                        {
                            vehicleFilterMenus.vehicleTypes = new List<string>();
                            vehicleFilterMenus.vehicleMakes = new List<string>();
                            vehicleFilterMenus.vehicleModels = new List<string>();
                            vehicleFilterMenus.VehicleYear = new List<int>();
                            vehicleFilterMenus.NumberOfseats = new List<int>();
                            vehicleFilterMenus.Colors = new List<string>();

                            List<int> vehicleTypeIds = new List<int>();

                            foreach (ReservationVehicleSearchViewModel rvsv in vehicleMobileResponse.listVehicle)
                            {
                                if (rvsv.VehicleStatusId == (int)VehicleStatus.Available)
                                {
                                    if (type==null || (type != null && type== "Yacht" && rvsv.VehicleType== "Yacht") ||(type != null && type != "Yacht" && rvsv.VehicleType != "Yacht")) {
                                        vehicleTypeIds.Add(rvsv.VehicleTypeId);
                                        VehicleViewByTypeForMobile typeForMobile = new VehicleViewByTypeForMobile();
                                        typeForMobile.VehicleTypeId = rvsv.VehicleTypeId;
                                        typeForMobile.VehicleType = rvsv.VehicleType;
                                        typeForMobile.vehicleMakeName = rvsv.VehicleMakeName;
                                        typeForMobile.vehicleModelName = rvsv.ModelName;
                                        typeForMobile.vehicleYear = (int)rvsv.Year;
                                        typeForMobile.Transmission = rvsv.Transmission;
                                        typeForMobile.Seats = rvsv.Seats;
                                        typeForMobile.NoOfLuggage = rvsv.Baggages;
                                        typeForMobile.DailyRate = decimal.Round(((decimal)rvsv.RateDetail.DailyRate + calculateMantatryCharges(rvsv.mantatoryMiscChargeDetails, 1)), 2);
                                        typeForMobile.DailyRateWithInsurance = decimal.Round(((decimal)rvsv.RateDetail.DailyRate + calculateMantatryChargesWithInsurance(rvsv.mantatoryMiscChargeDetails, 1)), 2);
                                        typeForMobile.VehicleTypeImageUrl = rvsv.VehicleTypeImage;
                                        typeForMobile.RateDetail = rvsv.RateDetail;
                                        typeForMobile.MileagePerDay = rvsv.MileagePerDay;
                                        typeForMobile.doors = rvsv.Doors;
                                        typeForMobile.IsVehicleAvailableDescription = rvsv.IsVehicleAvailableDescription;
                                        typeForMobile.HtmlContent = rvsv.HtmlContent;
                                        typeForMobile.SharableLink = rvsv.SharableLink;
                                        typeForMobile.VehicleImageUrl = rvsv.VehicleImageUrl;
                                        typeForMobile.mantatoryMiscChargeDetails = rvsv.mantatoryMiscChargeDetails;
                                        typeForMobile.mantatoryMiscChargeNonTaxableTotalForOneDay = rvsv.mantatoryMiscChargeNonTaxableTotalForOneDay;
                                        typeForMobile.mantatoryMiscChargeTotalForOneDay = rvsv.mantatoryMiscChargeTotalForOneDay;
                                        typeForMobile.color = rvsv.Color;


                                        typeForMobile.sample = rvsv.Sample;
                                        typeForMobile.locationIdList = new List<int>();
                                        if (!typeForMobile.locationIdList.Contains(rvsv.LocationId))
                                        {
                                            typeForMobile.locationIdList.Add(rvsv.LocationId);
                                        }
                                        typeForMobile.vehicleId = rvsv.vehicleId;
                                        typeForMobile.vehicleName = rvsv.VehicleMakeName + " " + rvsv.ModelName;

                                        forlistViewItemSource.Add(typeForMobile);
                                        if (!vehicleFilterMenus.vehicleTypes.Contains(rvsv.VehicleType))
                                        {
                                            vehicleFilterMenus.vehicleTypes.Add(rvsv.VehicleType);
                                        }
                                        if (!vehicleFilterMenus.vehicleMakes.Contains(rvsv.VehicleMakeName))
                                        {
                                            vehicleFilterMenus.vehicleMakes.Add(rvsv.VehicleMakeName);
                                        }
                                        if (!vehicleFilterMenus.vehicleModels.Contains(rvsv.ModelName))
                                        {
                                            vehicleFilterMenus.vehicleModels.Add(rvsv.ModelName);
                                        }
                                        if (!vehicleFilterMenus.VehicleYear.Contains((int)rvsv.Year))
                                        {
                                            vehicleFilterMenus.VehicleYear.Add((int)rvsv.Year);
                                        }
                                        if (!vehicleFilterMenus.NumberOfseats.Contains(rvsv.Seats))
                                        {
                                            vehicleFilterMenus.NumberOfseats.Add(rvsv.Seats);
                                        }
                                        if (!vehicleFilterMenus.Colors.Contains(rvsv.Color.ToLower().Trim()))
                                        {
                                            vehicleFilterMenus.Colors.Add(rvsv.Color.ToLower().Trim());
                                        }
                                    }
                                   
                                }
                                //else
                                //{

                                //    foreach (VehicleViewByTypeForMobile listIntype in forlistViewItemSource)
                                //    {
                                //        if (rvsv.VehicleTypeId == listIntype.VehicleTypeId)
                                //        {
                                //            if (rvsv.Transmission != listIntype.Transmission)
                                //            {
                                //                listIntype.Transmission = "Auto, Manual";
                                //            }
                                //        }

                                //        if (!listIntype.locationIdList.Contains(rvsv.LocationId))
                                //        {
                                //            listIntype.locationIdList.Add(rvsv.LocationId);
                                //        }
                                //    }

                                //}

                            }
                            if (forlistViewItemSource.Count > 0)
                            {
                                vehicleDetailList.ItemsSource = forlistViewItemSource;
                                vehicleDetailList.HeightRequest = (forlistViewItemSource.Count * 270);
                                vehicleDetailList.IsVisible = true;

                            }
                            else
                            {
                                vehicleDetailList.IsVisible = false;
                                //noVehicleLabel.IsVisible = true;
                                // buttonGrid.IsVisible = true;
                                noVehicleLabel.IsVisible = true;
                                noVehicleLabelImage.IsVisible = true;
                                //filterbuttonGrid.IsVisible = false;

                            }

                        }

                        else
                        {
                            vehicleDetailList.IsVisible = false;
                            //noVehicleLabel.IsVisible = true;
                            // buttonGrid.IsVisible = true;
                            noVehicleLabel.IsVisible = true;
                            noVehicleLabelImage.IsVisible = true;
                            //filterbuttonGrid.IsVisible = false;

                        }

                    }
                    else
                    {
                        await PopupNavigation.Instance.PushAsync(new ErrorWithClosePagePopup(vehicleMobileResponse.message.ErrorMessage));
                    }
                }
            }
        }

        private decimal calculateMantatryChargesWithInsurance(List<ReservationMiscDetails> mantatoryMiscChargeDetails, int v)
        {
            decimal sum = 0;
            if (mantatoryMiscChargeDetails != null)
            {
                foreach (ReservationMiscDetails rmd in mantatoryMiscChargeDetails)
                {
                    if (rmd.Name == "Jax Protection Plan" || rmd.Name == "Jax Maintenance Plan" || rmd.Name == "Insurance")
                    {
                        switch (rmd.CalculationType)
                        {
                            case "Fixed":
                                sum += (Math.Round(Convert.ToDecimal(rmd.MiscChargePerDay), 2));
                                break;

                            case "Perday":
                                sum += ((Math.Round(Convert.ToDecimal(rmd.MiscChargePerDay), 2)) * v);
                                break;
                        }
                    }
                }
            }
            return sum;
        }

        private void refreshVehicleList()
        {
            if (forlistViewItemSource != null)
            {
                if (forlistViewItemSource.Count > 0)
                {
                    forlistViewItemSourceWithFiter = new List<VehicleViewByTypeForMobile>();
                    forlistViewItemSourceWithFiter = forlistViewItemSource;
                    List<filterFormat> filterFormats = new List<filterFormat>();
                    if (VehicleFilter != null)
                    {
                        if (VehicleFilter.vehicleTypes != null)
                        {
                            if (VehicleFilter.vehicleTypes.Count > 0)
                            {
                                forlistViewItemSourceWithFiter = filterByVehicleType();
                            }
                            foreach (string s in VehicleFilter.vehicleTypes)
                            {
                                filterFormat filter = new filterFormat() { title = s, isAvailable = true, type = "VehicleType" };
                                filterFormats.Add(filter);
                            }
                        }
                        if (VehicleFilter.vehicleMakes != null)
                        {
                            if (VehicleFilter.vehicleMakes.Count > 0)
                            {
                                forlistViewItemSourceWithFiter = filterByVehicleMake();
                            }
                            foreach (string s in VehicleFilter.vehicleMakes)
                            {
                                filterFormat filter = new filterFormat() { title = s, isAvailable = true, type = "VehicleMake" };
                                filterFormats.Add(filter);
                            }
                        }
                        if (VehicleFilter.vehicleModels != null)
                        {
                            if (VehicleFilter.vehicleModels.Count > 0)
                            {
                                forlistViewItemSourceWithFiter = filterByVehicleModel();
                            }
                            foreach (string s in VehicleFilter.vehicleModels)
                            {
                                filterFormat filter = new filterFormat() { title = s, isAvailable = true, type = "VehicleModel" };
                                filterFormats.Add(filter);
                            }
                        }
                        if (VehicleFilter.VehicleYear != null)
                        {
                            if (VehicleFilter.VehicleYear.Count > 0)
                            {
                                forlistViewItemSourceWithFiter = filterByVehicleYear();
                            }
                            foreach (int s in VehicleFilter.VehicleYear)
                            {
                                filterFormat filter = new filterFormat() { title = s.ToString(), isAvailable = true, type = "VehicleYear" };
                                filterFormats.Add(filter);
                            }
                        }
                        if (VehicleFilter.Colors != null)
                        {
                            if (VehicleFilter.Colors.Count > 0)
                            {
                                forlistViewItemSourceWithFiter = filterByVehicleColor();
                            }
                            foreach (string s in VehicleFilter.Colors)
                            {
                                filterFormat filter = new filterFormat() { title = s, isAvailable = true, type = "VehicleColor" };
                                filterFormats.Add(filter);
                            }
                        }
                    }
                    //if (lthSortbtn.IsChecked && forlistViewItemSourceWithFiter != null)
                    //{
                    //    forlistViewItemSourceWithFiter = forlistViewItemSourceWithFiter.OrderBy(x => x.DailyRate).ToList();
                    //}
                    //if (htlSortbtn.IsChecked && forlistViewItemSourceWithFiter != null)
                    //{
                    //    forlistViewItemSourceWithFiter = forlistViewItemSourceWithFiter.OrderByDescending(x => x.DailyRate).ToList();
                    //}

                    //if (VehicleFilter.Price > 0)
                    //{
                    //    forlistViewItemSourceWithFiter = filterbyPrice();
                    //}
                    //if (VehicleFilter.MinPrice > 0)
                    //{
                    //    forlistViewItemSourceWithFiter = filterbyPriceMin();
                    //}
                    //if (VehicleFilter.VehicleType != null && forlistViewItemSourceWithFiter.Count > 0)
                    //{
                    //    forlistViewItemSourceWithFiter = filterByVehType();
                    //}

                    //if (VehicleFilter.seatsCount > 0 && forlistViewItemSourceWithFiter.Count > 0)
                    //{
                    //    forlistViewItemSourceWithFiter = filterbySeatCount();
                    //}
                    //if (VehicleFilter.buggageCount > 0 && forlistViewItemSourceWithFiter.Count > 0)
                    //{
                    //    forlistViewItemSourceWithFiter = filterbyBAgCount();
                    //}
                    //if (VehicleFilter.DoorsCount > 0 && forlistViewItemSourceWithFiter.Count > 0)
                    //{
                    //    forlistViewItemSourceWithFiter = filterbyDoorCount();
                    //}
                    //if (VehicleFilter.SortingOrder > -1 && forlistViewItemSourceWithFiter.Count > 0)
                    //{
                    //    forlistViewItemSourceWithFiter = filterbySortingOrder();
                    //}
                    if (filterFormats != null)
                    {
                        //filterList.ItemsSource = null;
                        //if (filterFormats.Count > 0)
                        //{
                        //    filterList.ItemsSource = filterFormats;
                        //}
                    }

                    if (forlistViewItemSourceWithFiter.Count > 0)
                    {
                        vehicleDetailList.ItemsSource = null;
                        vehicleDetailList.ItemsSource = forlistViewItemSourceWithFiter;
                        vehicleDetailList.HeightRequest = forlistViewItemSourceWithFiter.Count * 270;
                        noVehicleLabel.IsVisible = false;
                        noVehicleLabelImage.IsVisible = false;
                        //filterbuttonGrid.IsVisible = true;
                        vehicleDetailList.IsVisible = true;
                        //VehicleFilter = null;
                        //VehicleFilter = new VehicleFilterSearch();
                    }
                    else
                    {
                        vehicleDetailList.IsVisible = false;
                        //noVehicleLabel.IsVisible = true;
                        // buttonGrid.IsVisible = true;
                        noVehicleLabel.IsVisible = true;
                        noVehicleLabelImage.IsVisible = true;
                        //filterbuttonGrid.IsVisible = false;
                        //VehicleFilter = null;
                        //VehicleFilter = new VehicleFilterSearch();

                    }
                }
            }
        }

        private List<VehicleViewByTypeForMobile> filterByVehicleColor()
        {
            return forlistViewItemSourceWithFiter.Where(c => VehicleFilter.Colors.Contains(c.color.ToLower().Trim())).ToList();
        }

        private List<VehicleViewByTypeForMobile> filterByVehicleYear()
        {
            return forlistViewItemSourceWithFiter.Where(c => VehicleFilter.VehicleYear.Contains(c.vehicleYear)).ToList();
        }

        private List<VehicleViewByTypeForMobile> filterByVehicleModel()
        {
            return forlistViewItemSourceWithFiter.Where(c => VehicleFilter.vehicleModels.Contains(c.vehicleModelName)).ToList();
        }

        private List<VehicleViewByTypeForMobile> filterByVehicleMake()
        {
            return forlistViewItemSourceWithFiter.Where(c => VehicleFilter.vehicleMakes.Contains(c.vehicleMakeName)).ToList();
        }

        private List<VehicleViewByTypeForMobile> filterByVehicleType()
        {
            return forlistViewItemSourceWithFiter.Where(c => VehicleFilter.vehicleTypes.Contains(c.VehicleType)).ToList();
        }

        private decimal calculateMantatryCharges(List<ReservationMiscDetails> mantatoryMiscChargeDetails, int noOfDays)
        {
            decimal sum = 0;
            if (mantatoryMiscChargeDetails != null)
            {
                foreach (ReservationMiscDetails rmd in mantatoryMiscChargeDetails)
                {
                    if (rmd.Name == "Jax Protection Plan" || rmd.Name == "Jax Maintenance Plan")
                    {
                        switch (rmd.CalculationType)
                        {
                            case "Fixed":
                                sum += (Math.Round(Convert.ToDecimal(rmd.MiscChargePerDay), 2));
                                break;

                            case "Perday":
                                sum += ((Math.Round(Convert.ToDecimal(rmd.MiscChargePerDay), 2)) * noOfDays);
                                break;
                        }
                    }
                }
            }
            return sum;
        }

        //private List<VehicleViewByTypeForMobile> filterbyPriceMin()
        //{
        //    return forlistViewItemSourceWithFiter.Where(c => (int)c.DailyRate >= (int)VehicleFilter.MinPrice).ToList();
        //}

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<FilterPopup>(this, "FilterUpdated");
        }

        //private List<VehicleViewByTypeForMobile> filterbySortingOrder()
        //{
        //    if (VehicleFilter.SortingOrder == 0)
        //    {
        //        return forlistViewItemSourceWithFiter.OrderBy(x => x.DailyRate).ToList();
        //    }
        //    else if (VehicleFilter.SortingOrder == 1)
        //    {
        //        return forlistViewItemSourceWithFiter.OrderByDescending(x => x.DailyRate).ToList();
        //    }
        //    else
        //    {
        //        return forlistViewItemSourceWithFiter;
        //    }
        //}

        //private List<VehicleViewByTypeForMobile> filterbyDoorCount()
        //{
        //    return forlistViewItemSourceWithFiter.Where(c => int.Parse(c.doors) == VehicleFilter.DoorsCount).ToList();
        //}

        //private List<VehicleViewByTypeForMobile> filterbyBAgCount()
        //{
        //    return forlistViewItemSourceWithFiter.Where(c => c.NoOfLuggage == VehicleFilter.buggageCount).ToList();
        //}

        //private List<VehicleViewByTypeForMobile> filterByVehType()
        //{
        //    return forlistViewItemSourceWithFiter.Where(c => c.VehicleType.ToLower().Contains(VehicleFilter.VehicleType.ToLower())).ToList();
        //}

        //private List<VehicleViewByTypeForMobile> filterbyPrice()
        //{
        //    return forlistViewItemSourceWithFiter.Where(c => (int)c.DailyRate <= (int)VehicleFilter.Price).ToList();
        //}

        //private List<VehicleViewByTypeForMobile> filterbySeatCount()
        //{
        //    return forlistViewItemSourceWithFiter.Where(c => c.Seats == VehicleFilter.seatsCount).ToList();
        //}

        private GetReservationConfigurationResponse getVehicleTypesMobileNew(GetReservationConfigurationMobileRequest vehicleMobileRequest, string token)
        {
            GetReservationConfigurationResponse vehicleTypeResults = null;
            VehicleController vehicle = new VehicleController();
            try
            {
                vehicleTypeResults = vehicle.getVehicleTypesMobileNew(vehicleMobileRequest, token);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return vehicleTypeResults;
        }

        //private GetVehicleDetailsMobileListResponse getVehicleTypesMobile(string token)
        //{
        //    GetVehicleDetailsMobileListResponse vehicleTypeResults = null;
        //    VehicleController vehicle = new VehicleController();
        //    try
        //    {
        //        vehicleTypeResults = vehicle.getVehicleTypesMobile(token);

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return vehicleTypeResults;
        //}

        //private List<VehicleTypeResult> getVehicleTypes(string token)
        //{
        //    List<VehicleTypeResult> vehicleTypeResults = null;
        //    VehicleController vehicle = new VehicleController();
        //    try
        //    {
        //        vehicleTypeResults = vehicle.getVehicleTypes(token);

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return vehicleTypeResults;

        //}

        private void BacKBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }


        //private async void LoginIcon_Clicked(object sender, EventArgs e)
        //{
        //    var assembly = typeof(BookNow);
        //    if ((int)App.Current.Properties["CustomerId"] == 0)
        //    {

        //        loginIcon.IconImageSource = ImageSource.FromResource("MaxVonGrafKftMobile.Assets.logOutTool.png", assembly);
        //        await Navigation.PushAsync(new LoginPage());

        //    }
        //    else
        //    {
        //        bool logout = await DisplayAlert("Alert", "Are you sure want to logout", "Yes", "No");
        //        if (logout)
        //        {
        //            App.Current.Properties["CustomerId"] = 0;
        //            loginIcon.IconImageSource = ImageSource.FromResource("MaxVonGrafKftMobile.Assets.LogInTool.png", assembly);
        //            await Navigation.PushAsync(new WelcomPage());
        //        }
        //    }
        //}

        //private void VehicleDetailList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        //{
        //    VehicleViewByTypeForMobile selectedVehicle = vehicleDetailList.SelectedItem as VehicleViewByTypeForMobile;
        //    reservationView.VehicleTypeID = selectedVehicle.VehicleTypeId;
        //    reservationView.VehicleType = selectedVehicle.VehicleType;
        //    Rates rates = JsonConvert.DeserializeObject<Rates>(JsonConvert.SerializeObject(selectedVehicle.RateDetail));
        //    rates.RateId = selectedVehicle.RateDetail.RateID;
        //    List<Rates> rateDewtails = new List<Rates>();
        //    rates.StartDateStr = reservationView.StartDateStr;
        //    rates.EndDateStr = reservationView.EndDateStr;
        //    rateDewtails.Add(rates);
        //    reservationView.RateDetailsList = rateDewtails;
        //    reservationView.TotalDays = rates.TotalDays;
        //    Navigation.PushAsync(new VechicleInformationPage(reservationView, selectedVehicle));
        //}

        private void VehicleDetailList_Refreshing(object sender, EventArgs e)
        {
            refreshVehicleList();
            vehicleDetailList.IsVisible = true;
            vehicleDetailList.IsRefreshing = false;
        }

        private void btnFilter_Tapped(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new Popups.FilterPopup(VehicleFilter, vehicleFilterMenus));
        }

        private void btnBookNow_Clicked(object sender, EventArgs e)
        {
            //List<VehicleTypeResult> results = new List<VehicleTypeResult>();
            //VehicleTypeResult selectedVehicle = vehicleDetailList.SelectedItem as VehicleTypeResult;
            //int vehiId = selectedVehicle.VehicleTypeId;
            //foreach(VehicleTypeResult v in vehicleResults)
            //{
            //    if (v.VehicleTypeId == vehiId)
            //    {
            //        v.selected = true;
            //    }
            //    else
            //    {
            //        v.selected = false;
            //    }
            //    results.Add(v);
            //}
            //vehicleDetailList.ItemsSource = results;
            var obj = (Button)sender;
            VehicleViewByTypeForMobile selectedVehicle = obj.BindingContext as VehicleViewByTypeForMobile;

            reservationView.VehicleTypeID = selectedVehicle.VehicleTypeId;
            reservationView.VehicleType = selectedVehicle.VehicleType;

            Rates rates = JsonConvert.DeserializeObject<Rates>(JsonConvert.SerializeObject(selectedVehicle.RateDetail));
            rates.RateId = selectedVehicle.RateDetail.RateID;
            List<Rates> rateDewtails = new List<Rates>();
            //rates.StartDateStr = reservationView.StartDateStr;
            //rates.EndDateStr = reservationView.EndDateStr;
            rateDewtails.Add(rates);
            reservationView.RateDetailsList = rateDewtails;
            reservationView.TotalDays = rates.TotalDays;
            reservationView.VehicleId = selectedVehicle.vehicleId;
            reservationView.VehicleId2 = selectedVehicle.vehicleId;
            //Navigation.PushAsync(new BookNow(reservationView, selectedVehicle, selectedVehicle.locationIdList));
            //Navigation.PushModalAsync(new VechicleInformationPage(reservationView, selectedVehicle, selectedVehicle.locationIdList));
        }

        private void vehicleDetailList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            VehicleViewByTypeForMobile selectedVehicle = vehicleDetailList.SelectedItem as VehicleViewByTypeForMobile;
            reservationView.VehicleTypeID = selectedVehicle.VehicleTypeId;
            reservationView.VehicleType = selectedVehicle.VehicleType;
            Rates rates = JsonConvert.DeserializeObject<Rates>(JsonConvert.SerializeObject(selectedVehicle.RateDetail));
            rates.RateId = selectedVehicle.RateDetail.RateID;
            List<Rates> rateDewtails = new List<Rates>();
            //rates.StartDateStr = reservationView.StartDateStr;
            //rates.EndDateStr = reservationView.EndDateStr;
            rateDewtails.Add(rates);
            reservationView.RateDetailsList = rateDewtails;
            reservationView.TotalDays = rates.TotalDays;
            reservationView.VehicleId = selectedVehicle.vehicleId;
            reservationView.VehicleId2 = selectedVehicle.vehicleId;
            Navigation.PushModalAsync(new VehicleRates(reservationView, selectedVehicle));
            //Navigation.PushAsync(new BookNow(reservationView, selectedVehicle, selectedVehicle.locationIdList));
        }

        private async void filterDetailLabel_Tapped(object sender, EventArgs e)
        {
            //if (sortDetailsGrid.IsVisible)
            //{
            //    sortDetailsGrid.IsVisible = false;
            //    await SortArrow.RotateTo(360, 200);
            //}

            //if (!filterDetailsgrid.IsVisible)
            //{
            //    filterDetailsgrid.IsVisible = !filterDetailsgrid.IsVisible;
            //    await filterArrow.RotateTo(180, 200);
            //}
            //else
            //{
            //    filterDetailsgrid.IsVisible = !filterDetailsgrid.IsVisible;
            //    await filterArrow.RotateTo(360, 200);
            //}

        }

        private async void sortDetailLabel_Tapped(object sender, EventArgs e)
        {
            //if (filterDetailsgrid.IsVisible)
            //{
            //    filterDetailsgrid.IsVisible = false;
            //    await filterArrow.RotateTo(360, 200);
            //}
            //if (!sortDetailsGrid.IsVisible)
            //{
            //    sortDetailsGrid.IsVisible = !sortDetailsGrid.IsVisible;
            //    await SortArrow.RotateTo(180, 200);
            //}
            //else
            //{
            //    sortDetailsGrid.IsVisible = !sortDetailsGrid.IsVisible;
            //    await SortArrow.RotateTo(360, 200);
            //}

        }

        private void resetFilter_Clicked(object sender, EventArgs e)
        {
            VehicleFilter = null;
            VehicleFilter = new VehicleFilterSearch();
            refreshVehicleList();
        }

        private void filterItemDeleteBtn_Clicked(object sender, EventArgs e)
        {
            var obj = (ImageButton)sender;
            filterFormat selectedfilter = obj.BindingContext as filterFormat;
            if (selectedfilter != null)
            {
                if (selectedfilter.type == "VehicleType")
                {
                    VehicleFilter.vehicleTypes.Remove(selectedfilter.title);
                }
                if (selectedfilter.type == "VehicleMake")
                {
                    VehicleFilter.vehicleMakes.Remove(selectedfilter.title);
                }
                if (selectedfilter.type == "VehicleModel")
                {
                    VehicleFilter.vehicleModels.Remove(selectedfilter.title);
                }
                if (selectedfilter.type == "VehicleYear")
                {
                    VehicleFilter.VehicleYear.Remove(Convert.ToInt32(selectedfilter.title));
                }
                if (selectedfilter.type == "VehicleColor")
                {
                    VehicleFilter.Colors.Remove(selectedfilter.title);
                }
            }
            refreshVehicleList();
        }

        //private void resetsort_Clicked(object sender, EventArgs e)
        ////{
        ////    lthSortbtn.IsChecked = false;
        ////    htlSortbtn.IsChecked = false;
        //    refreshVehicleList();
        //}

        private void sortRadioBtn_Checked(object sender, EventArgs e)
        {
            refreshVehicleList();
        }

        private void sortRadioBtn_SelectedItemChanged(object sender, EventArgs e)
        {
            refreshVehicleList();
        }

        private void btnBack_Clicked(object sender, EventArgs e)
        {
            //if (Navigation.ModalStack.Count == 1)
            //{
            //    Navigation.PushModalAsync(new HomePage());
            //}
            //else if (Navigation.ModalStack.Count == 0 && Navigation.NavigationStack.Count==1)
            //{
            //    Navigation.PushModalAsync(new HomePage());

            //}
            //else if (Navigation.ModalStack.Count > 1)
            //{
            //    if (Navigation.ModalStack[Navigation.ModalStack.Count - 2] is LoginPage)
            //    {
            //        //do nothing
            //        //Navigation.RemovePage(Navigation.ModalStack[Navigation.ModalStack.Count - 1]);
            //        Navigation.PushModalAsync(new HomePage());
            //    }                
            //    else if (Navigation.ModalStack[Navigation.ModalStack.Count - 2] is HomePage)
            //    {
            //        // do nothing
            //        Navigation.PopModalAsync();
            //    }
            //    else
            //    {
            //        Navigation.PopModalAsync();
            //    }
            //}
            //else if (Navigation.ModalStack.Count > 2)
            //{
            //    Navigation.PopModalAsync();

            //}
            //else
            //{
            //    //do nothing
            //}

            //if (customerId > 0)
            //{
            //    Navigation.PushModalAsync(new HomePage());
            //}
            //else
            //{
                Navigation.PopModalAsync();
            //}

        }

        //[Obsolete]
        //void novehicleTap_Tapped(System.Object sender, System.EventArgs e)
        //{
        //    Device.OpenUri(new Uri("https://forms.gle/tezxDibPZQGVYAb29"));
        //}

    }

    public class filterFormat
    {
        public string title { get; set; }
        public string type { get; set; }
        public bool isAvailable { get; set; }

    }
}