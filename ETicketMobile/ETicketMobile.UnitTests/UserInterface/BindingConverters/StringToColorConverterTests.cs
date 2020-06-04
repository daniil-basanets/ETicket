using System;
using ETicketMobile.UserInterface.BindingConverters;
using Xamarin.Forms;
using Xunit;

namespace ETicketMobile.UnitTests.UserInterface.BindingConverters
{
    public class StringToColorConverterTests
    {
        #region Fields

        private readonly StringToColorConverter stringToColorConverter;

        #endregion

        public StringToColorConverterTests()
        {
            stringToColorConverter = new StringToColorConverter();
        }

        [Theory]
        [InlineData("abc")]
        [InlineData("123")]
        public void Convert_Positive(string value)
        {
            // Arrange
            var expectedValue = Color.Red;

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

            // Act
            var actualValue = (Color)stringToColorConverter.Convert(value, null, null, null);

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void ConvertBack()
        {
            // Assert
            Assert.Throws<NotImplementedException>(() => stringToColorConverter.ConvertBack(null, null, null, null));
        }
    }
}