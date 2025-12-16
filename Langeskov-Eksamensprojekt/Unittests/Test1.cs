using Microsoft.VisualStudio.TestTools.UnitTesting;
using Infrastructure.Model;
using System;
using Infrastructure;
using Infrastructure.Repository;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Unittests
{
    [TestClass]
    public sealed class Test1
    {
        private static IConfiguration _configuration;
        private static string _connectionString;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            _configuration = configurationBuilder.Build();
            _connectionString = _configuration.GetConnectionString("DefaultConnection");

            // Initialize the database once for the test class.
            DatabaseInitializer.Initialize(_connectionString);
        }


        [TestMethod]
        public void Runner_Constructor_SetsProperties()
        {
            // Arrange -> dob = Date of Birth
            var dob = new DateTime(2000, 5, 20);

            // Act
            var runner = new Runner("Bob", "b@hotmail.com", "Addr", "1234", "555", Gender.Mand, dob, 1);

            // Assert
            Assert.AreEqual("Bob", runner.Name);
            Assert.AreEqual("b@hotmail.com", runner.Email);
            Assert.AreEqual(Gender.Mand, runner.Gender);
            Assert.AreEqual(1, runner.RunnerGroupID);
            Assert.AreEqual(dob, runner.DateOfBirth);
        }

        [TestMethod]
        public void SubsidyGroup_Constructor_SetsValues()
        {
            // Arrange & Act
            var sg = new SubsidyGroup(5, "Senior_60_Plus", "60+");

            // Assert
            Assert.AreEqual(5, sg.SubsidyGroupID);
            Assert.AreEqual("Senior_60_Plus", sg.SubsidyGroupNameText);
            Assert.AreEqual("60+", sg.AgeRange);
        }

        [TestMethod]
        public void GetRunnerById_ReturnsCorrectRunner()
        {
            // Arrange
            var repository = new SQLRunnerRepository(_connectionString);

            // Act
            var runner = repository.GetById(4);

            // Assert
            Assert.IsNotNull(runner);
            Assert.AreEqual("Sanne Sprinter", runner.Name);
            Assert.AreEqual(2, runner.SubsidyGroupID);
            Assert.AreEqual(1, runner.RunnerGroupID);
        }
        
    }
}
