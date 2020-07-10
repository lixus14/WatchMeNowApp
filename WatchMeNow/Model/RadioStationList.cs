using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace WatchMeNow.Model
{
    public class RadioStationList:BasicModel
    {
        private ObservableCollection<RadioStationListItem> _radioStationsCatalog;
        public ObservableCollection<RadioStationListItem> RadioStationsCatalog
        {
            get { return _radioStationsCatalog; }
            set { _radioStationsCatalog = value; OnPropertyChanged(); }
        }
    }
}
