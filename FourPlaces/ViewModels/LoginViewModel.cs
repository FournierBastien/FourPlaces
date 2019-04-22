using FourPlaces.Views;
using Storm.Mvvm;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FourPlaces.ViewModels
{
    // connexion
    public class LoginViewModel : ViewModelBase
    {
        private string _msg = "";
        private string _email = "bastien@gmail.com";
        private string _password = "imis";
        public ICommand LoginCommand { protected set; get; }
        public ICommand NavigateToRegisterCommand { protected set; get; }
        public INavigation Navigation { get; set; }

        public string Msg
        {
            get => _msg;
            set => SetProperty(ref _msg, value);
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public LoginViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            LoginCommand = new Command(async () => { await LoginAsync(); });
            NavigateToRegisterCommand = new Command(async () => { await NavigateToRegisterAsync(); });

        }

        public async Task LoginAsync()
        {
            if (Email != "" && Password != "")
            {
                bool res = await App.SERVICE.Authentification(Email, Password);
                if (res)
                {
                    Application.Current.MainPage = new RootPage();

                }
                else
                {
                    Password = "";
                    Msg = "Erreur lors de la connection";
                }
            }
            else
            {
                Password = "";
                //MessagingCenter.Send(this, "Login", "Champs Incorret !");
                Msg = "Champs incorrect !";
            }
            return;
        }

        public async Task NavigateToRegisterAsync()
        {
            await Navigation.PushAsync(new RegisterPage());
            return;
        }
    }
    
}
