using System;
using ETicketMobile.UserInterface.BindingConverters;
using Xunit;

namespace ETicketMobile.UnitTests.UserInterface.BindingConverters
{
    public class StringToBooleanConverterTests
    {
        #region Fields

        private readonly StringToBooleanConverter stringToBooleanConverter;

        #endregion

        public StringToBooleanConverterTests()
        {
            stringToBooleanConverter = new StringToBooleanConverter();
        }

        [Theory]
        [InlineData("abc")]
        [InlineData("123")]
        public void Convert_Positive(string value)
        {
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
            // Act
            var actualValue = (bool)stringToBooleanConverter.Convert(value, null, null, null);

            // Assert
            Assert.False(actualValue);
        }

        [Fact]
        public void ConvertBack()
        {
            // Assert
            Assert.Throws<NotImplementedException>(() => stringToBooleanConverter.ConvertBack(null, null, null, null));
        }
    }
}