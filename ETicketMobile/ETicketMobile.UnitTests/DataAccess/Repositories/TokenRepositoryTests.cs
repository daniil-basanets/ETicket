using System;
using System.Threading.Tasks;
using ETicketMobile.Data.Entities;
using ETicketMobile.DataAccess.Interfaces;
using ETicketMobile.DataAccess.Repositories;
using ETicketMobile.UnitTests.Comparers;
using Moq;
using Xunit;

namespace ETicketMobile.UnitTests.DataAccess.Repositories
{
    public class TokenRepositoryTests
    {
        #region Fields

        private readonly Mock<ISettingsRepository> settingsRepositoryMock;

        private readonly TokenRepository tokenRepository;
        private readonly string setting;
        private readonly Token token;

        #endregion

        public TokenRepositoryTests()
        {
            token = new Token
            {
                AcessJwtToken = "AccessToken",
                RefreshJwtToken = "RefreshToken"
            };

            setting = "{\"AcessJwtToken\":\"AccessToken\"," +
                      "\"RefreshJwtToken\":\"RefreshToken\"}";

            settingsRepositoryMock = new Mock<ISettingsRepository>();
            settingsRepositoryMock
                    .Setup(sr => sr.GetByNameAsync(It.IsAny<string>()))
                    .ReturnsAsync(setting);

            settingsRepositoryMock.Setup(sr => sr.SaveAsync(It.IsAny<string>(), It.IsAny<string>()));

            tokenRepository = new TokenRepository(settingsRepositoryMock.Object);
        }

        [Fact]
        public void CheckConstructorWithParameters_CheckNullableSettingsRepository_ShouldThrowException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => new TokenRepository(null));
        }

        [Fact]
        public async Task GetTokenAsync_CompareTokens_ShouldBeEqual()
        {
            // Arrange
            var tokenEqualityComparer = new TokenEqualityComparer();

            // Act
            var actualToken = await tokenRepository.GetTokenAsync();

            // Assert
            Assert.Equal(token, actualToken, tokenEqualityComparer);
        }

        [Fact]
        public async Task GetTokenAsync_CheckNullableSettingName_TokenShouldBeNull()
        {
            // Arrange
            settingsRepositoryMock
                    .Setup(sr => sr.GetByNameAsync(It.IsAny<string>()))
                    .ReturnsAsync(() => null);

            // Act
            var actualToken = await tokenRepository.GetTokenAsync();

            // Assert
            Assert.Null(actualToken);
        }

        [Fact]
        public async Task Create_Token()
        {
            // Act
            await tokenRepository.SaveTokenAsync(token);

            // Assert
            settingsRepositoryMock.Verify();
        }
    }
}