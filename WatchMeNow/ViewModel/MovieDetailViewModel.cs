using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using WatchMeNow.Model;
using WatchMeNow.Services;
using WatchMeNow.Utils;
using Xamarin.Forms;
using static WatchMeNow.Model.MovieDetail;

namespace WatchMeNow.ViewModel
{
    public class MovieDetailViewModel
    {
        #region Constructor

        public MovieDetailViewModel()
        {
            _movieService = new MovieService();

            MovieDetail = new MovieDetail();

            MovieDetail.Ratings = new List<RatingDetail>();

            RatingsList = new List<RatingUIDetail>();

            LoadData();
        }

        #endregion

        #region Properties

        private string ImdbId { get; set; }

        public string MovieTitle { get; set; }

        public HtmlWebViewSource MovieEmbedCode { get; set; }

        public bool IsBusy { get; set; }

        private MovieService _movieService { get; set; }

        public MovieDetail MovieDetail { get; set; }

        public string VideoSourceES { get; set; }

        public string VideoSourceEN { get; set; }

        public List<RatingUIDetail> RatingsList { get;set; }

        public double RatingsListHeight { get; set; }

        #endregion

        #region Methods

        public void LoadData()
        {
            using (SQLiteConnection cnn = new SQLiteConnection(Settings.LocalDataBasePath))
            {
                var currentMovieTable = cnn.Table<Utilities.CurrentMovie>();

                var row = currentMovieTable.FirstOrDefault();

                if(row != null)
                {
                    ImdbId = row.ImdbId;

                    MovieTitle = row.Title;

                    VideoSourceEN = row.VideoSourceEN;

                    VideoSourceES = row.VideoSourceES;

                    MovieEmbedCode = new HtmlWebViewSource()
                    {
                        Html = @"<html bgcolor='#00000'><head></head><body>" + VideoSourceES + @"</body></html>"
                    };

                    IsBusy = true;

                    try
                    {
                        var mdetails = _movieService.GetMovieDetail(Settings.ImdbApiUri + ImdbId);

                        MovieDetail = mdetails;

                        SetRatingsList();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                cnn.Execute("DELETE FROM CurrentMovie");

            }
            IsBusy = false;


        }

        private void SetRatingsList()
        {
            foreach(var rating in MovieDetail.Ratings)
            {
                var newRating = new RatingUIDetail()
                {
                    Details = new RatingDetail()
                    {
                        Source = rating.Source,
                        Value = rating.Value
                    }
                };

                if (newRating.Details.Source == "Internet Movie Database")
                    newRating.RatingSiteUrl = Utilities.InternetMovieDatabaseLogoUrl;
                else if (newRating.Details.Source == "Metacritic")
                    newRating.RatingSiteUrl = Utilities.MetacriticLogoUrl;
                else if (newRating.Details.Source == "Rotten Tomatoes")
                    newRating.RatingSiteUrl = Utilities.RottenTomatoesLogoUrl;
                else
                    newRating.RatingSiteUrl = Utilities.DefaultLogo;

                RatingsList.Add(newRating);
            }

            RatingsListHeight = RatingsList.Count * 65;
        }


        #endregion
    }
}
