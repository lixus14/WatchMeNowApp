using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using WatchMeNow.Model;
using System.Threading.Tasks;
using WatchMeNow.Utils;

namespace WatchMeNow.Services
{
    public class MovieService
    {

        public MovieList GetMovieCatalog(string serviceUri)
        {
            try
            {
                RestClient<MovieList> restClient = new RestClient<MovieList>();

                MovieList list = Task.Run(async () => await restClient.GetAsync(serviceUri)).Result;

                return list;
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException(ex.Message, ex);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public MovieDetail GetMovieDetail(string serviceUri)
        {
            try
            {
                RestClient<MovieDetail> restClient = new RestClient<MovieDetail>();

                MovieDetail detail = Task.Run(async () => await restClient.GetAsync(serviceUri)).Result;

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
