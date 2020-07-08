using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace WatchMeNow.Model
{
    public class MusicArtistList:BasicModel
    {
        private ObservableCollection<ArtistListItem> _artistCatalog;
        public ObservableCollection<ArtistListItem> ArtistCatalog
        {
            get { return _artistCatalog; }
            set { _artistCatalog = value; OnPropertyChanged(); }
        }
    }
}
