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
    public class RadioStationsViewModel : BasicViewModel
    {
        #region Constructor
        public RadioStationsViewModel()
        {
            _radioStationsService = new RadioStationService();
           
            Title = "Radio";

            ItemTappedCommand = new Command(async (param) => 
                await OpenRadioStation(param)
            );

            CurrentRadioStation = new RadioStationListItem()
            {
                CoverArt = "https://www.pinclipart.com/picdir/middle/368-3680846_commercial-radio-icon-iconos-de-radio-png-clipart.png",
                Frequency = 0.0,
                FrequencyType = "AM/FM",
                Name = "DEFAULT",
                StationUrl = string.Empty
            };

            LoadData();
        }

        #endregion

        #region Properties
        private RadioStationService _radioStationsService { get; set; }

        private ObservableCollection<RadioStationListItem> _radioStationList;
        public ObservableCollection<RadioStationListItem> RadioStationList
        {
            get { return _radioStationList; }
            set { _radioStationList = value; OnPropertyChanged("RadioStationList"); }
        }

        private RadioStationListItem _currentRadioStation;
        public RadioStationListItem CurrentRadioStation
        {
            get { return _currentRadioStation; }
            set { _currentRadioStation = value; OnPropertyChanged("CurrentRadioStation"); }
        }

        public ICommand ItemTappedCommand { get; set; }

        #endregion

        #region Methods

        private async Task OpenRadioStation(object param)
        {
            var radioStation = param as RadioStationListItem;

            IsBusy = true;

            if (CrossMediaManager.Current != null && CrossMediaManager.Current.Queue != null &&
                CrossMediaManager.Current.Queue.Current != null && CrossMediaManager.Current.Queue.Current.MediaUri.Equals(radioStation.StationUrl))
            {
                IsBusy = false;

                return;
            }

            var stop = CrossMediaManager.Current.Stop();

            var play =  CrossMediaManager.Current.Play(radioStation.StationUrl);

            var setNewRadio = SetNewRadioStation(radioStation);

            await Task.WhenAll(stop, play, setNewRadio);

            Utilities.SongTypeCode = (int)Utilities.SongType.Radio;

            IsBusy = false;
        }

        public async Task SetNewRadioStation(RadioStationListItem item)
        {
            CurrentRadioStation = item;

            await Task.Delay(0);
        }

        public void LoadData()
        {
            Task.Run(async () =>
            {
                IsBusy = true;

                try
                {

                    var newList = new ObservableCollection<RadioStationListItem>();

                    var catalog = await _radioStationsService.GetRadioStationCatalog(Settings.RadioStationCatalogUrl);

                    foreach (var station in catalog.RadioStationsCatalog)
                    {
                        newList.Add(station);
                    }

                    RadioStationList = newList;
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

        #endregion
    }
}
