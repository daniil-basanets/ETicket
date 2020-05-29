using ETicketMobile.Business.Model.UserAccount;
using Xunit;

namespace ETicketMobile.UnitTests.Business.Model.UserAccount
{
    public class LocalizationTests
    {
        [Fact]
        public void CreateInstance()
        {
            // Arrange
            var culture = "ru-RU";
            var language = "Russian";
            var isChoosed = true;

            // Act
            var localization = new Localization
            {
                Culture = culture,
                Language = language,
                IsChoosed = isChoosed
            };

            // Assert
            Assert.Equal(culture, localization.Culture);
            Assert.Equal(language, localization.Language);
            Assert.Equal(isChoosed, localization.IsChoosed);
        }
    }
}