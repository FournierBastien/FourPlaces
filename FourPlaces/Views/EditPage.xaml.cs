using FourPlaces.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FourPlaces.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditPage : ContentPage
	{
		public EditPage ()
		{
			InitializeComponent ();
            BindingContext = new EditViewModel(Navigation);
        }
	}
}