namespace SelfHealthCheck.Test.TestCases
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// DatabaseConnectionStrings Tests
    /// </summary>
    [TestClass]
    public class DatabaseConnectionStringsTest
    {
        /// <summary>
        /// Not so much a test as a driver for the Validate method
        /// </summary>
        [TestMethod]
        public void Driver()
        {
            var expectedResult = new POCO.DatabaseConnectionStringResult();
            expectedResult.Name = "Either no configuration file exists or no connectionString entry exists";

            var databaseConnectionStrings = new DatabaseConnectionStrings();

            var actualResult = databaseConnectionStrings.Validate();

            Assert.AreEqual(expectedResult, actualResult, "Validate method returned unexpected result");
        }
    }
}
