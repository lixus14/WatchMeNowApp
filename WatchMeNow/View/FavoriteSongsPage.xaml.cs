using MediaManager;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchMeNow.Model;
using WatchMeNow.Utils;
using WatchMeNow.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WatchMeNow.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FavoriteSongsPage : ContentPage
    {
        public FavoriteSongsPage()
        {
            InitializeComponent();
        }

        private async void BtnClose_Clicked(object sender, EventArgs e)
        {
            await App.Current.MainPage.Navigation.PopModalAsync();
        }

        private async void TrackList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var track = e.Item as MusicTrack;

            var artistTrackList = (BindingContext as FavoriteSongsViewModel).TrackList;

            var songUris = new List<string>();

            using (SQLiteConnection cnn = new SQLiteConnection(Settings.LocalDataBasePath))
            {
                cnn.Execute("DELETE FROM CurrentTrack");

                cnn.Insert(new Utilities.CurrentTrack()
                {
                    Id = track.TrackNumber,
                    Name = track.Name,
                    ArtistName = track.Name,
                    FileUrl = track.FileUrl,
                    TrackLenght = track.TrackLenght,
                    TrackNumber = track.TrackNumber,
                    IsFavorite = track.IsFavorite,
                    CoverArt = track.TrackCoverArt
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
                            CoverArt = song.TrackCoverArt
                        });
                }

            }

            songUris.Add(track.FileUrl);

            if (CrossMediaManager.Current.Queue.Count > 0)
                CrossMediaManager.Current.Queue.Clear();

            await CrossMediaManager.Current.Play(songUris);

            await App.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new CurrentTrackPage())
            {
                BarBackgroundColor = Color.Black,
                BarTextColor = Color.Orange
            });

        }
    }
}