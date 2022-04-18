using SnapCarHire.Utilties;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SnapCarHire.Views
{
    public partial class HomePage : MasterDetailPage
    {
        public HomePage()
        {
            InitializeComponent();
            Common.mMasterPage = this;
            Common.mMasterPage.Master = new HomePageMaster();
            //MasterPage.ListView.ItemSelected += ListView_ItemSelected;
            Constants.IsHome = true;



            if (PopupNavigation.Instance.PopupStack.Count > 0)
            {
                PopupNavigation.Instance.PopAllAsync();
            }


        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as HomePageMasterMenuItem;
            ((ListView)sender).SelectedItem = null;
            if (item == null)
                return;
            if (item.Id == 0)
            {
                Navigation.PushAsync(new HomePage());
            }
            if (item.Id == 1)
            {
                Navigation.PushAsync(new VehicleDetailPage());

            }
            if (item.Id == 2)
            {
                Navigation.PushAsync(new UpcomingReservations());
            }
            if (item.Id == 3)
            {
                Navigation.PushAsync(new MyRentals());
            }
            if (item.Id == 4)
            {
                Navigation.PushAsync(new MyProfile());
            }
            if (item.Id == 5)
            {
                App.Current.Properties["CustomerId"] = 0;
                Navigation.PushAsync(new WelcomPage());
            }
        }
        protected override bool OnBackButtonPressed()
        {
            if (Constants.IsHomeDetail)
            {
                Type type = typeof(WelcomPage);
                if (PopupNavigation.Instance.PopupStack.Count > 0) { return true; }
                else if (Constants.IsHome == true)
                {
                    Constants.IsHome = false;

                    int c = Navigation.NavigationStack.Count;
                    
                    while (Navigation.NavigationStack.Count >= 2)
                    {
                        Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
                    }
                    bool result =false;
                    //Device.BeginInvokeOnMainThread(async () =>
                    //{
                    //     result = await this.DisplayAlert("Alert!", "Are you sure you want to close this application ?", "Yes", "No");
                    //    if (result)
                    //    {
                    //        base.OnBackButtonPressed();
                    //        //Process.GetCurrentProcess().CloseMainWindow();
                    //        //Process.GetCurrentProcess().Close();

                            
                    //    }
                       
                    //    //else
                    //    //{
                    //    //    return false;
                    //    //}
                    //});

                    return false;
                    //bool isWantToExit = await DisplayAlert("Alert", "are you sure you want to close this application?", "Yes", "Cancel");
                }

                else
                {
                    bool result=false;
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                         result = await this.DisplayAlert("Alert!", "Are you sure want to close this application?", "Yes", "No");
                        
                    });
                    if (result)
                    {
                        //Process.GetCurrentProcess().CloseMainWindow();
                        //Process.GetCurrentProcess().Close();
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            else
            {
                if (PopupNavigation.Instance.PopupStack.Count > 0) { return true; }
                // Always return true because this method is not asynchronous.
                // We must handle the action ourselves: see above.
                else if (Navigation.ModalStack.Count > 0)
                {
                    return false;
                }
                else
                {
                    var pageOne = new HomePage();
                    NavigationPage.SetHasNavigationBar(pageOne, false);
                    NavigationPage mypage = new NavigationPage(pageOne);
                    Application.Current.MainPage = mypage;
                    return true;
                }
            }

            // Always return true because this method is not asynchronous.
            // We must handle the action ourselves: see above.

        }
    }
}