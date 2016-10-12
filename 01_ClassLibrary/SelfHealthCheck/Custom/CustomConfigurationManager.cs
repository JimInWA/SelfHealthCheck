namespace SelfHealthCheck.Custom
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Linq;

    /// <summary>
    /// CustomConfigurationManager manager gives you the properties for items
    /// from the configuration file while being able to fake/stub in the test classes.
    /// </summary>
    public class CustomConfigurationManager : Interfaces.ICustomConfigurationManager
    {
        // ToDo: Write tests for this class

        /// <summary>
        /// AppSettings used to get the AppSettings key where the value is an actual string that doesn't need to be converted to something else.
        /// Caller will need to handle any exception.
        /// </summary>
        public NameValueCollection AppSettings
        {
            get
            {
                return ConfigurationManager.AppSettings;
            }
        }

        /// <summary>
        /// ConnectionStrings used to get the connectionStrings from the configuration file.
        /// Caller will need to handle any exception.
        /// </summary>
        public ConnectionStringSettingsCollection ConnectionStrings
        {
            get
            {
                return ConfigurationManager.ConnectionStrings;
            }
        }

        /// <summary>
        /// TryGetValueCollectionByKey used to AppSettings keys where the values represent collections (lists).
        /// If exception occurs, empty collection of T is returned.
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<T> TryGetValueCollectionByKey<T>(string key)
        {
            var valueAsString = ConfigurationManager.AppSettings[key];

            try
            {
                if (valueAsString == null)
                {
                    throw new SettingsPropertyNotFoundException(string.Format("AppSettings key [{0}] not found", key));
                }

                return valueAsString.Split(',').Select(s => (T)Convert.ChangeType(s.Trim(), typeof(T)));
            }
            catch (Exception)
            {
                return Enumerable.Empty<T>();
            }
        }

        /// <summary>
        /// TryGetValueByKey used to get AppSettings keys where the values are being converted from the stored string to some other data type
        /// If exception occurs, default value of T is returned.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T TryGetValueByKey<T>(string key)
        {
            var valueAsString = ConfigurationManager.AppSettings[key];

            try
            {
                if (valueAsString == null)
                {
                    throw new SettingsPropertyNotFoundException(string.Format("AppSettings key [{0}] not found", key));
                }

                return (T)Convert.ChangeType(valueAsString.Trim(), typeof(T));
            }
            catch (Exception)
            {
                return default(T);
            }
        }
    }
}
