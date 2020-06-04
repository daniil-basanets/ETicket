using System;
using System.Threading.Tasks;
using ETicketMobile.Data.Entities;
using ETicketMobile.DataAccess.Interfaces;
using ETicketMobile.DataAccess.LocalAPI;
using ETicketMobile.DataAccess.Repositories;
using ETicketMobile.UnitTests.DataAccess.Comparers;
using Moq;
using Xunit;

namespace ETicketMobile.UnitTests.DataAccess.LocalAPI
{
    public class LocalApiTests
    {
        #region Fields

        private readonly LocalApi localApi;

        private readonly Mock<ITokenRepository> tokenRepositoryMock;
        private readonly Mock<ILocalizationRepository> localizationRepositoryMock;

        private readonly Token token;
        private readonly Localization localization;

        #endregion

        public LocalApiTests()
        {
            tokenRepositoryMock = new Mock<ITokenRepository>();
            localizationRepositoryMock = new Mock<ILocalizationRepository>();

            token = new Token
            {
                AcessJwtToken = "AccessToken",
                RefreshJwtToken = "RefreshToken"
            };

            localization = new Localization { Culture = "ru-RU" };

            tokenRepositoryMock.Setup(tr => tr.SaveTokenAsync(It.IsAny<Token>()));

            localizationRepositoryMock.Setup(l => l.SaveLocalizationAsync(It.IsAny<Localization>()));

            localizationRepositoryMock
                    .Setup(l => l.GetLocalizationAsync())
                    .ReturnsAsync(localization);

            tokenRepositoryMock
                    .Setup(tr => tr.GetTokenAsync())
                    .ReturnsAsync(token);

            localApi = new LocalApi(tokenRepositoryMock.Object, localizationRepositoryMock.Object);
        }

        [Fact]
        public void CtoWithParameters_NullTokenRepository()
        {
            // Arrange
            var localizationRepository = new LocalizationRepository();

            // Assert
            Assert.Throws<ArgumentNullException>(() => new LocalApi(null, localizationRepository));
        }

        [Fact]
        public void CtoWithParameters_NullLocalizationRepository()
        {
            // Arrange
            var tokenRepository = new TokenRepository();

            // Assert
            Assert.Throws<ArgumentNullException>(() => new LocalApi(tokenRepository, null));
        }

        [Fact]
        public void AddAsync_Token()
        {
            // Act
            localApi.AddAsync(token);

            // Assert
            tokenRepositoryMock.Verify();
        }

        [Fact]
        public void AddAsync_Localization()
        {
            // Act
            localApi.AddAsync(localization);
            
            // Assert
            localizationRepositoryMock.Verify();
        }

        [Fact]
        public async Task GetTokenAsync()
        {
            // Arrange
            var tokenEqualityComparer = new TokenEqualityComparer();
            
            // Act
            var actualToken = await localApi.GetTokenAsync();

            // Assert
            Assert.Equal(token, actualToken, tokenEqualityComparer);
        }

        [Fact]
        public async Task GetLocalizationAsync()
        {
            // Act
            var actualLocalization = await localApi.GetLocalizationAsync();

            // Assert
            Assert.Equal(localization, actualLocalization);
        }
    }
}