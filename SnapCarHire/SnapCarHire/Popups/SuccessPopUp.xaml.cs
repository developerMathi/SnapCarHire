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
    public partial class SuccessPopUp : PopupPage
    {
        private int v;
        private int r;
        private string email;

        public SuccessPopUp(string content)
        {
            InitializeComponent();
            contentText.Text = content;
            v = 0;
        }

        public SuccessPopUp(string content, int v)
        {
            InitializeComponent();
            contentText.Text = content;
            this.v = v;
        }
        public SuccessPopUp(string content, int v,int r)
        {
            InitializeComponent();
            contentText.Text = content;
            this.r = r;
            this.v = v;
        }

        public SuccessPopUp(string content, int v, string email)
        {
            InitializeComponent();
            contentText.Text = content;
            this.email = email;
            this.v = v;
            Okbtn.Text = "Continue Change Password";
            LoginBtn.IsVisible = true;
        }

        private async void Okbtn_Clicked(object sender, EventArgs e)
        {
            if (v == 0)
            {
                await PopupNavigation.Instance.PopAllAsync();
            }
            if (v == 1)
            {
                var pageOne = new HomePage();
                NavigationPage.SetHasNavigationBar(pageOne, false);
                NavigationPage mypage = new NavigationPage(pageOne);
                Application.Current.MainPage = mypage;
                while (Navigation.ModalStack.Count > 1)
                {
                    Navigation.PopModalAsync();
                }
                Navigation.PopModalAsync();
                PopupNavigation.Instance.PopAllAsync();
                
            }
            if (v == 2)
            {
                MessagingCenter.Send<App>((App)Application.Current, "profileImageUpdated");

                await PopupNavigation.PopAllAsync();
            }
            if (v == 3)
            {
                await Navigation.PopModalAsync();
                await PopupNavigation.PopAsync();
            }
            if (v == 5)
            {
                await Navigation.PushModalAsync(new ChangePasswordWithoutLogin(email));
            }
            if (v == 6)
            {
                while (Navigation.ModalStack.Count > 1)
                {
                    await Navigation.PopModalAsync();
                }
                await Navigation.PopModalAsync();
                await PopupNavigation.Instance.PopAsync();




            }
            if (v == 7)
            {
                while (Navigation.ModalStack.Count > 1)
                {
                    Navigation.PopModalAsync();
                }
                Navigation.PopModalAsync();
                PopupNavigation.Instance.PopAllAsync();
                App.Current.Properties["CustomerId"] = 0;
                App.Current.Properties["InquiryID"] = 0;
                Constants.cutomerAuthContext = null;
                var pageOne = new LoginPage();
                NavigationPage.SetHasNavigationBar(pageOne, false);
                NavigationPage mypage = new NavigationPage(pageOne);
                Application.Current.MainPage = mypage;


            }
        }

        void LoginBtn_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PushModalAsync(new LoginPage());
        }
    }
}