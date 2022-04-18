using SnapCarHire.Views;
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
    public partial class DetailPopUp : PopupPage
    {
        private int v;

        public DetailPopUp(string name, string description)
        {
            InitializeComponent();
            titleLabel.Text = name;
            descriptionText.Text = description;
            v = 0;
        }

        public DetailPopUp(string name, string description, int v) : this(name, description)
        {
            this.v = v;
            if (v == 1)
            {
                reseendPassEmail.IsVisible = true;
            }
            else
            {
                reseendPassEmail.IsVisible = false;
            }
        }

        private void btnClose_Tapped(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }

        void reseendPassEmail_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PushModalAsync(new ForgetPasswordPage());
        }
    }
}