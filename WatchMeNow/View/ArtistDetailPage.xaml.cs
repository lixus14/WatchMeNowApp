
using MediaManager;
using MediaManager.Library;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WatchMeNow.Model;
using WatchMeNow.Utils;
using WatchMeNow.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WatchMeNow.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ArtistDetailPage : ContentPage
    {
        #region Constructor
        public ArtistDetailPage()
        {
            InitializeComponent();
        }

        #endregion

        #region Events

        private async void BtnClose_Clicked(object sender, EventArgs e)
        {
            await App.Current.MainPage.Navigation.PopModalAsync();
        }


        private async void TrackList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var track = e.Item as MusicTrack;

            var viewModel = (BindingContext as ArtistDetailViewModel);

            viewModel.IsBusy = true;

            if (CrossMediaManager.Current != null && CrossMediaManager.Current.Queue != null &&
                CrossMediaManager.Current.Queue.Current != null && CrossMediaManager.Current.Queue.Current.MediaUri.Equals(track.FileUrl))
            {
                await App.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new CurrentTrackPage())
                {
                    BarBackgroundColor = Color.Black,
                    BarTextColor = Color.Orange
                });

                viewModel.IsBusy = false;
            }
            else
            {
                var artist = viewModel.MusicArtistDetail;

                var artistTrackList = artist.TrackList;

                var songUris = new List<string>();

                using (SQLiteConnection cnn = new SQLiteConnection(Settings.LocalDataBasePath))
                {
                    cnn.Execute("DELETE FROM CurrentTrack");

                    cnn.Insert(new Utilities.CurrentTrack()
                    {
                        Id = track.TrackNumber,
                        Name = track.Name,
                        ArtistName = artist.Name,
                        FileUrl = track.FileUrl,
                        TrackLenght = track.TrackLenght,
                        TrackNumber = track.TrackNumber,
                        IsFavorite = track.IsFavorite,
                        CoverArt = artist.CoverArt,
                        LyricsUrl = track.LyricsUrl
                    });

                    foreach (var song in artistTrackList)
                    {
                        if (song.FileUrl != track.FileUrl)
                            cnn.Insert(new Utilities.CurrentTrack()
                            {
                                Id = song.TrackNumber,
                                Name = song.Name,
                                ArtistName = song.Name,
                                FileUrl = song.FileUrl,
                                TrackLenght = song.TrackLenght,
                                TrackNumber = song.TrackNumber,
                                IsFavorite = song.IsFavorite,
                                CoverArt = artist.CoverArt,
                                LyricsUrl = song.LyricsUrl
                            });
                    }

                }

                songUris.Add(track.FileUrl);

                if (CrossMediaManager.Current.Queue.Count > 0)
                    CrossMediaManager.Current.Queue.Clear();

                await Task.Run(async () => { await CrossMediaManager.Current.Play(songUris); });

                await App.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new CurrentTrackPage())
                {
                    BarBackgroundColor = Color.Black,
                    BarTextColor = Color.Orange
                });

                viewModel.IsBusy = false;
            }

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var vm = (BindingContext as ArtistDetailViewModel);

            if(vm.MusicArtistDetail.TrackList != null)
                vm.SetFavoriteSongsInList(vm.MusicArtistDetail);
        }

        #endregion

    }
}