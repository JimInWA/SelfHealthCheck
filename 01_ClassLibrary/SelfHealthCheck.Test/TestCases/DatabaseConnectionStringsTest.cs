namespace SelfHealthCheck.Test.TestCases
{
    using FakeItEasy;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Configuration;

    /// <summary>
    /// DatabaseConnectionStrings Tests
    /// </summary>
    [TestClass]
    public class DatabaseConnectionStringsTest
    {
        #region Private Fields

        private static Interfaces.ICustomConfigurationManager _fakeCustomConfigurationManager;

        private static Interfaces.IDatabaseConnectionStrings _databaseConnectionStrings;

        #endregion

        #region Test Initialization

        /// <summary>
        /// TestInitializer method
        /// </summary>
        [TestInitialize]
        public void TestInitializer()
        {
            // explicitly create fake dependencies that need to be intercepted
            _fakeCustomConfigurationManager = A.Fake<Interfaces.ICustomConfigurationManager>();

            _databaseConnectionStrings = new DatabaseConnectionStrings(_fakeCustomConfigurationManager);            
        }

        #endregion

        /// <summary>
        /// When_ConnectionStrings_in_configuration_file_is_empty_GetConnectionStrings_Method_returns_empty_ConnectionStringSettingsCollection_result
        /// </summary>
        [TestMethod]
        public void When_ConnectionStrings_in_configuration_file_is_empty_GetConnectionStrings_Method_returns_empty_ConnectionStringSettingsCollection_result()
        {
            // Arrange
            var expectedFakeConnectionStrings = new ConnectionStringSettingsCollection();
            A.CallTo(() => _fakeCustomConfigurationManager.ConnectionStrings).Returns(expectedFakeConnectionStrings);

            // Act
            var actualConnectionStrings = _databaseConnectionStrings.GetConnectionStrings();

            // Assert
            Assert.IsNotNull(actualConnectionStrings);
            Assert.AreEqual(expectedFakeConnectionStrings, actualConnectionStrings, "The ConnectionStringCollection that was returned didn't match the expected value");
        }

        /// <summary>
        /// When_ConnectionStrings_in_configuration_file_has_one_entry_GetConnectionStrings_Method_returns_ConnectionStringSettingsCollection_result_with_one_result
        /// </summary>
        [TestMethod]
        public void When_ConnectionStrings_in_configuration_file_has_one_entry_GetConnectionStrings_Method_returns_ConnectionStringSettingsCollection_result_with_one_result()
        {
            // Arrange            
            var expectedFakeConnectionStrings = new ConnectionStringSettingsCollection();
            expectedFakeConnectionStrings.Add(new ConnectionStringSettings{ Name = "TestName", ConnectionString = "Data Source=TestDataSource;Initial Catalog=TestInitialCatalog;integrated security=SSPI;" });

            // Act
            A.CallTo(() => _fakeCustomConfigurationManager.ConnectionStrings).Returns(expectedFakeConnectionStrings);
            var actualConnectionStrings = _databaseConnectionStrings.GetConnectionStrings();

            // Assert
            Assert.IsNotNull(actualConnectionStrings);
            Assert.AreEqual(expectedFakeConnectionStrings, actualConnectionStrings, "The ConnectionStringCollection that was returned didn't match the expected value");
        }

        /// <summary>
        /// When_ConnectionStrings_in_configuration_file_is_empty_Validate_Method_returns_error_condition
        /// </summary>
        [TestMethod]
        public void When_ConnectionStrings_in_configuration_file_is_empty_Validate_Method_returns_error_condition()
        {
            // Arrange
            var expectedFakeConnectionStrings = new ConnectionStringSettingsCollection();
            A.CallTo(() => _fakeCustomConfigurationManager.ConnectionStrings).Returns(expectedFakeConnectionStrings);

            var expectedResult = new POCO.DatabaseConnectionStringItem()
            {
                Name = "Either no configuration file exists or no connectionString section exists"
            };

            // Act
            var actualResult = _databaseConnectionStrings.Validate();

            // Assert
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(expectedResult.Name, actualResult.Name, "Validate method returned unexpected result");
        }

        /// <summary>
        /// When_ConnectionStrings_in_configuration_file_has_one_entry_Validate_Method_but_WhiteListDataSourceItems_is_missing__returns_error_condition
        /// </summary>
        [TestMethod]
        public void When_ConnectionStrings_in_configuration_file_has_one_entry_Validate_Method_but_WhiteListDataSourceItems_is_missing__returns_error_condition()
        {
            // Arrange            
            var expectedFakeConnectionStrings = new ConnectionStringSettingsCollection();
            expectedFakeConnectionStrings.Add(new ConnectionStringSettings { Name = "TestName", ConnectionString = "Data Source=TestDataSource;Initial Catalog=TestInitialCatalog;integrated security=SSPI;" });
            A.CallTo(() => _fakeCustomConfigurationManager.ConnectionStrings).Returns(expectedFakeConnectionStrings);

            A.CallTo(() => _fakeCustomConfigurationManager.AppSettings.Get("WhiteListDataSourceItems")).Returns(null);

            var expectedResult = new POCO.DatabaseConnectionStringItem()
            {
                Name = "Either no configuration file exists or not appSettings section exists or the WhiteListDataSourceItems appSettings key doesn't exist"
            };

            // Act
            var actualResult = _databaseConnectionStrings.Validate();

            // Assert
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(expectedResult.Name, actualResult.Name, "Validate method returned unexpected result");
        }

        /// <summary>
        /// When_ConnectionStrings_in_configuration_file_has_one_entry_Validate_Method_but_WhiteListDataSourceItems_has_one_item_that_does_not_match_the_datasource_entry_returns_error_condition
        /// </summary>
        [TestMethod]
        public void When_ConnectionStrings_in_configuration_file_has_one_entry_Validate_Method_but_WhiteListDataSourceItems_has_one_item_that_does_not_match_the_datasource_entry_returns_error_condition()
        {
            // Arrange            
            var expectedFakeConnectionStrings = new ConnectionStringSettingsCollection();
            expectedFakeConnectionStrings.Add(new ConnectionStringSettings { Name = "TestName", ConnectionString = "Data Source=TestDataSource;Initial Catalog=TestInitialCatalog;integrated security=SSPI;" });
            A.CallTo(() => _fakeCustomConfigurationManager.ConnectionStrings).Returns(expectedFakeConnectionStrings);

            A.CallTo(() => _fakeCustomConfigurationManager.AppSettings.Get("WhiteListDataSourceItems")).Returns("Test");

            var expectedResult = new POCO.DatabaseConnectionStringItem()
            {
                Name = "TestName",
                DatabaseSource = "TestDataSource",
                InitialCatalog = "TestInitialCatalog",
                IsUsingIntegratedSecurity = true,
                IsInWhiteList = false
            };

            // Act
            var actualResult = _databaseConnectionStrings.Validate();

            // Assert
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(expectedResult.Name, actualResult.Name, "Validate method returned unexpected result");
            Assert.AreEqual(expectedResult.DatabaseSource, actualResult.DatabaseSource, "Validate method returned unexpected result for the DatabaseSource");
            Assert.AreEqual(expectedResult.InitialCatalog, actualResult.InitialCatalog, "Validate method returned unexpected result for the InitialCatalog");
            Assert.AreEqual(expectedResult.IsUsingIntegratedSecurity, actualResult.IsUsingIntegratedSecurity, "Validate method returned unexpected result for the IsUsingIntegratedSecurity");
            Assert.AreEqual(expectedResult.IsInWhiteList, actualResult.IsInWhiteList, "Validate method returned unexpected result for the IsInWhiteList");
        }

        /// <summary>
        /// When_ConnectionStrings_in_configuration_file_has_one_entry_Validate_Method_but_WhiteListDataSourceItems_has_one_item_matches_the_spelling_but_not_the_capitalization_of_datasource_entry_returns_error_condition
        /// </summary>
        [TestMethod]
        public void When_ConnectionStrings_in_configuration_file_has_one_entry_Validate_Method_but_WhiteListDataSourceItems_has_one_item_matches_the_spelling_but_not_the_capitalization_of_datasource_entry_returns_error_condition()
        {
            // Arrange            
            var expectedFakeConnectionStrings = new ConnectionStringSettingsCollection();
            expectedFakeConnectionStrings.Add(new ConnectionStringSettings { Name = "TestName", ConnectionString = "Data Source=TestDataSource;Initial Catalog=TestInitialCatalog;integrated security=SSPI;" });
            A.CallTo(() => _fakeCustomConfigurationManager.ConnectionStrings).Returns(expectedFakeConnectionStrings);

            // the white list value is in all lower case, not the mixed case of the connection string value
            A.CallTo(() => _fakeCustomConfigurationManager.AppSettings.Get("WhiteListDataSourceItems")).Returns("testdatasource"); 

            var expectedResult = new POCO.DatabaseConnectionStringItem()
            {
                Name = "TestName",
                DatabaseSource = "TestDataSource",
                InitialCatalog = "TestInitialCatalog",
                IsUsingIntegratedSecurity = true,
                IsInWhiteList = false
            };

            // Act
            var actualResult = _databaseConnectionStrings.Validate();

            // Assert
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(expectedResult.Name, actualResult.Name, "Validate method returned unexpected result");
            Assert.AreEqual(expectedResult.DatabaseSource, actualResult.DatabaseSource, "Validate method returned unexpected result for the DatabaseSource");
            Assert.AreEqual(expectedResult.InitialCatalog, actualResult.InitialCatalog, "Validate method returned unexpected result for the InitialCatalog");
            Assert.AreEqual(expectedResult.IsUsingIntegratedSecurity, actualResult.IsUsingIntegratedSecurity, "Validate method returned unexpected result for the IsUsingIntegratedSecurity");
            Assert.AreEqual(expectedResult.IsInWhiteList, actualResult.IsInWhiteList, "Validate method returned unexpected result for the IsInWhiteList");
        }

        /// <summary>
        /// When_ConnectionStrings_in_configuration_file_has_one_entry_and_WhiteList_has_one_value_Validate_Method_returns_the_distinct_values_for_the_one_entry
        /// </summary>
        [TestMethod]
        public void When_ConnectionStrings_in_configuration_file_has_one_entry_and_WhiteList_has_one_value_Validate_Method_returns_the_distinct_values_for_the_one_entry()
        {
            // Arrange            
            var expectedFakeConnectionStrings = new ConnectionStringSettingsCollection();
            expectedFakeConnectionStrings.Add(new ConnectionStringSettings { Name = "TestName", ConnectionString = "Data Source=TestDataSource;Initial Catalog=TestInitialCatalog;integrated security=SSPI;" });
            A.CallTo(() => _fakeCustomConfigurationManager.ConnectionStrings).Returns(expectedFakeConnectionStrings);

            A.CallTo(() => _fakeCustomConfigurationManager.AppSettings.Get("WhiteListDataSourceItems")).Returns("TestDataSource");

            var expectedResult = new POCO.DatabaseConnectionStringItem()
            {
                Name = "TestName",
                DatabaseSource = "TestDataSource",
                InitialCatalog = "TestInitialCatalog",
                IsUsingIntegratedSecurity = true,
                IsInWhiteList = true
            };

            // Act
            var actualResult = _databaseConnectionStrings.Validate();

            // Assert
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(expectedResult.Name, actualResult.Name, "Validate method returned unexpected result for the Name");
            Assert.AreEqual(expectedResult.DatabaseSource, actualResult.DatabaseSource, "Validate method returned unexpected result for the DatabaseSource");
            Assert.AreEqual(expectedResult.InitialCatalog, actualResult.InitialCatalog, "Validate method returned unexpected result for the InitialCatalog");
            Assert.AreEqual(expectedResult.IsUsingIntegratedSecurity, actualResult.IsUsingIntegratedSecurity, "Validate method returned unexpected result for the IsUsingIntegratedSecurity");
            Assert.AreEqual(expectedResult.IsInWhiteList, actualResult.IsInWhiteList, "Validate method returned unexpected result for the IsInWhiteList");
        }

        /// <summary>
        /// When_ConnectionStrings_in_configuration_file_has_one_entry_and_WhiteList_has_two_values_Validate_Method_returns_the_distinct_values_for_the_one_entry()
        /// </summary>
        [TestMethod]
        public void When_ConnectionStrings_in_configuration_file_has_one_entry_and_WhiteList_has_two_values_Validate_Method_returns_the_distinct_values_for_the_one_entry()
        {
            // Arrange            
            var expectedFakeConnectionStrings = new ConnectionStringSettingsCollection();
            expectedFakeConnectionStrings.Add(new ConnectionStringSettings { Name = "TestName", ConnectionString = "Data Source=TestDataSource;Initial Catalog=TestInitialCatalog;integrated security=SSPI;" });
            A.CallTo(() => _fakeCustomConfigurationManager.ConnectionStrings).Returns(expectedFakeConnectionStrings);

            A.CallTo(() => _fakeCustomConfigurationManager.AppSettings.Get("WhiteListDataSourceItems")).Returns("Dummy,TestDataSource");

            var expectedResult = new POCO.DatabaseConnectionStringItem()
            {
                Name = "TestName",
                DatabaseSource = "TestDataSource",
                InitialCatalog = "TestInitialCatalog",
                IsUsingIntegratedSecurity = true,
                IsInWhiteList = true
            };

            // Act
            var actualResult = _databaseConnectionStrings.Validate();

            // Assert
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(expectedResult.Name, actualResult.Name, "Validate method returned unexpected result for the Name");
            Assert.AreEqual(expectedResult.DatabaseSource, actualResult.DatabaseSource, "Validate method returned unexpected result for the DatabaseSource");
            Assert.AreEqual(expectedResult.InitialCatalog, actualResult.InitialCatalog, "Validate method returned unexpected result for the InitialCatalog");
            Assert.AreEqual(expectedResult.IsUsingIntegratedSecurity, actualResult.IsUsingIntegratedSecurity, "Validate method returned unexpected result for the IsUsingIntegratedSecurity");
            Assert.AreEqual(expectedResult.IsInWhiteList, actualResult.IsInWhiteList, "Validate method returned unexpected result for the IsInWhiteList");
        }
    }
}
