using System;
using System.Collections.Generic;
using System.Text;

namespace WatchMeNow.Model
{
    public class MusicTrack
    {
        public string Name { get; set; }

        public int TrackNumber { get; set; }

        public string TrackLenght { get; set; }

        public string FileUrl { get; set; }

        public string LyricsUrl { get; set; }

        public bool IsFavorite { get; set; }

        public string TrackCoverArt { get; set; }

        public bool IsPlaying { get; set; }
    }
}
