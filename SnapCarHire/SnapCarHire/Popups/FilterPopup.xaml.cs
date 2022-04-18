using MaxVonGrafKftMobileModel;
using SnapCarHireModel;
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
    public partial class FilterPopup : PopupPage
    {
        private VehicleFilterSearch vehicleFilter;
        private List<string> vehicletypeList;
        private VehicleFilterMenus vehicleFilterMenus;



        public FilterPopup(VehicleFilterSearch vehicleFilter, VehicleFilterMenus vehicleFilterMenus)
        {
            InitializeComponent();
            this.vehicleFilter = vehicleFilter;
            this.vehicleFilterMenus = vehicleFilterMenus;
            this.vehicletypeList = vehicleFilterMenus.vehicleTypes;
            //           vehtypePicker.ItemsSource = vehicletypeList;

            fixModel(vehicleFilterMenus);
            //vehMakePicker.ItemsSource = vehicleFilterMenus.vehicleMakes; 
            //vehModelPicker.ItemsSource = vehicleFilterMenus.vehicleModels; 
            //vehYearPicker.ItemsSource = vehicleFilterMenus.VehicleYear; 
            //seatPicker.ItemsSource = vehicleFilterMenus.NumberOfseats; 
            //colorPicker.ItemsSource = vehicleFilterMenus.Colors;        
        }

        private void fixModel(VehicleFilterMenus vehicleFilterMenus)
        {
            if (vehicleFilter == null)
            {
                vehicleFilter = new VehicleFilterSearch();
            }

            if (vehicleFilterMenus.vehicleTypes != null)
            {
                List<FilterListViewCells> filterLists = new List<FilterListViewCells>();
                foreach (string s in vehicleFilterMenus.vehicleTypes)
                {
                    if (vehicleFilter.vehicleTypes != null)
                    {
                        if (vehicleFilter.vehicleTypes.Contains(s))
                        {
                            FilterListViewCells viewCells = new FilterListViewCells() { isSelected = true, title = s };
                            filterLists.Add(viewCells);
                        }
                        else
                        {
                            FilterListViewCells viewCells = new FilterListViewCells() { isSelected = false, title = s };
                            filterLists.Add(viewCells);
                        }
                    }
                    else
                    {
                        FilterListViewCells viewCells = new FilterListViewCells() { isSelected = false, title = s };
                        filterLists.Add(viewCells);
                    }

                }
                vehicleTypeList.ItemsSource = filterLists;
                vehicleTypeList.HeightRequest = filterLists.Count * 35;
            }
            if (vehicleFilterMenus.vehicleMakes != null)
            {
                List<FilterListViewCells> filterLists = new List<FilterListViewCells>();
                foreach (string s in vehicleFilterMenus.vehicleMakes)
                {
                    if (vehicleFilter.vehicleMakes != null)
                    {
                        if (vehicleFilter.vehicleMakes.Contains(s))
                        {
                            FilterListViewCells viewCells = new FilterListViewCells() { isSelected = true, title = s };
                            filterLists.Add(viewCells);
                        }
                        else
                        {
                            FilterListViewCells viewCells = new FilterListViewCells() { isSelected = false, title = s };
                            filterLists.Add(viewCells);
                        }
                    }
                    else
                    {
                        FilterListViewCells viewCells = new FilterListViewCells() { isSelected = false, title = s };
                        filterLists.Add(viewCells);
                    }


                }
                vehicleMakeList.ItemsSource = filterLists;
                vehicleMakeList.HeightRequest = filterLists.Count * 35;
            }
            if (vehicleFilterMenus.vehicleModels != null)
            {
                List<FilterListViewCells> filterLists = new List<FilterListViewCells>();
                foreach (string s in vehicleFilterMenus.vehicleModels)
                {
                    if (vehicleFilter.vehicleModels != null)
                    {
                        if (vehicleFilter.vehicleModels.Contains(s))
                        {
                            FilterListViewCells viewCells = new FilterListViewCells() { isSelected = true, title = s };
                            filterLists.Add(viewCells);
                        }
                        else
                        {
                            FilterListViewCells viewCells = new FilterListViewCells() { isSelected = false, title = s };
                            filterLists.Add(viewCells);
                        }
                    }
                    else
                    {
                        FilterListViewCells viewCells = new FilterListViewCells() { isSelected = false, title = s };
                        filterLists.Add(viewCells);
                    }

                }
                vehicleModelList.ItemsSource = filterLists;
                vehicleModelList.HeightRequest = filterLists.Count * 35;
            }
            if (vehicleFilterMenus.VehicleYear != null)
            {
                List<FilterListViewCells> filterLists = new List<FilterListViewCells>();
                foreach (int s in vehicleFilterMenus.VehicleYear)
                {
                    if (vehicleFilter.VehicleYear != null)
                    {
                        if (vehicleFilter.VehicleYear.Contains(s))
                        {
                            FilterListViewCells viewCells = new FilterListViewCells() { isSelected = true, title = s.ToString() };
                            filterLists.Add(viewCells);
                        }
                        else
                        {
                            FilterListViewCells viewCells = new FilterListViewCells() { isSelected = false, title = s.ToString() };
                            filterLists.Add(viewCells);
                        }
                    }
                    else
                    {
                        FilterListViewCells viewCells = new FilterListViewCells() { isSelected = false, title = s.ToString() };
                        filterLists.Add(viewCells);
                    }


                }
                vehicleYearList.ItemsSource = filterLists;
                vehicleYearList.HeightRequest = filterLists.Count * 35;
            }
            if (vehicleFilterMenus.Colors != null)
            {
                List<FilterListViewCells> filterLists = new List<FilterListViewCells>();
                foreach (string s in vehicleFilterMenus.Colors)
                {
                    if (vehicleFilter.Colors != null)
                    {
                        if (vehicleFilter.Colors.Contains(s))
                        {
                            FilterListViewCells viewCells = new FilterListViewCells() { isSelected = true, title = s };
                            filterLists.Add(viewCells);
                        }
                        else
                        {
                            FilterListViewCells viewCells = new FilterListViewCells() { isSelected = false, title = s };
                            filterLists.Add(viewCells);
                        }
                    }
                    else
                    {
                        FilterListViewCells viewCells = new FilterListViewCells() { isSelected = false, title = s };
                        filterLists.Add(viewCells);
                    }
                }
                vehicleColorList.ItemsSource = filterLists;
                vehicleColorList.HeightRequest = filterLists.Count * 35;
            }

        }

        private void btnClose_Tapped(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }

        private void cancelBtn_Clicked(object sender, EventArgs e)
        {
            vehicleFilter = new VehicleFilterSearch();
            PopupNavigation.Instance.PopAsync();
        }

        private void applyBtn_Clicked(object sender, EventArgs e)
        {
            List<FilterListViewCells> vehTypeItemSource = (List<FilterListViewCells>)vehicleTypeList.ItemsSource;
            List<FilterListViewCells> vehMakeItemSource = (List<FilterListViewCells>)vehicleMakeList.ItemsSource;
            List<FilterListViewCells> vehModelItemSource = (List<FilterListViewCells>)vehicleModelList.ItemsSource;
            List<FilterListViewCells> vehYearItemSource = (List<FilterListViewCells>)vehicleYearList.ItemsSource;
            List<FilterListViewCells> vehColorItemSource = (List<FilterListViewCells>)vehicleColorList.ItemsSource;
            vehicleFilter.vehicleTypes = new List<string>();
            vehicleFilter.vehicleMakes = new List<string>();
            vehicleFilter.vehicleModels = new List<string>();
            vehicleFilter.VehicleYear = new List<int>();
            vehicleFilter.Colors = new List<string>();
            if (vehTypeItemSource != null)
            {
                foreach (FilterListViewCells f in vehTypeItemSource)
                {
                    if (f.isSelected)
                    {
                        vehicleFilter.vehicleTypes.Add(f.title);
                    }
                }
            }
            if (vehMakeItemSource != null)
            {
                foreach (FilterListViewCells f in vehMakeItemSource)
                {
                    if (f.isSelected)
                    {
                        vehicleFilter.vehicleMakes.Add(f.title);
                    }
                }
            }
            if (vehModelItemSource != null)
            {
                foreach (FilterListViewCells f in vehModelItemSource)
                {
                    if (f.isSelected)
                    {
                        vehicleFilter.vehicleModels.Add(f.title);
                    }
                }
            }
            if (vehYearItemSource != null)
            {
                foreach (FilterListViewCells f in vehYearItemSource)
                {
                    if (f.isSelected)
                    {
                        vehicleFilter.VehicleYear.Add(Convert.ToInt32(f.title));
                    }
                }
            }
            if (vehColorItemSource != null)
            {
                foreach (FilterListViewCells f in vehColorItemSource)
                {
                    if (f.isSelected)
                    {
                        vehicleFilter.Colors.Add(f.title);
                    }
                }
            }

            //if (!string.IsNullOrEmpty(txtPrice.Text))
            //{
            //    vehicleFilter.Price = decimal.Parse(txtPrice.Text);
            //}
            //if (!string.IsNullOrEmpty(minPriceEntry.Text))
            //{
            //    vehicleFilter.MinPrice = decimal.Parse(minPriceEntry.Text);
            //}
            //if (!string.IsNullOrEmpty(seatEntry.Text ))
            //{
            //    vehicleFilter.seatsCount = int.Parse(seatEntry.Text);
            //}
            //if (!string.IsNullOrEmpty(bagEntry.Text))
            //{
            //    vehicleFilter.buggageCount = int.Parse(bagEntry.Text);
            //}
            //if (!string.IsNullOrEmpty(doorEntry.Text))
            //{
            //    vehicleFilter.DoorsCount = int.Parse(doorEntry.Text);
            //}
            //if(vehtypePicker.SelectedIndex!= -1)
            //{
            //    vehicleFilter.VehicleType = vehtypePicker.SelectedItem.ToString();
            //}
            //vehicleFilter.SortingOrder = orderPicker.SelectedIndex;

            MessagingCenter.Send(this, "FilterUpdated");
            PopupNavigation.Instance.PopAsync();
        }

        private async void vehtypePicker_Tapped(object sender, EventArgs e)
        {
            vehicleTypeList.IsVisible = !vehicleTypeList.IsVisible;
            await vehiTypeArrow.RotateTo(180, 200);
            if (!vehicleTypeList.IsVisible)
            {
                vehiTypeArrow.Rotation = 0;
            }
        }

        private async void vehMakePicker_Tapped(object sender, EventArgs e)
        {
            vehicleMakeList.IsVisible = !vehicleMakeList.IsVisible;
            await vehiMakeArrow.RotateTo(180, 200);
            if (!vehicleMakeList.IsVisible)
            {
                vehiMakeArrow.Rotation = 0;
            }
        }

        private async void vehModelPicker_Tapped(object sender, EventArgs e)
        {
            vehicleModelList.IsVisible = !vehicleModelList.IsVisible;
            await vehiModelArrow.RotateTo(180, 200);
            if (!vehicleModelList.IsVisible)
            {
                vehiModelArrow.Rotation = 0;
            }
        }

        private async void vehYearPicker_Tapped(object sender, EventArgs e)
        {
            vehicleYearList.IsVisible = !vehicleYearList.IsVisible;
            await vehiYearArrow.RotateTo(180, 200);
            if (!vehicleYearList.IsVisible)
            {
                vehiYearArrow.Rotation = 0;
            }
        }

        private async void vehColorPicker_Tapped(object sender, EventArgs e)
        {
            vehicleColorList.IsVisible = !vehicleColorList.IsVisible;
            await vehiColorArrow.RotateTo(180, 200);
            if (!vehicleColorList.IsVisible)
            {
                vehiColorArrow.Rotation = 0;
            }
        }
    }
}