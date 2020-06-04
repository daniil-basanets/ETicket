using System;
using System.Threading.Tasks;
using ETicketMobile.Data.Entities;
using ETicketMobile.DataAccess.Interfaces;
using ETicketMobile.DataAccess.Repositories;
using ETicketMobile.UnitTests.DataAccess.Comparers;
using Moq;
using Xunit;

namespace ETicketMobile.UnitTests.DataAccess.Repositories
{
    public class TokenRepositoryTests
    {
        #region Fields

        private readonly Mock<ISettingsRepository> settingsRepositoryMock;

        private TokenRepository tokenRepository;
        private readonly string setting;
        private readonly Token token;

        #endregion

        public TokenRepositoryTests()
        {
            settingsRepositoryMock = new Mock<ISettingsRepository>();

            token = new Token
            {
                AcessJwtToken = "AccessToken",
                RefreshJwtToken = "RefreshToken"
            };

            setting = "{\"AcessJwtToken\":\"AccessToken\"," +
                      "\"RefreshJwtToken\":\"RefreshToken\"}";
        }

        [Fact]
        public void CtorWithParameters_Positive()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => new TokenRepository(null));
        }

        [Fact]
        public void CtorWithParameters_Negative()
        {
            // Arrange
            var settingsRepository = new SettingsRepository();

            // Act
            var tokenRepository = new TokenRepository(settingsRepository);

            // Assert
            Assert.IsNotType<ArgumentNullException>(tokenRepository);
        }

        [Fact]
        public async Task GetTokenAsync()
        {
            // Arrange
            settingsRepositoryMock
                    .Setup(sr => sr.GetByNameAsync(It.IsAny<string>()))
                    .ReturnsAsync(setting);

            tokenRepository = new TokenRepository(settingsRepositoryMock.Object);

            var tokenEqualityComparer = new TokenEqualityComparer();

            // Act
            var actualToken = await tokenRepository.GetTokenAsync();

            // Assert
            Assert.Equal(token, actualToken, tokenEqualityComparer);
        }

        [Fact]
        public async Task GetTokenAsync_TokenShouldBeNull()
        {
            // Arrange
            settingsRepositoryMock
                    .Setup(sr => sr.GetByNameAsync(It.IsAny<string>()))
                    .ReturnsAsync(() => null);

            tokenRepository = new TokenRepository(settingsRepositoryMock.Object);

            // Act
            var actualToken = await tokenRepository.GetTokenAsync();

            // Assert
            Assert.Null(actualToken);
        }

        [Fact]
        public async Task SaveTokenAsync()
        {
            // Arrange
            settingsRepositoryMock.Setup(sr => sr.SaveAsync(It.IsAny<string>(), It.IsAny<string>()));

            tokenRepository = new TokenRepository(settingsRepositoryMock.Object);

            // Act
            await tokenRepository.SaveTokenAsync(token);            

            // Assert
            settingsRepositoryMock.Verify();
        }
    }
}