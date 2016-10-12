using System.Globalization;
using SelfHealthCheck.Custom;

namespace SelfHealthCheck.Test.TestCases.Custom
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Linq;

    /// <summary>
    /// CustomConfigurationManager Tests
    /// </summary>
    [TestClass]
    public class CustomConfigurationManagerTest
    {
        #region Private Fields

        private static SelfHealthCheck.Custom.Interfaces.ICustomConfigurationManager _customConfigurationManager;

        #endregion

        #region Test Initialization

        /// <summary>
        /// TestInitializer method
        /// </summary>
        [TestInitialize]
        public void TestInitializer()
        {
            _customConfigurationManager = new CustomConfigurationManager();
        }

        #endregion

        /// <summary>
        /// AppSettings_Property_Happy_Path_returns_expected_values
        /// </summary>
        [TestMethod]
        public void AppSettings_Property_Happy_Path_returns_expected_values()
        {
            // Arrange
            const string expectedKeyName = "sampleKey";

            // Act
            var result = _customConfigurationManager.AppSettings;

            // Assert
            Assert.IsNotNull(result, "AppSettings public property returned null");
            Assert.IsTrue(result.Count >= 1, "AppSettings public property has an unexpected Count value");
            Assert.IsTrue(result.AllKeys.Contains(expectedKeyName), "AppSettings public property does not have the \"{0}\" key", expectedKeyName);
        }

        /// <summary>
        /// ConnectionStrings_Property_Happy_Path_returns_expected_values
        /// </summary>
        [TestMethod]
        public void ConnectionStrings_Property_Happy_Path_returns_expected_values()
        {
            // Arrange
            const string expectedConnectionStringName1 = "LocalSqlServer";
            const string expectedConnectionStringName2 = "OraAspNetConString";
            const string expectedConnectionStringName3 = "SampleLoggingConnectionString";

            // Act
            var result = _customConfigurationManager.ConnectionStrings;

            // Note: if there is no configuration file or an empty configuration file
            // or a configuration file that does not have the connectionStrings section,
            // on my box the ConnectionStrings section still has 2 entries:
            // 1. LocalSqlServer
            // 2. OraAspNetConString
            //
            // Also on my box, if there is a connectionStrings section then the above
            // entries are the first two valus
            //
            // At this point the only difference that I have found is that the ElementInformation Source
            // property is null for the first two values and is not null for any of the values that
            // actually exist in the connectionStrings section.
            // 
            // I have found not documentation that explains this.

            // Assert
            Assert.IsNotNull(result, "ConnectionStrings public property returned null");
            Assert.IsTrue(result.Count >= 3, "ConnectionStrings public property has an unexpected Count value");
            Assert.IsTrue(result[0].Name.Contains(expectedConnectionStringName1), "ConnectionStrings public property does not have the \"{0}\" Name", expectedConnectionStringName1);
            Assert.IsNull(((result[0]).ElementInformation).Source, "ConnectionStrings public property does not have the expected value for ElementInformation Source");
            Assert.IsTrue(result[1].Name.Contains(expectedConnectionStringName2), "ConnectionStrings public property does not have the \"{0}\" Name", expectedConnectionStringName2);
            Assert.IsNull(((result[1]).ElementInformation).Source, "ConnectionStrings public property does not have the expected value for ElementInformation Source");
            Assert.IsTrue(result[2].Name.Contains(expectedConnectionStringName3), "ConnectionStrings public property does not have the \"{0}\" Name", expectedConnectionStringName3);
            Assert.IsNotNull(((result[2]).ElementInformation).Source, "ConnectionStrings public property does not have the expected value for ElementInformation Source");
        }
    }
}
