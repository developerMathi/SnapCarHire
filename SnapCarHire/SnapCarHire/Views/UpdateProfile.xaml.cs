using SnapCarHire.Popups;
using SnapCarHireController;
using SnapCarHireModel;
using SnapCarHireModel.AccessModels;
using SnapCarHireModel.Constants;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SnapCarHire.Views
{
    public partial class UpdateProfile : ContentPage
    {
        string _token;
        GetAllCountryForMobileResponse countryResponse;
        GetAllStateForMobileResponse stateResponse;
        private CustomerReview customerReview;
        UpdateCustomerProfileDetailsMobileRequest ProfileDetailsMobileRequest;
        UpdateCustomerProfileDetailsMobileResponse profileDetailsMobileResponse;
        GetCustomerPortalDetailsMobileRequest portalDetailsMobileRequest;
        GetCustomerPortalDetailsMobileResponse PortalDetailsMobileResponse;
        int customerId;
        CustomerController customoerController;
        static CustomerImages Images;
        bool licExpireDateSelected;
        bool licIssueDateSelected;
        public static CustomerImages licfrontIamgeStat;
        public static CustomerImages licBackIamgeStat;
        AddLicenceImagesRequest LicenceImagesRequest;
        AddLicenceImageResponse licenceImageResponse;
        string oldEmail;


        public UpdateProfile(CustomerReview customerReview)
        {
            InitializeComponent();
            DateOfBithEntry.MaximumDate = DateTime.Now.AddYears(-18);
            //licIssueDate.MaximumDate = DateTime.Now;
            licenceexpiDate.MinimumDate = DateTime.Now;
            this.customerReview = customerReview;
            _token = App.Current.Properties["currentToken"].ToString();
            customerId = (int)Application.Current.Properties["CustomerId"];
            countryResponse = null;
            stateResponse = null;
            ProfileDetailsMobileRequest = new UpdateCustomerProfileDetailsMobileRequest();
            profileDetailsMobileResponse = null;
            portalDetailsMobileRequest = new GetCustomerPortalDetailsMobileRequest();
            portalDetailsMobileRequest.customerId = customerId;
            PortalDetailsMobileResponse = null;
            customoerController = new CustomerController();
            Images = null;
            //licenceIssueDate.MaximumDate = DateTime.Now;
            //licenceExpiryDate.MinimumDate = DateTime.Now;
            licExpireDateSelected = false;
            licIssueDateSelected = false;
            licfrontIamgeStat = new CustomerImages();
            licBackIamgeStat = new CustomerImages();
            LicenceImagesRequest = new AddLicenceImagesRequest();
            licenceImageResponse = null;

            var editPhoto = new TapGestureRecognizer();
            editPhoto.Tapped += (s, e) =>
            {
                if (Images != null)
                {
                    if (Images.Base64 == null)
                    {
                        PopupNavigation.PushAsync(new editPrrofilePhotoPage());
                    }
                    else
                    {

                        PopupNavigation.PushAsync(new editPrrofilePhotoPage(Images));
                    }
                }
                else
                {
                    PopupNavigation.PushAsync(new editPrrofilePhotoPage());
                }

            };
            profileImage.GestureRecognizers.Add(editPhoto);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (PopupNavigation.Instance.PopupStack.Count > 0)
            {
                if (PopupNavigation.Instance.PopupStack[PopupNavigation.Instance.PopupStack.Count - 1].GetType() == typeof(ErrorWithClosePagePopup))
                {
                    await PopupNavigation.Instance.PopAllAsync();
                }
            }

            //MessagingCenter.Subscribe<AddCustomerPhotoPopup>(this, "LicenceFrontImageAdded", sender =>
            //{
            //    if (licfrontIamgeStat != null)
            //    {
            //        licFrontImage.Source = ImageSource.FromFile(licfrontIamgeStat.PhysicalPath);
            //    }

            //});

            //MessagingCenter.Subscribe<AddCustomerPhotoPopup>(this, "LicenceBackImageAdded", sender =>
            //{
            //    if (licBackIamgeStat != null)
            //    {
            //        licBackImage.Source = ImageSource.FromFile(licBackIamgeStat.PhysicalPath);
            //    }

            //});

            bool busy = false;
            if (!busy)
            {
                try
                {
                    busy = true;
                    await PopupNavigation.Instance.PushAsync(new LoadingPopup("Loading details..."));

                    await Task.Run(() =>
                    {

                        try
                        {
                            countryResponse = getAllCountry(_token);
                            PortalDetailsMobileResponse = getCustomerDetailsWithProfilePic(portalDetailsMobileRequest, _token);

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
                    else if (PopupNavigation.Instance.PopupStack.Count > 1)
                    {
                        if (PopupNavigation.Instance.PopupStack[PopupNavigation.Instance.PopupStack.Count - 2].GetType() == typeof(editPrrofilePhotoPage))
                        {
                            await PopupNavigation.Instance.PopAsync();
                        }
                        else if (PopupNavigation.Instance.PopupStack[PopupNavigation.Instance.PopupStack.Count - 2].GetType() == typeof(AddCustomerPhotoPopup))
                        {
                            await PopupNavigation.Instance.PopAsync();
                        }

                        else if (PopupNavigation.Instance.PopupStack[PopupNavigation.Instance.PopupStack.Count - 1].GetType() != typeof(ErrorWithClosePagePopup))
                        {
                            await PopupNavigation.Instance.PopAllAsync();
                        }
                    }
                }
                List<string> countryList = new List<string>();
                if (countryResponse.countryList.Count > 0) { foreach (Country k in countryResponse.countryList) { countryList.Add(k.CountryName); }; }
                //countryPicker.ItemsSource = countryList;
                FirstNameEntry.Text = customerReview.FirstName;

                oldEmail = customerReview.Email;
                emailEntry.Text = customerReview.Email;

                LastNameEntry.Text = customerReview.LastName;
                AddressEntry.Text = customerReview.Address1 + " " + customerReview.Address2;
                CityEntry.Text = customerReview.City;
                licenceNumber.Text = customerReview.LicenseNumber;
                DateOfBithEntry.Date = (DateTime)customerReview.DateOfbirth;
                if (customerReview.LicenseIssueDate == null)
                {
                    licIssueDate.Placeholder = "Licence Issue Date";
                }
                else if (((DateTime)customerReview.LicenseIssueDate).Date == DateTime.Now.Date)
                {
                    licIssueDate.Placeholder = DateTime.Now.ToString("MM/dd/yyyy");
                    licIssueDate.Date = (DateTime)customerReview.LicenseIssueDate;
                }
                else
                {
                    licIssueDate.Date = (DateTime)customerReview.LicenseIssueDate;
                }

                if (customerReview.LicenseExpiryDate == null)
                {
                    licenceexpiDate.Placeholder = "Licence Expiry Date";
                }

                else if (((DateTime)customerReview.LicenseExpiryDate).Date == DateTime.Now.Date)
                {
                    licenceexpiDate.Placeholder = DateTime.Now.ToString("dd/MM/yyyy");
                    licenceexpiDate.Date = (DateTime)customerReview.LicenseExpiryDate;
                }
                else
                {
                    licenceexpiDate.Date = (DateTime)customerReview.LicenseExpiryDate;

                }


                //if (customerReview.LicenseIssueDate != null)
                //{
                //    licenceIssueDate.Date = (DateTime)customerReview.LicenseIssueDate;
                //}
                //licenceExpiryDate.Date = (DateTime)customerReview.LicenseExpiryDate;
                //countryPicker.SelectedItem = customerReview.CountryName;
                if (PortalDetailsMobileResponse.customerReview != null)
                {
                    if (PortalDetailsMobileResponse.customerReview.CustomerImages.Count > 0)
                    {
                        if (PortalDetailsMobileResponse.customerReview.CustomerImages[PortalDetailsMobileResponse.customerReview.CustomerImages.Count - 1].Base64 != null)
                        {
                            Images = PortalDetailsMobileResponse.customerReview.CustomerImages[PortalDetailsMobileResponse.customerReview.CustomerImages.Count - 1];
                            byte[] Base64Stream = Convert.FromBase64String(PortalDetailsMobileResponse.customerReview.CustomerImages[PortalDetailsMobileResponse.customerReview.CustomerImages.Count - 1].Base64);
                            profileImage.Source = ImageSource.FromStream(() => new MemoryStream(Base64Stream));
                        }
                    }
                }

                string countryName = customerReview.CountryName;
                List<string> stateList = new List<string>();
                List<string> stateListForLicence = new List<string>();
                int? counid = null;
                foreach (Country c in countryResponse.countryList) { if (c.CountryName == countryName) { counid = c.CountryId; } };

                if (counid != null)
                {
                    GetAllStateForMobileRequest stateRequest = new GetAllStateForMobileRequest();
                    stateRequest.CountryID = counid.Value;
                    stateResponse = getStates(stateRequest, _token);
                    if (stateResponse.stateList.Count > 0) { foreach (State s in stateResponse.stateList) { stateList.Add(s.StateName); stateListForLicence.Add(s.StateCode); }; }
                    statePicker.ItemsSource = stateList;
                    LicStatePicker.ItemsSource = stateListForLicence;
                }

                statePicker.SelectedItem = customerReview.StateName;
                if (stateListForLicence.Contains(customerReview.LicenseIssueState))
                {
                    LicStatePicker.SelectedItem = customerReview.LicenseIssueState;
                }
                PostalCodeEntry.Text = customerReview.ZipCode;
                ContactNoEntry.Text = customerReview.hPhone;




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

        private async void UpdateBtn_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(FirstNameEntry.Text))
            {
                await PopupNavigation.Instance.PushAsync(new Error_popup("Please enter a first name."));
            }
            else if (string.IsNullOrEmpty(LastNameEntry.Text))
            {
                await PopupNavigation.Instance.PushAsync(new Error_popup("Please enter a last name."));
            }

            else if (!new EmailAddressAttribute().IsValid(emailEntry.Text) || string.IsNullOrEmpty(emailEntry.Text))
            {
                await PopupNavigation.Instance.PushAsync(new Error_popup("Please enter a valid email address"));
            }
            else if (string.IsNullOrEmpty(AddressEntry.Text))
            {
                await PopupNavigation.Instance.PushAsync(new Error_popup("Please enter your address."));
            }
            else if (DateOfBithEntry.Date.AddYears(18).AddDays(1) > DateTime.Now || DateOfBithEntry.Date == null)
            {
                await PopupNavigation.Instance.PushAsync(new Error_popup("You must be at least 21 years old to reserve a vehicle"));
            }
            else if (string.IsNullOrEmpty(CityEntry.Text))
            {
                await PopupNavigation.Instance.PushAsync(new Error_popup("Please enter your city."));
            }
            //else if (countryPicker.SelectedIndex == -1)
            //{
            //    await PopupNavigation.Instance.PushAsync(new Error_popup("Please select your country"));
            //}
            else if (statePicker.SelectedIndex == -1)
            {
                await PopupNavigation.Instance.PushAsync(new Error_popup("Please select your state"));
            }
            else if (string.IsNullOrEmpty(PostalCodeEntry.Text))
            {
                await PopupNavigation.Instance.PushAsync(new Error_popup("Please enter your zip code"));
            }
            else if (string.IsNullOrEmpty(ContactNoEntry.Text))
            {
                await PopupNavigation.Instance.PushAsync(new Error_popup("Please enter a valid contact number"));
            }

            else if (string.IsNullOrEmpty(licenceNumber.Text))
            {
                await PopupNavigation.Instance.PushAsync(new Error_popup("Please enter your drivers license number"));
            }
            //else if (!licExpireDateSelected)
            //{
            //    await PopupNavigation.Instance.PushAsync(new Error_popup("Please enter your drivers license expiration date"));
            //}
            //else if (licenceExpiryDate.Date <= DateTime.Now)
            //{
            //    await PopupNavigation.Instance.PushAsync(new Error_popup("Your license has expired"));
            //}
            else
            {
                customerReview.FirstName = FirstNameEntry.Text;
                customerReview.LastName = LastNameEntry.Text;
                customerReview.Address1 = AddressEntry.Text;
                customerReview.City = CityEntry.Text;
                //customerReview.CountryId = returnCountryIdByCountryName(countryPicker.SelectedItem.ToString());
                //customerReview.CountryName = countryPicker.SelectedItem.ToString();
                customerReview.StateId = returnStateIdByStateName(statePicker.SelectedItem.ToString());
                customerReview.StateName = statePicker.SelectedItem.ToString();
                customerReview.ZipCode = PostalCodeEntry.Text;
                customerReview.hPhone = ContactNoEntry.Text.Replace(" ", "").Replace("(", "").Replace(")", "").Replace("-", "");
                customerReview.ClientId = Constants.ClientId;
                customerReview.LicenseNumber = licenceNumber.Text;
                customerReview.LicenseExpiryDate = licenceexpiDate.Date;
                customerReview.LicenseIssueDate = licIssueDate.Date;
                customerReview.DateOfbirth = DateOfBithEntry.Date;

                if (licfrontIamgeStat.Base64 != null || licBackIamgeStat.Base64 != null)
                {
                    if (licfrontIamgeStat.Base64 != null)
                    {
                        LicenceImage licenceImage = new LicenceImage();
                        licenceImage.base64DocumentContent = licfrontIamgeStat.Base64;
                        licenceImage.refType = (short)ReferenceType.Customer;
                        licenceImage.refId = customerId;
                        LicenceImagesRequest.FrontImage = licenceImage;

                    }
                    if (licBackIamgeStat.Base64 != null)
                    {
                        LicenceImage licenceImage = new LicenceImage();
                        licenceImage.base64DocumentContent = licBackIamgeStat.Base64;
                        licenceImage.refType = (short)ReferenceType.Customer;
                        licenceImage.refId = customerId;
                        LicenceImagesRequest.BackImage = licenceImage;

                    }


                }



                //if (!licIssueDateSelected)
                //{
                //    customerReview.LicenseIssueDate = null;
                //}
                //else if (licIssueDateSelected)
                //{
                //    customerReview.LicenseIssueDate = licenceIssueDate.Date;
                //}

                if (LicStatePicker.SelectedIndex != -1)
                {
                    customerReview.LicenseIssueState = LicStatePicker.SelectedItem.ToString();
                }
                //else
                //{
                //    customerReview.LicenseIssueState = null;
                //}

                bool isExisting = false;

                if (oldEmail.ToLower() != emailEntry.Text.Replace(" ", "").ToLower())
                {
                    CustomerSerach customerSerach = new CustomerSerach();
                    customerSerach.Email = emailEntry.Text;
                    customerSerach.Active = true;
                    List<CustomerSeachResult> customerSeachResults = null;

                    customerSeachResults = customoerController.getCustomerByFilter(customerSerach, _token);

                    if (customerSeachResults != null)
                    {
                        if (customerSeachResults.Count > 0)
                        {
                            foreach (CustomerSeachResult csr in customerSeachResults)
                            {
                                if (csr.Email.ToLower() == emailEntry.Text.ToLower())
                                {
                                    isExisting = true;
                                }
                            }
                        }
                    }
                }
                customerReview.Email = emailEntry.Text;
                ProfileDetailsMobileRequest.custReview = customerReview;


                bool busy = false;
                if (!busy)
                {
                    if (!isExisting)
                    {
                        try
                        {
                            busy = true;
                            await PopupNavigation.Instance.PushAsync(new LoadingPopup("Updating your informations"));
                            await Task.Run(() =>
                            {
                                RegisterController registerController = new RegisterController();

                                try
                                {
                                    profileDetailsMobileResponse = registerController.updateUser(ProfileDetailsMobileRequest, _token);
                                    //if (licfrontIamgeStat.Base64 != null || licBackIamgeStat.Base64 != null)
                                    //{
                                    //    licenceImageResponse = registerController.addLicenceImage(LicenceImagesRequest, _token);
                                    //}

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
                                await PopupNavigation.Instance.PopAsync();
                            }
                            if (PopupNavigation.Instance.PopupStack.Count > 1)
                            {
                                if (PopupNavigation.Instance.PopupStack[PopupNavigation.Instance.PopupStack.Count - 1].GetType() != typeof(ErrorWithClosePagePopup) && PopupNavigation.Instance.PopupStack[PopupNavigation.Instance.PopupStack.Count - 1].GetType() != typeof(SuccessWithClosePopup))
                                {
                                    await PopupNavigation.Instance.PopAllAsync();
                                }
                            }
                            if (profileDetailsMobileResponse != null)
                            {
                                if (profileDetailsMobileResponse.message.ErrorCode == "200")
                                {
                                    Constants.customerDetails.FirstName = ProfileDetailsMobileRequest.custReview.FirstName;
                                    await PopupNavigation.Instance.PushAsync(new SuccessPopUp("Profile updated successfully!", 3));
                                }
                            }
                        }
                    }
                    else
                    {
                        await PopupNavigation.Instance.PushAsync(new Error_popup("Email address already exists.Please try a different email address."));
                    }
                }
            }
        }

        private int? returnStateIdByStateName(string v)
        {
            int staID = 0;
            foreach (State p in stateResponse.stateList) { if (p.StateName == v) { staID = p.StateID; } }
            return staID;
        }

        private int? returnCountryIdByCountryName(string v)
        {
            int? cntryId = null;
            foreach (Country o in countryResponse.countryList) { if (o.CountryName == v) { cntryId = o.CountryId; } }
            return cntryId.Value;
        }

        private GetAllCountryForMobileResponse getAllCountry(string access_token)
        {
            CommonController commonControllerCountry = new CommonController();
            GetAllCountryForMobileResponse countryResponse;
            countryResponse = commonControllerCountry.GetAllCountry(access_token);
            return countryResponse;
        }

        //private void CountryPicker_Unfocused(object sender, FocusEventArgs e)
        //{
        //    if (countryPicker.SelectedIndex != -1)
        //    {
        //        string countryName = countryPicker.SelectedItem.ToString();
        //        List<string> stateList = new List<string>();
        //        List<string> stateListForLicence = new List<string>();
        //        int? counid = null;
        //        foreach (Country c in countryResponse.countryList) { if (c.CountryName == countryName) { counid = c.CountryId; } };

        //        if (counid != null)
        //        {
        //            GetAllStateForMobileRequest stateRequest = new GetAllStateForMobileRequest();
        //            stateRequest.CountryID = counid.Value;
        //            stateResponse = getStates(stateRequest, _token);
        //            if (stateResponse.stateList.Count > 0) { foreach (State s in stateResponse.stateList) { stateList.Add(s.StateName); stateListForLicence.Add(s.StateCode); }; }
        //            statePicker.ItemsSource = stateList;
        //            licenceStatePicker.ItemsSource = stateListForLicence;
        //        }

        //    }

        //}
        private GetAllStateForMobileResponse getStates(GetAllStateForMobileRequest stateRequest, string token)
        {
            CommonController stateController = new CommonController();
            GetAllStateForMobileResponse sResponse;
            sResponse = stateController.GetAllStateByCountryID(stateRequest, token);
            return sResponse;
        }

        private void LicenceNumber_Focused(object sender, FocusEventArgs e)
        {

        }

        private void licenceExpiryDate_DateSelected(object sender, DateChangedEventArgs e)
        {
            licExpireDateSelected = true;
        }

        private void licenceIssueDate_DateSelected(object sender, DateChangedEventArgs e)
        {
            licIssueDateSelected = true;
        }

        private void calenderBtn_Clicked(object sender, EventArgs e)
        {
            DateOfBithEntry.Focus();
        }

        private void backImageTap_Tapped(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new AddCustomerPhotoPopup(licBackIamgeStat, 2));
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new AddCustomerPhotoPopup(licfrontIamgeStat, 1));
        }

        void btnBack_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}