using MediaManager;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using WatchMeNow.Model;
using WatchMeNow.Services;
using WatchMeNow.Utils;
using Xamarin.Forms;

namespace WatchMeNow.ViewModel
{
    public class ArtistDetailViewModel: BasicViewModel
    {
        #region Constructor

        public ArtistDetailViewModel()
        {
            _artistService = new ArtistService();

            MusicArtistDetail = new MusicArtistDetail();

            LoadData();
        }

        #endregion

        #region Properties

        public string Name { get; set; }

        public string CoverArt { get; set; }

        public ArtistService _artistService { get; set; }

        public MusicArtistDetail _musicArtistDetail;
        public MusicArtistDetail MusicArtistDetail 
        {
            get { return _musicArtistDetail; }
            set { _musicArtistDetail = value; OnPropertyChanged(); }
        }

        #endregion

        #region Methods

        public void LoadData()
        {
            IsBusy = true;

            using (SQLiteConnection cnn = new SQLiteConnection(Settings.LocalDataBasePath))
            {
                var currentArtistTable = cnn.Table<Utilities.CurrentArtist>();

                var row = currentArtistTable.FirstOrDefault();

                if(row != null)
                {
                    Name = row.Name;

                    CoverArt = row.CoverArt;
                    
                    try
                    {
                        var adetails = _artistService.GetArtistDetail(row.TrackList);

                        MusicArtistDetail = adetails;

                        SetFavoriteSongsInList(MusicArtistDetail);

                        IsBusy = false;

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                cnn.Execute("DELETE FROM CurrentArtist");

            }

        }

        private void SetFavoriteSongsInList(MusicArtistDetail musicArtistDetail)
        {
            var songList = musicArtistDetail.TrackList;

            foreach (var track in songList)
            {
                using (SQLiteConnection cnn = new SQLiteConnection(Settings.LocalDataBasePath))
                {
                    var favoriteSongs = cnn.Table<Utilities.FavoriteSongs>();

                    var songIsFavorite = favoriteSongs.ToList().Where(x => x.TrackFileUrl == track.FileUrl);

                    if (songIsFavorite.Any())
                        track.IsFavorite = true;

                }

                if (CrossMediaManager.Current != null && CrossMediaManager.Current.Queue != null &&
                    CrossMediaManager.Current.Queue.Current != null && CrossMediaManager.Current.Queue.Current.MediaUri.Equals(track.FileUrl))
                    track.IsPlaying = true;
                
            }
        }

        #endregion
    }
}
