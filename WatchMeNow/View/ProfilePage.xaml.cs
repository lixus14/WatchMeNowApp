using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchMeNow.Utils;
using WatchMeNow.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WatchMeNow.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();
        }

        private async void BtnLogout_Clicked(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Logout?", "Are you sure you want to logout?", "Yes", "No");

            if (answer)
            {
                Utilities.DeleteLocalDataBase();

                App.Current.MainPage = new NavigationPage(new LoginPage());
            }

        }

        private async void BtnUpdateCatalogAddress_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCatalogEntry.Text))
                await DisplayAlert("Error!", "Error! Catalog address must be specified!", "OK");
            else
            {
                using (SQLiteConnection cnn = new SQLiteConnection(Settings.LocalDataBasePath))
                {
                    var moviecatalogTable = cnn.Table<Utilities.MovieCatalog>();

                    var row = moviecatalogTable.FirstOrDefault();

                    if (row == null)
                    {
                        cnn.Insert(new Utilities.MovieCatalog()
                        {
                            Id = 1,
                            CatalogAddress = txtCatalogEntry.Text
                        });
                    }
                    else
                    {
                        cnn.Update(new Utilities.MovieCatalog()
                        {
                            Id = 1,
                            CatalogAddress = txtCatalogEntry.Text
                        });

                        //cnn.Execute("UPDATE MovieCatalog SET CatalogAddress = " + txtCatalogEntry.Text + " WHERE Id = 1");
                    }
                    
                }

                await DisplayAlert("Complete", "Update Complete!", "OK");
            }
        }
    }
}