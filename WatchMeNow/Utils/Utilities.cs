using Android.Content.Res;
using Android.Util;
using Com.OneSignal;
using Com.OneSignal.Abstractions;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WatchMeNow.Model;

namespace WatchMeNow.Utils
{
    public class Utilities
    {

        #region Methods

        //Starts the onesignal notification service on device
        public static async Task InitializeOneSignal()
        {
            OneSignal.Current.StartInit(Settings.OneSignalApiKey)
                .InFocusDisplaying(OSInFocusDisplayOption.Notification)
                .HandleNotificationOpened(HandleNotificationOpened)
                  .EndInit();
        }

        //Handles what the app does when the push notification is opened.
        public static void HandleNotificationOpened(OSNotificationOpenedResult result)
        {
            OSNotificationPayload payload = result.notification.payload;
            Dictionary<string, object> additionalData = payload.additionalData;
            string message = payload.body;
            string actionId = result.action.actionID;

        }

        public static bool IsCurrentSongFavorite(string trackFileUrl)
        {
            using (SQLiteConnection cnn = new SQLiteConnection(Settings.LocalDataBasePath))
            {
                var favoriteSongs = cnn.Table<FavoriteSongs>();

                var songIsFavorite = favoriteSongs.ToList().Where(x => x.TrackFileUrl.Contains(trackFileUrl));

                if (songIsFavorite.Any())
                    return true;

                else
                    return false;

            }
        }

        #endregion

        #region Database Table definitions

        [Table("LoginTable")]
        public class LoginTable
        {
            [PrimaryKey, Column("Id")]
            public int Id { get; set; }

            [Column("User")]
            public string User { get; set; }

        }

        [Table("MovieListTable")]
        public class MovieListTable
        {
            [PrimaryKey, Column("Id")]
            public int Id { get; set; }

            [Column("Title")]
            public string Title { get; set; }

            [Column("ImdbId")]
            public string ImdbId { get; set; }

            [Column("VideoAddress")]
            public string VideoAddress { get; set; }

            [Column("PosterImageAddress")]
            public string PosterImageAddress { get; set; }

        }

        [Table("MovieCatalog")]
        public class MovieCatalog
        {
            [PrimaryKey, Column("Id")]
            public int Id { get; set; }

            [Column("CatalogAddress")]
            public string CatalogAddress { get; set; }
        }

        [Table("ArtistCatalog")]
        public class ArtistCatalog
        {
            [PrimaryKey, Column("Id")]
            public int Id { get; set; }

            [Column("CatalogAddress")]
            public string CatalogAddress { get; set; }
        }

        [Table("CurrentMovie")]
        public class CurrentMovie
        {
            [PrimaryKey, Column("Id")]
            public int Id { get; set; }

            [Column("ImdbId")]
            public string ImdbId { get; set; }

            [Column("Title")]
            public string Title { get; set; }

            [Column("Year")]
            public int Year { get; set; }

            [Column("VideoSourceES")]
            public string VideoSourceES { get; set; }

            [Column("VideoSourceEN")]
            public string VideoSourceEN { get; set; }
        }

        [Table("CurrentArtist")]
        public class CurrentArtist
        {
            [PrimaryKey, Column("Id")]
            public int Id { get; set; }

            [Column("Name")]
            public string Name { get; set; }

            [Column("CoverArt")]
            public string CoverArt { get; set; }

            [Column("TrackList")]
            public string TrackList { get; set; }

        }

        [Table("FavoriteSongs")]
        public class FavoriteSongs
        {
            [PrimaryKey, Column("Id")]
            [AutoIncrement]
            public int Id { get; set; }

            [Column("ArtistName")]
            public string ArtistName { get; set; }

            [Column("ArtistCoverArt")]
            public string ArtistCoverArt { get; set; }

            [Column("TrackName")]
            public string TrackName { get; set; }

            [Column("TrackNumber")]
            public int TrackNumber { get; set; }

            [Column("TrackLength")]
            public string TrackLength { get; set; }

            [Column("TrackFileUrl")]
            public string TrackFileUrl { get; set; }

            [Column("LyricsUrl")]
            public string LyricsUrl { get; set; }
        }


        [Table("CurrentTrack")]
        public class CurrentTrack
        {
            [PrimaryKey, Column("Id")]
            public int Id { get; set; }

            [Column("Name")]
            public string Name { get; set; }

            [Column("ArtistName")]
            public string ArtistName { get; set; }

            [Column("TrackNumber")]
            public int TrackNumber { get; set; }

            [Column("TrackLenght")]
            public string TrackLenght { get; set; }

            [Column("FileUrl")]
            public string FileUrl { get; set; }

            [Column("LyricsUrl")]
            public string LyricsUrl { get; set; }

            [Column("IsFavorite")]
            public bool IsFavorite { get; set; }

            [Column("CoverArt")]
            public string CoverArt { get; set; }
        }

        public static void DeleteLocalDataBase()
        {
            using (SQLiteConnection cnn = new SQLiteConnection(Settings.LocalDataBasePath))
            {
                cnn.Execute("DELETE FROM LoginTable");
                cnn.Execute("DELETE FROM MovieListTable");
                cnn.Execute("DELETE FROM MovieCatalog");
                cnn.Execute("DELETE FROM ArtistCatalog");
                cnn.Execute("DELETE FROM CurrentMovie");
                cnn.Execute("DELETE FROM CurrentArtist");
                cnn.Execute("DELETE FROM FavoriteSongs");
                cnn.Execute("DELETE FROM CurrentTrack");
            }
        }

        #endregion

        #region Properties

        public static bool IsCurrentSongPlaying { get; set; }

        public static int SongTypeCode { get; set; }

        public enum SongType
        {
            Music = 1,
            Radio = 2
        }

        public static string InternetMovieDatabaseLogoUrl => "https://m.media-amazon.com/images/G/01/imdb/images-ANDW73HA/imdb_fb_logo._CB1542065250_.png";

        public static string RottenTomatoesLogoUrl => "https://www.rottentomatoes.com/assets/pizza-pie/images/icons/global/cf-lg.3c29eff04f2.png";

        public static string MetacriticLogoUrl => "https://upload.wikimedia.org/wikipedia/commons/thumb/2/20/Metacritic.svg/1024px-Metacritic.svg.png";

        public static string DefaultLogo => "http://www.nashikproperty.com/uploads/builder-logo/default-logo.png";
        #endregion
    }
}
