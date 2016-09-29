namespace SelfHealthCheck
{
    using System.Configuration;

    /// <summary>
    /// DatabaseConnectionStrings class - with respect to Self Health Check,
    /// this class includes all of the methods necessary for checking
    /// the health of the database connection strings.
    /// </summary>
    public class DatabaseConnectionStrings : Interfaces.IDatabaseConnectionStrings
    {
        #region Private Fields

        private static Interfaces.ICustomConfigurationManager _customConfigurationManager;

        #endregion

        /// <summary>
        /// DatabaseConnectionStrings constructor 
        /// </summary>
        /// <param name="customConfigurationManager"></param>
        public DatabaseConnectionStrings(Interfaces.ICustomConfigurationManager customConfigurationManager)
        {
            _customConfigurationManager = customConfigurationManager;
        }

        /// <summary>
        /// Fetch the connection strings
        /// </summary>
        /// <returns></returns>
        public ConnectionStringSettingsCollection GetConnectionStrings()
        {
            var result = _customConfigurationManager.ConnectionStrings;

            return result;
        }

        /// <summary>
        /// Get the WhiteListDataSourceItems
        /// </summary>
        /// <returns></returns>
        public string[] GetWhiteListDataSourceItems()
        {
            var whiteListDataSourceItemsRaw = _customConfigurationManager.AppSettings.Get("WhiteListDataSourceItems");
            string[] result = (whiteListDataSourceItemsRaw == null) ? new string[0] : whiteListDataSourceItemsRaw.Split(',');

            return result;
        }

        /// <summary>
        /// Determines if the connection strings in the configuration file are valid
        /// </summary>
        /// <returns></returns>
        public POCO.DatabaseConnectionStringResult Validate()
        {
            var result = new POCO.DatabaseConnectionStringResult();
            var item = new POCO.DatabaseConnectionStringItem();

            // TODOs:
            // 1. Determine how to represent a list of results along with an error message
            //    and update the tests
            // 2. Change Validate to return a JSON data structure and update the tests
            // 3. Create an ASP.NET MVC Controller that calls the Validate method and 
            //    returns the JSON data structure
            var connectionStringSettings = GetConnectionStrings();

            if (connectionStringSettings.Count < 1)
            {
                item.Name = "Either no configuration file exists or no connectionString section exists";
                result.ItemFromConfigurationFile.Add(item);
                return result;
            }

            var whiteListDataSourceItems = GetWhiteListDataSourceItems();

            if (whiteListDataSourceItems.Length == 0)
            {
                item.Name = "Either no configuration file exists or not appSettings section exists or the WhiteListDataSourceItems appSettings key doesn't exist";
                result.ItemFromConfigurationFile.Add(item);
                return result;
            }

            result = BreakConnectionStringIntoSeparateValues(connectionStringSettings, whiteListDataSourceItems);
            return result;
        }

        private POCO.DatabaseConnectionStringResult BreakConnectionStringIntoSeparateValues(ConnectionStringSettingsCollection connectionStringSettings, string[] whiteListDataSourceItems)
        {
            var result = new POCO.DatabaseConnectionStringResult();
            POCO.DatabaseConnectionStringItem item = null;

            var dataSourceSearchString = "Data Source=";
            var initialCatalogSearchString = "Initial Catalog=";
            var integratedSecuritySearchString = "integrated security=";
            for (int i = 0; i < connectionStringSettings.Count; i++)
            {
                item = new POCO.DatabaseConnectionStringItem();
                item.Name = connectionStringSettings[i].Name;
                string[] connectionStringItems = connectionStringSettings[i].ConnectionString.Split(';');

                var dataSourceFound = false;
                var initialCatalogFound = false;
                var integratedSecurityFound = false;
                foreach (string individualItem in connectionStringItems)
                {
                    if (!string.IsNullOrWhiteSpace(individualItem))
                    {
                        var localIndividualItem = individualItem.Replace(" =", "=").Replace("= ", "=");

                        if ((!dataSourceFound) && (localIndividualItem.StartsWith(dataSourceSearchString)))
                        {
                            item.DatabaseSource = localIndividualItem.Substring(dataSourceSearchString.Length, localIndividualItem.Length - dataSourceSearchString.Length).Trim();
                        }
                        else if ((!initialCatalogFound) && (localIndividualItem.StartsWith(initialCatalogSearchString)))
                        {
                            item.InitialCatalog = localIndividualItem.Substring(initialCatalogSearchString.Length, localIndividualItem.Length - initialCatalogSearchString.Length).Trim();
                        }
                        else if ((!integratedSecurityFound) && (localIndividualItem.StartsWith(integratedSecuritySearchString)))
                        {
                            item.IsUsingIntegratedSecurity = true;
                        }
                    }
                }

                item.IsInWhiteList = IsDataSourceInWhiteList(item, whiteListDataSourceItems);

                result.ItemFromConfigurationFile.Add(item);
            }

            return result;
        }

        private bool IsDataSourceInWhiteList(POCO.DatabaseConnectionStringItem result, string[] whiteListDataSourceItems)
        {
            var isDataSourceInWhiteList = false;
            foreach (string individualItem in whiteListDataSourceItems)
            {
                // case sensitive matching
                if (result.DatabaseSource == individualItem)
                {
                    isDataSourceInWhiteList = true;
                    break;
                }
            }

            return isDataSourceInWhiteList;
        }
    }
}
