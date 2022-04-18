using SnapCarHireController;
using SnapCarHireModel;
using SnapCarHireModel.AccessModels;
using SnapCarHireModel.Constants;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SnapCarHire.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExtendPopup : PopupPage
    {
        private AgreementReview agreementDetail;

        private string _token;
        private ReservationViewModel reservationData;
        ReservationView reservationView;
        UpdateReservationMobileRequest ReservationMobileRequest;
        UpdateReservationMobileResponse ReservationMobileResponse;
        GetReservationConfigurationMobileRequest vehicleMobileRequest;
        GetReservationConfigurationResponse vehicleMobileResponse;
        ReservationConfigurationVehicleSearch search;
        UpdateReservationMobileResponse response;
        GetCalculateSummaryMobileRequest summaryMobileRequest;
        GetCalculateSummaryMobileResponsecs summaryMobileResponsecs;
        decimal paidAmmount;

        ReferenceType type;

        public ExtendPopup(AgreementReview agreementDetail)
        {
            InitializeComponent();
            this.agreementDetail = agreementDetail;
            extendedDate.MinimumDate = agreementDetail.CheckinDate.AddHours(24);
            extendedDate.Date = agreementDetail.CheckinDate.AddHours(24);
            extendedDate.Format = "dddd, MMMM d, yyyy";
            extendedTime.Time = new TimeSpan(agreementDetail.CheckinDate.AddHours(24).Hour, agreementDetail.CheckinDate.AddHours(24).Minute, agreementDetail.CheckinDate.AddHours(24).Second);
            extendedTime.Format = "hh mm tt";
            _token = App.Current.Properties["currentToken"].ToString();
            type = ReferenceType.Agreement;

        }

        public ExtendPopup(ReservationViewModel reservationData)
        {
            InitializeComponent();
            _token = App.Current.Properties["currentToken"].ToString();
            this.reservationData = reservationData;
            reservationView = new ReservationView();
            reservationView = reservationData.Reservationview;
            reservationView.RateDetailsList = reservationData.RateDetailsList;
            reservationView.MiscList2 = reservationData.MiscChargeList;
            reservationView.TaxList2 = reservationData.TaxList;
            extendedDate.MinimumDate = ((DateTime)reservationView.EndDate).AddHours(24);
            extendedDate.Format = "dddd, MMMM d, yyyy";
            extendedTime.Time = new TimeSpan(((DateTime)reservationView.EndDate).Hour, ((DateTime)reservationView.EndDate).Minute, ((DateTime)reservationView.EndDate).Second);
            extendedTime.Format = "hh mm tt";
            ReservationMobileRequest = new UpdateReservationMobileRequest();
            response = null;


            search = new ReservationConfigurationVehicleSearch();
            vehicleMobileRequest = new GetReservationConfigurationMobileRequest();
            vehicleMobileResponse = null;
            search.ClientId = Constants.ClientId;
            search.LocationId = (int)reservationView.StartLocationId;
            search.CheckInLocationId = (int)reservationView.EndLocationId;
            search.IsOnline = true;
            search.StartDate = (DateTime)reservationView.StartDate;
            search.EndDate = (DateTime)reservationView.EndDate;
            search.VehicleTypeId = (int)reservationView.VehicleTypeID;
            search.VehicleCategoryId = 0;
            search.VehicleMakeID = 0;
            search.ModelId = 0;
            search.NumberOfSeats = 0;
            search.RentalPeriod = 0;
            search.VehicleId = 0;

            vehicleMobileRequest.search = search;


            type = ReferenceType.Reservation;
            summaryMobileRequest = new GetCalculateSummaryMobileRequest();
            summaryMobileResponsecs = null;
            paidAmmount = (decimal)reservationData.ReservationTotal.AmountPaid;
        }

        private void btnClose_Tapped(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }

        private void canelBtn_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }

        private async void confirmBtn_Clicked(object sender, EventArgs e)
        {
            if (type == ReferenceType.Agreement)
            {
                if (NumberOfDays.SelectedIndex == -1)
                {
                    await PopupNavigation.Instance.PushAsync(new Error_popup("Please select the number of days for extend your rental."));
                }
                //if (extendedDate.Date.DayOfYear < agreementDetail.CheckinDate.AddHours(24).DayOfYear)
                //{
                //    PopupNavigation.Instance.PushAsync(new Error_popup("please select a date greater than " + agreementDetail.CheckinDate.AddHours(24).ToString("dddd, MM/dd/yyyy")));
                //}
                //else if (extendedTime.Time == new TimeSpan(0, 0, 0))
                //{
                //    PopupNavigation.Instance.PushAsync(new Error_popup("please select a valid time "));
                //}
                else
                {
                    DateTime seleTime = agreementDetail.CheckinDate.AddDays(int.Parse(NumberOfDays.SelectedItem.ToString()));
                    //GetStoreHoursMobileResponse getStoreHoursMobileResponse = null;
                    //try
                    //{
                    //    GetStoreHoursMobileRequest getStoreHoursMobileRequest = new GetStoreHoursMobileRequest();
                    //    ReservationController reservationController = new ReservationController();
                    //    getStoreHoursMobileRequest.locationId = (int)agreementDetail.CheckinLocation;
                    //    getStoreHoursMobileResponse = reservationController.getStoreHoursMobile(getStoreHoursMobileRequest, _token);
                    //}
                    //catch (Exception ex)
                    //{
                    //    PopupNavigation.Instance.PushAsync(new Error_popup(ex.Message));
                    //}
                    //finally
                    //{
                    //    if (getStoreHoursMobileResponse != null)
                    //    {
                    //        if (getStoreHoursMobileResponse.storeHourModels != null)
                    //        {
                    //            if (getStoreHoursMobileResponse.storeHourModels.Count > 0)
                    //            {
                    //                foreach (SearchStoreHourModel sshm in getStoreHoursMobileResponse.storeHourModels)
                    //                {
                    //                    if ((seleTime).DayOfWeek.ToString() == Enum.GetName(typeof(DayOfWeek), sshm.Day - 1))
                    //                    {
                    //                        if (((DateTime)sshm.StartTime).TimeOfDay <= seleTime.TimeOfDay && seleTime.TimeOfDay <= ((DateTime)sshm.EndTime).TimeOfDay)
                    //                        {
                    //                            updateAgreement();
                    //                        }
                    //                        else
                    //                        {
                    //                            PopupNavigation.Instance.PushAsync(new Error_popup("Sorry, we are only open during " + sshm.StartTimeStr + " to " + sshm.EndTimeStr + " Please select a different time"));
                    //                        }
                    //                    }
                    //                }
                    //            }
                    //            else
                    //            {
                    //                updateAgreement();
                    //            }
                    //        }
                    //        else
                    //        {
                    //            updateAgreement();
                    //        }
                    //    }
                    //}
                    bool isExtendAvailable = await DisplayAlert("Alert", "Are you sure you want to extend ?","Yes","Cancel");
                    if (isExtendAvailable)
                    {
                        updateAgreement();
                    }
                    
                }
            }

            else if (type == ReferenceType.Reservation)
            {
                //if (extendedDate.Date <= ((DateTime)reservationView.EndDate).AddDays(1))
                //{
                //    PopupNavigation.Instance.PushAsync(new Error_popup("please select a date greater than " + ((DateTime)reservationView.EndDate).AddDays(1).ToString("dddd, MM/dd/yyyy")));
                //}
                //else if (extendedTime.Time == new TimeSpan(0, 0, 0))
                //{
                //    PopupNavigation.Instance.PushAsync(new Error_popup("please select a valid time "));
                //}
                if (NumberOfDays.SelectedIndex == -1)
                {
                    PopupNavigation.Instance.PushAsync(new Error_popup("Please select the number of days for extend your rental."));
                }
                else
                {
                    DateTime endDate = ((DateTime)reservationView.EndDate).AddDays(int.Parse(NumberOfDays.SelectedItem.ToString())); 
                    string endDateStr = endDate.ToString("MM/dd/yyyy hh:mm tt").Replace("-", "/");






                    //GetStoreHoursMobileResponse getStoreHoursMobileResponse = null;
                    //try
                    //{
                    //    GetStoreHoursMobileRequest getStoreHoursMobileRequest = new GetStoreHoursMobileRequest();
                    //    ReservationController reservationController = new ReservationController();
                    //    getStoreHoursMobileRequest.locationId = (int)reservationView.EndLocationId;
                    //    getStoreHoursMobileResponse = reservationController.getStoreHoursMobile(getStoreHoursMobileRequest, _token);
                    //}
                    //catch (Exception ex)
                    //{
                    //    PopupNavigation.Instance.PushAsync(new Error_popup(ex.Message));
                    //}
                    //finally
                    //{
                    //    if (getStoreHoursMobileResponse != null)
                    //    {
                    //        if (getStoreHoursMobileResponse.storeHourModels != null)
                    //        {
                    //            if (getStoreHoursMobileResponse.storeHourModels.Count > 0)
                    //            {
                    //                foreach (SearchStoreHourModel sshm in getStoreHoursMobileResponse.storeHourModels)
                    //                {
                    //                    if ((endDate).DayOfWeek.ToString() == Enum.GetName(typeof(DayOfWeek), sshm.Day - 1))
                    //                    {
                    //                        if (((DateTime)sshm.StartTime).TimeOfDay <= endDate.TimeOfDay && endDate.TimeOfDay <= ((DateTime)sshm.EndTime).TimeOfDay)
                    //                        {
                    //                            updateReservation();
                    //                        }
                    //                        else
                    //                        {
                    //                            PopupNavigation.Instance.PushAsync(new Error_popup("Sorry, we are only open during " + sshm.StartTimeStr + " to " + sshm.EndTimeStr + " Please select a different time"));
                    //                        }
                    //                    }
                    //                }
                    //            }
                    //            else
                    //            {
                    //                updateReservation();
                    //            }
                    //        }
                    //        else
                    //        {
                    //            updateReservation();
                    //        }
                    //    }
                    //    else
                    //    {
                    //        updateReservation();
                    //    }
                    //}
                    bool isExtendAvailable = await DisplayAlert("Alert", "Are you sure you want to extend ?", "Yes", "Cancel");
                    if (isExtendAvailable)
                    {
                        updateReservation();
                    }
                    
                }
            }
        }

        private async void updateReservation()
        {
            DateTime endDate = ((DateTime)reservationView.EndDate).AddDays(int.Parse(NumberOfDays.SelectedItem.ToString()));
            string endDateStr = endDate.ToString("MM/dd/yyyy hh:mm tt").Replace("-", "/");

            reservationView.EndDate = endDate;
            reservationView.EndDateStr = endDateStr;
            vehicleMobileRequest.search.EndDate = endDate;
            if (reservationView.VehicleId > 0) { vehicleMobileRequest.search.VehicleId = reservationView.VehicleId; };




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
                            vehicleMobileResponse = getVehicleTypesMobileNew(vehicleMobileRequest, _token);
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
                    await PopupNavigation.Instance.PopAsync();
                    if (vehicleMobileResponse != null)
                    {
                        if (vehicleMobileResponse.listVehicle != null)
                        {
                            if (vehicleMobileResponse.listVehicle.Count > 0)
                            {
                                if (reservationView.VehicleId > 0)
                                {
                                    foreach (ReservationVehicleSearchViewModel rvsvm in vehicleMobileResponse.listVehicle)
                                    {
                                        if (rvsvm.vehicleId == reservationView.VehicleId)
                                        {
                                            Rates rates = JsonConvert.DeserializeObject<Rates>(JsonConvert.SerializeObject(rvsvm.RateDetail));
                                            rates.RateId = rvsvm.RateDetail.RateID;
                                            List<Rates> rateDewtails = new List<Rates>();
                                            rates.StartDateStr = reservationView.StartDateStr;
                                            rates.EndDateStr = reservationView.EndDateStr;
                                            rateDewtails.Add(rates);
                                            reservationView.RateDetailsList = rateDewtails;
                                            reservationView.TotalDays = rates.TotalDays;
                                            reservationView.RateTotal = (decimal)rates.RateTotal;
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    foreach (ReservationVehicleSearchViewModel rvsvm in vehicleMobileResponse.listVehicle)
                                    {
                                        if (rvsvm.VehicleTypeId == reservationView.VehicleTypeID)
                                        {
                                            Rates rates = JsonConvert.DeserializeObject<Rates>(JsonConvert.SerializeObject(rvsvm.RateDetail));
                                            rates.RateId = rvsvm.RateDetail.RateID;
                                            List<Rates> rateDewtails = new List<Rates>();
                                            rates.StartDateStr = reservationView.StartDateStr;
                                            rates.EndDateStr = reservationView.EndDateStr;
                                            rateDewtails.Add(rates);
                                            reservationView.RateDetailsList = rateDewtails;
                                            reservationView.TotalDays = rates.TotalDays;
                                            reservationView.RateTotal = (decimal)rates.RateTotal;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (reservationView.MiscList2 != null)
            {
                foreach (MiscChargeSearchReview msv in reservationView.MiscList2)
                {
                    msv.EndDate = endDate;
                    msv.StartDateString = reservationView.StartDateStr;
                    msv.EndDateString = endDateStr;
                    msv.IsSelected = true;
                }
            }

            if (reservationView.TaxList2 != null)
            {
                foreach (LocationTaxModel ltm in reservationView.TaxList2)
                {
                    ltm.IsSelected = true;
                }
            }

            ReservationMobileRequest.reservationData = reservationView;
            ReservationController reservationController = new ReservationController();
            try
            {
                response = reservationController.updateReservationMobile(ReservationMobileRequest, _token);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (response != null)
            {
                if (response.message.ErrorCode == "200")
                {
                    MessagingCenter.Send(this, "reservationUpdated");
                    await PopupNavigation.Instance.PopAllAsync();
                }
                else
                {
                    await PopupNavigation.Instance.PushAsync(new Error_popup(response.message.ErrorMessage));
                }
            }

            //summaryMobileRequest.reservation = reservationView;
            //bool isbusy = false;
            //if (!isbusy)
            //{
            //    try
            //    {
            //        isbusy = true;
            //        await PopupNavigation.Instance.PushAsync(new LoadingPopup("checking bookable..."));
            //        await Task.Run(() =>
            //        {
            //            try
            //            {
            //                summaryMobileResponsecs = getSummaryDetails(summaryMobileRequest, _token);
            //            }
            //            catch (Exception ex)
            //            {
            //                PopupNavigation.Instance.PushAsync(new ErrorWithClosePagePopup(ex.Message));
            //            }
            //        });

            //    }
            //    finally
            //    {
            //        isbusy = false;
            //        if (PopupNavigation.Instance.PopupStack.Count == 1)
            //        {
            //            await PopupNavigation.Instance.PopAllAsync();
            //        }
            //        if (PopupNavigation.Instance.PopupStack.Count > 1)
            //        {
            //            if (PopupNavigation.Instance.PopupStack[PopupNavigation.Instance.PopupStack.Count - 1].GetType() != typeof(ErrorWithClosePagePopup))
            //            {
            //                await PopupNavigation.Instance.PopAsync();
            //            }
            //        }
            //        decimal amountWantToPay = 0;
            //        if (summaryMobileResponsecs.rate.ReservationSummary.EstimatedTotal == null) { amountWantToPay = (decimal)summaryMobileResponsecs.rate.EstimatedTotal - paidAmmount; }
            //        else { amountWantToPay = Convert.ToDecimal(summaryMobileResponsecs.rate.ReservationSummary.EstimatedTotal) - paidAmmount; }

            //        if (amountWantToPay > 0)
            //        {
            //            await Navigation.PushModalAsync(new ProcessPaymentPage(amountWantToPay, reservationView));
            //        }
            //        else
            //        {
            //            await PopupNavigation.Instance.PushAsync(new Error_popup("Some thing went wrong while updating your booking."));
            //        }
            //    }
            //}


        }

        private GetCalculateSummaryMobileResponsecs getSummaryDetails(GetCalculateSummaryMobileRequest summaryMobileRequest, string token)
        {
            GetCalculateSummaryMobileResponsecs summaryResponse = null;
            ReservationController controller = new ReservationController();
            try
            {
                summaryResponse = controller.getSummaryDetails(summaryMobileRequest, token);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return summaryResponse;
        }


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

        private void updateAgreement()
        {
            ExtendAgreementRequest request = new ExtendAgreementRequest();
            ExtendAgreementResponse response = null;
            request.agreementId = agreementDetail.AgreementID;
            request.extendDate = agreementDetail.CheckinDate.AddDays(int.Parse(NumberOfDays.SelectedItem.ToString())).ToString("MM/dd/yyyy hh:mm tt").Replace("PM", "pm").Replace("AM", "am");

            int agreementID = 0;
            //AgreementReview agreementReview = new AgreementReview();
            //agreementReview = agreementDetail;
            //agreementReview.OldChcekInDate = agreementReview.CheckinDate;
            //agreementReview.CheckinDate = extendedDate.Date + extendedTime.Time;
            //agreementReview.CheckinDateString = (extendedDate.Date + extendedTime.Time).ToString();
            //agreementReview.CheckoutDateString = agreementReview.CheckoutDate.ToString();
            //agreementReview.AgreementReferral = new Referral();
            //agreementReview.UpdatedDate = DateTime.Now;
            //agreementReview.ChangedDate = DateTime.Now;
            //agreementReview.AgreementStatus = SnapCarHireModel.Constants.AgreementStatusConst.Open;
            //agreementReview.AgreementInsurance = new AgreementInsurence();
            //agreementReview.Customer = new Customer();
            AgreementController controller = new AgreementController();
            response = controller.extendAgreement(request, _token);
            if (response.message.ErrorCode == "200")
            {
                MessagingCenter.Send(this, "agreementUpdated");
                PopupNavigation.Instance.PopAllAsync();
            }
            else
            {
                PopupNavigation.Instance.PushAsync(new Error_popup("Update failed. Please try again."));
            }
            PopupNavigation.Instance.PopAllAsync();
        }

        private void NumberOfDaysArrow_Clicked(object sender, EventArgs e)
        {
            NumberOfDays.Focus();
        }
    }
}