using FourPlaces.Model;
using FourPlaces.Model.Services;
using FourPlaces.Services;
using FourPlaces.Views;
using MonkeyCache.SQLite;
using Plugin.Geolocator.Abstractions;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FourPlaces
{
    

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class App : Application
    {

        // service permettant l'accès à l'API
        public static APIService SERVICE { get; set; }
        public static MediaService MEDIA_SERVICE { get; set; }

        // stocke les données de l'utilisateur
        public static UserItem SESSION_USER { get; set; }

        // permet de stocker les token et refresh
        public static LoginResult SESSION_LOGIN { get; set; }

        public static string URI_API = "https://td-api.julienmialon.com/";

        public App()
        {
            SERVICE = new APIService();
            MEDIA_SERVICE = new MediaService();

            Barrel.ApplicationId = "FourPlace";
            Barrel.Current.Add(key: "Position", data: new Position(0.0, 0.0), expireIn: TimeSpan.FromDays(1));

            InitializeComponent();
            //MainPage = new NavigationPage(new LoginPage());
            MainPage = new LoginPage();
        }

        protected override void OnResume()
        {
            base.OnResume();
        }

        protected override void OnSleep()
        {
            base.OnSleep();
        }

        protected override void OnStart()
        {
            base.OnStart();
        }
    }
}