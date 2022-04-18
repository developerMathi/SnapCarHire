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
    public partial class VechicleInspectionPage : ContentPage
    {
        ImageSource iconUp = ImageSource.FromResource("SnapCarHire.Assets.iconWhiteUp.png");
        ImageSource iconDown = ImageSource.FromResource("SnapCarHire.Assets.iconWhiteDown.png");
        ImageSource iconUnSelect = ImageSource.FromResource("SnapCarHire.Assets.iconCircleUnselect.png");
        ImageSource iconSelect = ImageSource.FromResource("SnapCarHire.Assets.iconCircleSelected.png");
        public VechicleInspectionPage()
        {
            InitializeComponent();
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
        }

        public void unselectedTab()
        {
            grdWheelCover.IsVisible = false;
            grdMirrorDamage.IsVisible = false;
            grdWindshieldDamage.IsVisible = false;
            grdAntenna.IsVisible = false;
            grdEx1.IsVisible = false;
            grdStains.IsVisible = false;
            grdExample2.IsVisible = false;
            grdSpareTire.IsVisible = false;

            imgWheel.Source = iconDown;
            imgMirror.Source = iconDown;
            imgWind.Source = iconDown;
            imgAntenna.Source = iconDown;
            imgExample1.Source = iconDown;
            imgStains.Source = iconDown;
            imgExample2.Source = iconDown;
            imgSpaire.Source = iconDown;
        }

        private void btnWheel_Tapped(object sender, EventArgs e)
        {

            if (imgWheel.Source == iconUp)
            {
                unselectedTab();
                imgWheel.Source = iconDown;
                grdWheelCover.IsVisible = false;
            }
            else
            {
                unselectedTab();
                imgWheel.Source = iconUp;
                grdWheelCover.IsVisible = true;
            }
        }

        private void btnTireYes_Clicked(object sender, EventArgs e)
        {
            if (btnTireYes.Source == iconUnSelect)
            {
                btnTireYes.Source = iconSelect;
                grdWheel1.IsVisible = true;
                btnTireNo.Source = iconUnSelect;
            }
            else
            {
                grdWheel1.IsVisible = false;
                btnTireYes.Source = iconUnSelect;
            }
        }

        private void btnTireNo_Clicked(object sender, EventArgs e)
        {
            if (btnTireNo.Source == iconUnSelect)
            {
                grdWheel1.IsVisible = false;
                btnTireNo.Source = iconSelect;
                btnTireYes.Source = iconUnSelect;
            }
            else
            {
                grdWheel1.IsVisible = false;
                btnTireNo.Source = iconUnSelect;
            }
        }


        private void btnMirror_Tapped(object sender, EventArgs e)
        {

            if (imgMirror.Source == iconUp)
            {
                unselectedTab();
                imgMirror.Source = iconDown;
                grdMirrorDamage.IsVisible = false;
            }
            else
            {
                unselectedTab();
                imgMirror.Source = iconUp;
                grdMirrorDamage.IsVisible = true;
            }
        }

        private void btnMirrorYes_Tapped(object sender, EventArgs e)
        {
            if (btnMirrorYes.Source == iconUnSelect)
            {
                btnMirrorYes.Source = iconSelect;
                grdMirrorDamage1.IsVisible = true;
                btnMirrorNo.Source = iconUnSelect;
            }
            else
            {
                grdMirrorDamage1.IsVisible = false;
                btnMirrorYes.Source = iconUnSelect;
            }
        }

        private void btnMirrorNo_Tapped(object sender, EventArgs e)
        {
            if (btnMirrorNo.Source == iconUnSelect)
            {
                grdMirrorDamage1.IsVisible = false;
                btnMirrorNo.Source = iconSelect;
                btnMirrorYes.Source = iconUnSelect;
            }
            else
            {
                grdMirrorDamage1.IsVisible = false;
                btnMirrorNo.Source = iconUnSelect;
            }
        }


        private void btnWind_Tapped(object sender, EventArgs e)
        {

            if (imgWind.Source == iconUp)
            {
                unselectedTab();
                imgWind.Source = iconDown;
                grdWindshieldDamage.IsVisible = false;
            }
            else
            {
                unselectedTab();
                imgWind.Source = iconUp;
                grdWindshieldDamage.IsVisible = true;
            }
        }

        private void btnWindshieldYes_Tapped(object sender, EventArgs e)
        {
            if (btnWindshieldYes.Source == iconUnSelect)
            {
                btnWindshieldYes.Source = iconSelect;
                grdWindshieldDamage1.IsVisible = true;
                btnWindshieldNo.Source = iconUnSelect;
            }
            else
            {
                grdWindshieldDamage1.IsVisible = false;
                btnWindshieldYes.Source = iconUnSelect;
            }
        }
        private void btnWindshieldNo_Tapped(object sender, EventArgs e)
        {
            if (btnMirrorNo.Source == iconUnSelect)
            {
                grdWindshieldDamage1.IsVisible = false;
                btnWindshieldNo.Source = iconSelect;
                btnWindshieldYes.Source = iconUnSelect;
            }
            else
            {
                grdWindshieldDamage1.IsVisible = false;
                btnWindshieldNo.Source = iconUnSelect;
            }
        }


        private void btnAntenna_Tapped(object sender, EventArgs e)
        {

            if (imgAntenna.Source == iconUp)
            {
                unselectedTab();
                imgAntenna.Source = iconDown;
                grdAntenna.IsVisible = false;
            }
            else
            {
                unselectedTab();
                imgAntenna.Source = iconUp;
                grdAntenna.IsVisible = true;
            }
        }

        private void btnAntennaYes_Tapped(object sender, EventArgs e)
        {
            if (btnAntennaYes.Source == iconUnSelect)
            {
                btnAntennaYes.Source = iconSelect;
                grdAntenna1.IsVisible = true;
                btnAntennaNo.Source = iconUnSelect;
            }
            else
            {
                grdAntenna1.IsVisible = false;
                btnAntennaYes.Source = iconUnSelect;
            }
        }

        private void btnAntennaNo_Tapped(object sender, EventArgs e)
        {
            if (btnAntennaNo.Source == iconUnSelect)
            {
                grdAntenna1.IsVisible = false;
                btnAntennaNo.Source = iconSelect;
                btnAntennaYes.Source = iconUnSelect;
            }
            else
            {
                grdAntenna1.IsVisible = false;
                btnAntennaNo.Source = iconUnSelect;
            }
        }



        private void btnExample1_Tapped(object sender, EventArgs e)
        {

            if (imgExample1.Source == iconUp)
            {
                unselectedTab();
                imgExample1.Source = iconDown;
                grdEx1.IsVisible = false;
            }
            else
            {
                unselectedTab();
                imgExample1.Source = iconUp;
                grdEx1.IsVisible = true;
            }
        }

        private void btnEx1Yes_Tapped(object sender, EventArgs e)
        {
            if (btnEx1Yes.Source == iconUnSelect)
            {
                btnEx1Yes.Source = iconSelect;
                grdExp11.IsVisible = true;
                btnEx1No.Source = iconUnSelect;
            }
            else
            {
                grdExp11.IsVisible = false;
                btnEx1Yes.Source = iconUnSelect;
            }
        }
        private void btnEx1No_Tapped(object sender, EventArgs e)
        {
            if (btnEx1No.Source == iconUnSelect)
            {
                grdExp11.IsVisible = false;
                btnEx1No.Source = iconSelect;
                btnEx1Yes.Source = iconUnSelect;
            }
            else
            {
                grdExp11.IsVisible = false;
                btnEx1No.Source = iconUnSelect;
            }
        }


        private void btnStains_Tapped(object sender, EventArgs e)
        {

            if (imgStains.Source == iconUp)
            {
                unselectedTab();
                imgStains.Source = iconDown;
                grdStains.IsVisible = false;
            }
            else
            {
                unselectedTab();
                imgStains.Source = iconUp;
                grdStains.IsVisible = true;
            }
        }

        private void btnStainsYes_tapped(object sender, EventArgs e)
        {
            if (btnStainsYes.Source == iconUnSelect)
            {
                btnStainsYes.Source = iconSelect;
                grdStains1.IsVisible = true;
                btnStainsNo.Source = iconUnSelect;
            }
            else
            {
                grdStains1.IsVisible = false;
                btnStainsYes.Source = iconUnSelect;
            }
        }

        private void btnStainsNo_tapped(object sender, EventArgs e)
        {
            if (btnStainsNo.Source == iconUnSelect)
            {
                grdStains1.IsVisible = false;
                btnStainsNo.Source = iconSelect;
                btnStainsYes.Source = iconUnSelect;
            }
            else
            {
                grdStains1.IsVisible = false;
                btnStainsNo.Source = iconUnSelect;
            }
        }



        private void btnExample2_Tapped(object sender, EventArgs e)
        {

            if (imgExample2.Source == iconUp)
            {
                unselectedTab();
                imgExample2.Source = iconDown;
                grdExample2.IsVisible = false;
            }
            else
            {
                unselectedTab();
                imgExample2.Source = iconUp;
                grdExample2.IsVisible = true;
            }
        }

        private void btnEx2Yes_Tapped(object sender, EventArgs e)
        {
            if (btnEx2Yes.Source == iconUnSelect)
            {
                btnEx2Yes.Source = iconSelect;
                grdExample21.IsVisible = true;
                btnEx2No.Source = iconUnSelect;
            }
            else
            {
                grdExample21.IsVisible = false;
                btnEx2Yes.Source = iconUnSelect;
            }
        }

        private void btnEx2No_Tapped(object sender, EventArgs e)
        {
            if (btnEx2No.Source == iconUnSelect)
            {
                grdExample21.IsVisible = false;
                btnEx2No.Source = iconSelect;
                btnEx2Yes.Source = iconUnSelect;
            }
            else
            {
                grdExample21.IsVisible = false;
                btnEx2No.Source = iconUnSelect;
            }
        }



        private void btnSpaire_Tapped(object sender, EventArgs e)
        {

            if (imgSpaire.Source == iconUp)
            {
                unselectedTab();
                imgSpaire.Source = iconDown;
                grdSpareTire.IsVisible = false;
            }
            else
            {
                unselectedTab();
                imgSpaire.Source = iconUp;
                grdSpareTire.IsVisible = true;
            }
        }

        private void btnSpareTireYes_Tapped(object sender, EventArgs e)
        {
            if (btnSpareTireYes.Source == iconUnSelect)
            {
                btnSpareTireYes.Source = iconSelect;
                grdSpareTire1.IsVisible = true;
                btnSpareTireNo.Source = iconUnSelect;
            }
            else
            {
                grdSpareTire1.IsVisible = false;
                btnSpareTireYes.Source = iconUnSelect;
            }
        }

        private void btnSpareTireNo_Tapped(object sender, EventArgs e)
        {
            if (btnSpareTireNo.Source == iconUnSelect)
            {
                grdSpareTire1.IsVisible = false;
                btnSpareTireNo.Source = iconSelect;
                btnSpareTireYes.Source = iconUnSelect;
            }
            else
            {
                grdSpareTire1.IsVisible = false;
                btnSpareTireNo.Source = iconUnSelect;
            }
        }

        private void btnWheelUpload_Tapped(object sender, EventArgs e)
        {
            PopupNavigation.PushAsync(new Popups.UploadImages());
        }
    }
}