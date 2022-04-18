using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaxVonGrafKftMobileModel.AccessModels;
using SnapCarHire.Popups;
using SnapCarHireController;
using SnapCarHireModel;
using SnapCarHireModel.AccessModels;
using SnapCarHireModel.Constants;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SnapCarHire.Views
{
    public partial class OtherInformationPage : ContentPage
    {
        private CustomerReview customer;
        private string _token;
        GetAllStateForMobileResponse stateResponse = null;
        int customerID = 0;
        UploadCustomerImageMobileRequest imageMobileRequest;
        UploadCustomerImageMobileResponse imageMobileResponse;
        CustomerImages images;
        bool licExpireDateSelected;
        bool licIssueDateSelected;
        public static CustomerImages licfrontIamgeStat;
        public static CustomerImages licBackIamgeStat;
        AddLicenceImagesRequest LicenceImagesRequest;
        AddLicenceImageResponse licenceImageResponse;
        ConfirmEmailAddressRequest confirmEmailAddressRequest;
        ConfirmEmailAddressResponse emailAddressResponse;
        private int fromVal;

        public OtherInformationPage(CustomerReview customer, CustomerImages images)
        {
            InitializeComponent();
            this.customer = customer;
            licenceIssueDate.MaximumDate = DateTime.Now;
            licenceExpiryDate.MinimumDate = DateTime.Now;
            _token = App.Current.Properties["currentToken"].ToString();
            GetAllStateForMobileRequest stateRequest = new GetAllStateForMobileRequest();
            List<string> stateList = new List<string>();
            stateRequest.CountryID = (int)customer.CountryId;
            stateResponse = getStates(stateRequest, _token);
            if (stateResponse.stateList.Count > 0) { foreach (State s in stateResponse.stateList) { stateList.Add(s.StateCode); }; }
            licenceStatePicker.ItemsSource = stateList;
            imageMobileRequest = new UploadCustomerImageMobileRequest();
            imageMobileResponse = null;
            this.images = images;
            licExpireDateSelected = false;
            licIssueDateSelected = false;
            licfrontIamgeStat = new CustomerImages();
            licBackIamgeStat = new CustomerImages();
            LicenceImagesRequest = new AddLicenceImagesRequest();
            licenceImageResponse = null;
            customer.CustomerType = "Retail";
            confirmEmailAddressRequest = new ConfirmEmailAddressRequest();
            emailAddressResponse = null;
            this.fromVal = 0;

        }

        public OtherInformationPage(CustomerReview customer, CustomerImages images, int fromVal) : this(customer, images)
        {
            this.fromVal = fromVal;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            MessagingCenter.Subscribe<AddCustomerPhotoPopup>(this, "LicenceFrontImageAdded", sender =>
            {
                if (licfrontIamgeStat != null)
                {
                    licFrontImage.Source = ImageSource.FromFile(licfrontIamgeStat.PhysicalPath);
                }

            });

            MessagingCenter.Subscribe<AddCustomerPhotoPopup>(this, "LicenceBackImageAdded", sender =>
            {
                if (licBackIamgeStat != null)
                {
                    licBackImage.Source = ImageSource.FromFile(licBackIamgeStat.PhysicalPath);
                }

            });

            if (PopupNavigation.Instance.PopupStack.Count > 0)
            {
                if (PopupNavigation.Instance.PopupStack[PopupNavigation.Instance.PopupStack.Count - 1].GetType() == typeof(ErrorWithClosePagePopup))
                {
                    await PopupNavigation.Instance.PopAllAsync();
                }
            }

        }

        private GetAllStateForMobileResponse getStates(GetAllStateForMobileRequest stateRequest, string token)
        {
            CommonController stateController = new CommonController();
            GetAllStateForMobileResponse sResponse;
            sResponse = stateController.GetAllStateByCountryID(stateRequest, token);
            return sResponse;
        }

        private async void DoneBtn_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(licenceNumber.Text))
            {
                await PopupNavigation.Instance.PushAsync(new Error_popup("Please enter your drivers license number."));
            }
            else if (!licExpireDateSelected)
            {
                await PopupNavigation.Instance.PushAsync(new Error_popup("Please enter your drivers license expiration date."));
            }
            else if (licenceExpiryDate.Date <= DateTime.Now)
            {
                await PopupNavigation.Instance.PushAsync(new Error_popup("Your license has expired"));
            }
            else if (!licIssueDateSelected)
            {
                await PopupNavigation.Instance.PushAsync(new Error_popup("Please enter your drivers license issue date."));
            }
            else if (licenceIssueDate.Date >= DateTime.Now)
            {
                await PopupNavigation.Instance.PushAsync(new Error_popup("Please check your licence issue date."));
            }

            else if (licfrontIamgeStat.Base64 == null || licBackIamgeStat.Base64 == null)
            {
                await PopupNavigation.Instance.PushAsync(new Error_popup("Please add images of your driver licence. "));
            }
            else
            {
                bool busy = false;
                if (!busy)
                {
                    try
                    {
                        busy = true;
                        await PopupNavigation.Instance.PushAsync(new LoadingPopup("Saving your Information"));
                        await Task.Run(() =>
                        {
                            customer.LicenseNumber = licenceNumber.Text;
                            customer.LicenseExpiryDate = licenceExpiryDate.Date;

                            if (!licIssueDateSelected)
                            {
                                customer.LicenseIssueDate = null;
                            }
                            else if (licIssueDateSelected)
                            {
                                customer.LicenseIssueDate = licenceIssueDate.Date;
                            }

                            if (licenceStatePicker.SelectedIndex != -1)
                            {
                                customer.LicenseIssueState = licenceStatePicker.SelectedItem.ToString();
                            }
                            if (images != null)
                            {
                                imageMobileRequest.custImag = images;
                            }

                            RegisterController registerController = new RegisterController();
                            customerID = registerController.registerUser(customer, _token);
                            if (images != null && customerID > 0)
                            {
                                imageMobileRequest.custImag.CustomerID = customerID;
                                imageMobileRequest.custImag.ImageID = customerID;
                                imageMobileRequest.custImag.Title = "My Image";
                                imageMobileRequest.custImag.FileName = "My Image";
                                imageMobileRequest.custImag.Description = "My ImageMy ImageMy Image";
                                imageMobileResponse = registerController.addCustomerImage(imageMobileRequest, _token);
                            }
                            if (customerID > 0)
                            {
                                confirmEmailAddressRequest.ClientId = Constants.ClientId;
                                confirmEmailAddressRequest.CustomerId = customerID;
                                confirmEmailAddressRequest.Email = customer.Email;


                                if (licfrontIamgeStat.Base64 != null || licBackIamgeStat.Base64 != null)
                                {
                                    if (licfrontIamgeStat.Base64 != null)
                                    {
                                        LicenceImage licenceImage = new LicenceImage();
                                        licenceImage.base64DocumentContent = licfrontIamgeStat.Base64;
                                        licenceImage.refType = (short)ReferenceType.Customer;
                                        licenceImage.refId = customerID;
                                        LicenceImagesRequest.FrontImage = licenceImage;

                                    }
                                    if (licBackIamgeStat.Base64 != null)
                                    {
                                        LicenceImage licenceImage = new LicenceImage();
                                        licenceImage.base64DocumentContent = licBackIamgeStat.Base64;
                                        licenceImage.refType = (short)ReferenceType.Customer;
                                        licenceImage.refId = customerID;
                                        LicenceImagesRequest.BackImage = licenceImage;

                                    }
                                    try
                                    {
                                        licenceImageResponse = registerController.addLicenceImage(LicenceImagesRequest, _token);
                                        emailAddressResponse = ConfirmEmailAddress(confirmEmailAddressRequest);

                                    }
                                    catch (Exception ex)
                                    {
                                        PopupNavigation.Instance.PushAsync(new Error_popup(ex.Message));
                                    }
                                }
                            }

                        });
                    }
                    finally
                    {
                        busy = false;
                        await PopupNavigation.Instance.PopAllAsync();
                    }
                }
                if (customerID > 0)
                {
                    Constants.IsRegisteredandNotLogin = true;
                    Constants.cutomerAuthContext = new CutomerAuthContext() { CustomerEmail = customer.Email, CustomerId = customerID };

                    if (fromVal == 1)
                    {
                        await PopupNavigation.Instance.PushAsync(new SavedSuccessfullyPopup(customerID, fromVal));
                    }
                    else
                    {
                        await PopupNavigation.Instance.PushAsync(new SavedSuccessfullyPopup(customerID));
                    }

                }
                else
                {
                    await PopupNavigation.Instance.PushAsync(new Error_popup("Registration failed. Please try again"));
                }



            }

        }

        private string returnStateIdByStateName(string v)
        {
            int staID = 0;
            foreach (State p in stateResponse.stateList) { if (p.StateCode == v) { staID = p.StateID; } }
            return staID.ToString();
        }

        private void LicenceNumber_Focused(object sender, FocusEventArgs e)
        {
            licenceNumber.TextColor = (Color)Application.Current.Resources["GrayColor"];
        }

        private void BacKBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }



        private void licenceExpiryDate_DateSelected(object sender, DateChangedEventArgs e)
        {
            licExpireDateSelected = true;
        }

        private void licenceIssueDate_DateSelected(object sender, DateChangedEventArgs e)
        {
            licIssueDateSelected = true;
        }

        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new AddCustomerPhotoPopup(licfrontIamgeStat, 1));
        }

        private void backImageTap_Tapped(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new AddCustomerPhotoPopup(licBackIamgeStat, 2));
        }

        private void licimageUplaodViewer_Tapped(object sender, EventArgs e)
        {
            var assembly = typeof(OtherInformationPage);
            if (licImageFrame.IsVisible)
            {
                licImageFrame.IsVisible = false;
                licImageBtn.Source = ImageSource.FromResource("MaxVonGrafKftMobile.Assets.iconWhiteCamera.png", assembly);
            }
            else
            {
                licImageFrame.IsVisible = true;
                licImageBtn.Source = ImageSource.FromResource("MaxVonGrafKftMobile.Assets.iconWhiteUp.png", assembly);
            }
        }
        //protected override void OnDisappearing()
        //{
        //    base.OnDisappearing();
        //    MessagingCenter.Unsubscribe<AddCustomerPhotoPopup>(this, "LicenceFrontImageAdded");
        //    MessagingCenter.Unsubscribe<AddCustomerPhotoPopup>(this, "LicenceBackImageAdded");
        //}

        private ConfirmEmailAddressResponse ConfirmEmailAddress(ConfirmEmailAddressRequest confirmEmailAddressRequest)
        {
            ConfirmEmailAddressResponse response = null;
            LoginController controller = new LoginController();

            response = controller.ConfirmEmailAddress(confirmEmailAddressRequest, _token);
            return response;

        }
    }
}