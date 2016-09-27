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
            // 1. Update the logic in loop to use splits for connectionString
            // 2. Figure out how to write the following tests using FakeItEasy
            //    2a. Simulate App.config that has a connectionStrings section with the 
            //        existing SampleLoggingConnectionString 
            //    2b. Simulate App.config that has a connectionStrings section with the 
            //        existing SampleLoggingConnectionString 
            // 3. Determine how to represent a list of results along with an error message
            // 4. Figure out how to write the following tests using FakeItEasy
            //    4a. Simulate no App.config - verify that you get the expected error
            //    4b. Simulate App.config that doesn't have a connectionStrings section
            //        - verify that you get the expected error
            //    4c. Simulate App.config that has multiple connectionStrings
            //        - verify that you get the expected error
            // 5. Once we have the above test coverage, refactor the logic out
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
                var setting = connectionStringSettings[i];
                result.Name = setting.Name;

                var connectionString = setting.ConnectionString;
                // Positive case only for right now, exact spacing and punctuation matching below
                // Data Source=.;Initial Catalog=SampleLogging_Tests;integrated security=SSPI;

                var searchString = "Data Source=";
                var dataSourceStubStart = connectionString.IndexOf(searchString, StringComparison.CurrentCultureIgnoreCase);
                var dataSourceStubEnd = connectionString.IndexOf(";");
                var startingPoint = dataSourceStubStart + searchString.Length;
                var endingPoint = dataSourceStubEnd - startingPoint;
                result.DatabaseSource = connectionString.Substring(startingPoint, endingPoint);
                connectionString = connectionString.Remove(0, dataSourceStubEnd + 1);

                searchString = "Initial Catalog=";
                var initialCatalogStubStart = connectionString.IndexOf(searchString, StringComparison.CurrentCultureIgnoreCase);
                var initialCatalogStubEnd = connectionString.IndexOf(";");
                startingPoint = initialCatalogStubStart + searchString.Length;
                endingPoint = initialCatalogStubEnd - startingPoint;
                result.InitialCatalog = connectionString.Substring(startingPoint, endingPoint);
                connectionString = connectionString.Remove(0, initialCatalogStubEnd + 1);

                searchString = "integrated security=";
                var integratedSecurityStringLocation = connectionString.IndexOf(searchString, StringComparison.CurrentCultureIgnoreCase);
                if (integratedSecurityStringLocation != -1)
                {
                    result.IsUsingIntegratedSecurity = true;
                }
            }

            return result;
        }
    }
}
