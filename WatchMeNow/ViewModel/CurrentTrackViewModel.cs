using Lottie.Forms;
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
using WatchMeNow.View;
using Xamarin.Forms;

namespace WatchMeNow.ViewModel
{
    public class CurrentTrackViewModel
    {
        #region Constructor

        public CurrentTrackViewModel()
        {
            LoadData();
        }

        #endregion

        #region Properties

        public MusicTrack Track { get; set; }

        public string CoverArtUrl { get; set; }

        public string TrackIsLikedSource { get; set; }

        public string ArtistName { get; set; }

        public bool AnimationIsPlaying { get; set; }

        public string PlayPauseSource { get; set; }

        #endregion

        #region Methods

        public void LoadData()
        {
            using (SQLiteConnection cnn = new SQLiteConnection(Settings.LocalDataBasePath))
            {
                var currentTrackTable = cnn.Table<Utilities.CurrentTrack>();

                var currentTrackUrl = CrossMediaManager.Current.Queue.Current.MediaUri;

                var row = currentTrackTable.ToList().Where(x => x.FileUrl == currentTrackUrl).FirstOrDefault();

                if (row != null)
                {
                    Track = new MusicTrack()
                    {
                        Name = row.Name,
                        FileUrl = row.FileUrl,
                        TrackNumber = row.TrackNumber,
                        TrackLenght = row.TrackLenght,
                        LyricsUrl = row.LyricsUrl,
                        IsFavorite = Utilities.IsCurrentSongFavorite(row.FileUrl)
                    };

                    CoverArtUrl = row.CoverArt;

                    TrackIsLikedSource = Utilities.IsCurrentSongFavorite(row.FileUrl) ? "Heart.png" : "Heart_Empty.png";

                    ArtistName = row.ArtistName;
                }
                
                //cnn.Execute("DELETE FROM CurrentTrack");

            }

            PlayPauseSource = "Play.png";
            AnimationIsPlaying = false;

            CrossMediaManager.Current.BufferedChanged += (sender, e) =>
            {
                var currentPageCount = App.Current.MainPage.Navigation.ModalStack.Count;

                if (currentPageCount > 0)
                {
                    var currentModalPage = App.Current.MainPage.Navigation.ModalStack[currentPageCount - 1];

                    var currentModalNavigationCount = currentModalPage.Navigation.NavigationStack.Count;

                    if (currentModalNavigationCount > 0)
                    {
                        var currentPage = currentModalPage.Navigation.NavigationStack[currentModalNavigationCount - 1];

                        if (currentPage is CurrentTrackPage)
                        {
                            var bufferProgress = currentPage.FindByName<ProgressBar>("bufferProgress");

                            //Update buffer line
                            using (SQLiteConnection cnn = new SQLiteConnection(Settings.LocalDataBasePath))
                            {
                                var currentTrackTable = cnn.Table<Utilities.CurrentTrack>();

                                var currentTrackUrl = CrossMediaManager.Current.Queue.Current.MediaUri;

                                var row = currentTrackTable.ToList().Where(x => x.FileUrl == currentTrackUrl).FirstOrDefault();

                                var trackLengthParts = row.TrackLenght.Split(':');

                                var songLengthInTime = TimeSpan.FromMinutes(Convert.ToDouble(trackLengthParts[0])).TotalSeconds + TimeSpan.FromSeconds(Convert.ToDouble(trackLengthParts[1])).TotalSeconds;

                                bufferProgress.Progress = CrossMediaManager.Current.Buffered.TotalSeconds / songLengthInTime;
                            }
                        }
                    }
                }
            };

            CrossMediaManager.Current.PositionChanged += (sender, e) =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    var currentPageCount = App.Current.MainPage.Navigation.ModalStack.Count;

                    if(currentPageCount > 0)
                    {
                        var currentModalPage = App.Current.MainPage.Navigation.ModalStack[currentPageCount - 1];

                        var currentModalNavigationCount = currentModalPage.Navigation.NavigationStack.Count;

                        if(currentModalNavigationCount > 0)
                        {
                            var currentPage = currentModalPage.Navigation.NavigationStack[currentModalNavigationCount - 1];

                            if (currentPage is  CurrentTrackPage)
                            {
                                var frame = currentPage.FindByName<Frame>("imgFrame");
                                var trackImage = currentPage.FindByName<Image>("trackImage");
                                var currentPosition = currentPage.FindByName<Label>("currentPosition");
                                var currentSongProgress = currentPage.FindByName<ProgressBar>("currentSongProgress");
                                var currentSongLenght = currentPage.FindByName<Label>("currentSongLenght");
                                var musicPlayingAnimation = currentPage.FindByName<AnimationView>("MusicPlayingAnimation");
                                var btnPlayPause = currentPage.FindByName<ImageButton>("btnPlayPause");

                                //Update buffer line
                                using (SQLiteConnection cnn = new SQLiteConnection(Settings.LocalDataBasePath))
                                {
                                    var currentTrackTable = cnn.Table<Utilities.CurrentTrack>();

                                    var currentTrackUrl = CrossMediaManager.Current.Queue.Current.MediaUri;

                                    var row = currentTrackTable.ToList().Where(x => x.FileUrl == currentTrackUrl).FirstOrDefault();

                                    if (trackImage.Source.IsEmpty)
                                        trackImage.Source = row.CoverArt;

                                    currentSongLenght.Text = row.TrackLenght;

                                    var trackLengthParts = row.TrackLenght.Split(':');

                                    var songLengthInTime = TimeSpan.FromMinutes(Convert.ToDouble(trackLengthParts[0])).TotalSeconds + TimeSpan.FromSeconds(Convert.ToDouble(trackLengthParts[1])).TotalSeconds;

                                    //Update running data
                                    if (CrossMediaManager.Current.IsPlaying())
                                    {

                                        currentPosition.Text = CrossMediaManager.Current.Position.Minutes + ":" + string.Format("{0:00}", CrossMediaManager.Current.Position.Seconds);

                                        currentSongProgress.Progress = CrossMediaManager.Current.Position.TotalSeconds / songLengthInTime;

                                        if (!musicPlayingAnimation.AutoPlay)
                                            musicPlayingAnimation.AutoPlay = true;

                                        if(!musicPlayingAnimation.IsPlaying)
                                            musicPlayingAnimation.IsPlaying = true;

                                        if (!musicPlayingAnimation.IsEnabled)
                                            musicPlayingAnimation.IsEnabled = true;

                                        btnPlayPause.Source = "Pause.png";
                                    }

                                }

                            }
                        }

                        
                    }

                });
            };
        }

        #endregion
    }
}
