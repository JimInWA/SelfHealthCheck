namespace SelfHealthCheck.POCO
{
    /// <summary>
    /// DatabaseConnectionStringItem class
    /// </summary>
    public class DatabaseConnectionStringItem
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
        /// ConnectionString IsUsingIntegratedSecurity (part of the connectionString attribute) 
        /// </summary>
        public bool IsUsingIntegratedSecurity { get; set; }

        /// <summary>
        /// Is this connection string in the WhiteList?
        /// </summary>
        public bool IsInWhiteList { get; set; }
    }
}
