using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using WatchMeNow.Utils;

namespace WatchMeNow.ViewModel
{
    public class ProfileViewModel
    {
        #region Constructor

        public ProfileViewModel()
        {
            LoadData();
        }

        #endregion

        #region Properties

        public string UserName { get; set; }

        public string CatalogLink { get; set; }

        #endregion

        #region Methods

        public void LoadData()
        {
            using (SQLiteConnection cnn = new SQLiteConnection(Settings.LocalDataBasePath))
            {
                var loginTable = cnn.Table<Utilities.LoginTable>();

                var row = loginTable.FirstOrDefault();

                if (row != null)
                    UserName = row.User;

                var catalogTable = cnn.Table<Utilities.MovieCatalog>();

                var catalogRow = catalogTable.FirstOrDefault();

                if (catalogRow != null)
                    CatalogLink = catalogRow.CatalogAddress;
            }
                
        }

        #endregion

    }
}
