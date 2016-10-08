namespace SelfHealthCheck.Test.TestCases
{
    using FakeItEasy;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Configuration;
    using System.Linq;

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

        #region GetConnectionStrings Method - Happy Path

        /// <summary>
        /// GetConnectionStrings_Method_Happy_Path_When_ConnectionStrings_in_configuration_file_is_empty_method_returns_empty_ConnectionStringSettingsCollection_result
        /// </summary>
        [TestMethod]
        public void GetConnectionStrings_Method_Happy_Path_When_ConnectionStrings_in_configuration_file_is_empty_method_returns_empty_ConnectionStringSettingsCollection_result()
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
        /// GetConnectionStrings_Method_Happy_Path_When_ConnectionStrings_in_configuration_file_has_one_entry_method_returns_ConnectionStringSettingsCollection_result_with_one_result
        /// </summary>
        [TestMethod]
        public void GetConnectionStrings_Method_Happy_Path_When_ConnectionStrings_in_configuration_file_has_one_entry_method_returns_ConnectionStringSettingsCollection_result_with_one_result()
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

        #endregion GetConnectionStrings Method - Happy Path

        #region Validate Method - Sad Path

        /// <summary>
        /// Validate_Method_Sad_Path_When_ConnectionStrings_in_configuration_file_is_empty_method_returns_error_condition
        /// </summary>
        [TestMethod]
        public void Validate_Method_Sad_Path_When_ConnectionStrings_in_configuration_file_is_empty_method_returns_error_condition()
        {
            // Arrange
            var expectedFakeConnectionStrings = new ConnectionStringSettingsCollection();
            A.CallTo(() => _fakeCustomConfigurationManager.ConnectionStrings).Returns(expectedFakeConnectionStrings);

            var expectedResult = new POCO.DatabaseConnectionStringResult();
            expectedResult.ErrorMessageInformation.Add("Either no configuration file exists or no connectionString section exists");

            // Act
            var actualResult = _databaseConnectionStrings.Validate();

            // Assert
            Assert.IsNotNull(actualResult, "The Validate method returned null");
            Assert.IsNotNull(actualResult.ItemFromConfigurationFile, "ItemFromConfigurationFile is null");
            Assert.AreEqual(0, actualResult.ItemFromConfigurationFile.Count, "actualResult.ItemFromConfigurationFile.Count);");
            Assert.IsNotNull(actualResult.ErrorMessageInformation, "ErrorMessageInformation is null");
            Assert.AreEqual(1, actualResult.ErrorMessageInformation.Count, "ErrorMessageInformation.Count");
            Assert.AreEqual(expectedResult.ErrorMessageInformation.First(), actualResult.ErrorMessageInformation.First(), "Validate method returned unexpected result for the ErrorMessageInformation");
        }

        /// <summary>
        /// Validate_Method_Sad_Path_When_ConnectionStrings_in_configuration_file_has_one_entry_but_WhiteListDataSourceItems_is_missing_method_returns_error_condition
        /// </summary>
        [TestMethod]
        public void Validate_Method_Sad_Path_When_ConnectionStrings_in_configuration_file_has_one_entry_but_WhiteListDataSourceItems_is_missing_method_returns_error_condition()
        {
            // Arrange            
            var expectedFakeConnectionStrings = new ConnectionStringSettingsCollection();
            expectedFakeConnectionStrings.Add(new ConnectionStringSettings { Name = "TestName", ConnectionString = "Data Source=TestDataSource;Initial Catalog=TestInitialCatalog;integrated security=SSPI;" });
            A.CallTo(() => _fakeCustomConfigurationManager.ConnectionStrings).Returns(expectedFakeConnectionStrings);

            A.CallTo(() => _fakeCustomConfigurationManager.AppSettings.Get("WhiteListDataSourceItems")).Returns(null);

            var expectedResult = new POCO.DatabaseConnectionStringResult();
            expectedResult.ErrorMessageInformation.Add("Either no configuration file exists or not appSettings section exists or the WhiteListDataSourceItems appSettings key doesn't exist");

            // Act
            var actualResult = _databaseConnectionStrings.Validate();

            // Assert
            Assert.IsNotNull(actualResult, "The Validate method returned null");
            Assert.IsNotNull(actualResult.ItemFromConfigurationFile, "ItemFromConfigurationFile is null");
            Assert.AreEqual(0, actualResult.ItemFromConfigurationFile.Count, "actualResult.ItemFromConfigurationFile.Count);");
            Assert.IsNotNull(actualResult.ErrorMessageInformation, "ErrorMessageInformation is null");
            Assert.AreEqual(1, actualResult.ErrorMessageInformation.Count, "ErrorMessageInformation.Count");
            Assert.AreEqual(expectedResult.ErrorMessageInformation.First(), actualResult.ErrorMessageInformation.First(), "Validate method returned unexpected result for the ErrorMessageInformation");
        }

        /// <summary>
        /// Validate_Method_Sad_Path_When_ConnectionStrings_in_configuration_file_has_one_entry_but_WhiteListDataSourceItems_has_one_item_that_does_not_match_the_datasource_entry_method_returns_error_condition
        /// </summary>
        [TestMethod]
        public void Validate_Method_Sad_Path_When_ConnectionStrings_in_configuration_file_has_one_entry_but_WhiteListDataSourceItems_has_one_item_that_does_not_match_the_datasource_entry_method_returns_error_condition()
        {
            // Arrange            
            var expectedFakeConnectionStrings = new ConnectionStringSettingsCollection();
            expectedFakeConnectionStrings.Add(new ConnectionStringSettings { Name = "TestName", ConnectionString = "Data Source=TestDataSource;Initial Catalog=TestInitialCatalog;integrated security=SSPI;" });
            A.CallTo(() => _fakeCustomConfigurationManager.ConnectionStrings).Returns(expectedFakeConnectionStrings);

            A.CallTo(() => _fakeCustomConfigurationManager.AppSettings.Get("WhiteListDataSourceItems")).Returns("Test");

            var item = new POCO.DatabaseConnectionStringItem()
            {
                Name = "TestName",
                DatabaseSource = "TestDataSource",
                InitialCatalog = "TestInitialCatalog",
                IsUsingIntegratedSecurity = true,
                IsInWhiteList = false
            };
            var expectedResult = new POCO.DatabaseConnectionStringResult();
            expectedResult.ItemFromConfigurationFile.Add(item);
            expectedResult.ErrorMessageInformation.Add("Name TestName, DataSource TestDataSource is not in the whitelist");
            expectedResult.WhiteListDatabaseConnectionString.Add("Test");

            // Act
            var actualResult = _databaseConnectionStrings.Validate();

            // Assert
            Assert.IsNotNull(actualResult, "The Validate method returned null");
            Assert.IsNotNull(actualResult.ItemFromConfigurationFile, "ItemFromConfigurationFile is null");
            Assert.AreEqual(1, actualResult.ItemFromConfigurationFile.Count, "actualResult.ItemFromConfigurationFile.Count);");
            Assert.AreEqual(expectedResult.ItemFromConfigurationFile.First().Name, actualResult.ItemFromConfigurationFile.First().Name, "Validate method returned unexpected result for the Name");
            Assert.AreEqual(expectedResult.ItemFromConfigurationFile.First().DatabaseSource, actualResult.ItemFromConfigurationFile.First().DatabaseSource, "Validate method returned unexpected result for the DatabaseSource");
            Assert.AreEqual(expectedResult.ItemFromConfigurationFile.First().InitialCatalog, actualResult.ItemFromConfigurationFile.First().InitialCatalog, "Validate method returned unexpected result for the InitialCatalog");
            Assert.AreEqual(expectedResult.ItemFromConfigurationFile.First().IsUsingIntegratedSecurity, actualResult.ItemFromConfigurationFile.First().IsUsingIntegratedSecurity, "Validate method returned unexpected result for the IsUsingIntegratedSecurity");
            Assert.AreEqual(expectedResult.ItemFromConfigurationFile.First().IsInWhiteList, actualResult.ItemFromConfigurationFile.First().IsInWhiteList, "Validate method returned unexpected result for the IsInWhiteList");
            Assert.IsNotNull(actualResult.ErrorMessageInformation, "ErrorMessageInformation is null");
            Assert.AreEqual(1, actualResult.ErrorMessageInformation.Count, "ErrorMessageInformation.Count");
            Assert.AreEqual(expectedResult.ErrorMessageInformation.First(), actualResult.ErrorMessageInformation.First(), "Validate method returned unexpected result for the ErrorMessageInformation");
            Assert.IsNotNull(actualResult.WhiteListDatabaseConnectionString, "WhiteListDatabaseConnectionString is null");
            Assert.AreEqual(1, actualResult.WhiteListDatabaseConnectionString.Count, "WhiteListDatabaseConnectionString.Count");
            Assert.AreEqual(expectedResult.WhiteListDatabaseConnectionString.First(), actualResult.WhiteListDatabaseConnectionString.First(), "Validate method returned unexpected result for the WhiteListDatabaseConnectionString");
        }
        /// <summary>
        /// Validate_Method_Sad_Path_When_ConnectionStrings_in_configuration_file_has_one_entry_but_WhiteListDataSourceItems_has_one_item_matches_the_spelling_but_not_the_capitalization_of_datasource_entry_method_returns_error_condition
        /// </summary>
        [TestMethod]
        public void Validate_Method_Sad_Path_When_ConnectionStrings_in_configuration_file_has_one_entry_but_WhiteListDataSourceItems_has_one_item_matches_the_spelling_but_not_the_capitalization_of_datasource_entry_method_returns_error_condition()
        {
            // Arrange            
            var expectedFakeConnectionStrings = new ConnectionStringSettingsCollection();
            expectedFakeConnectionStrings.Add(new ConnectionStringSettings { Name = "TestName", ConnectionString = "Data Source=TestDataSource;Initial Catalog=TestInitialCatalog;integrated security=SSPI;" });
            A.CallTo(() => _fakeCustomConfigurationManager.ConnectionStrings).Returns(expectedFakeConnectionStrings);

            // the white list value is in all lower case, not the mixed case of the connection string value
            A.CallTo(() => _fakeCustomConfigurationManager.AppSettings.Get("WhiteListDataSourceItems")).Returns("testdatasource");

            var item = new POCO.DatabaseConnectionStringItem()
            {
                Name = "TestName",
                DatabaseSource = "TestDataSource",
                InitialCatalog = "TestInitialCatalog",
                IsUsingIntegratedSecurity = true,
                IsInWhiteList = false
            };
            var expectedResult = new POCO.DatabaseConnectionStringResult();
            expectedResult.ItemFromConfigurationFile.Add(item);
            expectedResult.ErrorMessageInformation.Add("Name TestName, DataSource TestDataSource is not in the whitelist");
            expectedResult.WhiteListDatabaseConnectionString.Add("testdatasource");

            // Act
            var actualResult = _databaseConnectionStrings.Validate();

            // Assert
            Assert.IsNotNull(actualResult, "The Validate method returned null");
            Assert.IsNotNull(actualResult.ItemFromConfigurationFile, "ItemFromConfigurationFile is null");
            Assert.AreEqual(1, actualResult.ItemFromConfigurationFile.Count, "actualResult.ItemFromConfigurationFile.Count);");
            Assert.AreEqual(expectedResult.ItemFromConfigurationFile.First().Name, actualResult.ItemFromConfigurationFile.First().Name, "Validate method returned unexpected result for the Name");
            Assert.AreEqual(expectedResult.ItemFromConfigurationFile.First().DatabaseSource, actualResult.ItemFromConfigurationFile.First().DatabaseSource, "Validate method returned unexpected result for the DatabaseSource");
            Assert.AreEqual(expectedResult.ItemFromConfigurationFile.First().InitialCatalog, actualResult.ItemFromConfigurationFile.First().InitialCatalog, "Validate method returned unexpected result for the InitialCatalog");
            Assert.AreEqual(expectedResult.ItemFromConfigurationFile.First().IsUsingIntegratedSecurity, actualResult.ItemFromConfigurationFile.First().IsUsingIntegratedSecurity, "Validate method returned unexpected result for the IsUsingIntegratedSecurity");
            Assert.AreEqual(expectedResult.ItemFromConfigurationFile.First().IsInWhiteList, actualResult.ItemFromConfigurationFile.First().IsInWhiteList, "Validate method returned unexpected result for the IsInWhiteList");
            Assert.IsNotNull(actualResult.ErrorMessageInformation, "ErrorMessageInformation is null");
            Assert.IsNotNull(actualResult.ErrorMessageInformation, "ErrorMessageInformation is null");
            Assert.AreEqual(1, actualResult.ErrorMessageInformation.Count, "ErrorMessageInformation.Count");
            Assert.AreEqual(expectedResult.ErrorMessageInformation.First(), actualResult.ErrorMessageInformation.First(), "Validate method returned unexpected result for the ErrorMessageInformation");
            Assert.IsNotNull(actualResult.WhiteListDatabaseConnectionString, "WhiteListDatabaseConnectionString is null");
            Assert.AreEqual(1, actualResult.WhiteListDatabaseConnectionString.Count, "WhiteListDatabaseConnectionString.Count");
            Assert.AreEqual(expectedResult.WhiteListDatabaseConnectionString.First(), actualResult.WhiteListDatabaseConnectionString.First(), "Validate method returned unexpected result for the WhiteListDatabaseConnectionString");
        }

        #endregion Validate Method - Sad Path

        #region Validate Method - Happy Path
        /// <summary>
        /// Validate_Method_Happy_Path_When_ConnectionStrings_in_configuration_file_has_one_entry_and_WhiteList_has_one_value_method_returns_the_distinct_values_for_the_one_entry
        /// </summary>
        [TestMethod]
        public void Validate_Method_Happy_Path_When_ConnectionStrings_in_configuration_file_has_one_entry_and_WhiteList_has_one_value_method_returns_the_distinct_values_for_the_one_entry()
        {
            // Arrange            
            var expectedFakeConnectionStrings = new ConnectionStringSettingsCollection();
            expectedFakeConnectionStrings.Add(new ConnectionStringSettings { Name = "TestName", ConnectionString = "Data Source=TestDataSource;Initial Catalog=TestInitialCatalog;integrated security=SSPI;" });
            A.CallTo(() => _fakeCustomConfigurationManager.ConnectionStrings).Returns(expectedFakeConnectionStrings);

            A.CallTo(() => _fakeCustomConfigurationManager.AppSettings.Get("WhiteListDataSourceItems")).Returns("TestDataSource");

            var item = new POCO.DatabaseConnectionStringItem()
            {
                Name = "TestName",
                DatabaseSource = "TestDataSource",
                InitialCatalog = "TestInitialCatalog",
                IsUsingIntegratedSecurity = true,
                IsInWhiteList = true
            };
            var expectedResult = new POCO.DatabaseConnectionStringResult();
            expectedResult.ItemFromConfigurationFile.Add(item);
            expectedResult.WhiteListDatabaseConnectionString.Add("TestDataSource");

            // Act
            var actualResult = _databaseConnectionStrings.Validate();

            // Assert
            Assert.IsNotNull(actualResult, "The Validate method returned null");
            Assert.IsNotNull(actualResult.ItemFromConfigurationFile, "ItemFromConfigurationFile is null");
            Assert.AreEqual(1, actualResult.ItemFromConfigurationFile.Count, "actualResult.ItemFromConfigurationFile.Count);");
            Assert.AreEqual(expectedResult.ItemFromConfigurationFile.First().Name, actualResult.ItemFromConfigurationFile.First().Name, "Validate method returned unexpected result for the Name");
            Assert.AreEqual(expectedResult.ItemFromConfigurationFile.First().DatabaseSource, actualResult.ItemFromConfigurationFile.First().DatabaseSource, "Validate method returned unexpected result for the DatabaseSource");
            Assert.AreEqual(expectedResult.ItemFromConfigurationFile.First().InitialCatalog, actualResult.ItemFromConfigurationFile.First().InitialCatalog, "Validate method returned unexpected result for the InitialCatalog");
            Assert.AreEqual(expectedResult.ItemFromConfigurationFile.First().IsUsingIntegratedSecurity, actualResult.ItemFromConfigurationFile.First().IsUsingIntegratedSecurity, "Validate method returned unexpected result for the IsUsingIntegratedSecurity");
            Assert.AreEqual(expectedResult.ItemFromConfigurationFile.First().IsInWhiteList, actualResult.ItemFromConfigurationFile.First().IsInWhiteList, "Validate method returned unexpected result for the IsInWhiteList");
            Assert.IsNotNull(actualResult.ErrorMessageInformation, "ErrorMessageInformation is null");
            Assert.AreEqual(0, actualResult.ErrorMessageInformation.Count, "ErrorMessageInformation.Count");
            Assert.IsNotNull(actualResult.WhiteListDatabaseConnectionString,"WhiteListDatabaseConnectionString is null");
            Assert.AreEqual(1, actualResult.WhiteListDatabaseConnectionString.Count, "WhiteListDatabaseConnectionString.Count");
            Assert.AreEqual(expectedResult.WhiteListDatabaseConnectionString.First(), actualResult.WhiteListDatabaseConnectionString.First(), "Validate method returned unexpected result for the WhiteListDatabaseConnectionString");
        }

        /// <summary>
        /// Validate_Method_Happy_Path_When_ConnectionStrings_in_configuration_file_has_one_entry_and_WhiteList_has_two_values_method_returns_the_distinct_values_for_the_one_entry()
        /// </summary>
        [TestMethod]
        public void Validate_Method_Happy_Path_When_ConnectionStrings_in_configuration_file_has_one_entry_and_WhiteList_has_two_values_method_returns_the_distinct_values_for_the_one_entry()
        {
            // Arrange            
            var expectedFakeConnectionStrings = new ConnectionStringSettingsCollection();
            expectedFakeConnectionStrings.Add(new ConnectionStringSettings { Name = "TestName", ConnectionString = "Data Source=TestDataSource;Initial Catalog=TestInitialCatalog;integrated security=SSPI;" });
            A.CallTo(() => _fakeCustomConfigurationManager.ConnectionStrings).Returns(expectedFakeConnectionStrings);

            A.CallTo(() => _fakeCustomConfigurationManager.AppSettings.Get("WhiteListDataSourceItems")).Returns("Dummy,TestDataSource");

            var item = new POCO.DatabaseConnectionStringItem()
            {
                Name = "TestName",
                DatabaseSource = "TestDataSource",
                InitialCatalog = "TestInitialCatalog",
                IsUsingIntegratedSecurity = true,
                IsInWhiteList = true
            };
            var expectedResult = new POCO.DatabaseConnectionStringResult();
            expectedResult.ItemFromConfigurationFile.Add(item);
            expectedResult.WhiteListDatabaseConnectionString.Add("Dummy");
            expectedResult.WhiteListDatabaseConnectionString.Add("TestDataSource");

            // Act
            var actualResult = _databaseConnectionStrings.Validate();

            // Assert
            Assert.IsNotNull(actualResult, "The Validate method returned null");
            Assert.IsNotNull(actualResult.ItemFromConfigurationFile, "ItemFromConfigurationFile is null");
            Assert.AreEqual(1, actualResult.ItemFromConfigurationFile.Count, "actualResult.ItemFromConfigurationFile.Count);");
            Assert.AreEqual(expectedResult.ItemFromConfigurationFile.First().Name, actualResult.ItemFromConfigurationFile.First().Name, "Validate method returned unexpected result for the Name");
            Assert.AreEqual(expectedResult.ItemFromConfigurationFile.First().DatabaseSource, actualResult.ItemFromConfigurationFile.First().DatabaseSource, "Validate method returned unexpected result for the DatabaseSource");
            Assert.AreEqual(expectedResult.ItemFromConfigurationFile.First().InitialCatalog, actualResult.ItemFromConfigurationFile.First().InitialCatalog, "Validate method returned unexpected result for the InitialCatalog");
            Assert.AreEqual(expectedResult.ItemFromConfigurationFile.First().IsUsingIntegratedSecurity, actualResult.ItemFromConfigurationFile.First().IsUsingIntegratedSecurity, "Validate method returned unexpected result for the IsUsingIntegratedSecurity");
            Assert.AreEqual(expectedResult.ItemFromConfigurationFile.First().IsInWhiteList, actualResult.ItemFromConfigurationFile.First().IsInWhiteList, "Validate method returned unexpected result for the IsInWhiteList");
            Assert.IsNotNull(actualResult.ErrorMessageInformation, "ErrorMessageInformation is null");
            Assert.AreEqual(0, actualResult.ErrorMessageInformation.Count, "ErrorMessageInformation.Count");
            Assert.IsNotNull(actualResult.WhiteListDatabaseConnectionString,"WhiteListDatabaseConnectionString is null");
            Assert.AreEqual(2, actualResult.WhiteListDatabaseConnectionString.Count, "WhiteListDatabaseConnectionString.Count");
            Assert.AreEqual(expectedResult.WhiteListDatabaseConnectionString.First(), actualResult.WhiteListDatabaseConnectionString.First(), "Validate method returned unexpected result for the WhiteListDatabaseConnectionString");
            Assert.AreEqual(expectedResult.WhiteListDatabaseConnectionString.ElementAt(1), actualResult.WhiteListDatabaseConnectionString.ElementAt(1), "Validate method returned unexpected result for the WhiteListDatabaseConnectionString");
        }

        /// <summary>
        /// Validate_Method_Happy_Path_When_ConnectionStrings_in_configuration_file_has_two_entries_and_WhiteList_has_one_value_that_only_matches_the_first_connection_string_entry_method_for_WhiteList_value_returns_true_for_first_and_false_for_second
        /// </summary>
        [TestMethod]
        public void Validate_Method_Happy_Path_When_ConnectionStrings_in_configuration_file_has_two_entries_and_WhiteList_has_one_value_that_only_matches_the_first_connection_string_entry_method_for_WhiteList_value_returns_true_for_first_and_false_for_second()
        {
            // Arrange            
            var expectedFakeConnectionStrings = new ConnectionStringSettingsCollection();
            expectedFakeConnectionStrings.Add(new ConnectionStringSettings { Name = "TestName", ConnectionString = "Data Source=TestDataSource;Initial Catalog=TestInitialCatalog;integrated security=SSPI;" });
            expectedFakeConnectionStrings.Add(new ConnectionStringSettings { Name = "TestName2", ConnectionString = "Data Source=TestDataSource2;Initial Catalog=TestInitialCatalog2;integrated security=SSPI;" });
            A.CallTo(() => _fakeCustomConfigurationManager.ConnectionStrings).Returns(expectedFakeConnectionStrings);

            A.CallTo(() => _fakeCustomConfigurationManager.AppSettings.Get("WhiteListDataSourceItems")).Returns("TestDataSource");

            var item = new POCO.DatabaseConnectionStringItem()
            {
                Name = "TestName",
                DatabaseSource = "TestDataSource",
                InitialCatalog = "TestInitialCatalog",
                IsUsingIntegratedSecurity = true,
                IsInWhiteList = true
            };
            var expectedResult = new POCO.DatabaseConnectionStringResult();
            expectedResult.ItemFromConfigurationFile.Add(item);
            expectedResult.WhiteListDatabaseConnectionString.Add("TestDataSource");

            item = new POCO.DatabaseConnectionStringItem()
            {
                Name = "TestName2",
                DatabaseSource = "TestDataSource2",
                InitialCatalog = "TestInitialCatalog2",
                IsUsingIntegratedSecurity = true,
                IsInWhiteList = false
            };
            expectedResult.ItemFromConfigurationFile.Add(item);
            expectedResult.ErrorMessageInformation.Add("Name TestName2, DataSource TestDataSource2 is not in the whitelist");

            // Act
            var actualResult = _databaseConnectionStrings.Validate();

            // Assert
            Assert.IsNotNull(actualResult, "The Validate method returned null");
            Assert.IsNotNull(actualResult.ItemFromConfigurationFile, "ItemFromConfigurationFile is null");
            Assert.AreEqual(2, actualResult.ItemFromConfigurationFile.Count, "actualResult.ItemFromConfigurationFile.Count);");
            Assert.AreEqual(expectedResult.ItemFromConfigurationFile.First().Name, actualResult.ItemFromConfigurationFile.First().Name, "Validate method returned unexpected result for the Name");
            Assert.AreEqual(expectedResult.ItemFromConfigurationFile.First().DatabaseSource, actualResult.ItemFromConfigurationFile.First().DatabaseSource, "Validate method returned unexpected result for the DatabaseSource");
            Assert.AreEqual(expectedResult.ItemFromConfigurationFile.First().InitialCatalog, actualResult.ItemFromConfigurationFile.First().InitialCatalog, "Validate method returned unexpected result for the InitialCatalog");
            Assert.AreEqual(expectedResult.ItemFromConfigurationFile.First().IsUsingIntegratedSecurity, actualResult.ItemFromConfigurationFile.First().IsUsingIntegratedSecurity, "Validate method returned unexpected result for the IsUsingIntegratedSecurity");
            Assert.AreEqual(expectedResult.ItemFromConfigurationFile.First().IsInWhiteList, actualResult.ItemFromConfigurationFile.First().IsInWhiteList, "Validate method returned unexpected result for the IsInWhiteList");
            Assert.AreEqual(expectedResult.ItemFromConfigurationFile.ElementAt(1).Name, actualResult.ItemFromConfigurationFile.ElementAt(1).Name, "Validate method returned unexpected result for the Name");
            Assert.AreEqual(expectedResult.ItemFromConfigurationFile.ElementAt(1).DatabaseSource, actualResult.ItemFromConfigurationFile.ElementAt(1).DatabaseSource, "Validate method returned unexpected result for the DatabaseSource");
            Assert.AreEqual(expectedResult.ItemFromConfigurationFile.ElementAt(1).InitialCatalog, actualResult.ItemFromConfigurationFile.ElementAt(1).InitialCatalog, "Validate method returned unexpected result for the InitialCatalog");
            Assert.AreEqual(expectedResult.ItemFromConfigurationFile.ElementAt(1).IsUsingIntegratedSecurity, actualResult.ItemFromConfigurationFile.ElementAt(1).IsUsingIntegratedSecurity, "Validate method returned unexpected result for the IsUsingIntegratedSecurity");
            Assert.AreEqual(expectedResult.ItemFromConfigurationFile.ElementAt(1).IsInWhiteList, actualResult.ItemFromConfigurationFile.ElementAt(1).IsInWhiteList, "Validate method returned unexpected result for the IsInWhiteList");
            Assert.IsNotNull(actualResult.ErrorMessageInformation, "ErrorMessageInformation is null");
            Assert.AreEqual(1, actualResult.ErrorMessageInformation.Count, "ErrorMessageInformation.Count");
            Assert.AreEqual(expectedResult.ErrorMessageInformation.First(), actualResult.ErrorMessageInformation.First(), "Validate method returned unexpected result for the ErrorMessageInformation");
            Assert.IsNotNull(actualResult.WhiteListDatabaseConnectionString,"WhiteListDatabaseConnectionString is null");
            Assert.AreEqual(1, actualResult.WhiteListDatabaseConnectionString.Count, "WhiteListDatabaseConnectionString.Count");
            Assert.AreEqual(expectedResult.WhiteListDatabaseConnectionString.First(), actualResult.WhiteListDatabaseConnectionString.First(), "Validate method returned unexpected result for the WhiteListDatabaseConnectionString");
        }

        #endregion Validate Method - Happy Path

    }
}
