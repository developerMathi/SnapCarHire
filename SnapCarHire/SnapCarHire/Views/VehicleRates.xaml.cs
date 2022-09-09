using SnapCarHire.Popups;
using SnapCarHire.Renders;
using SnapCarHireController;
using SnapCarHireModel;
using SnapCarHireModel.AccessModels;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SnapCarHire.Views
{
    public partial class VehicleRates : ContentPage
    {
        //MisChargeFilter misChargeFilter;
        GetMischargeSearchDetailsMobileRequest misChargeRequest;
        //TaxFilter taxFilter;
        GetTaxMobileListRequest taxRequest;
        GetPromotionMobileRequest promotionMobileRequest;
        GetPromotionMobileResponse promotionMobileResponse;
        List<MiscChargeSearchReview> misChargeResults;
        List<MiscChargeSearchReview> misChargeResultsNonSelectable;
        List<MiscChargeSearchReview> misChargeResultsSelectable;
        GetMischargeSearchDetailsMobileResponse misChargeResponse;
        List<LocationTaxModel> taxResults;
        GetTaxMobileListResponse taxResponse;
        ReservationController reservationController;
        string token;
        private ReservationView reservationView;
        private VehicleViewByTypeForMobile selectedVehicle;



        public VehicleRates(ReservationView reservationView, VehicleViewByTypeForMobile selectedVehicle)
        {
            InitializeComponent();
            this.reservationView = reservationView;
            //misChargeFilter = new MisChargeFilter();
            misChargeRequest = new GetMischargeSearchDetailsMobileRequest();
            //taxFilter = new TaxFilter();
            taxRequest = new GetTaxMobileListRequest();
            promotionMobileRequest = new GetPromotionMobileRequest();
            //misChargeFilter.LocationId =(int) reservationView.StartLocationId;
            //misChargeFilter.VehicleTypeId =(int) reservationView.VehicleTypeID;
            misChargeRequest.LocationId = (int)reservationView.StartLocationId;
            misChargeRequest.VehicleTypeId = (int)reservationView.VehicleTypeID;
            //taxFilter.LocationId = (int)reservationView.StartLocationId;
            taxRequest.LocationId = (int)reservationView.StartLocationId;
            promotionMobileResponse = null;
            misChargeResults = null;
            misChargeResultsNonSelectable = null;
            misChargeResultsSelectable = null;
            misChargeResponse = null;
            taxResults = null;
            taxResponse = null;
            reservationController = new ReservationController();
            token = App.Current.Properties["currentToken"].ToString();
            this.selectedVehicle = selectedVehicle;
            startDateLabel.Text = ((DateTime)reservationView.StartDate).ToString("MM/dd/yyyy");
            endDateLabel.Text = ((DateTime)reservationView.EndDate).ToString("MM/dd/yyyy");
            startTimeLabel.Text = ((DateTime)reservationView.StartDate).ToString("hh:mm tt");
            endTimeLabel.Text = ((DateTime)reservationView.EndDate).ToString("hh:mm tt");
            if (selectedVehicle.VehicleImageUrl != null)
            {
                vehilcleTypeImage.Source = ImageSource.FromUri(new Uri(selectedVehicle.VehicleImageUrl));
            }
            else if(selectedVehicle.VehicleTypeImageUrl != null)
            {
                vehilcleTypeImage.Source = ImageSource.FromUri(new Uri(selectedVehicle.VehicleTypeImageUrl));
            }
            vehicleSampleLabel.Text = selectedVehicle.vehicleName;
            vehilcleTypeLabel.Text = selectedVehicle.VehicleType;
            priceLabel.Text = "Days: " + selectedVehicle.RateDetail.TotalDays.ToString();

            dailyRateDetailLabel.Text = "( " + selectedVehicle.RateDetail.DailyQty + " x $ " + ((decimal)selectedVehicle.RateDetail.DailyRate).ToString("0.00") + " )";
            DailyRateTotal.Text = "$ " + ((decimal)selectedVehicle.RateDetail.DailyQty * (decimal)selectedVehicle.RateDetail.DailyRate).ToString("0.00");

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var assembly = typeof(VehicleRates);
            //if ((int)App.Current.Properties["CustomerId"] == 0)
            //{
            // loginIcon.IconImageSource = ImageSource.FromResource("MaxVonGrafKftMobile.Assets.LogInTool.png", assembly);

            //}
            //else
            //{
            // loginIcon.IconImageSource = ImageSource.FromResource("MaxVonGrafKftMobile.Assets.logOutTool.png", assembly);
            //}

            if (reservationView != null)
            {
                startDateLabel.Text = ((DateTime)reservationView.StartDate).ToString("MM/dd/yyyy");
                endDateLabel.Text = ((DateTime)reservationView.EndDate).ToString("MM/dd/yyyy");
                startTimeLabel.Text = ((DateTime)reservationView.StartDate).ToString("hh:mm tt");
                endTimeLabel.Text = ((DateTime)reservationView.EndDate).ToString("hh:mm tt");
                startLoc.Text = reservationView.StartLocationName;
                endLoc.Text = reservationView.EndLocationName;
                if (selectedVehicle.VehicleImageUrl != null)
                {
                    vehilcleTypeImage.Source = ImageSource.FromUri(new Uri(selectedVehicle.VehicleImageUrl));
                    VehImageFill.Source = ImageSource.FromUri(new Uri(selectedVehicle.VehicleImageUrl));
                }
                else if (selectedVehicle.VehicleTypeImageUrl != null)
                {
                    vehilcleTypeImage.Source = ImageSource.FromUri(new Uri(selectedVehicle.VehicleTypeImageUrl));
                    VehImageFill.Source = ImageSource.FromUri(new Uri(selectedVehicle.VehicleTypeImageUrl));
                }
                vehicleSampleLabel.Text = selectedVehicle.vehicleName;
                vehilcleTypeLabel.Text = selectedVehicle.VehicleType;
                priceLabel.Text = "Days: " + selectedVehicle.RateDetail.TotalDays.ToString();

                dailyRateDetailLabel.Text = "( " + selectedVehicle.RateDetail.DailyQty + " x $ " + ((decimal)selectedVehicle.RateDetail.DailyRate).ToString("0.00") + " )";
                DailyRateTotal.Text = "$ " + ((decimal)selectedVehicle.RateDetail.DailyQty * (decimal)selectedVehicle.RateDetail.DailyRate).ToString("0.00");

            }

            if (PopupNavigation.Instance.PopupStack.Count > 0)
            {
                //if (PopupNavigation.Instance.PopupStack[PopupNavigation.Instance.PopupStack.Count - 1].GetType() == typeof(ErrorWithClosePagePopup))
                //{
                await PopupNavigation.Instance.PopAllAsync();
                //}
            }

            bool busy = false;
            if (!busy)
            {
                try
                {
                    busy = true;
                    await PopupNavigation.Instance.PushAsync(new LoadingPopup("Getting vehicles rates..."));

                    await Task.Run(() =>
                    {
                        try
                        {
                            //misChargeResults = reservationController.getMisCharge(misChargeFilter,token);
                            misChargeResponse = reservationController.getMisChargeMobile(misChargeRequest, token);
                            //taxResults = reservationController.getTax(taxFilter,token);
                            taxResponse = reservationController.GetTaxMobileList(taxRequest, token);



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


                    if (taxResponse.message.ErrorCode == "200")
                    {
                        taxResults = taxResponse.LocationTaxResult;
                        foreach (LocationTaxModel t in taxResults)
                        {
                            if (t.IsOption)
                            {
                                t.IsSelected = false;
                                if (reservationView.TaxList2 != null)
                                {
                                    if (reservationView.TaxList2.Count > 0)
                                    {
                                        foreach (LocationTaxModel ltm in reservationView.TaxList2)
                                        {
                                            if (ltm.TaxId == t.TaxId)
                                            {
                                                t.IsSelected = true;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                t.IsSelected = true;
                            }
                        }
                    };
                    if (taxResponse.message.ErrorCode != "200")
                    {
                        await PopupNavigation.Instance.PushAsync(new Error_popup(taxResponse.message.ErrorMessage));
                    };


                    if (misChargeResponse.message.ErrorCode == "200")
                    {
                        misChargeResults = misChargeResponse.MischargeResultList;
                        misChargeResultsSelectable = new List<MiscChargeSearchReview>();
                        misChargeResultsNonSelectable = new List<MiscChargeSearchReview>();
                        if (misChargeResults != null)
                        {
                            foreach (MiscChargeSearchReview m in misChargeResults)
                            {
                                switch (m.CalculationType)
                                {
                                    case "Perday":
                                        m.ViewString = "( " + m.CalculationType + " $" + m.Value + " ) x " + selectedVehicle.RateDetail.TotalDays;
                                        m.price = (decimal)m.Value * (decimal)selectedVehicle.RateDetail.TotalDays;
                                        break;
                                    case "Fixed":
                                        m.ViewString = "( " + m.CalculationType + " $" + m.Value + " )";
                                        m.price = m.Value;
                                        break;
                                }

                                //if (m.IsQuantity) { m.price=(decimal)m.Value *(decimal)m.Unit; }
                                //else
                                //{
                                // m.price=m.Value;
                                //}

                                if (!m.IsOptional)
                                {
                                    m.IsSelected = false;
                                    if (reservationView.MiscList2 != null)
                                    {
                                        if (reservationView.MiscList2.Count > 0)
                                        {
                                            foreach (MiscChargeSearchReview msv in reservationView.MiscList2)
                                            {
                                                if (msv.MiscChargeID == m.MiscChargeID)
                                                {
                                                    m.IsSelected = true;
                                                }
                                            }
                                        }
                                    }
                                    misChargeResultsSelectable.Add(m);

                                }
                                else
                                {
                                    m.IsSelected = true;

                                    misChargeResultsNonSelectable.Add(m);

                                }
                            }
                        }
                    };
                    if (misChargeResponse.message.ErrorCode != "200")
                    {
                        await PopupNavigation.Instance.PushAsync(new Error_popup(misChargeResponse.message.ErrorMessage));
                    };


                    if (misChargeResultsNonSelectable.Count() > 0)
                    {
                        RateList.ItemsSource = misChargeResultsNonSelectable;
                        RateList.HeightRequest = misChargeResultsNonSelectable.Count() * 65;
                    }
                    if (misChargeResultsNonSelectable.Count() == 0)
                    {
                        RateList.IsVisible = false;
                    }


                    if (misChargeResultsSelectable.Count() > 0)
                    {
                        RateListSelectLabel.ItemsSource = misChargeResultsSelectable;
                        RateListSelectLabel.HeightRequest = misChargeResultsSelectable.Count() * 72;
                    }
                    if (misChargeResultsSelectable.Count() == 0)
                    {
                        RateListSelectLabel.IsVisible = false;
                    }

                    if (taxResults.Count() > 0)
                    {
                        taxList.ItemsSource = taxResults;
                        taxList.HeightRequest = taxResults.Count() * 65;
                    }
                    if (taxResults.Count() == 0)
                    {
                        taxList.IsVisible = false;
                        taxHeadingLabel.IsVisible = false;
                    }


                }

            }



        }

        //private async void LoginIcon_Clicked(object sender, EventArgs e)
        //{
        // var assembly = typeof(VehicleRates);
        // if ((int)App.Current.Properties["CustomerId"] == 0)
        // {

        // loginIcon.IconImageSource = ImageSource.FromResource("MaxVonGrafKftMobile.Assets.logOutTool.png", assembly);
        // await Navigation.PushAsync(new LoginPage());

        // }
        // else
        // {
        // bool logout = await DisplayAlert("Alert", "Are you sure want to logout", "Yes", "No");
        // if (logout)
        // {
        // App.Current.Properties["CustomerId"] = 0;
        // loginIcon.IconImageSource = ImageSource.FromResource("MaxVonGrafKftMobile.Assets.LogInTool.png", assembly);
        // await Navigation.PushAsync(new WelcomPage());
        // }
        // }
        //}

        private void BacKBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private async void NxtBtn_Clicked(object sender, EventArgs e)
        {
            List<MiscChargeSearchReview> miscChargeSearchReviews = new List<MiscChargeSearchReview>();

            if (misChargeResultsNonSelectable.Count() > 0)
            {
                List<MiscChargeSearchReview> itemListMis = RateList.ItemsSource as List<MiscChargeSearchReview>;
                foreach (MiscChargeSearchReview msr in itemListMis)
                {

                    msr.StartDate = (DateTime)reservationView.StartDate;
                    msr.EndDate = (DateTime)reservationView.EndDate;
                    msr.StartDateString = reservationView.StartDateStr;
                    msr.EndDateString = reservationView.EndDateStr;
                    if (msr.IsSelected)
                    {
                        miscChargeSearchReviews.Add(msr);
                    }

                }
            }

            if (misChargeResultsSelectable.Count() > 0)
            {
                List<MiscChargeSearchReview> itemListMis2 = RateListSelectLabel.ItemsSource as List<MiscChargeSearchReview>;
                foreach (MiscChargeSearchReview msr in itemListMis2)
                {

                    msr.StartDate = (DateTime)reservationView.StartDate;
                    msr.EndDate = (DateTime)reservationView.EndDate;
                    msr.StartDateString = reservationView.StartDateStr;
                    msr.EndDateString = reservationView.EndDateStr;
                    if (msr.IsSelected)
                    {
                        miscChargeSearchReviews.Add(msr);
                    }

                }
            }

            reservationView.MiscList2 = miscChargeSearchReviews;


            if (taxResults.Count() > 0)
            {
                List<LocationTaxModel> locationTaxModels = new List<LocationTaxModel>();
                List<LocationTaxModel> itemsourceTax = taxList.ItemsSource as List<LocationTaxModel>;
                foreach (LocationTaxModel ltm in itemsourceTax)
                {
                    LocationTaxModel locationTaxModel = new LocationTaxModel();
                    locationTaxModel.LocationId = ltm.LocationId;
                    locationTaxModel.TaxId = ltm.TaxId;
                    locationTaxModel.Name = ltm.Name;
                    locationTaxModel.Description = ltm.Description;
                    locationTaxModel.Value = ltm.Value;
                    locationTaxModel.LocationName = ltm.LocationName;
                    locationTaxModel.IsSelected = ltm.IsSelected;
                    if (locationTaxModel.IsSelected)
                    {
                        locationTaxModels.Add(locationTaxModel);
                    }

                }
                reservationView.TaxList2 = locationTaxModels;
            }
            await Navigation.PushModalAsync(new SummaryOfChargesPage(reservationView, selectedVehicle));

        }

        private void CheckBox_CheckChanged(object sender, EventArgs e)
        {
            var item = (sender as Plugin.InputKit.Shared.Controls.CheckBox).BindingContext as MisChargeResult;
            misChargeResults = (List<MiscChargeSearchReview>)RateList.ItemsSource;
        }

        private async void PromoBtn_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(promoCodeEntry.Text))
            {
                promotionMobileRequest.PromotionCode = promoCodeEntry.Text;
                promotionMobileRequest.LocationId = (int)reservationView.StartLocationId;
                promotionMobileRequest.VehicleTypeId = (int)reservationView.VehicleTypeID;

                bool busy = false;
                if (!busy)
                {
                    try
                    {
                        busy = true;
                        await PopupNavigation.Instance.PushAsync(new LoadingPopup("Checking for promotion..."));

                        await Task.Run(() =>
                        {
                            try
                            {
                                promotionMobileResponse = reservationController.checkPromotion(promotionMobileRequest, token);
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
                    if (promotionMobileResponse.promotion == null)
                    {
                        await PopupNavigation.Instance.PushAsync(new Error_popup("Invalid promo code"));
                    }
                    else
                    {
                        await PopupNavigation.Instance.PushAsync(new SuccessPopUp("Promo code has been applied successfully!"));
                        reservationView.PromotionCode = promoCodeEntry.Text;
                        //if(reservationView.PromotionList== null)
                        //{
                        reservationView.PromotionList = new List<PromotionItem>();
                        //}

                        reservationView.PromotionList.Add(new PromotionItem() { PromotionID = promotionMobileResponse.PromResult.PromotionID, PromotionDiscount = (decimal)promotionMobileResponse.PromResult.DiscountValue, PromotionCode = promotionMobileResponse.promotion.PromotionCode });
                        reservationView.PromotionID = promotionMobileResponse.PromResult.PromotionID;
                    }
                }
            }
        }

        private void TaxCheckbox_CheckChanged(object sender, EventArgs e)
        {

        }

        private void ExtStepper_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var obj = (ExtStepper)sender;
            var objGrid = (Grid)obj.Parent;
            var viewCell = (ExtendedViewCell)objGrid.Parent;

            //var data = obj.BindingContext as MischargeResultMobile;
            //foreach(MischargeResultMobile msrm in misChargeResults)
            //{
            // if(data.MiscChargeID== msrm.MiscChargeID)
            // {
            // msrm.Unit = obj.Value;
            // }
            //}
        }

        private void ExtStepper_AddClicked(object sender, EventArgs e)
        {
            var obj = (ExtStepper)sender;
            var objGrid = (Grid)obj.Parent;
            var viewCell = (ExtendedViewCell)objGrid.Parent;

            var data = obj.BindingContext as MischargeResultMobile;
            foreach (MiscChargeSearchReview msrm in misChargeResults)
            {
                if (data.MiscChargeID == msrm.MiscChargeID)
                {
                    msrm.Unit = obj.Value;

                }
            }
        }

        private void ExtStepper_SubClicked(object sender, EventArgs e)
        {
            var obj = (ExtStepper)sender;
            var objGrid = (Grid)obj.Parent;
            var viewCell = (ExtendedViewCell)objGrid.Parent;

            var data = obj.BindingContext as MischargeResultMobile;
            foreach (MiscChargeSearchReview msrm in misChargeResults)
            {
                if (data.MiscChargeID == msrm.MiscChargeID)
                {
                    msrm.Unit = obj.Value;
                }
            }
        }

        private void descriptionBtn_Tapped(object sender, EventArgs e)
        {
            var obj = (Image)sender;
            var objstac = (StackLayout)obj.Parent;
            var objGrid = (Grid)objstac.Parent;
            var viewcell = (ExtendedViewCell)objGrid.Parent;
            MiscChargeSearchReview selectedMischrge = objGrid.BindingContext as MiscChargeSearchReview;
            string discription = selectedMischrge.Description;
            if (selectedMischrge.Name == "Insurance")
            {
                discription = "Liability and physical damage coverage for rideshare is included ($1,000 deductible; see rental agreement for full coverage and exclusions) ";
            }
            if (selectedMischrge.Name == "Jax Maintenance Plan")
            {
                discription = "All mechanical failures including engine and transmission issues are covered with this rental (see rental agreement for full coverage and exclusions).";
            }
            if (selectedMischrge.Name == "Jax Protection Plan")
            {
                discription = "All wear parts including tires, brakes, windshield wipers and more are 100% covered with this rental (see rental agreement for full coverage and exclusions).";
            }
            PopupNavigation.Instance.PushAsync(new DetailPopUp(selectedMischrge.Name, discription));

        }

        void descriptionBtndailyRate_Tapped(System.Object sender, System.EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new DetailPopUp("Day Rate", "The base rate of the vehicle depends on the vehicle type and amenities included "));
        }
    }
}