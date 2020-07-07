using SQLite;
using System;
using System.IO;
using Xamarin.Essentials;

namespace WatchMeNow.Utils
{
    public static class Settings
    {
       
        public static string CurrentUserName { get; set; }

        public static string OneSignalApiKey
        {
            get
            {
                return "4ff0882b-0be1-429f-849e-17510889c10b";
            }
        }

        public static string MovieCatalog
        {
            get
            {
                using (SQLiteConnection cnn = new SQLiteConnection(Settings.LocalDataBasePath))
                {
                    var loginTable = cnn.Table<Utilities.MovieCatalog>();

                    var row = loginTable.FirstOrDefault();

                    if (row != null)
                        return row.CatalogAddress;

                    return string.Empty;
                }

            }
        }

        public static string ArtistCatalog
        {
            get
            {
                using (SQLiteConnection cnn = new SQLiteConnection(Settings.LocalDataBasePath))
                {
                    var loginTable = cnn.Table<Utilities.ArtistCatalog>();

                    var row = loginTable.FirstOrDefault();

                    if (row != null)
                        return row.CatalogAddress;

                    return string.Empty;
                }

            }
        }

        public static string TrackLyricsPath
        {
            get
            {
                return @"https://lixus14.github.io/WatchMeNow/SongLyrics";
            }
        }

        public static string TrackLyricsUri
        {
            get
            {
                return @"https://mourits.xyz:2096/?";
            }
        }

        public static string ImdbApiUri
        {
            get
            {
                return @"http://www.omdbapi.com/?apikey=" + ImdbApiKey + "&i=";
            }
        }

        public static string ImdbApiKey
        {
            get
            {
                return "e370807e";
            }
        }

        public static double DeviceWidth
        {
            get
            {
                return DeviceDisplay.MainDisplayInfo.Width;
            }
        }

        public static double DeviceHeight
        {
            get
            {
                return DeviceDisplay.MainDisplayInfo.Height;
            }
        }

        public static string LocalDataBasePath
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "database.db3"); 
            }
        }


    }
}

