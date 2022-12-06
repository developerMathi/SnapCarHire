using System;
using System.Collections.Generic;
using System.IO;
using Rg.Plugins.Popup.Services;
using SnapCarHireController;
using SnapCarHireModel;
using SnapCarHireModel.AccessModels;
using Xamarin.Forms;
using SnapCarHire.Popups;
using SignaturePad.Forms;

namespace SnapCarHire.Views
{
    public partial class AgreementScreen2 : ContentPage
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

        public AgreementScreen2(int agreementId, int vehicleId)
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

        private void VechileBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new DamageCheckListCheckOut(agreementId, AgreementByAgreementIdMobileResponse.custAgreement.AgreementDetail.AgreementNumber, (int)AgreementByAgreementIdMobileResponse.custAgreement.AgreementDetail.Status, vehicleId));
            //Navigation.PushAsync(new DamageCheckListCheckOut(agreementId, AgreementByAgreementIdMobileResponse.custAgreement.AgreementDetail.AgreementNumber, 2,vehicleId));
        }

        private void btnClear_Clicked(object sender, EventArgs e)
        {
            signatureView.Clear();
        }

        private void TCcheckBox_CheckChanged(object sender, EventArgs e)
        {
            //if (TCcheckBox.IsChecked)
            //{
            //    signatureFrame.IsVisible = true;
            //    signatureGrid.IsVisible = true;
            //}
            //else
            //{
            //    signatureFrame.IsVisible = false;
            //    signatureGrid.IsVisible = false;
            //}
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
                            await PopupNavigation.Instance.PushAsync(new Popups.SuccessPopUp("Your signature saved successfully"));
                            signatureView.IsEnabled = false;
                            signatureGrid.IsVisible = false;
                            //TCcheckBox.IsEnabled = false;
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
            MessagingCenter.Unsubscribe<Popups.FilterPopup>(this, "agreementUpdated");
        }

        private void btnBack_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}
