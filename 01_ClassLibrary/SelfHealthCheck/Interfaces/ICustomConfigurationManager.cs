namespace SelfHealthCheck.Interfaces
{
    using System.Configuration;

    /// <summary>
    /// ICustomConfigurationManager interface gives you the properties for items
    /// from the configuration file while being able to fake/stub in the test classes.
    /// </summary>
    public interface ICustomConfigurationManager
    {
        /// <summary>
        /// ConnectionStrings used to get the connectionStrings from the configuration file
        /// </summary>
        ConnectionStringSettingsCollection ConnectionStrings { get; }
    }
}
