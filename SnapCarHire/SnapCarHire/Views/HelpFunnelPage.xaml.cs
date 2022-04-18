using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace SnapCarHire.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HelpFunnelPage : ContentPage
    {
        ImageSource iconUp = ImageSource.FromResource("SnapCarHire.Assets.iconWhiteUp.png");
        ImageSource iconDown = ImageSource.FromResource("SnapCarHire.Assets.iconWhiteDown.png");
        ImageSource iconUnSelect = ImageSource.FromResource("SnapCarHire.Assets.iconCircleUnselect.png");
        ImageSource iconSelect = ImageSource.FromResource("SnapCarHire.Assets.iconCircleSelected.png");

        public HelpFunnelPage()
        {
            InitializeComponent();
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
        }

        public void unselectedTab()
        {
            grdFlatTire.IsVisible = false;
            grdCarNotStart.IsVisible = false;
            grdAccident.IsVisible = false;
            grdFunnyNoise.IsVisible = false;
            grdHeadlight.IsVisible = false;

            imgFlat.Source = iconDown;
            imgCarNotStart.Source = iconDown;
            imgAccident.Source = iconDown;
            imgFunnyNoise.Source = iconDown;
            imgHeadlight.Source = iconDown;
        }

        private void btnFlat_Tapped(object sender, EventArgs e)
        {
           
            if (imgFlat.Source == iconUp)
            {
                unselectedTab();
                imgFlat.Source = iconDown;
                grdFlatTire.IsVisible = false;
            }
            else
            {
                unselectedTab();
                imgFlat.Source = iconUp;
                grdFlatTire.IsVisible = true;
            }
        }

        private void btnTireYes_Clicked(object sender, EventArgs e)
        {
            if (btnTireYes.Source == iconUnSelect)
            {
                btnTireYes.Source = iconSelect;
                lblTierYes.IsVisible = true;
                lblTierNo.IsVisible = false;
                btnTireNo.Source = iconUnSelect;
            }
            else
            {
                lblTierYes.IsVisible = false;
                lblTierNo.IsVisible = false;
                btnTireYes.Source = iconUnSelect;
            }
        }

        private void btnTireNo_Clicked(object sender, EventArgs e)
        {
            if (btnTireNo.Source == iconUnSelect)
            {
                lblTierYes.IsVisible = false;
                lblTierNo.IsVisible = true;
                btnTireNo.Source = iconSelect;
                btnTireYes.Source = iconUnSelect;
            }
            else
            {
                lblTierYes.IsVisible = false;
                lblTierNo.IsVisible = false;
                btnTireNo.Source = iconUnSelect;
            }
        }


        private void btnCarNotStart_Tapped(object sender, EventArgs e)
        {
            
            if (imgCarNotStart.Source == iconUp)
            {
                unselectedTab();
                imgCarNotStart.Source = iconDown;
                grdCarNotStart.IsVisible = false;
            }
            else
            {
                unselectedTab();
                imgCarNotStart.Source = iconUp;
                grdCarNotStart.IsVisible = true;
            }
        }

        private void btnCarNotStartYes_Clicked(object sender, EventArgs e)
        {
            if (btnCarNotStartYes.Source == iconUnSelect)
            {
                btnCarNotStartYes.Source = iconSelect;
                lblCarNotStartYes.IsVisible = true;
                lblCarNotStartNo.IsVisible = false;
                btnCarNotStartNo.Source = iconUnSelect;
            }
            else
            {
                lblCarNotStartYes.IsVisible = false;
                lblCarNotStartNo.IsVisible = false;
                btnCarNotStartYes.Source = iconUnSelect;
            }
        }

        private void btnCarNotStartNo_Clicked(object sender, EventArgs e)
        {
            if (btnCarNotStartNo.Source == iconUnSelect)
            {
                lblCarNotStartYes.IsVisible = false;
                lblCarNotStartNo.IsVisible = true;
                btnCarNotStartNo.Source = iconSelect;
                btnTireYes.Source = iconUnSelect;
            }
            else
            {
                lblCarNotStartYes.IsVisible = false;
                lblCarNotStartNo.IsVisible = false;
                btnCarNotStartNo.Source = iconUnSelect;
            }
        }

        private void btnAccident_Tapped(object sender, EventArgs e)
        {
           
            if (imgAccident.Source == iconUp)
            {
                unselectedTab();
                imgAccident.Source = iconDown;
                grdAccident.IsVisible = false;
            }
            else
            {
                unselectedTab();
                imgAccident.Source = iconUp;
                grdAccident.IsVisible = true;
            }
        }

        private void btnFunnyNoise_Tapped(object sender, EventArgs e)
        {
          
            if (imgFunnyNoise.Source == iconUp)
            {
                unselectedTab();
                imgFunnyNoise.Source = iconDown;
                grdFunnyNoise.IsVisible = false;
            }
            else
            {
                unselectedTab();
                imgFunnyNoise.Source = iconUp;
                grdFunnyNoise.IsVisible = true;
            }
        }

        private void btnHeadlight_Tapped(object sender, EventArgs e)
        {
         
            if (imgHeadlight.Source == iconUp)
            {
                unselectedTab();
                imgHeadlight.Source = iconDown;
                grdHeadlight.IsVisible = false;
            }
            else
            {
                unselectedTab();
                imgHeadlight.Source = iconUp;
                grdHeadlight.IsVisible = true;
            }
        }
    }
}