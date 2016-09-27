namespace SelfHealthCheck
{
    using System;

    /// <summary>
    /// DatabaseConnectionStrings class - with respect to Self Health Check,
    /// this class includes all of the methods necessary for checking
    /// the health of the database connection strings.
    /// </summary>
    public class DatabaseConnectionStrings : Interfaces.IDatabaseConnectionStrings
    {
        /// <summary>
        /// Determines if the connection strings in the configuration file are valid
        /// </summary>
        /// <returns></returns>
        public POCO.DatabaseConnectionStringResult Validate()
        {
            // TODOs:
            // 1. Figure out how to write the following tests using FakeItEasy
            //    1a. Simulate App.config that has a connectionStrings section with the 
            //        existing SampleLoggingConnectionString 
            //    1b. Simulate App.config that has a connectionStrings section with the 
            //        existing SampleLoggingConnectionString 
            // 2. Determine how to represent a list of results along with an error message
            // 3. Figure out how to write the following tests using FakeItEasy
            //    3a. Simulate no App.config - verify that you get the expected error
            //    3b. Simulate App.config that doesn't have a connectionStrings section
            //        - verify that you get the expected error
            //    3c. Simulate App.config that has multiple connectionStrings
            //        - verify that you get the expected error
            // 4. Once we have the above test coverage, refactor the logic out
            //    of the Validate method but make sure the tests continue to pass
            var connectionStringSettings = System.Configuration.ConfigurationManager.ConnectionStrings;
            var result = new POCO.DatabaseConnectionStringResult();

            // if there isn't any config file, there apparently are default connection strings.
            // if there is a config file and no connectionStrings entry, there apparently are default connection strings.
            if (connectionStringSettings.Count < 3)
            {
                result.Name = "Either no configuration file exists or no connectionString entry exists";
                return result;
            }

            for (int i = 2; i < connectionStringSettings.Count; i++)
            {
                result.Name = connectionStringSettings[i].Name;
                string[] connectionStringItems = connectionStringSettings[i].ConnectionString.Split(';');

                var dataSourceSearchString = "Data Source=";
                var initialCatalogSearchString = "Initial Catalog=";
                var integratedSecuritySearchString = "integrated security=";
                foreach (string individualItem in connectionStringItems)
                {
                    if (!string.IsNullOrWhiteSpace(individualItem))
                    {
                        var localIndividualItem = individualItem.Replace(" =", "=").Replace("= ", "=");

                        if (localIndividualItem.StartsWith(dataSourceSearchString))
                        {
                            result.DatabaseSource = localIndividualItem.Substring(dataSourceSearchString.Length, localIndividualItem.Length - dataSourceSearchString.Length).Trim();
                        }
                        else if (localIndividualItem.StartsWith(initialCatalogSearchString))
                        {
                            result.InitialCatalog = localIndividualItem.Substring(initialCatalogSearchString.Length, localIndividualItem.Length - initialCatalogSearchString.Length).Trim();
                        }
                        else if (localIndividualItem.StartsWith(integratedSecuritySearchString))
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
