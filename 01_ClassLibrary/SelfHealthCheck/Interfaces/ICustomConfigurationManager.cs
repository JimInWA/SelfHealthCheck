namespace SelfHealthCheck.Interfaces
{
    using System.Collections.Specialized;
    using System.Configuration;

    /// <summary>
    /// ICustomConfigurationManager interface gives you the properties for items
    /// from the configuration file while being able to fake/stub in the test classes.
    /// </summary>
    public interface ICustomConfigurationManager
    {
        /// <summary>
        /// AppSettings used to get the appSettings from the configuration file
        /// </summary>
        NameValueCollection AppSettings { get; }

        /// <summary>
        /// ConnectionStrings used to get the connectionStrings from the configuration file
        /// </summary>
        ConnectionStringSettingsCollection ConnectionStrings { get; }
    }
}
