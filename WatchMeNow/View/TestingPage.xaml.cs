using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchMeNow.Services;
using WatchMeNow.Utils;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WatchMeNow.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TestingPage : ContentPage
    {
        public TestingPage()
        {
            InitializeComponent();

            LoadData();
        }

        public void LoadData()
        {


        }
    }
}