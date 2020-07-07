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
    public class LyricsPopUpViewModel
    {
        #region Constructor

        public LyricsPopUpViewModel()
        {
            _lyricsService = new LyricsService();

            TrackLyricsDetail = new TrackLyrics();

            LoadData();
        }

        #endregion

        #region Properties

        public string Lyrics { get; set; }

        public TrackLyrics TrackLyricsDetail { get; set; }

        public LyricsService _lyricsService { get; set; }

        public double ScreenHeight => Settings.DeviceHeight * 0.50;

        public double ScreenWidth => Settings.DeviceWidth * 0.90;

        #endregion

        #region Methods

        public void LoadData()
        {
            using (SQLiteConnection cnn = new SQLiteConnection(Settings.LocalDataBasePath))
            {
                var currentTrackTable = cnn.Table<Utilities.CurrentTrack>();

                var currentTrackUrl = CrossMediaManager.Current.Queue.Current.MediaUri;

                var row = currentTrackTable.ToList().Where(x => x.FileUrl == currentTrackUrl).FirstOrDefault();

                if (row != null && row.LyricsUrl != null)
                {
                    try
                    {
                        Lyrics = Settings.TrackLyricsPath + row.LyricsUrl;

                        //var tdetails = _lyricsService.GetTrackLyrics(Settings.TrackLyricsUri + row.LyricsUrl);

                        //TrackLyricsDetail = tdetails;

                        //Lyrics = string.Format(TrackLyricsDetail.result.lyrics);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
                
        }

        #endregion
    }
}
