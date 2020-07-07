using SQLite;
using System;
using WatchMeNow.Utils;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WatchMeNow.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {

        #region Constructor

        public LoginPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);
        }

        #endregion

        #region Events

        private async void BtnLogin_Clicked(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(txtUserName.Text))
            {
                bool answer = await DisplayAlert("Login?", "Are you sure you want to login as? \n\n" + txtUserName.Text, "Yes", "No");

                if (answer)
                {
                    Settings.CurrentUserName = txtUserName.Text;

                    using (SQLiteConnection cnn = new SQLiteConnection(Settings.LocalDataBasePath))
                    {
                        cnn.Insert(new Utilities.LoginTable()
                        {
                            Id = 1,
                            User = txtUserName.Text
                        });
                    }

                    App.Current.MainPage = new HomeScreenTabbedPage();
                }

            }
            else
            {
                await DisplayAlert("Error!","User Name must be provided!","OK");
            }

        }

        #endregion

    }
}