using Microsoft.VisualStudio.TestTools.UnitTesting;
using Infrastructure.Model;
using System;

namespace Unittests
{
    [TestClass]
    public sealed class Test1
    {
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
    }
}
