using MediaManager;
using MediaManager.Library;
using Rg.Plugins.Popup.Services;
using SQLite;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using WatchMeNow.Model;
using WatchMeNow.Utils;
using WatchMeNow.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static WatchMeNow.Utils.Utilities;

namespace WatchMeNow.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CurrentTrackPage : ContentPage
    {
        #region Constructor
        public CurrentTrackPage()
        {
            InitializeComponent();
        }

        #endregion

        #region Events

        private async void BtnClose_Clicked(object sender, EventArgs e)
        {
            await App.Current.MainPage.Navigation.PopModalAsync();
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            var context = (BindingContext as CurrentTrackViewModel);

            using (SQLiteConnection cnn = new SQLiteConnection(Settings.LocalDataBasePath))
            {

                if (context.Track.IsFavorite)
                {
                    cnn.Execute("DELETE FROM FavoriteSongs WHERE TrackFileUrl = '" + context.Track.FileUrl + "'");

                    context.Track.IsFavorite = false;

                    btnLiked.Source = "Heart_Empty.png";
                }
                else
                {
                    cnn.Insert(new FavoriteSongs()
                    {
                        ArtistCoverArt = context.CoverArtUrl,
                        ArtistName = context.ArtistName,
                        TrackFileUrl = context.Track.FileUrl,
                        TrackLength = context.Track.TrackLenght,
                        TrackName = context.Track.Name,
                        TrackNumber = context.Track.TrackNumber,
                        LyricsUrl = context.Track.LyricsUrl
                    });

                    context.Track.IsFavorite = true;

                    btnLiked.Source = "Heart.png";
                }

            }

        }

        private async void BtnSkipBack_Clicked(object sender, EventArgs e)
        {
            var currentSongUri = CrossMediaManager.Current.Queue.Current.MediaUri;

            var vm = (BindingContext as CurrentTrackViewModel);

            vm.IsBusy = true;

            if(CrossMediaManager.Current.Position.TotalSeconds < 5.0)
            {
                
                MusicPlayingAnimation.IsEnabled = false;

                using (SQLiteConnection cnn = new SQLiteConnection(Settings.LocalDataBasePath))
                {
                    var currentTrackTable = cnn.Table<CurrentTrack>();

                    var songList = currentTrackTable.ToList();

                    var currentlyPlayingSong = songList.Where(x => x.FileUrl == currentSongUri).FirstOrDefault();

                    if (currentlyPlayingSong != null)
                    {
                        var songNumber = songList.IndexOf(currentlyPlayingSong);

                        if (songNumber == 0)
                        {
                            var previousSong = songList.Where(x => songList.IndexOf(x) == songList.Count - 1).First();

                            Title = previousSong.Name;

                            btnLiked.Source = IsCurrentSongFavorite(previousSong.FileUrl) ? "Heart.png" : "Heart_Empty.png";

                            if (!currentlyPlayingSong.CoverArt.Equals(previousSong.CoverArt))
                                trackImage.Source = previousSong.CoverArt;

                            await CrossMediaManager.Current.Stop();

                            await CrossMediaManager.Current.Play(previousSong.FileUrl);
                        }
                        else
                        {
                            if(songList.Count == 1)
                            {
                                var previousSong = songList.Where(x => songList.IndexOf(x) == 0).First();

                                Title = previousSong.Name;

                                btnLiked.Source = IsCurrentSongFavorite(previousSong.FileUrl) ? "Heart.png" : "Heart_Empty.png";

                                if (!currentlyPlayingSong.CoverArt.Equals(previousSong.CoverArt))
                                    trackImage.Source = previousSong.CoverArt;

                                await CrossMediaManager.Current.Stop();

                                await CrossMediaManager.Current.Play(previousSong.FileUrl);
                            }
                            else
                            {
                                var previousSong = songList.Where(x => songList.IndexOf(x) == songNumber - 1).First();

                                Title = previousSong.Name;

                                btnLiked.Source = IsCurrentSongFavorite(previousSong.FileUrl) ? "Heart.png" : "Heart_Empty.png";

                                if (!currentlyPlayingSong.CoverArt.Equals(previousSong.CoverArt))
                                    trackImage.Source = previousSong.CoverArt;

                                await CrossMediaManager.Current.Stop();

                                await CrossMediaManager.Current.Play(previousSong.FileUrl);
                            }
                        }

                    }

                }

                vm.IsBusy = false;

                MusicPlayingAnimation.IsEnabled = true;
            }
            else
            {
                await CrossMediaManager.Current.Pause();

                await CrossMediaManager.Current.SeekToStart();

                await CrossMediaManager.Current.Play();

                vm.IsBusy = false;
            }
            
        }

        private async void BtnSkipForward_Clicked(object sender, EventArgs e)
        {
            var currentSongUri = CrossMediaManager.Current.Queue.Current.MediaUri;

            var vm = (BindingContext as CurrentTrackViewModel);

            vm.IsBusy = true;

            MusicPlayingAnimation.IsEnabled = false;

            using (SQLiteConnection cnn = new SQLiteConnection(Settings.LocalDataBasePath))
            {
                var currentTrackTable = cnn.Table<CurrentTrack>();

                var songList = currentTrackTable.ToList();

                var currentlyPlayingSong = songList.Where(x => x.FileUrl == currentSongUri).FirstOrDefault();

                if (currentlyPlayingSong != null)
                {
                    var songNumber = songList.IndexOf(currentlyPlayingSong);

                    if (songNumber == (songList.Count - 1))
                    {
                        var nextSong = songList.Where(x => songList.IndexOf(x) == 0).First();

                        Title = nextSong.Name;

                        btnLiked.Source = IsCurrentSongFavorite(nextSong.FileUrl) ? "Heart.png" : "Heart_Empty.png";

                        if (!currentlyPlayingSong.CoverArt.Equals(nextSong.CoverArt))
                            trackImage.Source = nextSong.CoverArt;

                        await CrossMediaManager.Current.Stop();

                        await CrossMediaManager.Current.Play(nextSong.FileUrl);
                    }
                    else
                    {
                        if(songList.Count == 1)
                        {
                            var nextSong = songList.Where(x => songList.IndexOf(x) == 0).First();

                            Title = nextSong.Name;

                            btnLiked.Source = IsCurrentSongFavorite(nextSong.FileUrl) ? "Heart.png" : "Heart_Empty.png";

                            if (!currentlyPlayingSong.CoverArt.Equals(nextSong.CoverArt))
                                trackImage.Source = nextSong.CoverArt;

                            await CrossMediaManager.Current.Stop();

                            await CrossMediaManager.Current.Play(nextSong.FileUrl);
                        }
                        else
                        {
                            var nextSong = songList.Where(x => songList.IndexOf(x) == (songNumber + 1)).First();

                            Title = nextSong.Name;

                            btnLiked.Source = IsCurrentSongFavorite(nextSong.FileUrl) ? "Heart.png" : "Heart_Empty.png";

                            if (!currentlyPlayingSong.CoverArt.Equals(nextSong.CoverArt))
                                trackImage.Source = nextSong.CoverArt;

                            await CrossMediaManager.Current.Stop();

                            await CrossMediaManager.Current.Play(nextSong.FileUrl);
                        }

                    }

                }

            }

            vm.IsBusy = false;

            MusicPlayingAnimation.IsEnabled = true;

        }

        private async void BtnSkipBackTenSeconds_Clicked(object sender, EventArgs e)
        {
            await CrossMediaManager.Current.StepBackward();
        }

        private async void BtnSkipForwardTenSeconds_Clicked(object sender, EventArgs e)
        {
            await CrossMediaManager.Current.StepForward();
        }

        private void BtnPlayPause_Clicked(object sender, EventArgs e)
        {
            var media = CrossMediaManager.Current;

            if(media.IsPlaying())
            {
                media.Pause();

                MusicPlayingAnimation.Pause();

                btnPlayPause.Source = "Play.png";
            }
            else
            {
                media.Play();

                MusicPlayingAnimation.Play();

                btnPlayPause.Source = "Pause.png";
            }
        }

        #endregion

        private async void btnLyrics_Clicked(object sender, EventArgs e)
        {
            var vm = (BindingContext as CurrentTrackViewModel);

            vm.IsBusy = true;

            await PopupNavigation.Instance.PushAsync(new LyricPopUpPage());

            vm.IsBusy = false;
        }
    }
}