using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FourPlaces.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RootPage : MasterDetailPage
    {
        public RootPage()
        {
            Master = new MenuPage(this);
            Detail = new NavigationPage(new PlacesListPage(this));

            InitializeComponent();
            //InvalidateMeasure();
        }

        /*public async Task NavigateAsync(int i)
        {

            if( i == 1)
            {
                Detail = new NavigationPage(new EditPage());
            }
            
        }*/
    }
}