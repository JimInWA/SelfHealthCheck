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
        public ConnectionStringSettingsCollection Fetch()
        {
            var result = _customConfigurationManager.ConnectionStrings;

            return result;
        }

        /// <summary>
        /// Determines if the connection strings in the configuration file are valid
        /// </summary>
        /// <returns></returns>
        public POCO.DatabaseConnectionStringResult Validate()
        {
            var result = new POCO.DatabaseConnectionStringResult();

            // TODOs:
            // 1. Determine how to represent a list of results along with an error message
            // 2. Introduce the WhiteList appSettings keys (and the ability to fake/stub
            //    those keys)
            var connectionStringSettings = Fetch();

            if (connectionStringSettings.Count < 1)
            {
                result.Name = "Either no configuration file exists or no connectionString entry exists";
                return result;
            }

            result = BreakConnectionStringIntoSeparateValues(connectionStringSettings);

            return result;
        }

        private POCO.DatabaseConnectionStringResult BreakConnectionStringIntoSeparateValues(ConnectionStringSettingsCollection connectionStringSettings)
        {
            var result = new POCO.DatabaseConnectionStringResult();

            var dataSourceSearchString = "Data Source=";
            var initialCatalogSearchString = "Initial Catalog=";
            var integratedSecuritySearchString = "integrated security=";
            for (int i = 0; i < connectionStringSettings.Count; i++)
            {
                result.Name = connectionStringSettings[i].Name;
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
                            result.DatabaseSource = localIndividualItem.Substring(dataSourceSearchString.Length, localIndividualItem.Length - dataSourceSearchString.Length).Trim();
                        }
                        else if ((!initialCatalogFound) && (localIndividualItem.StartsWith(initialCatalogSearchString)))
                        {
                            result.InitialCatalog = localIndividualItem.Substring(initialCatalogSearchString.Length, localIndividualItem.Length - initialCatalogSearchString.Length).Trim();
                        }
                        else if ((!integratedSecurityFound) && (localIndividualItem.StartsWith(integratedSecuritySearchString)))
                        {
                            result.IsUsingIntegratedSecurity = true;
                        }
                    }
                }
            }

            return result;
        }
    }
}
