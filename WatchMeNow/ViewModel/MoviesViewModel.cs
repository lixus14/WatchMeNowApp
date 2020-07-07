using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using WatchMeNow.Model;
using WatchMeNow.Services;
using WatchMeNow.Utils;
using WatchMeNow.View;
using Xamarin.Forms;

namespace WatchMeNow.ViewModel
{
    public class MoviesViewModel
    {
        #region Constructor
        public MoviesViewModel()
        {
            _movieService = new MovieService();

            MovieList = new List<MovieListItem>();
           
            Title = "Movies";

            ItemTappedCommand = new Command((param) => 
                OpenMovie(param)
            );

            LoadData();
        }

        #endregion

        #region Properties
        private MovieService _movieService { get; set; }

        public List<MovieListItem> MovieList { get; set; }

        public string Title { get; set; }

        public ICommand ItemTappedCommand { get; set; }

        #endregion

        #region Methods
        [Obsolete]
        private void OpenMovie(object param)
        {
            var movieDetail = param as MovieListItem;

            using (SQLiteConnection cnn = new SQLiteConnection(Settings.LocalDataBasePath))
            {
                cnn.Insert(new Utilities.CurrentMovie()
                {
                    Id = 1,
                    Title = movieDetail.Title,
                    ImdbId = movieDetail.ImdbId,
                    VideoSourceES = movieDetail.VideoSourceES,
                    VideoSourceEN = movieDetail.VideoSourceEN,
                    Year = movieDetail.Year
                });
            }

            App.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new VideoPage())
            {
                BarBackgroundColor = Color.Black,
                BarTextColor = Color.Orange
            });
        }

        public void LoadData()
        {

            try
            {
                MovieList.Clear();

                var catalog = _movieService.GetMovieCatalog(Settings.MovieCatalog);

                foreach(var movie in catalog.MovieCatalog)
                {
                    MovieList.Add(movie);
                }

                if (MoviesPage.MovieOrder == "AtoZ")
                    MovieList = MovieList.OrderBy(x => x.Title).ToList();
                else
                    MovieList = MovieList.OrderByDescending(x => x.Year).ToList();

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            

        }

        #endregion
    }
}
