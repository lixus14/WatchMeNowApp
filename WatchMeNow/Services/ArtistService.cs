using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using WatchMeNow.Model;
using System.Threading.Tasks;
using WatchMeNow.Utils;

namespace WatchMeNow.Services
{
    public class ArtistService
    {

        public MusicArtistList GetArtistCatalog(string serviceUri)
        {
            try
            {
                RestClient<MusicArtistList> restClient = new RestClient<MusicArtistList>();

                MusicArtistList list = Task.Run(async () => await restClient.GetAsync(serviceUri)).Result;

                return list;
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException(ex.Message, ex);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public MusicArtistDetail GetArtistDetail(string serviceUri)
        {
            try
            {
                RestClient<MusicArtistDetail> restClient = new RestClient<MusicArtistDetail>();

                MusicArtistDetail detail = Task.Run(async () => await restClient.GetAsync(serviceUri)).Result;

                return detail;
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException(ex.Message, ex);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

    }
}
