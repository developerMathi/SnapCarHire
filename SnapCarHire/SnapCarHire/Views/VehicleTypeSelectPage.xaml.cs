using SnapCarHireModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SnapCarHire.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VehicleTypeSelectPage : ContentPage
    {
        private ReservationView reservationView;


        public VehicleTypeSelectPage(ReservationView reservationView)
        {
            InitializeComponent();
            this.reservationView = reservationView;
            startDateLabel.Text = ((DateTime)reservationView.StartDate).ToString("MM/dd/yyyy");
            endDateLabel.Text = ((DateTime)reservationView.EndDate).ToString("MM/dd/yyyy");
            startTimeLabel.Text = ((DateTime)reservationView.StartDate).ToString("hh:mm tt");
            endTimeLabel.Text = ((DateTime)reservationView.EndDate).ToString("hh:mm tt");
        }

        private void carType_Tapped(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new VehicleDetailPage(reservationView,"Car"));
        }

        private void boatType_Tapped(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new VehicleDetailPage(reservationView,"Yacht"));
        }

        private void btnBack_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}