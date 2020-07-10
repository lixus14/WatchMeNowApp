using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using WatchMeNow.Model;
using System.Threading.Tasks;
using WatchMeNow.Utils;

namespace WatchMeNow.Services
{
    public class RadioStationService
    {

        public async Task<RadioStationList> GetRadioStationCatalog(string serviceUri)
        {
            try
            {
                RestClient<RadioStationList> restClient = new RestClient<RadioStationList>();

                RadioStationList list = await restClient.GetAsync(serviceUri);

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
