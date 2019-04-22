using FourPlaces.Model;
using Storm.Mvvm;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FourPlaces.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        public INavigation Navigation { get; set; }

        public ICommand RefreshCommand { protected set; get; }

        bool _isBusy = false;
        bool _isRefreshing = false;
        private string _msg = "";
        private List<PlaceItemSummary> _placesList;

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public string Msg
        {
            get => _msg;
            set => SetProperty(ref _msg, value);
        }


        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        public List<PlaceItemSummary> PlacesList
        {
            get => _placesList;
            set => SetProperty(ref _placesList, value);
        }

        public HomeViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            Console.WriteLine("init");
            RefreshCommand = new Command(async () => { await OnResume(); });
            Console.WriteLine("init2");

        }

        public override async Task OnResume()
        {

            Console.WriteLine("OnResume");
            IsBusy = true;
            PlacesList placesList = await App.SERVICE.GetListPlaces();

            Console.WriteLine("taille liste : " + placesList.Places.ToArray().Length);

            if (placesList != null)
            {
                Console.WriteLine("PlaceList not null");
                foreach (PlaceItemSummary element in placesList.Places)
                {
                    element.ImageUrl = "https://td-api.julienmialon.com/images/" + element.ImageId;
                }

                Console.WriteLine("after foreach");
                PlacesList = placesList.Places;
            }
            else
            {
                Console.WriteLine("PlaceList null");
                Msg = "Erreur lors de la récupération des lieux";
            }

            IsBusy = false;
           
        }
        
    }
}
