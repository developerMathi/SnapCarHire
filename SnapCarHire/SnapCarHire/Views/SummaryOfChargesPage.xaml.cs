using SnapCarHire.Popups;
using SnapCarHireController;
using SnapCarHireModel;
using SnapCarHireModel.AccessModels;
using SnapCarHireModel.Constants;
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
    public partial class SummaryOfChargesPage : ContentPage
    {
        private ReservationView reservationView;
        GetCalculateSummaryMobileRequest summaryMobileRequest;
        CreateReservationMobileRequest ReservationMobileRequest;
        CreateReservationMobileResponse ReservationMobileResponse;
        GetCalculateSummaryMobileResponsecs summaryMobileResponsecs;
        GetTermsandConditionByTypeRequest termsandConditionByTypeRequest;
        GetTermsandConditionByTypeResponse termsandConditionByTypeResponse;
        private string token;
        private VehicleViewByTypeForMobile selectedVehicle;
        EmailInvoiceRequest emailInvoiceRequest;
        InvoiceEmailResponse emailResponse;



        //public SummaryOfChargesPage()
        //{

        //    //if ((int)App.Current.Properties["CustomerId"] == 0)
        //    //{
        //    //    loginIcon.IconImageSource = ImageSource.FromResource("SnapCarHire.Assets.LogInTool.png", assembly);

        //    //}
        //    //else
        //    //{
        //    //    loginIcon.IconImageSource = ImageSource.FromResource("SnapCarHire.Assets.logOutTool.png", assembly);
        //    //}
        //}

        public SummaryOfChargesPage(ReservationView reservationView, VehicleViewByTypeForMobile selectedVehicle)
        {
            InitializeComponent();
            var assembly = typeof(SummaryOfChargesPage);
            this.reservationView = reservationView;
            summaryMobileRequest = new GetCalculateSummaryMobileRequest();
            summaryMobileResponsecs = null;
            ReservationMobileResponse = null;
            ReservationMobileRequest = new CreateReservationMobileRequest();
            summaryMobileRequest.reservation = reservationView;
            token = App.Current.Properties["currentToken"].ToString();
            termsandConditionByTypeRequest = new GetTermsandConditionByTypeRequest();
            termsandConditionByTypeRequest.clientId = Constants.ClientId;
            termsandConditionByTypeRequest.typeId = 3;
            termsandConditionByTypeResponse = null;
            this.selectedVehicle = selectedVehicle;
            startDateLabel.Text = ((DateTime)reservationView.StartDate).ToString("ddd MM/dd/yyyy");
            endDateLabel.Text = ((DateTime)reservationView.EndDate).ToString("ddd MM/dd/yyyy");
            startTimeLabel.Text = ((DateTime)reservationView.StartDate).ToString("hh:mm tt");
            endTimeLabel.Text = ((DateTime)reservationView.EndDate).ToString("hh:mm tt");
            if (selectedVehicle.VehicleImageUrl != null)
            {
                vehilcleTypeImage.Source = ImageSource.FromUri(new Uri(selectedVehicle.VehicleImageUrl));
            }
            else if (selectedVehicle.VehicleTypeImageUrl != null)
            {
                vehilcleTypeImage.Source = ImageSource.FromUri(new Uri(selectedVehicle.VehicleTypeImageUrl));
            }
            vehicleSampleLabel.Text = selectedVehicle.vehicleName;
            vehilcleTypeLabel.Text = selectedVehicle.VehicleType;
            priceLabel.Text = "$ " + selectedVehicle.RateDetail.RateTotal.ToString();
            emailInvoiceRequest = new EmailInvoiceRequest();
            emailResponse = null;
        }

        //private async void LoginIcon_Clicked(object sender, EventArgs e)
        //{
        //    var assembly = typeof(SummaryOfChargesPage);
        //    if ((int)App.Current.Properties["CustomerId"] == 0)
        //    {

        //        loginIcon.IconImageSource = ImageSource.FromResource("SnapCarHire.Assets.logOutTool.png", assembly);
        //        await Navigation.PushAsync(new LoginPage());

        //    }
        //    else
        //    {
        //        bool logout = await DisplayAlert("Alert", "Are you sure want to logout", "Yes", "No");
        //        if (logout)
        //        {
        //            App.Current.Properties["CustomerId"] = 0;
        //            loginIcon.IconImageSource = ImageSource.FromResource("SnapCarHire.Assets.LogInTool.png", assembly);
        //            await Navigation.PushAsync(new BookNow());
        //        }
        //    }
        //}

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (PopupNavigation.Instance.PopupStack.Count > 0)
            {
                if (PopupNavigation.Instance.PopupStack[PopupNavigation.Instance.PopupStack.Count - 1].GetType() == typeof(ErrorWithClosePagePopup))
                {
                    await PopupNavigation.Instance.PopAllAsync();
                }
                else if (PopupNavigation.Instance.PopupStack[PopupNavigation.Instance.PopupStack.Count - 1].GetType() == typeof(ReservationSavedPopup))
                {
                    await PopupNavigation.Instance.PopAllAsync();
                    await Navigation.PushAsync(new HomePageDetail());
                }

            }

            //if (Constants.IsRegisteredandNotLogin)
            //{
            //    await Navigation.PushAsync(new LoginPage());
            //}
            else
            {

                if (reservationView.CustomerDriverList != null)
                {
                    if (reservationView.CustomerDriverList.Count > 0)
                    {
                        List<Driver> listForListVew = new List<Driver>();
                        foreach (Driver d in reservationView.CustomerDriverList)
                        {
                            if (d.IsDelete == false)
                            {

                                listForListVew.Add(d);
                            }
                        }
                        reservationView.CustomerDriverList = listForListVew;
                        // NoAdditionalDriverLabel.IsVisible = false;
                        // additionalDriverList.IsVisible = true;
                        // additionalDriverList.ItemsSource = listForListVew;
                        // additionalDriverList.HeightRequest = (listForListVew.Count) * 130;
                    }
                    else
                    {
                        ///NoAdditionalDriverLabel.IsVisible = true;
                        // additionalDriverList.IsVisible = false;
                    }
                }
                else
                {
                    // NoAdditionalDriverLabel.IsVisible = true;
                    // additionalDriverList.IsVisible = false;
                }



                bool busy = false;
                if (!busy)
                {
                    try
                    {
                        busy = true;
                        await PopupNavigation.Instance.PushAsync(new LoadingPopup("Getting summary details..."));

                        await Task.Run(() =>
                        {
                            try
                            {
                                summaryMobileResponsecs = getSummaryDetails(summaryMobileRequest, token);
                                termsandConditionByTypeResponse = getTermsAndConditions(termsandConditionByTypeRequest, token);
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

                        if (summaryMobileResponsecs != null)
                        {
                            if (summaryMobileResponsecs.message.ErrorCode == "200")
                            {
                                noOfDaysEntry.Text = reservationView.TotalDays.ToString();

                                totalRentalFeeEntry.Text = "$" + summaryMobileResponsecs.rate.RateTotal.ToString("0.00");

                                totalMisChargeEntry.Text = "$" + (Convert.ToDecimal(summaryMobileResponsecs.rate.ReservationSummary.TotacMiscNonTaxable) + Convert.ToDecimal(summaryMobileResponsecs.rate.ReservationSummary.TotacMiscTaxable)).ToString();

                                if (summaryMobileResponsecs.rate.ReservationSummary.TotalTax == null)
                                {
                                    totalTaxEntry.Text = "$0.00";
                                }
                                else if (summaryMobileResponsecs.rate.ReservationSummary.TotalTax != null)
                                {
                                    totalTaxEntry.Text = "$" + summaryMobileResponsecs.rate.ReservationSummary.TotalTax;
                                }



                                if (summaryMobileResponsecs.rate.ReservationSummary.EstimatedTotal == null)
                                {
                                    totalAmountEntry.Text = "$" + summaryMobileResponsecs.rate.EstimatedTotal.ToString();
                                }
                                else if (summaryMobileResponsecs.rate.ReservationSummary.EstimatedTotal != null)
                                {
                                    totalAmountEntry.Text = "$" + summaryMobileResponsecs.rate.ReservationSummary.EstimatedTotal;
                                }
                                DiscountEntry.Text = "$" + (Convert.ToDecimal(summaryMobileResponsecs.rate.ReservationSummary.PromoDiscountOnBaseRate) + Convert.ToDecimal(summaryMobileResponsecs.rate.ReservationSummary.PromoDiscountOnSubtotal)).ToString();
                            }
                            else
                            {
                                await PopupNavigation.Instance.PushAsync(new ErrorWithClosePagePopup(summaryMobileResponsecs.message.ErrorMessage));
                            }
                        }

                        if (termsandConditionByTypeResponse != null)
                        {
                            if (termsandConditionByTypeResponse.message.ErrorCode == "200")
                            {
                                if (termsandConditionByTypeResponse.termlist != null)
                                {
                                    if (termsandConditionByTypeResponse.termlist.Count > 0)
                                    {
                                        //terms.Text = termsandConditionByTypeResponse.termlist[0].Term;
                                        //tacDescription.Text = termsandConditionByTypeResponse.termlist[0].Description;

                                        // termList.ItemsSource = termsandConditionByTypeResponse.termlist;

                                        //var myDataTemplate = new DataTemplate(() =>
                                        //{
                                        //    var cell = new ViewCell();
                                        //    var termGrid = new Grid();
                                        //    int r = termsandConditionByTypeResponse.termlist.Count;

                                        //    for (int i = 0; i < r; i++)
                                        //    {
                                        //        termGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                                        //    }



                                        //    foreach (Terms terms in termsandConditionByTypeResponse.termlist)
                                        //    {
                                        //        var tCGrid = new Grid() { VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.Fill };

                                        //        tCGrid.Children.Add(new Label()  // adding the item as label
                                        //        {
                                        //            Text = terms.Term,
                                        //            TextColor = Color.Black,
                                        //            HorizontalOptions = LayoutOptions.FillAndExpand,
                                        //            HorizontalTextAlignment = TextAlignment.Center,
                                        //            VerticalTextAlignment = TextAlignment.Center,
                                        //            FontSize = 16
                                        //        }, 0, 0);

                                        //        tCGrid.Children.Add(new Label()  // adding the item as label
                                        //        {
                                        //            Text = terms.Description,
                                        //            TextColor = Color.Gray,
                                        //            HorizontalOptions = LayoutOptions.FillAndExpand,
                                        //            HorizontalTextAlignment = TextAlignment.Center,
                                        //            VerticalTextAlignment = TextAlignment.Center,
                                        //            FontSize = 16
                                        //        }, 0, 1);
                                        //        termGrid.Children.Add(tCGrid);
                                        //    }
                                        //    cell.View = termGrid;
                                        //    return cell;
                                        //});
                                        //data = myDataTemplate;
                                    }
                                }
                            }
                            else
                            {
                                await PopupNavigation.Instance.PushAsync(new ErrorWithClosePagePopup(termsandConditionByTypeResponse.message.ErrorMessage));
                            }
                        }
                    }
                }
            }



        }

        private GetTermsandConditionByTypeResponse getTermsAndConditions(GetTermsandConditionByTypeRequest termsandConditionByTypeRequest, string token)
        {
            GetTermsandConditionByTypeResponse Response = null;
            ReservationController controller = new ReservationController();
            try
            {
                Response = controller.getTermsAndConditions(termsandConditionByTypeRequest, token);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Response;
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

        private async void BookNowBtn_Clicked(object sender, EventArgs e)
        {
            if (TCcheckBox.IsChecked)
            {
                if ((int)App.Current.Properties["CustomerId"] == 0)
                {
                    await PopupNavigation.Instance.PushAsync(new AskForLogin("Please log-in or sign up to continue your booking"));
                }
                else
                {
                    reservationView.BasePrice = Convert.ToDecimal(summaryMobileResponsecs.rate.ReservationSummary.FinalBaseRate);
                    reservationView.PreSubTotal = Convert.ToDecimal(summaryMobileResponsecs.rate.ReservationSummary.PreSubTotal);
                    reservationView.TotalDiscountOnBaseRate = Convert.ToDecimal(summaryMobileResponsecs.rate.ReservationSummary.PromoDiscountOnBaseRate);
                    reservationView.TotalDiscount = Convert.ToDecimal(summaryMobileResponsecs.rate.ReservationSummary.PromoDiscountOnBaseRate);
                    reservationView.TotalDiscountOnSubTotal = Convert.ToDecimal(summaryMobileResponsecs.rate.ReservationSummary.PromoDiscountOnSubtotal);
                    reservationView.Status = (short)ReservationStatuses.Quote;
                    reservationView.StatusId = (short)ReservationStatuses.Quote;
                    reservationView.ReservationType = "Phone";
                    ReservationMobileRequest.reversationData = reservationView;


                    bool busy = false;
                    if (!busy)
                    {
                        try
                        {
                            busy = true;
                            await PopupNavigation.Instance.PushAsync(new LoadingPopup("Creating reservation..."));

                            await Task.Run(() =>
                            {
                                try
                                {
                                    ReservationMobileResponse = createReservationMobile(ReservationMobileRequest, token);
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

                            if (ReservationMobileResponse != null)
                            {
                                if (ReservationMobileResponse.message.ErrorCode == "200")
                                {
                                    if (Convert.ToInt32(ReservationMobileResponse.ReserveId) > 0)
                                    {
                                        //DependencyService.Get<INotification>().CreateNotification("Booking completed successfully", "Your reservation " + ReservationMobileResponse.ReservationNumber + " created successfully. Please check the details.", "ViewReservation", ReservationMobileResponse.ReserveId.ToString());
                                        await PopupNavigation.Instance.PushAsync(new SuccessPopUp("Thank you for your Tesla car rental reservation. A $300 security deposit is required on the day of the reservation. If you need to make any changes to your reservation, please contact us through the website at evolvecarrentals.com/contact. We look forward to seeing you soon and thank you for choosing our company to lower your carbon footprint. #RentOnePlantOne.", 1));
                                    }
                                }
                                else if (ReservationMobileResponse.message.ErrorCode == "120")
                                {
                                   
                                    await PopupNavigation.Instance.PushAsync(new Error_popup("You already have an active reservation or rental. You may only reserve one vehicle at a time", true));
                                }
                                else
                                {
                                    await PopupNavigation.Instance.PushAsync(new ErrorWithClosePagePopup(ReservationMobileResponse.message.ErrorMessage));
                                }

                            }
                        }
                    }
                }
            }
            else
            {
                await PopupNavigation.Instance.PushAsync(new Error_popup("Please accept the terms and conditions"));
            }
        }

        private void sendReservationEmail()
        {
            ReservationController reservationController = new ReservationController();
            emailInvoiceRequest.ClientID = Constants.ClientId;
            emailInvoiceRequest.TemplateTypeId = (int)EmailPurpose.Reservation;
            emailInvoiceRequest.PurposeId = ReservationMobileResponse.ReserveId;
            emailInvoiceRequest.ReservationId = ReservationMobileResponse.ReserveId;
            emailResponse = reservationController.sendReservationEmail(emailInvoiceRequest, token);

        }

        private CreateReservationMobileResponse createReservationMobile(CreateReservationMobileRequest reservationMobileRequest, string token)
        {
            CreateReservationMobileResponse response = null;
            ReservationController reservationController = new ReservationController();
            RegisterController registerController = new RegisterController();
            try
            {
                //bool isBookable = registerController.checkBookable((int)reservationMobileRequest.reversationData.CustomerId, token);
                //if (isBookable)
                //{
                    response = reservationController.createReservationMobile(reservationMobileRequest, token);
                //}
                //else
                //{
                //    ApiMessage message = new ApiMessage();
                //    message.ErrorMessage = "Can't bookable";
                //    message.ErrorCode = "120";
                //    response = new CreateReservationMobileResponse();
                //    response.message = message;
                //}

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        protected override bool OnBackButtonPressed()
        {
            if (PopupNavigation.Instance.PopupStack.Count > 0) { return true; }



            // Always return true because this method is not asynchronous.
            // We must handle the action ourselves: see above.
            return false;
        }



        /// <summary>
        /// additoinal driver btn click method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addDriverBtn_Clicked(object sender, EventArgs e)
        {
            if ((int)App.Current.Properties["CustomerId"] == 0)
            {
                PopupNavigation.Instance.PushAsync(new AskForLogin("Please log-in to add aditional driver"));
            }
            else
            {
                Navigation.PushAsync(new AddAditionalDriver(reservationView));
            }
        }

        private void additionalDriverList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            // Driver selecteddriver = additionalDriverList.SelectedItem as Driver;
            // Navigation.PushAsync(new DriverDetailPage(selecteddriver, reservationView));
        }

        private void additionalDriverList_Refreshing(object sender, EventArgs e)
        {

        }

        private void btnTerms_Tapped(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new Popups.TermAndConditionPopup(termsandConditionByTypeResponse.termlist));
        }

        private void btnBack_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}