namespace SelfHealthCheck.Interfaces
{
    /// <summary>
    /// IDatabaseConnectionStrings interface - with respect to Self Health Check,
    /// this interface includes all of the methods necessary for checking
    /// the health of the database connection strings.
    /// </summary>
    public interface IDatabaseConnectionStrings
    {
        /// <summary>
        /// Determines if the connection strings in the configuration file are valid
        /// </summary>
        /// <returns></returns>
        POCO.DatabaseConnectionStringResult Validate();
    }
}
