using SnapCarHireController;
using SnapCarHireModel;
using SnapCarHireModel.AccessModels;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SnapCarHire.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class editPrrofilePhotoPage : PopupPage
    {
        private string _base64Image;
        private CustomerImages images;
        DateTime UploadedDate;
        string PhysicalPath;
        UploadCustomerImageMobileRequest imageMobileRequest;
        UploadCustomerImageMobileResponse imageMobileResponse;
        int customerID;
        string _token;

        public editPrrofilePhotoPage()
        {
            InitializeComponent();
            images = new CustomerImages();
            customerID = (int)App.Current.Properties["CustomerId"];
            _token = App.Current.Properties["currentToken"].ToString();
            imageMobileRequest = new UploadCustomerImageMobileRequest();
            imageMobileResponse = null;
        }

        public editPrrofilePhotoPage(CustomerImages images)
        {
            InitializeComponent();
            customerID = (int)App.Current.Properties["CustomerId"];
            _token = App.Current.Properties["currentToken"].ToString();
            imageMobileRequest = new UploadCustomerImageMobileRequest();
            imageMobileResponse = null;
            this.images = images;
            selectedImage.Source = ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(images.Base64)));
            _base64Image = images.Base64;
            UploadedDate = DateTime.Now;
            PhysicalPath = images.PhysicalPath;
            if (images.Base64 != null)
            {
                PhotoFrame.IsVisible = true;
                SaveBtn.IsVisible = true;
                cancelBtn.IsVisible = true;
            }
        }

        private async void CameraBtn_Clicked(object sender, EventArgs e)
        {
            try
            {
                await CrossMedia.Current.Initialize();

                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    _ = DisplayAlert("No Camera", ":( No camera available.", "OK");

                }
                else
                {
                    var files = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                    {
                        PhotoSize = PhotoSize.Medium,
                        SaveToAlbum = true,
                        ModalPresentationStyle = MediaPickerModalPresentationStyle.OverFullScreen
                    });

                    if (files == null)
                    {

                        return;
                    }
                    else
                    {
                        //imageBox.IsVisible = true;
                        //                    stream = files.GetStream();
                        selectedImage.Source = ImageSource.FromStream(() => files.GetStream());
                        PhotoFrame.IsVisible = true;
                        SaveBtn.IsVisible = true;
                        cancelBtn.IsVisible = true;


                        // provide read access to the file
                        FileStream fs = new FileStream(files.Path, FileMode.Open, FileAccess.Read);
                        // Create a byte array of file stream length
                        byte[] ImageData = new byte[fs.Length];
                        //Read block of bytes from stream into the byte array
                        fs.Read(ImageData, 0, System.Convert.ToInt32(fs.Length));
                        //Close the File Stream
                        fs.Close();
                        PhysicalPath = files.Path;
                        UploadedDate = DateTime.Now;
                        _base64Image = Convert.ToBase64String(ImageData);
                    }

                }



            }

            catch 
            {
                await DisplayAlert("Permissions Denied", "Unable to take photos.", "OK");
                //On iOS you may want to send your user to the settings screen.
                //CrossPermissions.Current.OpenAppSettings();
            }

        }

        private async void GaleryBtn_Clicked(object sender, EventArgs e)
        {

            try
            {
                await CrossMedia.Current.Initialize();
                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await DisplayAlert("Not Sopported", "Your device does not currently support this functionality", "OK");
                    return;

                }
                var selectedImages = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                {
                    PhotoSize = PhotoSize.Medium
                });

                if (selectedImages == null)
                {
                    await DisplayAlert("Error", "Could not get the image, Please try again", "Ok");
                    return;
                }
                else
                {
                    // provide read access to the file
                    FileStream fs = new FileStream(selectedImages.Path, FileMode.Open, FileAccess.Read);
                    // Create a byte array of file stream length
                    byte[] ImageData = new byte[fs.Length];
                    //Read block of bytes from stream into the byte array
                    fs.Read(ImageData, 0, System.Convert.ToInt32(fs.Length));
                    //Close the File Stream
                    fs.Close();
                    _base64Image = Convert.ToBase64String(ImageData);


                    //PhotoPath = selectedImage.Path;
                    //                    stream = selectedImage.GetStream();
                    //uploadTime = DateTime.Now;

                    //imageBox.IsVisible = true;
                    selectedImage.Source = ImageSource.FromStream(() => selectedImages.GetStream());
                    PhotoFrame.IsVisible = true;
                    SaveBtn.IsVisible = true;
                    cancelBtn.IsVisible = true;
                    PhysicalPath = selectedImages.Path;
                    UploadedDate = DateTime.Now;

                }
            }

            catch
            {
                await DisplayAlert("Permissions Denied", "Unable to access gallery.", "OK");
                //On iOS you may want to send your user to the settings screen.
                //CrossPermissions.Current.OpenAppSettings();
            }


        }

        private void CancelBtn_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAllAsync();
        }

        private async void SaveBtn_Clicked(object sender, EventArgs e)
        {
            CustomerImages customerImages = new CustomerImages();

            RegisterController registerController = new RegisterController();
            images.UploadedDate = UploadedDate;
            images.Base64 = _base64Image;
            images.PhysicalPath = PhysicalPath;
            if (images != null && customerID > 0)
            {
                imageMobileRequest.custImag = images;
                imageMobileRequest.custImag.CustomerID = customerID;
                imageMobileRequest.custImag.ImageID = customerID;
                imageMobileRequest.custImag.Title = "My Image";
                imageMobileRequest.custImag.FileName = "My Image";
                imageMobileRequest.custImag.Description = "My ImageMy ImageMy Image";

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

                                imageMobileResponse = registerController.addCustomerImage(imageMobileRequest, _token);
                            }
                            catch (Exception ex)
                            {
                                PopupNavigation.Instance.PushAsync(new ErrorWithClosePagePopup(ex.Message));
                            }
                            
                        });
                    }
                    finally
                    {
                        await PopupNavigation.Instance.PopAsync();
                        busy = false;
                        if (imageMobileResponse != null)
                        {
                            if (imageMobileResponse.message != null)
                            {
                                if (imageMobileResponse.message.ErrorCode == "200")
                                {
                                    if (Constants.customerDetails.CustomerImages == null)
                                    {
                                        Constants.customerDetails.CustomerImages = new List<CustomerImages>();
                                    }


                                    Constants.customerDetails.CustomerImages.Add(new CustomerImages() { Base64 = _base64Image });

                                    await PopupNavigation.Instance.PushAsync(new SuccessPopUp("Your profile picture updated successfully.",2));
                                }
                            }
                        }
                    }
                }
            }
        }

        private void btnClose_Tapped(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAllAsync();
        }
    }
}