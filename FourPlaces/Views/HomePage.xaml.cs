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

            //Detail = new NavigationPage(new PlacesListPage());

            IsPresented = false;

            MenuList = new List<MasterPageItem>();
            // Adding menu items to menuList and you can define title ,page and icon
            MenuList.Add(new MasterPageItem() { Title = "Home", TargetType = typeof(HomePage) });
            /*MenuList.Add(new MasterPageItem() { Title = "Setting", IconSource = "contacts.png", TargetType = typeof(SettingPage) });
            MenuList.Add(new MasterPageItem() { Title = "Help", IconSource = "contacts.png", TargetType = typeof(HelpPage) });
            MenuList.Add(new MasterPageItem() { Title = "LogOut", IconSource = "contacts.png", TargetType = typeof(LogoutPage) });*/
            // Setting our list to be ItemSource for ListView in MainPage.xaml
            navigationDrawerList.ItemsSource = MenuList;
            // Initial navigation, this can be used for our home page
            Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(PlacesListPage)));
            /*var vm = new HomeViewModel(Navigation);
            BindingContext = vm;

            Console.WriteLine("initpage");
            PlacesList.IsPullToRefreshEnabled = true;
            PlacesList.RefreshCommand = vm.RefreshCommand;
            PlacesList.SetBinding(ListView.IsRefreshingProperty, nameof(HomeViewModel.IsBusy));

            Console.WriteLine("initpage2");
            PlacesList.ItemTapped += PlacesList_ItemTapped;*/

            /*PlacesList.ItemTapped += (sender, e) =>
            {
                PlacesList.SelectedItem = null;
            };*/
            //Console.WriteLine("initpage3");
            InvalidateMeasure();
        }

        /*private void PlacesList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            PlaceItemSummary lieu = (PlaceItemSummary)e.Item;
            Navigation.PushAsync(new DetailPage(lieu));
        }*/

        private void OnMenuItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = (MasterPageItem)e.SelectedItem;
            Type page = item.TargetType;
            Detail = new NavigationPage((Page)Activator.CreateInstance(page));
            IsPresented = false;
        }
    }
}