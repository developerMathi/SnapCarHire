using SnapCarHireModel;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SnapCarHire.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PriceDetailPopup : PopupPage
    {
        private VehicleViewByTypeForMobile selectedVehicle;

        public PriceDetailPopup(VehicleViewByTypeForMobile selectedVehicle)
        {
            InitializeComponent();
            this.selectedVehicle = selectedVehicle;
            dailyRateDetailLabel.Text = "( " +selectedVehicle.RateDetail.DailyQty+" x $ "+((decimal)selectedVehicle.RateDetail.DailyRate).ToString("0.00")+ " )";
            weeklyRateDetailLabel.Text = "( " +selectedVehicle.RateDetail.WeeklyQty+" x $ "+((decimal)selectedVehicle.RateDetail.WeeklyRate).ToString("0.00")+ " )";
            monthlyRateDetailLabel.Text = "( " +selectedVehicle.RateDetail.MonthlyQty+" x $ "+((decimal)selectedVehicle.RateDetail.MonthlyRate).ToString("0.00")+ " )";
            weekEndRateDetailLabel.Text = "( " +selectedVehicle.RateDetail.WeekendDailyQty+" x $ "+((decimal)selectedVehicle.RateDetail.WeekendDayRate).ToString("0.00")+ " )";

            DailyRateTotal.Text= "$ "+((decimal)selectedVehicle.RateDetail.DailyQty * (decimal)selectedVehicle.RateDetail.DailyRate).ToString("0.00");
            weeklyRateTotal.Text= "$ "+((decimal)selectedVehicle.RateDetail.WeeklyQty * (decimal)selectedVehicle.RateDetail.WeeklyRate).ToString("0.00");
            monthlyRateTotal.Text= "$ "+((decimal)selectedVehicle.RateDetail.MonthlyQty * (decimal)selectedVehicle.RateDetail.MonthlyRate).ToString("0.00");
            weekEndRateTotal.Text= "$ "+((decimal)selectedVehicle.RateDetail.WeekendDailyQty * (decimal)selectedVehicle.RateDetail.WeekendDayRate).ToString("0.00");

            totalRateLabel.Text = "$ " + ((decimal)selectedVehicle.RateDetail.RateTotal).ToString("0.00");
        }

        private void btnClose_Tapped(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }
    }
}