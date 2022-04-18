using Rg.Plugins.Popup.Pages;
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
    public partial class LoadingPopup : PopupPage
    {
        public LoadingPopup( string content)
        {
            InitializeComponent();
            contentText.Text = content;
        }
    }
}