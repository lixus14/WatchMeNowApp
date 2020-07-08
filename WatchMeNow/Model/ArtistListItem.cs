using System;
using System.Collections.Generic;
using System.Text;

namespace WatchMeNow.Model
{
    public class ArtistListItem:BasicModel
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

        private string _trackList;
        public string TrackList
        {
            get { return _trackList; }
            set { _trackList = value; OnPropertyChanged(); }
        }

    }
}
