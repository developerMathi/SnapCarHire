using SnapCarHire.Utilties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;


namespace SnapCarHire.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeMenuPage : ContentPage
    {
        public List<HomePageMasterMenuItem> MenuItems { get; set; }
        public HomeMenuPage()
        {
            InitializeComponent();
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
            if (!Common.isRefresh)
            {
                MenuItems = new List<HomePageMasterMenuItem>(new[]
               {

                    new HomePageMasterMenuItem { Id = 0,BgColor = Color.Transparent,IconSource1=ImageSource.FromResource("SnapCarHire.Assets.iconWhiteDashboard.png"),IconSource=ImageSource.FromResource("SnapCarHire.Assets.iconWhiteDashboard.png"),  Title = "Dashboard" },
                    new HomePageMasterMenuItem { Id = 1,BgColor = Color.Transparent,IconSource1=ImageSource.FromResource("SnapCarHire.Assets.iconWhiteCar.png"),IconSource=ImageSource.FromResource("SnapCarHire.Assets.iconWhiteCar.png"), Title = "Book Now" },
                    new HomePageMasterMenuItem { Id = 2,BgColor = Color.Transparent,IconSource1=ImageSource.FromResource("SnapCarHire.Assets.iconWhiteRental.png"),IconSource=ImageSource.FromResource("SnapCarHire.Assets.iconWhiteRental.png"), Title = "My Rentals " },
                    new HomePageMasterMenuItem { Id = 4,BgColor = Color.Transparent,IconSource1=ImageSource.FromResource("SnapCarHire.Assets.iconWhiteUser.png"),IconSource=ImageSource.FromResource("SnapCarHire.Assets.iconWhiteUser.png"), Title = "My Profile" },
                    new HomePageMasterMenuItem { Id = 3,BgColor = Color.Transparent,IconSource1=ImageSource.FromResource("SnapCarHire.Assets.iconWhiteHelp.png"),IconSource=ImageSource.FromResource("SnapCarHire.Assets.iconWhiteHelp.png"), Title = "Help" },
                    new HomePageMasterMenuItem { Id = 5,BgColor = Color.Transparent,IconSource1=ImageSource.FromResource("SnapCarHire.Assets.iconWhiteStore.png"),IconSource=ImageSource.FromResource("SnapCarHire.Assets.iconWhiteStore.png"), Title = "Store" },
                    new HomePageMasterMenuItem { Id = 6,BgColor = Color.Transparent,IconSource1=ImageSource.FromResource("SnapCarHire.Assets.iconWhiteLogout.png"),IconSource=ImageSource.FromResource("SnapCarHire.Assets.iconWhiteLogout.png") , Title = "Log out" },
                   // new HomePageMasterMenuItem { Id = 2, Title = "Upcoming reservation " },
                   // new HomePageMasterMenuItem { Id = 3, Title = "My Rentals" },
                });
                Common.isRefresh = true;
                Common.homePageMasterMenuItems = MenuItems;
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            //foreach (var item in MenuItems)
            //{
            //    if(item.isSelected)
            //}



            MenuItemsListView.ItemsSource = Common.homePageMasterMenuItems.ToList();
        }
        private async void MenuItemsListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = e.Item as HomePageMasterMenuItem;
            ((Xamarin.Forms.ListView)sender).SelectedItem = null;
            foreach (var items in Common.homePageMasterMenuItems)
            {
                items.BgColor = Color.Transparent;
                items.txtColor = Color.White;
                items.IconSource1 = items.IconSource;
                item.isSelected = false;
            }

            if (item == null)
                return;

            item.BgColor = Color.White;
            item.txtColor = Color.FromHex("#DD0803");
            item.isSelected = true;

            if (item.Id == 0)
            {
                item.IconSource1 = ImageSource.FromResource("SnapCarHire.Assets.iconRedDashboard.png");
                Navigation.PushAsync(new HomePage());
            }
            if (item.Id == 1)
            {
                item.IconSource1 = ImageSource.FromResource("SnapCarHire.Assets.iconCar.png");
                Navigation.PushAsync(new BookNow());

            }
            if (item.Id == 2)
            {
                item.IconSource1 = ImageSource.FromResource("SnapCarHire.Assets.iconRentalFee.png");
                Navigation.PushAsync(new HomePage());
            }
            if (item.Id == 3)
            {
                item.IconSource1 = ImageSource.FromResource("SnapCarHire.Assets.iconRedHelp.png");
                //Help
                Navigation.PushAsync(new HelpFunnelPage());
            }
            if (item.Id == 4)
            {
                item.IconSource1 = ImageSource.FromResource("SnapCarHire.Assets.iconRedUser.png");
                Navigation.PushAsync(new MyProfile());
            }
            if (item.Id == 5)
            {
                item.IconSource1 = ImageSource.FromResource("SnapCarHire.Assets.iconRedStore.png");
                //Store
                Navigation.PushAsync(new HomePage());
            }
            if (item.Id == 6)
            {
                item.IconSource1 = ImageSource.FromResource("SnapCarHire.Assets.iconResOUTDT.png");
                //App.Current.Properties["CustomerId"] = 0;
                //Navigation.PushAsync(new WelcomPage());
                bool logout = await DisplayAlert("Alert", "Are you sure want to logout", "Yes", "No");
                if (logout)
                {
                    App.Current.Properties["CustomerId"] = 0;
                    await Navigation.PushAsync(new WelcomPage());
                }
                Common.isRefresh = false;
            }
        }
    }
}