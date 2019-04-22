using FourPlaces.Model;
using MonkeyCache.SQLite;
using Storm.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace FourPlaces.ViewModels
{
    public class AddPlaceViewModel : ViewModelBase
    {
        private List<ImageItem> _images;
        private ImageItem _selectedImage;

        private string _title = "";
        private string _description = "";
        private string _imageUrl;

        private int? _imageId;
        
        private double _latitude;
        private double _longitude;
        private Map map;

        public ICommand LoadImageButton { protected set; get; }
        public ICommand AddPlaceButton { protected set; get; }
        public INavigation Navigation { get; set; }

        public new event PropertyChangedEventHandler PropertyChanged;

        public ImageItem SelectedImage
        {
            get
            {
                return _selectedImage;
            }

            set
            {
                _selectedImage = value;
                OnPropertyChanged("SelectedImage");
                ImageId = value.Id.ToString();
                OnPropertyChanged("ImageId");
            }
        }

        public List<ImageItem> Images
        {
            get => _images;
            set => SetProperty(ref _images, value);
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

        public string ImageId
        {
            get
            {
                if (_imageId == null)
                    return "";
                else
                    return _imageId.ToString();
            }

            set
            {
                int? temp = int.Parse(value);
                SetProperty(ref _imageId, temp);
                if (ImageId == "")
                {
                    _imageUrl = "https://td-api.julienmialon.com/images/1";
                }
                else
                {
                    _imageUrl = "https://td-api.julienmialon.com/images/" + ImageId;
                }

            }
        }


        public double Latitude
        {
            get => _latitude;
            set
            {
                SetProperty(ref _latitude, value);
                UpdateMap();
            }
        }
        public double Longitude
        {
            get => _longitude;
            set
            {
                SetProperty(ref _longitude, value);
                UpdateMap();
            }
        }

        public Map Map
        {
            get => map;
            set => SetProperty(ref map, value);
        }

        public AddPlaceViewModel(INavigation navigation)
        {
            this.Navigation = navigation;

            AddPlaceButton = new Command(async () => { await AddPlace(); });
            LoadImageButton = new Command(async () => { await LoadImage(); });

            map = new Map();
            _images = new List<ImageItem>();
            _imageId = 1;
            _imageUrl = "https://td-api.julienmialon.com/images/1";
            _latitude = Barrel.Current.Get<Plugin.Geolocator.Abstractions.Position>(key: "Position").Latitude;
            _longitude = Barrel.Current.Get<Plugin.Geolocator.Abstractions.Position>(key: "Position").Longitude;
        }

        public override async Task OnResume()
        {
            await base.OnResume();
            await LoadImages();
            UpdateMap();
        }


        private async Task AddPlace()
        {
            bool res = await App.SERVICE.AddPlace(Title, Description, int.Parse(ImageId), Latitude, Longitude);
            if (res)
            {
                await Navigation.PopAsync();
            }
            else
            {
                Console.WriteLine("Erreur Add Place");
                await Navigation.PopAsync();
            }
        }

        // chargé une image
        public async Task LoadImage()
        {
            int? res = await App.SERVICE.LoadPicture();
            if (res != null)
            {
                ImageId = res.ToString();
                OnPropertyChanged("ImageId");
                await LoadImages();
            }
            else
            {
            }
        }


        public async Task LoadImages()
        {
            Images = new List<ImageItem>();
            int id = 1;
            int idMax = await App.MEDIA_SERVICE.ImageEdge();
            while (id != idMax + 1)
            {
                Images.Add(new ImageItem(id, "https://td-api.julienmialon.com/images/"+id));
                id++;
            }
            OnPropertyChanged("Images");
        }

        private void UpdateMap()
        {
            Position positionpin = new Position(Latitude, Longitude);
            Map.MoveToRegion(MapSpan.FromCenterAndRadius(positionpin, Distance.FromKilometers(150)));
            var pin = new Pin
            {
                Position = positionpin,
                Label = Title
            };
            Map.Pins.Clear();
            Map.Pins.Add(pin);
        }

        public virtual void OnPropertyChanged(string s)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(s));
        }
    }
}
