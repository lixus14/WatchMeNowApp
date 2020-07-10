using System;
using System.Collections.Generic;
using System.Text;

namespace WatchMeNow.Model
{
    public class RadioStationListItem : BasicModel
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }

        private string _coverArt;
        public string CoverArt
        {
            get { return _coverArt; }
            set { _coverArt = value; OnPropertyChanged(); }
        }

        private string _stationUrl;
        public string StationUrl
        {
            get { return _stationUrl; }
            set { _stationUrl = value; OnPropertyChanged(); }
        }

        private double _frequency;
        public double Frequency
        {
            get { return _frequency; }
            set { _frequency = value; OnPropertyChanged(); }
        }

        private string _frequencyType;
        public string FrequencyType
        {
            get { return _frequencyType; }
            set { _frequencyType = value; OnPropertyChanged(); }
        }

    }
}
