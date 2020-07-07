using System;
using System.Collections.Generic;
using System.Text;

namespace WatchMeNow.Model
{
    public class TrackLyrics
    {
        public bool success { get; set; }

        public int microDuration { get; set; }

        public string artist { get; set; }

        public string song { get; set; }

        public Data result { get; set; }

        public class Data
        {
            public string lyrics { get; set; }

            public bool fromCache { get; set; }

            public string cacheId { get; set; }

            public Source source { get; set; }

            public class Source
            {
                public string name { get; set; }

                public string homepage { get; set; }

                public string url { get; set; }
            }
        }

    }
}
