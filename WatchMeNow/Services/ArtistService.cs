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

        public async Task<MusicArtistList> GetArtistCatalog(string serviceUri)
        {
            try
            {
                RestClient<MusicArtistList> restClient = new RestClient<MusicArtistList>();

                MusicArtistList list = await restClient.GetAsync(serviceUri);

                return list;
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException(ex.Message, ex);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public async Task<MusicArtistDetail> GetArtistDetail(string serviceUri)
        {
            try
            {
                RestClient<MusicArtistDetail> restClient = new RestClient<MusicArtistDetail>();

                MusicArtistDetail detail = await restClient.GetAsync(serviceUri);

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
