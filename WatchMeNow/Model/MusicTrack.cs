using System;
using System.Collections.Generic;
using System.Text;

namespace WatchMeNow.Model
{
    public class MusicTrack:BasicModel
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }

        private int _trackNumber;
        public int TrackNumber
        {
            get { return _trackNumber; }
            set { _trackNumber = value; OnPropertyChanged(); }
        }

        private string _trackLenght;
        public string TrackLenght
        {
            get { return _trackLenght; }
            set { _trackLenght = value; OnPropertyChanged(); }
        }

        private string _fileUrl;
        public string FileUrl
        {
            get { return _fileUrl; }
            set { _fileUrl = value; OnPropertyChanged(); }
        }

        private string _lyricsUrl;
        public string LyricsUrl
        {
            get { return _lyricsUrl; }
            set { _lyricsUrl = value; OnPropertyChanged(); }
        }

        private bool _isFavorite;
        public bool IsFavorite
        {
            get { return _isFavorite; }
            set { _isFavorite = value; OnPropertyChanged(); }
        }

        private string _trackCoverArt;
        public string TrackCoverArt
        {
            get { return _trackCoverArt; }
            set { _trackCoverArt = value; OnPropertyChanged(); }
        }

        private bool _isPlaying;
        public bool IsPlaying
        {
            get { return _isPlaying; }
            set { _isPlaying = value; OnPropertyChanged(); }
        }
    }
}
