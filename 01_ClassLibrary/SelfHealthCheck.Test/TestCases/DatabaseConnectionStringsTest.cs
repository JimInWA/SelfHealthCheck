using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SelfHealthCheck.Test.TestCases
{
    [TestClass]
    public class DatabaseConnectionStringsTest
    {
        [TestMethod]
        public void Driver()
        {
            var expectedResult = "Either no configuration file exists or no connectionString entry exists";

            var databaseConnectionStrings = new DatabaseConnectionStrings();

            var actualResult = databaseConnectionStrings.Validate();

            Assert.AreEqual(expectedResult, actualResult, "Validate method returned unexpected result");
        }
    }
}
