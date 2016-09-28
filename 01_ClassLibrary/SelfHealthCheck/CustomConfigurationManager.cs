namespace SelfHealthCheck
{
    using System.Configuration;

    /// <summary>
    /// CustomConfigurationManager manager gives you the properties for items
    /// from the configuration file while being able to fake/stub in the test classes.
    /// </summary>
    public class CustomConfigurationManager : Interfaces.ICustomConfigurationManager
    {
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
