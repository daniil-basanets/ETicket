using System;
using System.Collections.Generic;
using System.Text;
using ETicketMobile.UserInterface.BindingConverters;
using Xamarin.Forms;
using Xunit;

namespace ETicketMobile.UnitTests.UserInterface.BindingConverters
{
    public class StringToColorConverterTests
    {
        [Theory]
        [InlineData("abc")]
        [InlineData("123")]
        public void Convert_Positive(string value)
        {
            // Arrange
            var expectedValue = Color.Red;
            var stringToColorConverter = new StringToColorConverter();

            // Act
            var actualValue = (Color)stringToColorConverter.Convert(value, null, null, null);

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Convert_Negative(string value)
        {
            // Arrange
            var expectedValue = Color.Default;
            var stringToColorConverter = new StringToColorConverter();

            // Act
            var actualValue = (Color)stringToColorConverter.Convert(value, null, null, null);

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void ConvertBack()
        {
            // Arrange
            var stringToColorConverter = new StringToColorConverter();

            // Assert
            Assert.Throws<NotImplementedException>(() => stringToColorConverter.ConvertBack(null, null, null, null));
        }
    }
}