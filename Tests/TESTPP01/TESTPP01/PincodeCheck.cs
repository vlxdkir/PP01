using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TESTPP01.models;
using CoffeeShop.Views;

namespace TESTPP01
{
    [TestClass]
    public class PincodeCheckerTests
    {
        [TestMethod]
        public void ValidatePincode_CorrectPincode_ReturnsTrue()
        {
            // Arrange
            string pincode = "1111"; // Используйте правильный PIN-код из вашей базы данных

            // Act
            bool result = PincodeValidator.ValidatePincode(pincode, out string errorMessage);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(string.Empty, errorMessage);
        }

        [TestMethod]
        public void ValidatePincode_IncorrectPincode_ReturnsFalse()
        {
            // Arrange
            string pincode = "0000"; // Используйте PIN-код, который отсутствует в вашей базе данных

            // Act
            bool result = PincodeValidator.ValidatePincode(pincode, out string errorMessage);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual("ПИН-код не найден.", errorMessage);
        }
    }
}
