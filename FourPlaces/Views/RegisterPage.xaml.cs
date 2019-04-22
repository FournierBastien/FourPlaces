using FourPlaces.ViewModels;
using Storm.Mvvm.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FourPlaces.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RegisterPage : BaseContentPage
    {
		public RegisterPage ()
		{
            BindingContext = new RegisterViewModel(Navigation);
            InitializeComponent();
		}
	}
}