using FourPlaces.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FourPlaces.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditPasswordPage : ContentPage
	{
		public EditPasswordPage()
		{
            InitializeComponent();
            BindingContext = new EditPasswordViewModel(Navigation);
        }
	}
}