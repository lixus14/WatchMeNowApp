using DLToolkit.Forms.Controls;
using SQLite;
using System;
using WatchMeNow.Utils;
using WatchMeNow.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WatchMeNow
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            InitializeTables();

            InitializeDefaultMovieCatalog();

            InitializeDefaultArtistCatalog();

            FlowListView.Init();
        }

        protected override void OnStart()
        {
            using (SQLiteConnection cnn = new SQLiteConnection(Settings.LocalDataBasePath))
            {
                var loginTable = cnn.Table<Utilities.LoginTable>();

                var row = loginTable.FirstOrDefault();

                if (row == null)
                {
                    MainPage = new NavigationPage(new LoginPage());
                }
                else
                {
                    Utilities.InitializeOneSignal();

                    Settings.CurrentUserName = row.User;

                    MainPage = new HomeScreenTabbedPage();

                }

            }
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        #region Database Methods

        public static SQLiteConnection LoginConnection;
        public static SQLiteConnection MovieListConnection;
        public static SQLiteConnection MovieCatalogConnection;
        public static SQLiteConnection ArtistCatalogConnection;
        public static SQLiteConnection CurrentMovieConnection;
        public static SQLiteConnection CurrentArtistConnection;
        public static SQLiteConnection FavoriteSongsConnection;
        public static SQLiteConnection CurrentTrackConnection;

        private void InitializeTables()
        {
            LoginConnection = new SQLiteConnection(Settings.LocalDataBasePath);

            LoginConnection.CreateTable<Utilities.LoginTable>();

            MovieListConnection = new SQLiteConnection(Settings.LocalDataBasePath);

            MovieListConnection.CreateTable<Utilities.MovieListTable>();

            MovieCatalogConnection = new SQLiteConnection(Settings.LocalDataBasePath);

            MovieCatalogConnection.CreateTable<Utilities.MovieCatalog>();

            ArtistCatalogConnection = new SQLiteConnection(Settings.LocalDataBasePath);

            ArtistCatalogConnection.CreateTable<Utilities.ArtistCatalog>();

            CurrentMovieConnection = new SQLiteConnection(Settings.LocalDataBasePath);

            CurrentMovieConnection.CreateTable<Utilities.CurrentMovie>();

            CurrentArtistConnection = new SQLiteConnection(Settings.LocalDataBasePath);

            CurrentArtistConnection.CreateTable<Utilities.CurrentArtist>();

            FavoriteSongsConnection = new SQLiteConnection(Settings.LocalDataBasePath);

            FavoriteSongsConnection.CreateTable<Utilities.FavoriteSongs>();

            CurrentTrackConnection = new SQLiteConnection(Settings.LocalDataBasePath);

            CurrentTrackConnection.CreateTable<Utilities.CurrentTrack>();

        }

        private void InitializeDefaultMovieCatalog()
        {
            using (SQLiteConnection cnn = new SQLiteConnection(Settings.LocalDataBasePath))
            {
                var moviecatalogTable = cnn.Table<Utilities.MovieCatalog>();

                var row = moviecatalogTable.FirstOrDefault();

                if(row == null)
                {
                    cnn.Insert(new Utilities.MovieCatalog()
                    {
                        Id = 1,
                        CatalogAddress = "https://lixus14.github.io/WatchMeNow/MovieCatalog.json"
                    });
                }

            }
        }

        private void InitializeDefaultArtistCatalog()
        {
            using (SQLiteConnection cnn = new SQLiteConnection(Settings.LocalDataBasePath))
            {
                var moviecatalogTable = cnn.Table<Utilities.ArtistCatalog>();

                var row = moviecatalogTable.FirstOrDefault();

                if (row == null)
                {
                    cnn.Insert(new Utilities.ArtistCatalog()
                    {
                        Id = 1,
                        CatalogAddress = "https://lixus14.github.io/WatchMeNow/MusicArtistCatalog.json"
                    });
                }

            }
        }

        #endregion
    }
}
