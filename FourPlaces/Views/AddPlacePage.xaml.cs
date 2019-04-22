using FourPlaces.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FourPlaces.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddPlacePage : ContentPage
	{
		public AddPlacePage ()
		{
			InitializeComponent ();
            BindingContext = new AddPlaceViewModel(Navigation);
        }
	}
}