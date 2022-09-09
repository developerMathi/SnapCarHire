using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using SnapCarHire.iOS;
using SnapCarHire.Renders;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtendedViewCell), typeof(ExtendedViewCellRenderer))]


namespace SnapCarHire.iOS
{
    public class ExtendedViewCellRenderer : ViewCellRenderer
    {
        public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
        {
            var cell = base.GetCell(item, reusableCell, tv);
            var view = item as ExtendedViewCell;
            if (cell != null)
            {
                cell.SelectedBackgroundView = new UIView
                {
                    BackgroundColor = view.SelectedBackgroundColor.ToUIColor(),
                };
            }

            return cell;
        }

    }
}