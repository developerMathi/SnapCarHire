using SnapCarHireModel;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SnapCarHire.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VechicleInformationPage : ContentPage
    {
        private ReservationView reservationView;
        private VehicleViewByTypeForMobile selectedVehicle;


        public VechicleInformationPage(ReservationView reservationView, VehicleViewByTypeForMobile selectedVehicle)
        {
            InitializeComponent();
            this.reservationView = reservationView;
            this.selectedVehicle = selectedVehicle;
            if (selectedVehicle.VehicleTypeImageUrl != null)
            {
                vehileTypeImage.Source = ImageSource.FromUri(new Uri(selectedVehicle.VehicleTypeImageUrl));
            }
            vehicleSampleLabel.Text = selectedVehicle.sample;
            vehicleTypeLabel.Text = selectedVehicle.VehicleType;
            loactionLabel.Text = reservationView.StartLocationName + " - " + reservationView.EndLocationName;
            timeLineLabel.Text = ((DateTime)reservationView.StartDate).ToString("ddd,MM/dd,hh:mm") + " - " + ((DateTime)reservationView.EndDate).ToString("ddd,MM/dd,hh:mm");
            seatCountLabel.Text = selectedVehicle.Seats.ToString() + " Seats";
            bagCountLabel.Text = selectedVehicle.NoOfLuggage.ToString() + " Bags";
            gearLabel.Text = selectedVehicle.Transmission.ToString();
            if (selectedVehicle.doors != null)
            {
                doorCountLabel.Text = selectedVehicle.doors.ToString() + " Doors";
            }
            if (selectedVehicle.IsVehicleAvailableDescription != null)
            {
                VehicleDiscription.Text = selectedVehicle.IsVehicleAvailableDescription;
            }
            totalAmtLabel.Text = "$ " + selectedVehicle.RateDetail.RateTotal.ToString("0.00");
            //if (selectedVehicle.HtmlContent != null)
            //{
            //    desWebView = new WebView();

            //    var htmlSource = new HtmlWebViewSource();
            //    htmlSource.Html = @"<html><body>
            //                          <h1>Xamarin.Forms</h1>
            //                          <p>Welcome to WebView.</p>
            //                          </body></html>";
            //    desWebView.Source = htmlSource;
            //}
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (PopupNavigation.Instance.PopupStack.Count > 0)
            {
                PopupNavigation.Instance.PopAllAsync();
            }
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            if (selectedVehicle.SharableLink != null)
            {
                _ = ShareUri(selectedVehicle.SharableLink);
            }
            else
            {
                _ = ShareUri("");
            }
        }

        private void btnPriceDetail_Tapped(object sender, EventArgs e)
        {
            Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new Popups.PriceDetailPopup(selectedVehicle));
        }

        private void btnContinue_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new VehicleRates(reservationView, selectedVehicle));
        }

        public async Task ShareUri(string uri)
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Uri = uri,
                Title = "Vehicle details"
            });
        }
    }
}