namespace SelfHealthCheck
{
    using System.Collections.Specialized;
    using System.Configuration;

    /// <summary>
    /// CustomConfigurationManager manager gives you the properties for items
    /// from the configuration file while being able to fake/stub in the test classes.
    /// </summary>
    public class CustomConfigurationManager : Interfaces.ICustomConfigurationManager
    {
        /// <summary>
        /// AppSettings used to get the appSettings from the configuration file
        /// </summary>
        public NameValueCollection AppSettings
        {
            get
            {
                return ConfigurationManager.AppSettings;
            }
        }

        /// <summary>
        /// ConnectionStrings used to get the connectionStrings from the configuration file
        /// </summary>
        public ConnectionStringSettingsCollection ConnectionStrings
        {
            get
            {
                return ConfigurationManager.ConnectionStrings;
            }
        }
    }
}
