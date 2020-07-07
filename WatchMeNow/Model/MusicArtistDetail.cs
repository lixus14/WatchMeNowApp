using System;
using System.Collections.Generic;
using System.Text;

namespace WatchMeNow.Model
{
    public class MusicArtistDetail
    {
        public string Name { get; set; }

        public string CoverArt { get; set; }

        public List<MusicTrack> TrackList { get; set; }

    }
}
