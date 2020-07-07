using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WatchMeNow.Model;
using WatchMeNow.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WatchMeNow.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MoviesPage : ContentPage
    {
        public MoviesPage()
        {
            MovieOrder = "AtoZ";

            InitializeComponent();

            flowList.RefreshCommand = RefreshCommand;
        }

        public static string MovieOrder { get; set; }

        private void BtnMovieSearch_Clicked(object sender, EventArgs e)
        {
            NavigationPage.SetHasNavigationBar(this, false);

            sbMovies.IsVisible = true;
            sbMovies.Focus();
        }

        private void SbMovies_TextChanged(object sender, TextChangedEventArgs e)
        {
            var movieList = (BindingContext as MoviesViewModel).MovieList;

            var searchText = e.NewTextValue;

            if (string.IsNullOrEmpty(searchText))
            {
                if (MovieOrder == "AtoZ")
                    movieList = movieList.OrderBy(m => m.Title).ToList();
                else
                    movieList = movieList.OrderByDescending(m => m.Year).ToList();
                    
                flowList.FlowItemsSource = movieList;
            }
                

            else
            {
                var filteredList = movieList.Where(m => m.Title.ToLower().Contains(searchText.ToLower()));

                if (MovieOrder == "AtoZ")
                    filteredList.OrderBy(m => m.Title);
                else
                    filteredList.OrderByDescending(m => m.Year);

                flowList.FlowItemsSource = filteredList.ToList();
            }
        }

        public ICommand RefreshCommand
        {
            get
            {
                return new Command(() =>
                {
                    var vm = (BindingContext as MoviesViewModel);

                    vm.LoadData();

                    flowList.FlowItemsSource = vm.MovieList.ToList();

                    flowList.EndRefresh();
                });
            }
        }

        private void SbMovies_Unfocused(object sender, FocusEventArgs e)
        {

            if(string.IsNullOrEmpty(sbMovies.Text) || string.IsNullOrWhiteSpace(sbMovies.Text))
            {
                sbMovies.IsVisible = false;
                sbMovies.Text = string.Empty;

                NavigationPage.SetHasNavigationBar(this, true);
            }

        }

        private void BtnOrder_Clicked(object sender, EventArgs e)
        {
            if (MovieOrder == "AtoZ")
            {
                MovieOrder = "Date";

                btnOrder.IconImageSource = "Date.png";

                var movieList = (BindingContext as MoviesViewModel).MovieList;

                var filteredList = movieList.OrderByDescending(m => m.Year).ToList();

                flowList.FlowItemsSource = filteredList;
            }
            else
            {
                MovieOrder = "AtoZ";

                btnOrder.IconImageSource = "AtoZ.png";

                var movieList = (BindingContext as MoviesViewModel).MovieList;

                var filteredList = movieList.OrderBy(m => m.Title).ToList();

                flowList.FlowItemsSource = filteredList;
            }
            
        }
    }
}