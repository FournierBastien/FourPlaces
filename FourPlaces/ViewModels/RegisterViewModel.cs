using FourPlaces.Views;
using Storm.Mvvm;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FourPlaces.ViewModels
{
    // inscription
    public class RegisterViewModel : ViewModelBase
    {

        private string _msg = "";
        private string _email = "";
        private string _password = "";
        private string _confirmPassword = "";
        private string _firstname = "";
        private string _lastname = "";

        public ICommand RegisterCommand { protected set; get; }
        public ICommand NavigateToLoginCommand { protected set; get; }
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

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
        }

        public string FirstName
        {
            get => _firstname;
            set => SetProperty(ref _firstname, value);
        }

        public string LastName
        {
            get => _lastname;
            set => SetProperty(ref _lastname, value);
        }

        public RegisterViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            RegisterCommand = new Command(async () => { await RegisterAsync(); });
            NavigateToLoginCommand = new Command(async () => { await NavigateToLoginAsync(); });
        }

        private async Task NavigateToLoginAsync()
        {
            await Navigation.PushAsync(new LoginPage());
        }

        private async Task RegisterAsync()
        {

            if(Password == ConfirmPassword && Email != "" && Password != "" && FirstName != "" && LastName != "")
            {
                bool res = await App.SERVICE.Register(Email, Password, FirstName, LastName);
                if (res)
                {
                    Application.Current.MainPage = new RootPage();
                    //await Navigation.PushAsync(new HomePage());
                }
                else
                {
                    Password = "";
                    Msg = "Erreur lors de l'inscription";
                }
            }
            else
            {
                Msg = "Champs incorrect !";
            }
            
        }
    }
}
