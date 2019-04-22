using FourPlaces.Model;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FourPlaces.Views
{

    // Menu liste
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MenuPage : ContentPage
	{
        RootPage root;
        
        List<MasterPageItem> menuItems;

        public MenuPage(RootPage root)
        {

            this.root = root;
            InitializeComponent();

            menuItems = new List<MasterPageItem>();
            menuItems.Add(new MasterPageItem() { Title = "Add Place", TargetType = typeof(AddPlacePage) });
            menuItems.Add(new MasterPageItem() { Title = "Places", TargetType = typeof(PlacesListPage) });
            menuItems.Add(new MasterPageItem() { Title = "Edit User", TargetType = typeof(EditPage) });
            menuItems.Add(new MasterPageItem() { Title = "Edit Password", TargetType = typeof(EditPasswordPage) });
            
            ListViewMenu.ItemsSource = menuItems;
          

            ListViewMenu.SelectedItem = menuItems[0];

            ListViewMenu.ItemSelected += async (sender, e) =>
            {
                if (ListViewMenu.SelectedItem == null)
                    return;


                if(((MasterPageItem)(e.SelectedItem)).TargetType == typeof(EditPage))
                {
                    this.root.Detail = new NavigationPage(new EditPage());

                }else if(((MasterPageItem)(e.SelectedItem)).TargetType == typeof(PlacesListPage))
                {
                    this.root.Detail = new NavigationPage(new PlacesListPage(this.root));
                }
                else if (((MasterPageItem)(e.SelectedItem)).TargetType == typeof(EditPasswordPage))
                {
                    this.root.Detail = new NavigationPage(new EditPasswordPage());
                }
                else if(((MasterPageItem)(e.SelectedItem)).TargetType == typeof(AddPlacePage))
                {
                    this.root.Detail = new NavigationPage(new AddPlacePage());
                }

                ListViewMenu.SelectedItem = null;
            };
        }
    }
}