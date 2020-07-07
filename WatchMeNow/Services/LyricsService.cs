using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using WatchMeNow.Model;
using System.Threading.Tasks;
using WatchMeNow.Utils;

namespace WatchMeNow.Services
{
    public class LyricsService
    {

        public TrackLyrics GetTrackLyrics(string serviceUri)
        {
            try
            {
                RestClient<TrackLyrics> restClient = new RestClient<TrackLyrics>();

                TrackLyrics list = Task.Run(async () => await restClient.GetAsync(serviceUri)).Result;

                return list;
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException(ex.Message, ex);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }


    }
}
