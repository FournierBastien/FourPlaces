using Storm.Mvvm;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FourPlaces.ViewModels
{
    public class EditPasswordViewModel : ViewModelBase
    {
        private string password;
        private string newPassword;
        private string newPassword2;

        public ICommand EditPasswordButton { protected set; get; }
        public INavigation Navigation { get; set; }


        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        public string NewPassword
        {
            get => newPassword;
            set => SetProperty(ref newPassword, value);
        }

        public string NewPassword2
        {
            get => newPassword2;
            set => SetProperty(ref newPassword2, value);
        }

        public EditPasswordViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            Password = "";
            NewPassword = "";
            NewPassword2 = "";
            EditPasswordButton= new Command(async () => { await ModifyPassword(); });
        }

        public async Task ModifyPassword()
        {
            if (NewPassword == NewPassword2 && NewPassword != "")
            {
                bool res = await App.SERVICE.EditPassword(Password, NewPassword);
                if (res)
                {
                    await Navigation.PopAsync();
                }
                else
                {
                }
            }
            else
            {
            }
        }
    }
}
