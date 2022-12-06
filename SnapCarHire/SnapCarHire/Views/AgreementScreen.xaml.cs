using SnapCarHire.Popups;
using SnapCarHireController;
using SnapCarHireModel;
using SnapCarHireModel.AccessModels;
using SnapCarHireModel.Constants;
using Rg.Plugins.Popup.Services;
using SignaturePad.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SnapCarHire.Views
{
    public partial class AgreementScreen : ContentPage
    {

        private int agreementId;
        private string token;
        GetAgreementByAgreementIdMobileRequest AgreementByAgreementIdMobileRequest;
        GetAgreementByAgreementIdMobileResponse AgreementByAgreementIdMobileResponse;
        AddAgreementSignMobileRequest signMobileRequest;
        AddAgreementSignMobileResponse signMobileResponse;
        ReloadSignatureImageURLMobileRequest imageURLMobileRequest;
        ReloadSignatureImageURLMobileResponse imageURLMobileResponse;
        GetVehicleDetailsMobileListResponse getVehicleDetailsMobile;
        VehicleController vehicleController;
        private int vehicleId;


        public AgreementScreen(int agreementId, int vehicleId)
        {
            InitializeComponent();
            this.agreementId = agreementId;
            token = App.Current.Properties["currentToken"].ToString();
            AgreementByAgreementIdMobileRequest = new GetAgreementByAgreementIdMobileRequest();
            AgreementByAgreementIdMobileRequest.agreementId = agreementId;
            AgreementByAgreementIdMobileResponse = null;
            signMobileResponse = null;
            signMobileRequest = new AddAgreementSignMobileRequest();
            imageURLMobileRequest = new ReloadSignatureImageURLMobileRequest();
            imageURLMobileRequest.agreementId = agreementId;
            imageURLMobileRequest.IsCheckIn = false;
            imageURLMobileRequest.isDamageView = false;
            imageURLMobileResponse = null;
            imageURLMobileRequest.SignatureImageUrl = "";
            this.vehicleId = vehicleId;
            vehicleController = new VehicleController();
            getVehicleDetailsMobile = null;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            MessagingCenter.Subscribe<ExtendPopup>(this, "agreementUpdated", sender =>
            {
                OnAppearing();

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
                    await PopupNavigation.Instance.PushAsync(new LoadingPopup("Loading agreement details..."));

                    await Task.Run(async () =>
                    {
                        try
                        {
                            //agreementResponse = getAgreementMobile(getAgreementByCustomerIdMobileRequest, token);
                            //customerAgreementModels = agreementResponse.listAgreements;
                            AgreementByAgreementIdMobileResponse = getAgreement(AgreementByAgreementIdMobileRequest, token, vehicleId);
                            imageURLMobileResponse = getDamageSignature(imageURLMobileRequest, token);
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
                    if (PopupNavigation.Instance.PopupStack.Count > 1)
                    {
                        if (PopupNavigation.Instance.PopupStack[PopupNavigation.Instance.PopupStack.Count - 1].GetType() != typeof(ErrorWithClosePagePopup))
                        {
                            await PopupNavigation.Instance.PopAllAsync();
                        }
                    }

                    if (getVehicleDetailsMobile != null)
                    {
                        if (getVehicleDetailsMobile.message.ErrorCode == "200")
                        {
                            if (getVehicleDetailsMobile.listVehicle.Count > 0)
                            {

                            }
                        }
                        else
                        {
                            await PopupNavigation.Instance.PushAsync(new ErrorWithClosePagePopup(getVehicleDetailsMobile.message.ErrorMessage));
                        }
                    }

                    if (AgreementByAgreementIdMobileResponse != null && imageURLMobileResponse != null)
                    {
                        if (AgreementByAgreementIdMobileResponse.message.ErrorCode == "200")
                        {
                            AgreementReviewDetailSet agreement = AgreementByAgreementIdMobileResponse.custAgreement;
                            agreementIDlabel.Text = agreement.AgreementDetail.AgreementNumber;
                            string statusString = Enum.GetName(typeof(AgreementStatusConst), agreement.AgreementDetail.Status);
                            statusLabel.Text = statusString;
                            CheckInLocation.Text = agreement.AgreementDetail.CheckoutLocationName;
                            //CheckInDate.Text = agreement.AgreementDetail.CheckoutDate.ToString("MM/dd/yyyy hh:mm tt");
                            CheckOutLocation.Text = agreement.AgreementDetail.CheckinLocationName;
                            //CheckOutDate.Text = agreement.AgreementDetail.CheckinDate.ToString("MM/dd/yyyy hh:mm tt");
                            CreateDate.Text = ((DateTime)agreement.AgreementDetail.CreatedDate).ToString("dddd, MM/dd/yyyy");
                            vehicleEntry.Text = agreement.AgreementDetail.VehicleMakeName + " " + agreement.AgreementDetail.ModelName + " " + agreement.AgreementDetail.Year;
                            VehicleType.Text = agreement.AgreementDetail.VehicleType;

                            pickUpDates.Text = String.Format("{0: dd MMMM yyyy}", agreement.AgreementDetail.CheckoutDate);
                            pickUpTime.Text = String.Format("{0: hh:mm tt}", agreement.AgreementDetail.CheckoutDate);
                            pickUpDay.Text = String.Format("{0: dddd}", agreement.AgreementDetail.CheckoutDate);

                            returnDates.Text = String.Format("{0: dd MMMM yyyy}", agreement.AgreementDetail.CheckinDate);
                            returnTime.Text = String.Format("{0: hh:mm tt}", agreement.AgreementDetail.CheckinDate);
                            returnDay.Text = String.Format("{0: dddd}", agreement.AgreementDetail.CheckinDate);

                            //licenceNoEntry.Text = agreement.AgreementDetail.LicenseNo;
                            if (AgreementByAgreementIdMobileResponse.agreementVehicle != null)
                            {
                                seatConut.Text = AgreementByAgreementIdMobileResponse.agreementVehicle.Seats;
                                bagCount.Text = AgreementByAgreementIdMobileResponse.agreementVehicle.Baggages.ToString();
                                //tranType.Text = AgreementByAgreementIdMobileResponse.agreementVehicle.Transmission;
                                vehicleImage.Source = ImageSource.FromUri(new Uri(AgreementByAgreementIdMobileResponse.agreementVehicle.ImageUrl));
                                carBackImage.Source = ImageSource.FromUri(new Uri(AgreementByAgreementIdMobileResponse.agreementVehicle.ImageUrl));
                            }



                            NoOfDays.Text = agreement.AgreementDetail.TotalDays.ToString();
                            TotalRentalFee.Text = "$ " + ((decimal)agreement.AgreementTotal.BaseCharge).ToString("0.00");
                            Discount.Text = "$ " + ((decimal)agreement.AgreementDetail.TotalDiscount).ToString("0.00");
                            TotalMisCharge.Text = "$ " + ((decimal)(agreement.AgreementTotal.TotalMiscCharge) + (decimal)(agreement.AgreementTotal.TotalMischargeWithOutTax == null ? (decimal)0.00 : agreement.AgreementTotal.TotalMischargeWithOutTax)).ToString("0.00");
                            TotalTaxCharge.Text = "$ " + ((decimal)agreement.AgreementTotal.TotalTax).ToString("0.00");
                            GrandTotal.Text = "$ " + ((decimal)agreement.AgreementTotal.TotalAmount).ToString("0.00");
                            AdvancePaid.Text = "$ " + ((decimal)agreement.AgreementTotal.AmountPaid).ToString("0.00");
                            BalanceDue.Text = "$ " + ((decimal)agreement.AgreementTotal.BalanceDue).ToString("0.00");

                            if(agreement.AgreementDetail.Status == (int)AgreementStatusConst.Open)
                            {
                               // ExtendBtn.IsVisible = true;
                            }
                            //if (agreement.AgreementDetail.SignatureImageUrl != null)
                            //{

                            //    signatureFrame.IsVisible = false;
                            //    signatureGrid.IsVisible = false;
                            //    imageFrame.IsVisible = true;
                            //    //WebClient webClient = new WebClient();

                            //    //byte[] imageBytes = webClient.DownloadData(agreement.AgreementDetail.SignatureImageUrl);


                            //    //FileStream file = new FileStream(agreement.AgreementDetail.SignatureImageUrl, FileMode.Open);

                            //    signatureImage.Source = ImageSource.FromFile(agreement.AgreementDetail.SignatureImageUrl);
                            //}

                            //else if (agreement.AgreementDetail.Status == 2)
                            //{

                            //    signatureFrame.IsVisible = true;
                            //    signatureGrid.IsVisible = true;
                            //}
                            //else
                            //{
                            //    signatureFrame.IsVisible = false;
                            //    signatureGrid.IsVisible = false;
                            //    noSignatureFrame.IsVisible = true;
                            //}
                            //if (agreement.AgreementDetail.Status == 3)
                            //{
                            //    signatureFrame.IsVisible = false;
                            //    signatureGrid.IsVisible = false;
                            //    imageFrame.IsVisible = false;
                            //    noSignatureFrame.IsVisible = false;
                            //}
                            //else 
                            if (imageURLMobileResponse.message.ErrorCode == "200")
                            {
                                if (!string.IsNullOrEmpty(imageURLMobileResponse.SignatureImageUrl))
                                {
                                    TCcheckBox.IsVisible = false;
                                    signatureFrame.IsVisible = false;
                                    signatureGrid.IsVisible = false;
                                    imageFrame.IsVisible = true;
                                    byte[] Base64Stream = Convert.FromBase64String(imageURLMobileResponse.SignatureImageUrl);
                                    signatureImage.Source = ImageSource.FromStream(() => new MemoryStream(Base64Stream));
                                }
                                else if (agreement.AgreementDetail.Status == (int)AgreementStatusConst.Open)
                                {
                                    TCcheckBox.IsVisible = true;
                                    signatureFrame.IsVisible = false;
                                    signatureGrid.IsVisible = false;
                                    imageFrame.IsVisible = false;
                                    noSignatureFrame.IsVisible = false;
                                }
                                else
                                {
                                    TCcheckBox.IsVisible = false;
                                    signatureFrame.IsVisible = false;
                                    signatureGrid.IsVisible = false;
                                    noSignatureFrame.IsVisible = true;
                                    imageFrame.IsVisible = false;
                                }
                            }
                            else if (agreement.AgreementDetail.Status == (int)AgreementStatusConst.Open)
                            {
                                //signatureFrame.IsVisible = false;
                                //signatureGrid.IsVisible = false;
                                //imageFrame.IsVisible = true;
                                //signatureImage.Source = ImageSource.FromUri(new Uri("D:\\Navotar\\Websites\\Images\\Signatures\\Agreement\\158233.jpg"));

                                TCcheckBox.IsVisible = true;
                                signatureFrame.IsVisible = false;
                                signatureGrid.IsVisible = false;
                                imageFrame.IsVisible = false;
                                noSignatureFrame.IsVisible = false;
                            }
                            else
                            {
                                TCcheckBox.IsVisible = false;
                                signatureFrame.IsVisible = false;
                                signatureGrid.IsVisible = false;
                                noSignatureFrame.IsVisible = true;
                                imageFrame.IsVisible = false;
                            }
                        }
                        else
                        {
                            await PopupNavigation.Instance.PushAsync(new ErrorWithClosePagePopup(AgreementByAgreementIdMobileResponse.message.ErrorMessage));
                        }
                    }
                }
            }
        }

        private void VechileBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new DamageCheckListCheckOut(agreementId, AgreementByAgreementIdMobileResponse.custAgreement.AgreementDetail.AgreementNumber, (int)AgreementByAgreementIdMobileResponse.custAgreement.AgreementDetail.Status,vehicleId));
            //Navigation.PushAsync(new DamageCheckListCheckOut(agreementId, AgreementByAgreementIdMobileResponse.custAgreement.AgreementDetail.AgreementNumber, 2,vehicleId));
        }

        private void btnClear_Clicked(object sender, EventArgs e)
        {
            signatureView.Clear();
        }

        private void TCcheckBox_CheckChanged(object sender, EventArgs e)
        {
            if (TCcheckBox.IsChecked)
            {
                signatureFrame.IsVisible = true;
                signatureGrid.IsVisible = true;
            }
            else
            {
                signatureFrame.IsVisible = false;
                signatureGrid.IsVisible = false;
            }
        }
        private GetAgreementByAgreementIdMobileResponse getAgreement(GetAgreementByAgreementIdMobileRequest agreementByAgreementIdMobileRequest, string token, int vehicleId)
        {
            AgreementController agreementController = new AgreementController();
            GetAgreementByAgreementIdMobileResponse response = null;
            try
            {
                response = agreementController.getAgreement(agreementByAgreementIdMobileRequest, token, vehicleId);
                //getVehicleDetailsMobile = vehicleController.getVehicleTypesMobile(token);
                //foreach(VehicleTypeMobileResult vtmr in getVehicleDetailsMobile.listVehicle)
                //{
                //    if(vtmr.ve)
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;
        }

        private ReloadSignatureImageURLMobileResponse getDamageSignature(ReloadSignatureImageURLMobileRequest imageURLMobileRequest, string token)
        {
            ReloadSignatureImageURLMobileResponse response = null;
            AgreementController controller = new AgreementController();
            try
            {
                response = controller.getDamageSignature(imageURLMobileRequest, token);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        private async void saveSignatureBtn_Clicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert("Alert", "Are you sure want to save ?", "Yes", "No");
            if (confirm)
            {

                signMobileRequest.agreementId = agreementId;
                //Stream bitmap = await signatureView.GetImageStreamAsync(SignatureImageFormat.Png);



                //StreamReader reader = new StreamReader(bitmap);

                //byte[] bytedata = System.Text.Encoding.Default.GetBytes(reader.ReadToEnd());

                Stream img = await signatureView.GetImageStreamAsync(SignatureImageFormat.Png);


                if (img != null)
                {
                    signatureView.IsEnabled = false;
                    BinaryReader br = new BinaryReader(img);
                    br.BaseStream.Position = 0;
                    Byte[] All = br.ReadBytes((int)img.Length);

                    string strBase64 = Convert.ToBase64String(All);
                    signMobileRequest.base64Img = strBase64;



                    signMobileResponse = saveSignature(signMobileRequest, token);
                    if (signMobileResponse != null)
                    {
                        if (signMobileResponse.message.ErrorCode == "200")
                        {
                            await PopupNavigation.Instance.PushAsync(new SuccessPopUp("Your signature saved successfully"));
                            signatureView.IsEnabled = false;
                            signatureGrid.IsVisible = false;
                            TCcheckBox.IsEnabled = false;
                        }
                        else
                        {
                            await PopupNavigation.Instance.PushAsync(new Error_popup(signMobileResponse.message.ErrorMessage));
                        }
                    }
                }
                else
                {
                    await PopupNavigation.Instance.PushAsync(new Error_popup("Invalid signature.Please try again"));
                }

            }
        }

        private AddAgreementSignMobileResponse saveSignature(AddAgreementSignMobileRequest signMobileRequest, string token)
        {
            AgreementController agreementController = new AgreementController();
            AddAgreementSignMobileResponse response = null;
            try
            {
                response = agreementController.saveSignature(signMobileRequest, token);
            }
            catch (Exception e)
            {
                throw e;
            }
            return response;
        }
     

        private void ExtendBtn_Clicked(object sender, EventArgs e)
        {
            AgreementByAgreementIdMobileResponse.custAgreement.AgreementDetail.RateDetailsList = AgreementByAgreementIdMobileResponse.custAgreement.RateDetailsList;
            AgreementByAgreementIdMobileResponse.custAgreement.AgreementDetail.AgreementInsurance = AgreementByAgreementIdMobileResponse.custAgreement.AgreementInsuranceReview;
            AgreementByAgreementIdMobileResponse.custAgreement.AgreementDetail.vehicleResponse = new GetVehicleIdByCodeResponse();
            AgreementByAgreementIdMobileResponse.custAgreement.AgreementDetail.vehicleResponse.VehicleID = AgreementByAgreementIdMobileResponse.agreementVehicle.VehicleId.ToString();
            PopupNavigation.Instance.PushAsync(new Popups.ExtendPopup(AgreementByAgreementIdMobileResponse.custAgreement.AgreementDetail));
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<FilterPopup>(this, "agreementUpdated");
        }

        private void btnBack_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}