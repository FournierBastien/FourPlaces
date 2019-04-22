using FourPlaces.Model;
using Storm.Mvvm;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace FourPlaces.ViewModels
{


    public class DetailViewModel : ViewModelBase
    {
        private bool _isRefreshing = false;
        private int _id;
        private string _msg = "";
        private string _newComment;
        private string _title;
        private string _description;
        private string _imageUrl;
        private double _latitude;
        private double _longitude;
        private double _distanceP;
        private Map _map;
        private List<CommentItem> _commentsList;

        public ICommand SubmitCommentCommand { protected set; get; }
        public ICommand RefreshCommand { protected set; get; }
        public INavigation Navigation { get; set; }

        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public string Msg
        {
            get => _msg;
            set => SetProperty(ref _msg, value);
        }

        public string NewComment
        {
            get => _newComment;
            set => SetProperty(ref _newComment, value);
        }

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public string ImageUrl
        {
            get => _imageUrl;
            set => SetProperty(ref _imageUrl, value);
        }


        public double Latitude
        {
            get => _latitude;
            set => SetProperty(ref _latitude, value);
        }
        public double Longitude
        {
            get => _longitude;
            set => SetProperty(ref _longitude, value);
        }

        public double DistanceP
        {
            get => _distanceP;
            set => SetProperty(ref _distanceP, value);
        }

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        public Map Map
        {
            get => _map;
            set => SetProperty(ref _map, value);
        }

        public List<CommentItem> CommentsList
        {
            get => _commentsList;
            set => SetProperty(ref _commentsList, value);
        }

        public DetailViewModel(INavigation navigation, PlaceItemSummary lieu)
        {

            Navigation = navigation;
            _id = lieu.Id;
            _title = lieu.Title;
            _description = lieu.Description;
            _imageUrl = lieu.ImageUrl;
            _latitude = lieu.Latitude;
            _longitude = lieu.Longitude;
            _newComment = "";
            _distanceP = lieu.Distance;
            _map = new Map();
            _commentsList = new List<CommentItem>();
            SubmitCommentCommand = new Command(async () => { await SubmitComment(); });
            RefreshCommand = new Command(async () => { await OnResume(); });
        }

        // Met à jour la page
        public override async Task OnResume()
        {
            await base.OnResume();
            PlaceItem place = await App.SERVICE.GetPlace(Id);
            if (place != null)
            {
                CommentsList = place.Comments;
                Map.MoveToRegion(
                    MapSpan.FromCenterAndRadius(
                    new Position(place.Latitude, place.Longitude), Distance.FromMiles(1)));
            
                
                Position position_pin = new Position(place.Latitude, place.Longitude);
                Map.MoveToRegion(MapSpan.FromCenterAndRadius(position_pin, Distance.FromKilometers(DistanceP * 1.5)));
                var pin = new Pin
                {
                    Position = position_pin,
                    Label = Title
                };
                Map.Pins.Add(pin);
            }
            else
            {
                Msg = "Erreur lors du chargement du lieu";
            }
        }


        // soumet un commentaire
        public async Task SubmitComment()
        {
            if (NewComment != "" && NewComment != null)
            {
                if (await App.SERVICE.SubmitComment(NewComment, Id))
                {
                    NewComment = "";
                    await this.OnResume();
                }
                else
                {
                    Msg = "Erreur commentaire";
                }
            }
            else
            {
                Msg = "Commentaire vide";
            }
        }
    }
}
