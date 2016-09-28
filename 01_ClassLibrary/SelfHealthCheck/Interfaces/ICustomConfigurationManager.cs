namespace SelfHealthCheck.Interfaces
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;

    /// <summary>
    /// ICustomConfigurationManager interface gives you the properties for items
    /// from the configuration file while being able to fake/stub in the test classes.
    /// </summary>
    public interface ICustomConfigurationManager
    {
        /// <summary>
        /// AppSettings used to get the AppSettings key where the value is an actual string that doesn't need to be converted to something else
        /// </summary>
        NameValueCollection AppSettings { get; }

        /// <summary>
        /// ConnectionStrings used to get the connectionStrings from the configuration file
        /// </summary>
        ConnectionStringSettingsCollection ConnectionStrings { get; }

        /// <summary>
        /// TryGetValueCollectionByKey used to AppSettings keys where the values represent collections (lists)
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEnumerable<T> TryGetValueCollectionByKey<T>(string key);

        /// <summary>
        /// TryGetValueByKey used to get AppSettings keys where the values are being converted from the stored string to some other data type
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T TryGetValueByKey<T>(string key);
    }
}
