using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WatchMeNow.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeScreenTabbedPage : TabbedPage
    {
        public HomeScreenTabbedPage ()
        {
            InitializeComponent();

            BarBackgroundColor = Color.Black;
        }
    }
}