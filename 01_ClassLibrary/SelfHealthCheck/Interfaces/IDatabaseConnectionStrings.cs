namespace SelfHealthCheck.Interfaces
{
    using System.Configuration;

    /// <summary>
    /// IDatabaseConnectionStrings interface - with respect to Self Health Check,
    /// this interface includes all of the methods necessary for checking
    /// the health of the database connection strings.
    /// </summary>
    public interface IDatabaseConnectionStrings
    {
        /// <summary>
        /// Get the connection strings
        /// </summary>
        /// <returns></returns>
        ConnectionStringSettingsCollection GetConnectionStrings();

        /// <summary>
        /// Get the WhiteListDataSourceItems
        /// </summary>
        /// <returns></returns>
        string[] GetWhiteListDataSourceItems();

        /// <summary>
        /// Determines if the connection strings in the configuration file are valid
        /// </summary>
        /// <returns></returns>
        POCO.DatabaseConnectionStringItem Validate();
    }
}
