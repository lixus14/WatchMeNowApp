using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WatchMeNow.Utils
{
    /// <summary>
    /// RestClient implements methods for calling CRUD operations using HTTP.
    /// </summary>
    public class RestClient<T>
    {

        /// <summary>
        /// Gets a string return message for the specified keyword.
        /// </summary>
        /// <param name="channelId">Keyword to be attached to the api service uri.</param>
        /// <returns>The string message returned by the api service.</returns>
        public async Task<T> GetAsync(string serviceUri)
        {
            try
            {
                HttpClient httpClient = new HttpClient();

                HttpRequestMessage request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(serviceUri)
                };

                HttpResponseMessage response = await httpClient.SendAsync(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string data = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<T>(data);
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    // No data was found
                    return default;
                }
                else
                {
                    // Throw new Exception
                    throw new HttpRequestException(response.Content.ToString());
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }


        }
    }

}
