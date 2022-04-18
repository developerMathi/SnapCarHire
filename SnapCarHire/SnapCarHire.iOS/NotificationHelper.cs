using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using SnapCarHire.iOS;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(NotificationHelper))]

namespace SnapCarHire.iOS
{
    class NotificationHelper : INotification
    {
        public void CreateNotification(string title, string message, string pageName, string data)
        {
            new NotificationDelegate().RegisterNotification(title, message);
        }
    }
}