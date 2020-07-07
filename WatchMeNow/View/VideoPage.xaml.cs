using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchMeNow.Model;
using WatchMeNow.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WatchMeNow.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VideoPage : ContentPage
    {
        #region Constructor
        public VideoPage()
        {
            InitializeComponent();

            //lblwebsite.GestureRecognizers.Add(new TapGestureRecognizer((view) => OnWebsiteLinkClicked()));
        }

        #endregion

        #region Events
        private void OnWebsiteLinkClicked()
        {
            var vm = (BindingContext as MovieDetailViewModel);

            if(!string.IsNullOrEmpty(vm.MovieDetail.Website) && vm.MovieDetail.Website != "N/A")
                Device.OpenUri(new Uri(vm.MovieDetail.Website));
        }

        private async void BtnClose_Clicked(object sender, EventArgs e)
        {
            await App.Current.MainPage.Navigation.PopModalAsync();
        }

        private void WvVideo_Navigating(object sender, WebNavigatingEventArgs e)
        {
            e.Cancel = true;
        }

        private void BtnFullScreen_Clicked(object sender, EventArgs e)
        {
            svMovieInfo.IsVisible = false;

            gridOptionButtons.IsVisible = false;

            videoRowHeight.Height = GridLength.Star;

            Grid.SetRowSpan(wvVideo, 3);

            MessagingCenter.Send(this, "LandScape");

            NavigationPage.SetHasNavigationBar(this, false);

            MessagingCenter.Send(this, "HideStatusBar");
        }

        protected override void OnDisappearing()
        {
            MessagingCenter.Send(this, "Portrait");

            MessagingCenter.Send(this, "ShowStatusBar");

            base.OnDisappearing();
        }

        private void BtnSpanish_Clicked(object sender, EventArgs e)
        {
            btnEnglish.IsEnabled = true;
            btnSpanish.IsEnabled = false;

            var vm = (BindingContext as MovieDetailViewModel);

            var newWebSource = new HtmlWebViewSource()
            {
                Html = @"<html bgcolor='#00000'><head></head><body>" + vm.VideoSourceES + @"</body></html>"
            };

            webSource.Html = newWebSource.Html;
        }

        private void BtnEnglish_Clicked(object sender, EventArgs e)
        {
            btnEnglish.IsEnabled = false;
            btnSpanish.IsEnabled = true;

            var vm = (BindingContext as MovieDetailViewModel);

            var newWebSource = new HtmlWebViewSource()
            {
                Html = @"<html bgcolor='#00000'><head></head><body>" + vm.VideoSourceEN + @"</body></html>"
            };

            webSource.Html = newWebSource.Html;

        }

        #endregion
    }
}