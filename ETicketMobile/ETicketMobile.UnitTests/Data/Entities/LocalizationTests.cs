using ETicketMobile.Data.Entities;
using Xunit;

namespace ETicketMobile.UnitTests.Data.Entities
{
    public class LocalizationTests
    {
        [Theory]
        [InlineData("en-US")]
        [InlineData("ru-RU")]
        [InlineData("ua-UK")]
        public void CreateInstance(string culture)
        {
            // Act
            var localization = new Localization
            {
                Culture = culture
            };

            // Assert
            Assert.Equal(culture, localization.Culture);
        }
    }
}