using FourPlaces.Model;
using FourPlaces.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace FourPlaces.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DetailPage : TabbedPage
    {
		public DetailPage (PlaceItemSummary place)
		{
            InitializeComponent();
            var vm = new DetailViewModel(Navigation, place);
            BindingContext = vm;

            CommentsList.IsPullToRefreshEnabled = true;
            CommentsList.RefreshCommand = vm.RefreshCommand;
            CommentsList.SetBinding(ListView.IsRefreshingProperty, nameof(HomeViewModel.IsRefreshing));
        }
	}
}