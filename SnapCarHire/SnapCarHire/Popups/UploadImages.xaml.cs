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
    public partial class UploadImages : PopupPage
    {

        private string _base64Image;
        private CustomerImages images;
        DateTime UploadedDate;
        string PhysicalPath;
        public UploadImages()
        {
            InitializeComponent();
        }

        public UploadImages(CustomerImages images)
        {
            InitializeComponent();
            this.images = images;
            selectedImage.Source = ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(images.Base64)));
            _base64Image = images.Base64;
            UploadedDate = DateTime.Now;
            PhysicalPath = images.PhysicalPath;
            if (images.Base64 != null)
            {
                grdMain.IsVisible = false;
                PhotoFrame.IsVisible = true;
                // SaveBtn.IsVisible = true;
                // cancelBtn.IsVisible = true;
            }
            else
            {
                grdMain.IsVisible = true;
                PhotoFrame.IsVisible = false;
            }


        }

        private async void CameraBtn_Clicked(object sender, EventArgs e)
        {
            var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync<CameraPermission>();
            var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync<StoragePermission>();

            if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
            {
                cameraStatus = await CrossPermissions.Current.RequestPermissionAsync<CameraPermission>();
                storageStatus = await CrossPermissions.Current.RequestPermissionAsync<StoragePermission>();
            }

            if (cameraStatus == PermissionStatus.Granted && storageStatus == PermissionStatus.Granted)
            {


                await CrossMedia.Current.Initialize();

                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    _ = DisplayAlert("No Camera", ":( No camera available.", "OK");

                }


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
                    //PhotoFrame.IsVisible = true;
                    // SaveBtn.IsVisible = true;
                    // cancelBtn.IsVisible = true;
                    grdMain.IsVisible = false;
                    PhotoFrame.IsVisible = true;

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
            else
            {
                await DisplayAlert("Permissions Denied", "Unable to take photos.", "OK");
                //On iOS you may want to send your user to the settings screen.
                //CrossPermissions.Current.OpenAppSettings();
            }

        }

        private async void GaleryBtn_Clicked(object sender, EventArgs e)
        {
            var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync<CameraPermission>();
            var mediaLibraryStatus = await CrossPermissions.Current.CheckPermissionStatusAsync<MediaLibraryPermission>();
            var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync<StoragePermission>();

            if (mediaLibraryStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
            {
                mediaLibraryStatus = await CrossPermissions.Current.RequestPermissionAsync<MediaLibraryPermission>();
                storageStatus = await CrossPermissions.Current.RequestPermissionAsync<StoragePermission>();
            }
            if (mediaLibraryStatus == PermissionStatus.Granted && storageStatus == PermissionStatus.Granted)
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
                    //  PhotoFrame.IsVisible = true;
                    //SaveBtn.IsVisible = true;
                    //cancelBtn.IsVisible = true;
                    grdMain.IsVisible = false;
                    PhotoFrame.IsVisible = true;
                    PhysicalPath = selectedImages.Path;
                    UploadedDate = DateTime.Now;

                }
            }
            else
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

        private void SaveBtn_Clicked(object sender, EventArgs e)
        {
            images.UploadedDate = DateTime.Now;
            images.Base64 = _base64Image;
            images.PhysicalPath = PhysicalPath;
            PopupNavigation.Instance.PopAllAsync();
        }

        private void btnClose_Tapped(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAllAsync();
        }
    }
}