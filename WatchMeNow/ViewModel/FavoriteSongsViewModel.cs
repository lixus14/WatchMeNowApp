using MediaManager;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using WatchMeNow.Model;
using WatchMeNow.Utils;

namespace WatchMeNow.ViewModel
{
    public class FavoriteSongsViewModel
    {
        public FavoriteSongsViewModel()
        {
            TrackList = new List<MusicTrack>();

            Name = "Favorite songs";

            LoadData();
        }

        public List<MusicTrack> TrackList { get; set; } 

        public string Name { get; set; }

        public void LoadData()
        {
            using (SQLiteConnection cnn = new SQLiteConnection(Settings.LocalDataBasePath))
            {
                var favoriteSongsTable = cnn.Table<Utilities.FavoriteSongs>();

                var tracks = favoriteSongsTable.ToList();

                var number = 1;

                foreach(var track in tracks)
                {
                    var song = new MusicTrack()
                    {
                        Name = track.TrackName,
                        TrackLenght = track.TrackLength,
                        TrackCoverArt = track.ArtistCoverArt,
                        FileUrl = track.TrackFileUrl,
                        IsFavorite = true,
                        TrackNumber = number++,
                        LyricsUrl = track.LyricsUrl
                    };

                    if (CrossMediaManager.Current != null && CrossMediaManager.Current.Queue != null &&
                        CrossMediaManager.Current.Queue.Current != null 
                        && CrossMediaManager.Current.Queue.Current.MediaUri.Equals(song.FileUrl))
                        song.IsPlaying = true;

                    TrackList.Add(song);

                    
                }
            }
        }

    }
}
