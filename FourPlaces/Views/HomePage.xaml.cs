using FourPlaces.Model;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FourPlaces.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : MasterDetailPage
    {

        public List<MasterPageItem> MenuList { get; set; }

        public HomePage()
        {
            InitializeComponent();
            IsPresented = false;

            MenuList = new List<MasterPageItem>();

            MenuList.Add(new MasterPageItem() { Title = "Home", TargetType = typeof(HomePage) });

            navigationDrawerList.ItemsSource = MenuList;

            Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(PlacesListPage)));

            InvalidateMeasure();
        }

        private void OnMenuItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = (MasterPageItem)e.SelectedItem;
            Type page = item.TargetType;
            Detail = new NavigationPage((Page)Activator.CreateInstance(page));
            IsPresented = false;
        }
    }
}