namespace SelfHealthCheck.POCO
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// DatabaseConnectionStringResult class
    /// </summary>
    public class DatabaseConnectionStringResult
    {
        /// <summary>
        /// DatabaseConnectionStringResult constructor
        /// </summary>
        public DatabaseConnectionStringResult()
        {
            ItemFromConfigurationFile = new Collection<DatabaseConnectionStringItem>();
            ErrorMessageInformation = new Collection<string>();
        }

        /// <summary>
        /// ItemFromConfigurationFile 
        /// </summary>
        public ICollection<DatabaseConnectionStringItem> ItemFromConfigurationFile { get; set; }

        /// <summary>
        /// ErrorMessageInformation
        /// </summary>
        public ICollection<string> ErrorMessageInformation { get; set; }
    }
}
