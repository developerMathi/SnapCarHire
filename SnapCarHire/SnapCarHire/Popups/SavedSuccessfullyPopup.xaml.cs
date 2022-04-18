using SnapCarHire.Views;
using SnapCarHireModel.Constants;
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
    public partial class SavedSuccessfullyPopup : PopupPage
    {
        private int customerID;
        private int fromVal;

        public SavedSuccessfullyPopup()
        {
            InitializeComponent();
        }

        public SavedSuccessfullyPopup(int customerID)
        {
            InitializeComponent();
            this.customerID = customerID;
            this.fromVal = 0;
        }

        public SavedSuccessfullyPopup(int customerID, int fromVal) : this(customerID)
        {
            this.fromVal = fromVal;
        }

        private async void Okbtn_Clicked(object sender, EventArgs e)
        {
            //if (Navigation.NavigationStack.Count > 1)
            //{
            //    for (var counter = 1; counter < 3; counter++)
            //    {
            //        Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
            //    }
            //    if (Navigation.NavigationStack[Navigation.NavigationStack.Count - 2].GetType() == typeof(LoginPage))
            //    {
            //        await Navigation.PushModalAsync(new enterConfirmationCodePage(customerID, emailConfirmationType.Register));
            //    }
            //    else if (Navigation.NavigationStack[Navigation.NavigationStack.Count - 2].GetType() == typeof(WelcomPage))
            //    {
            //        await Navigation.PushModalAsync(new enterConfirmationCodePage(customerID, emailConfirmationType.Register));
            //    }
            //    else if (Navigation.NavigationStack[Navigation.NavigationStack.Count - 2].GetType() == typeof(SummaryOfChargesPage))
            //    {
            //        await Navigation.PushModalAsync(new enterConfirmationCodePage(customerID, emailConfirmationType.Register));
            //    }
            //    else
            //    {
            //        await Navigation.PushModalAsync(new enterConfirmationCodePage(customerID, emailConfirmationType.Register));
            //    }
            //}
            //else
            //{
            //    await Navigation.PushModalAsync(new enterConfirmationCodePage(customerID, emailConfirmationType.Register));
            //}
            if (fromVal == 1)
            {
                await Navigation.PushModalAsync(new enterConfirmationCodePage(customerID, emailConfirmationType.Register, fromVal));
            }
            else
            {
                await Navigation.PushModalAsync(new enterConfirmationCodePage(customerID, emailConfirmationType.Register));
            }



            //await PopupNavigation.Instance.PopAllAsync();
        }
    }
}