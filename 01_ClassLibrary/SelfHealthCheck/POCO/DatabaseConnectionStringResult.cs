﻿namespace SelfHealthCheck.POCO
{
    /// <summary>
    /// DatabaseConnectionString Result object
    /// </summary>
    public class DatabaseConnectionStringResult
    {
        /// <summary>
        /// ConnectionString Name attribute
        /// </summary>
        public string Name {get; set; }

        /// <summary>
        /// ConnectionString DataSource (part of the connectionString attribute)
        /// </summary>
        public string DatabaseSource { get; set; }

        /// <summary>
        /// ConnectionString InitialCatalog (part of the connectionString attribute)
        /// </summary>
        public string InitialCatalog { get; set; }

        /// <summary>
        /// ConnectionString (part of the connectionString attribute) 
        /// </summary>
        public bool IsUsingIntegratedSecurity { get; set; }
    }
}