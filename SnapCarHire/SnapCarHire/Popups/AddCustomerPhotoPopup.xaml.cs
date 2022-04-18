using SnapCarHireModel;
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
    public partial class AddCustomerPhotoPopup : PopupPage
    {
        private string _base64Image;
        private CustomerImages images;
        DateTime UploadedDate;
        string PhysicalPath;
        private int v;

        public AddCustomerPhotoPopup()
        {
            InitializeComponent();
        }

        public AddCustomerPhotoPopup(CustomerImages images)
        {
            InitializeComponent();
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

        public AddCustomerPhotoPopup(CustomerImages images, int v)
        {
            InitializeComponent();
            this.v = v;
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
            if (v == 1)
            {
                title.Text = "Licence Front Image";
            }
            else if (v == 2)
            {
                title.Text = "Licence Back Image";
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var results = CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera, Permission.Storage });
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
                        PhotoSize = PhotoSize.Small,
                        SaveToAlbum = true,
                        //ModalPresentationStyle = MediaPickerModalPresentationStyle.OverFullScreen,
                        AllowCropping = true
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
            catch (Exception ex)
            {
                //await PopupNavigation.Instance.PushAsync(new Error_popup(ex.Message));
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
                    PhotoSize = PhotoSize.Small
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
            catch (Exception ex)
            {
                //await PopupNavigation.Instance.PushAsync(new Error_popup(ex.Message));
            }
        }

        private void CancelBtn_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAllAsync();
        }

        private void SaveBtn_Clicked(object sender, EventArgs e)
        {
            if (images == null)
            {
                images = new CustomerImages();
            }
            images.UploadedDate = DateTime.Now;
            images.Base64 = _base64Image;
            images.PhysicalPath = PhysicalPath;
            if (v == 1)
            {
                MessagingCenter.Send(this, "LicenceFrontImageAdded");
            }
            else if (v == 2)
            {
                MessagingCenter.Send(this, "LicenceBackImageAdded");
            }
            else
            {
                MessagingCenter.Send(this, "profilePhotoAdded");
            }

            PopupNavigation.Instance.PopAllAsync();
        }
    }
}