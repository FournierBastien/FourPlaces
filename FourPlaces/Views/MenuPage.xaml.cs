using FourPlaces.Model;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FourPlaces.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MenuPage : ContentPage
	{
        RootPage root;
        
        List<MasterPageItem> menuItems;

        public MenuPage(RootPage root)
        {

            this.root = root;
            InitializeComponent();

            /*BindingContext = new BaseViewModel
            {
                Title = "Hanselman.Forms",
                Subtitle = "Hanselman.Forms",
                Icon = "slideout.png"
            };*/


            menuItems = new List<MasterPageItem>();
            menuItems.Add(new MasterPageItem() { Title = "Places", TargetType = typeof(PlacesListPage) });
            menuItems.Add(new MasterPageItem() { Title = "Edit", TargetType = typeof(EditPage) });
            menuItems.Add(new MasterPageItem() { Title = "EditPassword", TargetType = typeof(EditPasswordPage) });
            menuItems.Add(new MasterPageItem() { Title = "Add Place", TargetType = typeof(AddPlacePage) });
            ListViewMenu.ItemsSource = menuItems;
            /*ListViewMenu.ItemsSource = menuItems = new List<MasterPageItem>
                {
                    new MasterPageItem { Title = "About", TargetType = typeof(HomePage), IconSource ="contacts.png" },
                    new MasterPageItem { Title = "Blog", TargetType = typeof(LoginPage), IconSource = "contacts.png" },

                };*/
            
            
            ListViewMenu.SelectedItem = menuItems[0];

            ListViewMenu.ItemSelected += async (sender, e) =>
            {
                if (ListViewMenu.SelectedItem == null)
                    return;

                //this.root.Detail = (Page)Activator.CreateInstance(((MasterPageItem)(e.SelectedItem)).TargetType);


                if(((MasterPageItem)(e.SelectedItem)).TargetType == typeof(EditPage))
                {
                    this.root.Detail = new NavigationPage(new EditPage());

                }else if(((MasterPageItem)(e.SelectedItem)).TargetType == typeof(PlacesListPage))
                {
                    //Application.Current.MainPage = new RootPage();
                    this.root.Detail = new NavigationPage(new PlacesListPage(this.root));
                }
                else if (((MasterPageItem)(e.SelectedItem)).TargetType == typeof(EditPasswordPage))
                {
                    //Application.Current.MainPage = new RootPage();
                    this.root.Detail = new NavigationPage(new EditPasswordPage());
                }
                else if(((MasterPageItem)(e.SelectedItem)).TargetType == typeof(AddPlacePage))
                {
                    //Application.Current.MainPage = new RootPage();
                    this.root.Detail = new NavigationPage(new AddPlacePage());
                }

                ListViewMenu.SelectedItem = null;


                //await this.root.NavigateAsync((Page)Activator.CreateInstance(((MasterPageItem)(e.SelectedItem)).TargetType))();
            };
        }
    }
}