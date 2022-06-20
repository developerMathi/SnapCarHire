//using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace SnapCarHire.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeTabbedPage : Xamarin.Forms.TabbedPage
    {
        [Obsolete]
        public HomeTabbedPage()
        {
            InitializeComponent();

            On<Android>().SetToolbarPlacement(ToolbarPlacement.Bottom)
             .SetBarItemColor(Xamarin.Forms.Application.Current.RequestedTheme == OSAppTheme.Dark ? Color.Gray : Color.Black)
             .SetBarSelectedItemColor(Xamarin.Forms.Application.Current.RequestedTheme == OSAppTheme.Dark ? Color.FromHex("#fecd0d") : Color.FromHex("#fecd0d"))
             .SetIsSwipePagingEnabled(false);

        }

        [Obsolete]
        protected override void OnAppearing()
        {
            base.OnAppearing();
            On<Android>().SetBarItemColor(Xamarin.Forms.Application.Current.RequestedTheme == OSAppTheme.Dark ? Color.Gray : Color.Black)
                 .SetBarSelectedItemColor(Xamarin.Forms.Application.Current.RequestedTheme == OSAppTheme.Dark ? Color.FromHex("#fecd0d") : Color.FromHex("#fecd0d"));

            this.SelectedItem = Dashboard;
        }
    }
}
