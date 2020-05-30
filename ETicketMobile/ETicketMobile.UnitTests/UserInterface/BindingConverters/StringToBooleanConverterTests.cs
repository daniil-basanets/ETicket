using System;
using ETicketMobile.UserInterface.BindingConverters;
using Xunit;

namespace ETicketMobile.UnitTests.UserInterface.BindingConverters
{
    public class StringToBooleanConverterTests
    {
        [Theory]
        [InlineData("abc")]
        [InlineData("123")]
        public void Convert_Positive(string value)
        {
            // Arrange
            var stringToBooleanConverter = new StringToBooleanConverter();

            // Act
            var actualValue = (bool)stringToBooleanConverter.Convert(value, null, null, null);

            // Assert
            Assert.True(actualValue);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Convert_Negative(string value)
        {
            // Arrange
            var stringToBooleanConverter = new StringToBooleanConverter();

            // Act
            var actualValue = (bool)stringToBooleanConverter.Convert(value, null, null, null);

            // Assert
            Assert.False(actualValue);
        }

        [Fact]
        public void ConvertBack()
        {
            // Arrange
            var stringToColorConverter = new StringToBooleanConverter();

            // Assert
            Assert.Throws<NotImplementedException>(() => stringToColorConverter.ConvertBack(null, null, null, null));
        }
    }
}