using FourPlaces.ViewModels;
using Storm.Mvvm.Forms;
using Xamarin.Forms.Xaml;

namespace FourPlaces.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : BaseContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            
            BindingContext = new LoginViewModel(Navigation);

            //MessageCenter.Subscribe<LoginViewModel>(this, "Login");
            InvalidateMeasure();
        }
    }
}
