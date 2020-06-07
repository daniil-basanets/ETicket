using System;
using System.Threading.Tasks;
using ETicketMobile.Data.Entities;
using ETicketMobile.DataAccess.Interfaces;
using ETicketMobile.DataAccess.Repositories;
using Moq;
using Xunit;

namespace ETicketMobile.UnitTests.DataAccess.Repositories
{
    public class LocalizationRepositoryTests
    {
        #region Fields

        private readonly LocalizationRepository localizationRepository;

        private readonly Mock<ISettingsRepository> settingsRepositoryMock;
        private readonly Localization localization;

        #endregion

        public LocalizationRepositoryTests()
        {
            localization = new Localization { Culture = "ru-RU" };

            var culture = "{\"Culture\":\"ru-RU\"}";

            settingsRepositoryMock = new Mock<ISettingsRepository>();
            settingsRepositoryMock
                    .Setup(sr => sr.GetByNameAsync(It.IsAny<string>()))
                    .ReturnsAsync(culture);

            settingsRepositoryMock.Setup(sr => sr.SaveAsync(It.IsAny<string>(), It.IsAny<string>()));

            localizationRepository = new LocalizationRepository(settingsRepositoryMock.Object);
        }

        [Fact]
        public void CheckConstructorWithParameters_CheckNullableSettingsRepository_ShouldThrowException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => new LocalizationRepository(null));
        }

        [Fact]
        public async Task GetLocalizationAsync_CompareLocalizations_ShouldBeEqual()
        {
            // Act
            var actualLocalization = await localizationRepository.GetLocalizationAsync();

            // Assert
            Assert.Equal(localization.Culture, actualLocalization.Culture);
        }

        [Fact]
        public async Task GetLocalizationAsync_CheckNullableSettingName_LocalizationShouldBeNull()
        {
            // Arrange
            settingsRepositoryMock
                    .Setup(sr => sr.GetByNameAsync(It.IsAny<string>()))
                    .ReturnsAsync(() => null);

            // Act
            var actualLocalization = await localizationRepository.GetLocalizationAsync();

            // Assert
            Assert.Null(actualLocalization);
        }

        [Fact]
        public async Task Create_Localization()
        {
            // Act
            await localizationRepository.SaveLocalizationAsync(localization);

            // Assert
            settingsRepositoryMock.Verify();
        }
    }
}