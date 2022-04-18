using SnapCarHire.Popups;
using SnapCarHireController;
using SnapCarHireModel;
using SnapCarHireModel.AccessModels;
using SnapCarHireModel.Constants;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SnapCarHire.Views
{
    public partial class ViewReservation : ContentPage
    {
        private int reservationId;
        GetReservationByIDMobileRequest reservationByIDMobileRequest;
        CancelReservationMobileRequest cancelReservationMobileRequest;
        GetReservationByIDMobileResponse reservationByIDMobileResponse;
        CancelReservationMobileResponse cancelReservationMobileResponse;
        int customerId;
        string token;
        CustomerReservationModel reservationModel;

        public ViewReservation(int reservationId)
        {
            InitializeComponent();
            this.reservationId = reservationId;
            reservationByIDMobileRequest = new GetReservationByIDMobileRequest();
            reservationByIDMobileRequest.ReservationID = reservationId;
            customerId = (int)App.Current.Properties["CustomerId"];
            token = App.Current.Properties["currentToken"].ToString();
            cancelReservationMobileRequest = new CancelReservationMobileRequest();


        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Subscribe<Error_popup>(this, "reservationCancelled", sender =>
            {
                OnAppearing();
            });

            MessagingCenter.Subscribe<ExtendPopup>(this, "reservationUpdated", sender =>
            {
                OnAppearing();

            });

            //if (Navigation.NavigationStack[Navigation.NavigationStack.Count - 2].GetType() == typeof(SummaryOfChargesPage)) {
            //    int c = Navigation.NavigationStack.Count;
            //    for (var counter = 1; counter < c - 2; counter++)
            //    {
            //        Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);

            //    }
            if (Navigation.NavigationStack.Count > 2)
            {
                if (Navigation.NavigationStack[Navigation.NavigationStack.Count - 2].GetType() == typeof(SummaryOfChargesPage))
                {
                    int c = Navigation.NavigationStack.Count;
                    for (var counter = 1; counter < c - 2; counter++)
                    {
                        Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);

                    }


                }
            }



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
                    await PopupNavigation.Instance.PushAsync(new LoadingPopup("Getting reservation details..."));

                    await Task.Run(() =>
                    {

                        try
                        {
                            reservationByIDMobileResponse = getReservationByID(reservationByIDMobileRequest, token);
                            //reservationModel = getReservation(reservationId,token);
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
                        if (PopupNavigation.Instance.PopupStack[PopupNavigation.Instance.PopupStack.Count - 1].GetType() != typeof(ErrorWithClosePagePopup) || PopupNavigation.Instance.PopupStack[PopupNavigation.Instance.PopupStack.Count - 1].GetType() == typeof(ReservationSavedPopup))
                        {
                            await PopupNavigation.Instance.PopAllAsync();
                        }
                    }

                }

            }
            if (reservationByIDMobileResponse != null)
            {
                if (reservationByIDMobileResponse.reservationData != null)
                {
                    ReservationId.Text = reservationByIDMobileResponse.reservationData.Reservationview.ReservationNumber.ToString();
                    string statusString = Enum.GetName(typeof(ReservationStatuses), reservationByIDMobileResponse.reservationData.Reservationview.Status);
                    status.Text = statusString;
                    if (reservationByIDMobileResponse.reservationData.Reservationview.Status == (short)ReservationStatuses.Quote)
                    {
                        status.Text = "Pending";
                    }


                    CheckOutLocation.Text = reservationByIDMobileResponse.reservationData.Reservationview.StartLocationName;
                    CheckOutDate.Text = reservationByIDMobileResponse.reservationData.Reservationview.StartDateStr;
                    CheckInLocation.Text = reservationByIDMobileResponse.reservationData.Reservationview.EndLocationName;
                    CheckInDate.Text = reservationByIDMobileResponse.reservationData.Reservationview.EndDateStr;
                    VehicleType.Text = reservationByIDMobileResponse.reservationData.Reservationview.VehicleType;
                    NoOfDays.Text = reservationByIDMobileResponse.reservationData.Reservationview.TotalDays.ToString("00");
                    CreateDate.Text = String.Format("{0: MM/dd/yyyy hh:mm tt}", reservationByIDMobileResponse.reservationData.Reservationview.createdDate);
                    //CreateDate.Text = reservationByIDMobileResponse.reservationData.Reservationview.createdDate.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'", CultureInfo.InvariantCulture);
                    //Discount.Text = "[$" + reservationByIDMobileResponse.reservationData.Reservationview.TotalDiscount.ToString("0.00")+"]";
                    baseRate.Text = "$" + ((decimal)reservationByIDMobileResponse.reservationData.ReservationTotal.BaseCharge).ToString("0.00");
                    //finalBaseRate.Text = "$" + ((decimal)reservationByIDMobileResponse.reservationData.ReservationTotal.FinalBaseCharge).ToString("0.00");
                    //TotalMiscCharge.Text = "$" + ((decimal)reservationByIDMobileResponse.reservationData.ReservationTotal.TotalMiscCharge + (decimal)reservationByIDMobileResponse.reservationData.ReservationTotal.TotalMischargeWithOutTax).ToString("0.00");
                    //subTotal.Text = "$" + ((decimal)reservationByIDMobileResponse.reservationData.ReservationTotal.SubTotal).ToString("0.00");
                    //totalTax.Text = "$" + ((decimal)reservationByIDMobileResponse.reservationData.ReservationTotal.TotalTax).ToString("0.00");
                    //miscChargeNotax.Text = "$" + ((decimal)reservationByIDMobileResponse.reservationData.ReservationTotal.TotalMischargeWithOutTax).ToString("0.00");
                    estmateTotal.Text = "$" + ((decimal)reservationByIDMobileResponse.reservationData.ReservationTotal.TotalAmount).ToString("0.00");
                    amountPaid.Text = "$" + ((decimal)reservationByIDMobileResponse.reservationData.ReservationTotal.AmountPaid).ToString("0.00");
                    balancedue.Text = "$" + ((decimal)reservationByIDMobileResponse.reservationData.ReservationTotal.BalanceDue).ToString("0.00");
                    if (reservationByIDMobileResponse.vehicleTypeModel == null)
                    {
                        reservationByIDMobileResponse = FixAsResponsibleToReservationByVehicle(reservationByIDMobileResponse);
                    }

                    vehicleSampleEntry.Text = reservationByIDMobileResponse.vehicleTypeModel.Sample;
                    seatCount.Text = reservationByIDMobileResponse.vehicleTypeModel.Seats;
                    bagCount.Text = reservationByIDMobileResponse.vehicleTypeModel.Baggages.ToString();
                    transLabel.Text = reservationByIDMobileResponse.vehicleTypeModel.Transmission;
                    carImage.Source = ImageSource.FromUri(new Uri(reservationByIDMobileResponse.vehicleTypeModel.ImageUrl));

                    if (reservationByIDMobileResponse.reservationData.Reservationview.CustomerDriverList != null)
                    {
                        if (reservationByIDMobileResponse.reservationData.Reservationview.CustomerDriverList.Count > 0)
                        {

                            additionalDriverList.IsVisible = true;
                            additionalDriverList.ItemsSource = reservationByIDMobileResponse.reservationData.Reservationview.CustomerDriverList;
                            additionalDriverList.HeightRequest = (reservationByIDMobileResponse.reservationData.Reservationview.CustomerDriverList.Count) * 130;
                        }
                        else
                        {
                            additionalDriverList.IsVisible = false;
                        }
                    }
                    else
                    {
                        additionalDriverList.IsVisible = false;
                    }


                    cancelReservationMobileRequest.reservationNo = reservationByIDMobileResponse.reservationData.Reservationview.ReservationNumber.ToString();


                    if (reservationByIDMobileResponse.reservationData.Reservationview.Status == 2 || reservationByIDMobileResponse.reservationData.Reservationview.Status == (short)ReservationStatuses.Quote || reservationByIDMobileResponse.reservationData.Reservationview.Status == (short)ReservationStatuses.New)
                    {
                        cancelBtn.IsVisible = true;
                        ExtendBtn.IsVisible = true;
                    }

                    string nodeValue = reservationByIDMobileResponse.reservationData.Reservationview.StartDateStr;
                    DateTimeOffset dtOffset;

                    if (DateTimeOffset.TryParse(nodeValue, null, DateTimeStyles.None, out dtOffset))
                    {
                        DateTime checkOutDatetime = dtOffset.DateTime;
                        DateTime timeAfter1dayFromNow = DateTime.Now.AddHours(24);
                        int result = DateTime.Compare(checkOutDatetime, timeAfter1dayFromNow);
                        if (result < 0)
                        {
                            //editBtn.IsVisible = false;


                        }

                    }

                }
                else
                {
                    await PopupNavigation.Instance.PushAsync(new Error_popup(reservationByIDMobileResponse.message.ErrorMessage));
                }
            }
        }

        private GetReservationByIDMobileResponse FixAsResponsibleToReservationByVehicle(GetReservationByIDMobileResponse reservationByIDMobileResponse)
        {
            if (reservationByIDMobileResponse.vehicleModel != null)
            {
                reservationByIDMobileResponse.vehicleTypeModel = new VehicleTypeWithRatesViewModel();
                reservationByIDMobileResponse.vehicleTypeModel.ImageUrl = reservationByIDMobileResponse.vehicleModel.ImageUrl;
                reservationByIDMobileResponse.vehicleTypeModel.Seats = reservationByIDMobileResponse.vehicleModel.Seats;
                reservationByIDMobileResponse.vehicleTypeModel.Baggages = reservationByIDMobileResponse.vehicleModel.Baggages;
                reservationByIDMobileResponse.vehicleTypeModel.Transmission = reservationByIDMobileResponse.vehicleModel.Transmission;
                reservationByIDMobileResponse.vehicleTypeModel.VehicleTypeName = reservationByIDMobileResponse.vehicleModel.VehicleType;
                reservationByIDMobileResponse.vehicleTypeModel.Sample = reservationByIDMobileResponse.vehicleModel.Year.ToString() + ' ' + reservationByIDMobileResponse.vehicleModel.Make + " " + reservationByIDMobileResponse.vehicleModel.Model;
            }
            return reservationByIDMobileResponse;
        }

        private GetReservationByIDMobileResponse getReservationByID(GetReservationByIDMobileRequest reservationByIDMobileRequest, string token)
        {
            GetReservationByIDMobileResponse getReservationByID = null;
            RegisterController register = new RegisterController();
            try
            {
                getReservationByID = register.getReservationByID(reservationByIDMobileRequest, token);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return getReservationByID;
        }

        //private CustomerReservationModel getReservation(int reservationId, string token)
        //{
        //    CustomerReservationModel model = null;
        //    RegisterController register = new RegisterController();
        //    try
        //    {
        //        model = register.getReservation(reservationId, token);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return model;
        //}

        private void CancelBtn_Clicked(object sender, EventArgs e)
        {
            if (reservationByIDMobileResponse.reservationData.Reservationview.Status == 2 || reservationByIDMobileResponse.reservationData.Reservationview.Status == (short)ReservationStatuses.Quote || reservationByIDMobileResponse.reservationData.Reservationview.Status == (short)ReservationStatuses.New)
            {
                cancelReservationMobileRequest.reservationNo = reservationByIDMobileResponse.reservationData.Reservationview.ReservationNumber.ToString();
                cancelReservationMobileRequest.CurrentTime = DateTime.Now;
                cancelReservationMobileRequest.isTwoHour = true;

                PopupNavigation.Instance.PushAsync(new Error_popup("Are you sure want to cancel your reservation? ", cancelReservationMobileRequest, token));
            }
        }

        private async void cancelReservation(CancelReservationMobileRequest cancelReservationMobileRequest, string token)
        {
            ReservationController reservationController = new ReservationController();
            CancelReservationMobileResponse response = null;

            bool cancelConfirm = await DisplayAlert("Alert", "Are you sure want to cancel your reservation? ", "Confirm", "Back");
            if (cancelConfirm)
            {

                bool busy = false;
                if (!busy)
                {
                    try
                    {
                        busy = true;
                        await PopupNavigation.Instance.PushAsync(new LoadingPopup("Cancelling Reservation"));
                        await Task.Run(() =>
                        {

                            response = reservationController.cancelReservation(cancelReservationMobileRequest, token);
                        });
                    }
                    finally
                    {
                        busy = false;
                        await PopupNavigation.Instance.PopAllAsync();

                        if (response.ReservationNumber != null)
                        {
                            if (Navigation.NavigationStack[Navigation.NavigationStack.Count - 2].GetType() == typeof(SummaryOfChargesPage))
                            {
                                //int c = Navigation.NavigationStack.Count;
                                //for (var counter = 1; counter < c - 2; counter++)
                                //{
                                //    Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);

                                //}
                                await Navigation.PopModalAsync();
                            }

                            //if (Navigation.NavigationStack [Navigation.NavigationStack.Count - 1].GetType() == typeof(SummaryOfChargesPage))
                            //{
                            //    await Navigation.PushAsync(new HomePageDetail());
                            //}
                            else
                            {
                                await Navigation.PopModalAsync();
                            }
                        }

                    }
                }
            }


        }

        private void HomeBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
        //protected override bool OnBackButtonPressed()
        //{
        //    if (Navigation.NavigationStack.Count >= 2)
        //    {
        //        if (Navigation.NavigationStack[Navigation.NavigationStack.Count - 2].GetType() == typeof(SummaryOfChargesPage))
        //        {
        //            Navigation.PushAsync(new HomePage());
        //        }

        //    }
        //    return false;
        //}



        private void editBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new EditBookNow(reservationByIDMobileResponse.reservationData));
        }

        private void additionalDriverList_Refreshing(object sender, EventArgs e)
        {

        }

        private void ExtendBtn_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new Popups.ExtendPopup(reservationByIDMobileResponse.reservationData));
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<Error_popup>(this, "reservationCancelled");
            MessagingCenter.Unsubscribe<FilterPopup>(this, "reservationUpdated");
        }

        private void btnBack_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}