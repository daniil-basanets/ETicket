using System;
using System.Threading.Tasks;
using ETicketMobile.Data.Entities;
using ETicketMobile.DataAccess.Interfaces;
using ETicketMobile.DataAccess.LocalAPI;
using ETicketMobile.DataAccess.Repositories;
using Moq;
using Xunit;

namespace ETicketMobile.UnitTests.DataAccess.LocalAPI
{
    public class LocalApiTests
    {
        #region Fields

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
        }

        [Fact]
        public void Ctor()
        {
            // Act
            var exception = Record.Exception(() => new LocalApi());

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public void CtorWithParameters()
        {
            // Arrange
            var tokenRepository = new TokenRepository();
            var localizationRepository = new LocalizationRepository();

            // Act
            var exception = Record.Exception(() => new LocalApi(tokenRepository, localizationRepository));

            // Assert
            Assert.Null(exception);
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
            // Arrange
            tokenRepositoryMock.Setup(tr => tr.SaveTokenAsync(It.IsAny<Token>()));
            var localApi = new LocalApi(tokenRepositoryMock.Object, localizationRepositoryMock.Object);

            // Act
            localApi.AddAsync(token);

            // Assert
            tokenRepositoryMock.Verify(tr => tr.SaveTokenAsync(token));
        }

        [Fact]
        public void AddAsync_Localization()
        {
            // Arrange
            localizationRepositoryMock.Setup(l => l.SaveLocalizationAsync(It.IsAny<Localization>()));

            var localApi = new LocalApi(tokenRepositoryMock.Object, localizationRepositoryMock.Object);
            
            // Act
            localApi.AddAsync(localization);
            
            // Assert
            localizationRepositoryMock.Verify(tr => tr.SaveLocalizationAsync(localization));
        }

        [Fact]
        public async Task GetTokenAsync()
        {
            // Arrange
            tokenRepositoryMock
                    .Setup(tr => tr.GetTokenAsync())
                    .ReturnsAsync(token);

            var localApi = new LocalApi(tokenRepositoryMock.Object, localizationRepositoryMock.Object);

            // Act
            var actualToken = await localApi.GetTokenAsync();

            // Assert
            Assert.Equal(token.AcessJwtToken, actualToken.AcessJwtToken);
            Assert.Equal(token.RefreshJwtToken, actualToken.RefreshJwtToken);
        }

        [Fact]
        public async Task GetLocalizationAsync()
        {
            // Arrange
            localizationRepositoryMock
                    .Setup(l => l.GetLocalizationAsync())
                    .ReturnsAsync(localization);

            var localApi = new LocalApi(tokenRepositoryMock.Object, localizationRepositoryMock.Object);

            // Act
            var actualLocalization = await localApi.GetLocalizationAsync();

            // Assert
            Assert.Equal(localization, actualLocalization);
        }
    }
}