using FourPlaces.Model;
using MonkeyCache.SQLite;
using Storm.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FourPlaces.ViewModels
{

    // Modifications des données de l'utilisateur
    public class EditViewModel : ViewModelBase
    {
        private string msg = "";
        private string firstName;
        private string lastName;
        private List<ImageItem> images;
        private ImageItem selectedImage;
        private int? imageId;
        private string imageUrl;
        public ICommand SubmitEditCommand { protected set; get; }
        public ICommand LoadPictureCommand { protected set; get; }
        public INavigation Navigation { get; set; }

        public ImageItem SelectedImage
        {
            get
            {
                return selectedImage;
            }

            set
            {
                selectedImage = value;
                OnPropertyChanged("SelectedImage");
                ImageId = value.Id;
                OnPropertyChanged("ImageId");
            }
        }

        public List<ImageItem> Images
        {
            get => images;
            set => SetProperty(ref images, value);
        }

        public new event PropertyChangedEventHandler PropertyChanged;

        public string Msg
        {
            get => msg;
            set => SetProperty(ref msg, value);
        }

        public string FirstName
        {
            get => firstName;
            set => SetProperty(ref firstName, value);
        }

        public string LastName
        {
            get => lastName;
            set => SetProperty(ref lastName, value);
        }


        public string ImageUrl
        {
            get => imageUrl;
            set
            {
                SetProperty(ref imageUrl, value);
            }
        }

        public int? ImageId
        {
            get => imageId;
            set
            {
                SetProperty(ref imageId, value);
            }
        }

        public EditViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            firstName = Barrel.Current.Get<UserItem>(key: "User").FirstName;
            lastName = Barrel.Current.Get<UserItem>(key: "User").LastName;

            //firstName = App.SESSION_USER.FirstName;

            imageId = Barrel.Current.Get<UserItem>(key: "User").ImageId;
            imageUrl = App.URI_API + "images/" + Barrel.Current.Get<UserItem>(key: "User").ImageId;
            images = new List<ImageItem>();
            SubmitEditCommand = new Command(async () => { await Edit(); });
            LoadPictureCommand = new Command(async () => { await LoadPicture(); });
        }

        public override async Task OnResume()
        {
            await base.OnResume();
        }

        public async Task Edit()
        {
            Console.WriteLine("Erreur edit vm " + FirstName + " " + LastName + " " + ImageId);
            bool res = await App.SERVICE.EditUser(FirstName, LastName,ImageId);
            Console.WriteLine("Erreur edit vm 2");
            if (res)
            {
                //Barrel.Current.Get<UserItem>(key: "User").FirstName = FirstName;
                //Barrel.Current.Get<UserItem>(key: "User").LastName = LastName; 
                //await this.OnResume();
                Console.WriteLine("Erreur edit vm 3");
                await Navigation.PopAsync();
                Console.WriteLine("Erreur edit vm 4");
            }
            else
            {
                Msg = "Echec de la mise à jour du profil";
            }
        }


        public async Task LoadPicture()
        {
            Images = new List<ImageItem>();
            int id = 1;
            int idMax = await App.MEDIA_SERVICE.ImageEdge();
            while (id != idMax + 1)
            {
                Images.Add(new ImageItem(id, "https://td-api.julienmialon.com/images/" + id));
                id++;
            }
            OnPropertyChanged("Images");
        }

        public virtual void OnPropertyChanged(string s)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(s));
        }
    }
}
