namespace SelfHealthCheck
{
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
        public string Validate()
        {
            var connectionStringSettings = System.Configuration.ConfigurationManager.ConnectionStrings;

            // if there isn't any config file, there apparently are default connection strings.
            // if there is a config file and no connectionStrings entry, there apparently are default connection strings.
            if (connectionStringSettings.Count < 3)
            {
                return "Either no configuration file exists or no connectionString entry exists";
            }

            var result = string.Empty;

            for (int i = 2; i < connectionStringSettings.Count; i++)
            {
                var sub = connectionStringSettings[i];

                result = string.Format("Name \"{0}\", ProviderName \"{1}\", ConnectionString \"{2}\"", sub.Name, sub.ProviderName, sub.ConnectionString);
            }

            return result;
        }
    }
}
