using MediaManager;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using WatchMeNow.Model;
using WatchMeNow.Services;
using WatchMeNow.Utils;
using WatchMeNow.View;
using Xamarin.Forms;

namespace WatchMeNow.ViewModel
{
    public class ArtistsViewModel
    {
        #region Constructor
        public ArtistsViewModel()
        {
            _artistService = new ArtistService();

            ArtistList = new List<ArtistListItem>();
           
            Title = "Music";

            ItemTappedCommand = new Command((param) => 
                OpenArtist(param)
            );

            LoadData();
        }

        #endregion

        #region Properties
        private ArtistService _artistService { get; set; }

        public List<ArtistListItem> ArtistList { get; set; }

        public string Title { get; set; }

        public ICommand ItemTappedCommand { get; set; }

        public bool FavoritesIsEnabled => FavoriteExist();

        public bool CurrentSongIsPlaying => Utilities.IsCurrentSongPlaying;

        #endregion

        #region Methods
        [Obsolete]
        private void OpenArtist(object param)
        {
            var artistDetail = param as ArtistListItem;

            using (SQLiteConnection cnn = new SQLiteConnection(Settings.LocalDataBasePath))
            {

                cnn.Insert(new Utilities.CurrentArtist()
                {
                    Id = 1,
                    Name = artistDetail.Name,
                    CoverArt = artistDetail.CoverArt,
                    TrackList = artistDetail.TrackList
                });
            }

            App.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new ArtistDetailPage())
            {
                BarBackgroundColor = Color.Black,
                BarTextColor = Color.Orange
            });
        }

        public void LoadData()
        {

            try
            {
                ArtistList.Clear();

                var catalog = _artistService.GetArtistCatalog(Settings.ArtistCatalog);

                foreach(var artist in catalog.ArtistCatalog)
                {
                    ArtistList.Add(artist);
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            

        }

        private bool FavoriteExist()
        {
            using (SQLiteConnection cnn = new SQLiteConnection(Settings.LocalDataBasePath))
            {
                var favoriteSongsTable = cnn.Table<Utilities.FavoriteSongs>();

                var row = favoriteSongsTable.FirstOrDefault();

                return row != null;
            }
        }

        #endregion
    }
}
