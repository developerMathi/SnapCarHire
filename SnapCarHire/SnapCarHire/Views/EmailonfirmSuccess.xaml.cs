using SnapCarHireModel.Constants;
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
    public partial class EmailonfirmSuccess : ContentPage
    {
        private emailConfirmationType confirmationType;
        private int fromVal;

        public EmailonfirmSuccess()
        {
            InitializeComponent();
            confirmationType = emailConfirmationType.LogIn;
            this.fromVal = 0;
        }

        public EmailonfirmSuccess(emailConfirmationType confirmationType)
        {
            InitializeComponent();
            this.confirmationType = confirmationType;
            this.fromVal = 0;
        }

        public EmailonfirmSuccess(emailConfirmationType confirmationType, int fromVal) : this(confirmationType)
        {
            this.fromVal = fromVal;
        }

        private void doneBtn_Clicked(object sender, EventArgs e)
        {
            if (confirmationType == emailConfirmationType.LogIn)
            {
                //Navigation.PushAsync(new NavigationPage( new HomePage()));

                if (fromVal == 1)
                {
                    while (Navigation.ModalStack.Count > 1)
                    {
                        Navigation.PopModalAsync();
                    }
                    Navigation.PopModalAsync();
                }
                else
                {
                    while (Navigation.ModalStack.Count > 1)
                    {
                        Navigation.PopModalAsync();
                    }
                    Navigation.PopModalAsync();
                    var pageOne = new HomePage();
                    NavigationPage.SetHasNavigationBar(pageOne, false);
                    NavigationPage mypage = new NavigationPage(pageOne);
                    Application.Current.MainPage = mypage;
                }

            }
            else if (confirmationType == emailConfirmationType.Register)
            {
                if (fromVal == 1)
                {
                    gobackToLoginAsync();
                }
                else
                {
                    gobackToLoginAsync();
                    //Navigation.PushModalAsync(new LoginPage());
                }

            }
            else
            {
                if (fromVal == 1)
                {
                    gobackAsync();
                }
                else
                {
                    Navigation.PushModalAsync(new HomePage());
                }

            }

        }

        private void gobackToLoginAsync()
        {
            while (Navigation.ModalStack.Count > 1)
            {
                Navigation.PopModalAsync();
            }
            Navigation.PopModalAsync();


        }

        public async void gobackAsync()
        {


            await Navigation.PopToRootAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            return true;
        }
    }
}