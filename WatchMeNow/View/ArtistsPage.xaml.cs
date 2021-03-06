﻿using MediaManager;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WatchMeNow.Model;
using WatchMeNow.Services;
using WatchMeNow.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WatchMeNow.Utils;
using System.Collections.ObjectModel;

namespace WatchMeNow.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ArtistsPage : ContentPage
    {
        public ArtistsPage()
        {
            InitializeComponent();

            flowList.RefreshCommand = RefreshCommand;
        }

        private void BtnArtistSearch_Clicked(object sender, EventArgs e)
        {
            NavigationPage.SetHasNavigationBar(this, false);

            sbArtists.IsVisible = true;
            sbArtists.Focus();
        }

        private void SbArtists_TextChanged(object sender, TextChangedEventArgs e)
        {
            var artistList = (BindingContext as ArtistsViewModel).ArtistList;

            var searchText = e.NewTextValue;

            if (string.IsNullOrEmpty(searchText))
            {
                flowList.FlowItemsSource = artistList;
            }
            else
            {
                var filteredList = artistList.Where(m => m.Name.ToLower().Contains(searchText.ToLower()));

                flowList.FlowItemsSource = filteredList.ToList();
            }
        }

        public ICommand RefreshCommand
        {
            get
            {
                return new Command(() =>
                {
                    var vm = (BindingContext as ArtistsViewModel);

                    vm.IsLoading = true;

                    vm.LoadData();

                    vm.IsLoading = false;

                    flowList.FlowItemsSource = vm.ArtistList;
                });
            }
        }

        private void SbArtists_Unfocused(object sender, FocusEventArgs e)
        {
            if(string.IsNullOrEmpty(sbArtists.Text) || string.IsNullOrWhiteSpace(sbArtists.Text))
            {
                sbArtists.IsVisible = false;
                sbArtists.Text = string.Empty;

                NavigationPage.SetHasNavigationBar(this, true);
            }

        }

        private async void BtnFavoriteSongs_Clicked(object sender, EventArgs e)
        {
            await App.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new FavoriteSongsPage())
            {
                BarBackgroundColor = Color.Black,
                BarTextColor = Color.Orange
            });
        }

        private async void BtnCurrentlyPlaying_Clicked(object sender, EventArgs e)
        {
            if (CrossMediaManager.Current.IsPlaying() && Utilities.SongTypeCode == (int)Utilities.SongType.Music)
            {
                await App.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new CurrentTrackPage())
                {
                    BarBackgroundColor = Color.Black,
                    BarTextColor = Color.Orange
                });
            }
            else if(CrossMediaManager.Current.Queue != null && CrossMediaManager.Current.Queue.Count > 0 && Utilities.SongTypeCode == (int)Utilities.SongType.Music)
            {
                await App.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new CurrentTrackPage())
                {
                    BarBackgroundColor = Color.Black,
                    BarTextColor = Color.Orange
                });
            }
        }

        private async void btnShuffleSongs_Clicked(object sender, EventArgs e)
        {
            var vm = (BindingContext as ArtistsViewModel);

            vm.IsBusy = true;

            flowList.IsEnabled = false;

            var artistsList = vm.ArtistList;

            var ArtistTracksUrls = new List<string>();

            foreach(var artist in artistsList)
            {
                ArtistTracksUrls.Add(artist.TrackList);
            }

            var _artistService = new ArtistService();
            var trackList = new List<MusicTrack>();

            var taskList = new List<Task<MusicArtistDetail>>();

            foreach(var trackUrl in ArtistTracksUrls)
            {
                try
                {

                    taskList.Add(_artistService.GetArtistDetail(trackUrl));
                        
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            await Task.WhenAll(taskList);

            foreach(var task in taskList)
            {
                foreach (var track in task.Result.TrackList)
                {
                    var random = new Random();

                    track.TrackCoverArt = task.Result.CoverArt;

                    if (trackList.Count == 0)
                        trackList.Add(track);
                    else
                        trackList.Insert(random.Next(trackList.Count), track);
                }
            }

            var songUris = new List<string>();

            using (SQLiteConnection cnn = new SQLiteConnection(Settings.LocalDataBasePath))
            {
                cnn.Execute("DELETE FROM CurrentTrack");

                var id = 0;

                foreach(var track in trackList)
                {
                    cnn.Insert(new Utilities.CurrentTrack()
                    {
                        Id = id++,
                        Name = track.Name,
                        ArtistName = track.Name,
                        FileUrl = track.FileUrl,
                        TrackLenght = track.TrackLenght,
                        TrackNumber = track.TrackNumber,
                        IsFavorite = track.IsFavorite,
                        CoverArt = track.TrackCoverArt,
                        LyricsUrl = track.LyricsUrl
                    });
                }

            }

            songUris.Add(trackList.First().FileUrl);

            if (CrossMediaManager.Current.Queue.Count > 0)
                CrossMediaManager.Current.Queue.Clear();

            await CrossMediaManager.Current.Play(songUris);

            flowList.IsEnabled = true;

            await App.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new CurrentTrackPage())
            {
                BarBackgroundColor = Color.Black,
                BarTextColor = Color.Orange
            });

            vm.IsBusy = false;

            Utilities.SongTypeCode = (int)Utilities.SongType.Music;
        }


    }
}