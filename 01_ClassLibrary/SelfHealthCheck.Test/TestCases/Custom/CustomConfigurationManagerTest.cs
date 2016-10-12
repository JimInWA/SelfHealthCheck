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
        /// AppSettings_Property_returns_expected_values
        /// </summary>
        [TestMethod]
        public void AppSettings_Property_returns_expected_values()
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
        /// ConnectionStrings_Property_returns_expected_values
        /// </summary>
        [TestMethod]
        public void ConnectionStrings_Property_returns_expected_values()
        {
            // Arrange
            const string expectedConnectionStringName1 = "LocalSqlServer";
            const string expectedConnectionStringName2 = "OraAspNetConString";
            const string expectedConnectionStringName3 = "SampleLoggingConnectionString";

            // Act
            var result = _customConfigurationManager.ConnectionStrings;

            // Assert
            Assert.IsNotNull(result, "ConnectionStrings public property returned null");
            Assert.IsTrue(result.Count >= 3, "ConnectionStrings public property has an unexpected Count value");
            Assert.IsTrue(result[0].Name.Contains(expectedConnectionStringName1), "ConnectionStrings public property does not have the \"{0}\" Name", expectedConnectionStringName1);
            Assert.IsTrue(result[1].Name.Contains(expectedConnectionStringName2), "ConnectionStrings public property does not have the \"{0}\" Name", expectedConnectionStringName2);
            Assert.IsTrue(result[2].Name.Contains(expectedConnectionStringName3), "ConnectionStrings public property does not have the \"{0}\" Name", expectedConnectionStringName3);
        }
    }
}
