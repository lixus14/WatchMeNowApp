using MediaManager;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WatchMeNow.Model;
using WatchMeNow.Services;
using WatchMeNow.Utils;
using WatchMeNow.View;
using Xamarin.Forms;

namespace WatchMeNow.ViewModel
{
    public class ArtistsViewModel: BasicViewModel
    {
        #region Constructor
        public ArtistsViewModel()
        {
            _artistService = new ArtistService();
           
            Title = "Music";

            ItemTappedCommand = new Command(async (param) => 
                await OpenArtist(param)
            );

            LoadData();
        }

        #endregion

        #region Properties
        private ArtistService _artistService { get; set; }

        private ObservableCollection<ArtistListItem> _artistList;
        public ObservableCollection<ArtistListItem> ArtistList
        {
            get { return _artistList; }
            set { _artistList = value; OnPropertyChanged("ArtistList"); }
        }

        public ICommand ItemTappedCommand { get; set; }

        public bool FavoritesIsEnabled => FavoriteExist();

        public bool CurrentSongIsPlaying => Utilities.IsCurrentSongPlaying;

        #endregion

        #region Methods

        private async Task OpenArtist(object param)
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

            await App.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new ArtistDetailPage())
            {
                BarBackgroundColor = Color.Black,
                BarTextColor = Color.Orange
            });
        }

        public void LoadData()
        {
            Task.Run(async () =>
            {
                IsBusy = true;

                try
                {

                    var newList = new ObservableCollection<ArtistListItem>();

                    var catalog = await _artistService.GetArtistCatalog(Settings.ArtistCatalog);

                    foreach (var artist in catalog.ArtistCatalog)
                    {
                        newList.Add(artist);
                    }

                    ArtistList = newList;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    IsBusy = false;
                }
            });

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
