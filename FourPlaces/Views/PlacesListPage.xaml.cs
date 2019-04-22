using FourPlaces.Model;
using FourPlaces.ViewModels;
using Storm.Mvvm.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FourPlaces.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PlacesListPage : BaseContentPage
	{

        RootPage root;

        public PlacesListPage (RootPage root)
		{
            InitializeComponent();

            
            var vm = new HomeViewModel(Navigation);
            BindingContext = vm;
            this.root = root;

            Title = "PlacesList";

            PlacesList.IsPullToRefreshEnabled = true;
            PlacesList.RefreshCommand = vm.RefreshCommand;
            PlacesList.SetBinding(ListView.IsRefreshingProperty, nameof(HomeViewModel.IsBusy));
            PlacesList.ItemTapped += PlacesList_ItemTapped;
        }

        private void PlacesList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            PlaceItemSummary lieu = (PlaceItemSummary)e.Item;
            this.root.Detail = new NavigationPage(new DetailPage(lieu));
        }
    }
}