using MediaManager;
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
    public partial class RadioStationsPage : ContentPage
    {
        public RadioStationsPage()
        {
            InitializeComponent();

            flowList.RefreshCommand = RefreshCommand;
        }

        //private void BtnRadioStationSearch_Clicked(object sender, EventArgs e)
        //{
        //    NavigationPage.SetHasNavigationBar(this, false);

        //    sbRadioStation.IsVisible = true;
        //    sbRadioStation.Focus();
        //}

        //private void SbRadioStation_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    var stationList = (BindingContext as RadioStationsViewModel).RadioStationList;

        //    var searchText = e.NewTextValue;

        //    if (string.IsNullOrEmpty(searchText))
        //    {
        //        flowList.FlowItemsSource = stationList;
        //    }
        //    else
        //    {
        //        var filteredList = stationList.Where(m => m.Name.ToLower().Contains(searchText.ToLower()));

        //        flowList.FlowItemsSource = filteredList.ToList();
        //    }
        //}

        public ICommand RefreshCommand
        {
            get
            {
                return new Command(() =>
                {
                    var vm = (BindingContext as RadioStationsViewModel);

                    vm.IsBusy = true;

                    vm.LoadData();

                    vm.IsBusy = false;

                    flowList.FlowItemsSource = vm.RadioStationList;

                    flowList.EndRefresh();
                });
            }
        }

        //private void SbRadioStation_Unfocused(object sender, FocusEventArgs e)
        //{
        //    if(string.IsNullOrEmpty(sbRadioStation.Text) || string.IsNullOrWhiteSpace(sbRadioStation.Text))
        //    {
        //        sbRadioStation.IsVisible = false;
        //        sbRadioStation.Text = string.Empty;

        //        NavigationPage.SetHasNavigationBar(this, true);
        //    }

        //}

        //private async void BtnCurrentlyPlaying_Clicked(object sender, EventArgs e)
        //{
        //    if (CrossMediaManager.Current.IsPlaying() && Utilities.SongTypeCode == (int)Utilities.SongType.Radio)
        //    {
        //        //await App.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new CurrentTrackPage())
        //        //{
        //        //    BarBackgroundColor = Color.Black,
        //        //    BarTextColor = Color.Orange
        //        //});
        //    }
        //    else if(CrossMediaManager.Current.Queue != null && CrossMediaManager.Current.Queue.Count > 0 && Utilities.SongTypeCode == (int)Utilities.SongType.Radio)
        //    {
        //        //await App.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new CurrentTrackPage())
        //        //{
        //        //    BarBackgroundColor = Color.Black,
        //        //    BarTextColor = Color.Orange
        //        //});
        //    }
        //}

    }
}