using System;
using System.Threading.Tasks;
using ETicketMobile.Data.Entities;
using ETicketMobile.DataAccess.LocalAPI.Interfaces;
using ETicketMobile.DataAccess.Services;
using Moq;
using Xunit;

namespace ETicketMobile.UnitTests.DataAccess.Services
{
    public class LocalTokenServiceTests
    {
        #region Fields

        private readonly LocalTokenService localTokenService;

        private readonly Mock<ILocalApi> localApiMock;

        private readonly Token token;

        #endregion

        public LocalTokenServiceTests()
        {
            localApiMock = new Mock<ILocalApi>();

            token = new Token
            {
                AcessJwtToken = "AccessToken",
                RefreshJwtToken = "RefreshToken"
            };

            localApiMock.Setup(l => l.AddAsync(token));

            localApiMock
                    .Setup(l => l.GetTokenAsync())
                    .ReturnsAsync(token);

            localTokenService = new LocalTokenService(localApiMock.Object);
        }

        [Fact]
        public void CheckConstructorWithParameters_CheckNullableLocalApiService_ShouldThrowException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => new LocalTokenService(null));
        }

        [Fact]
        public async Task Add_Token()
        {
            // Act
            await localTokenService.AddAsync(token);

            // Assert
            localApiMock.Verify(l => l.AddAsync(It.IsAny<Token>()));
        }

        [Fact]
        public async Task GetAccessTokenAsync_AcessToken_CompareAcessTokens_ShouldBeEqual()
        {
            // Act
            var acessToken = await localTokenService.GetAccessTokenAsync();

            // Assert
            Assert.Equal(token.AcessJwtToken, acessToken);
        }

        [Fact]
        public async Task GetRefreshTokenAsync_RefreshToken_CompareRefreshTokens_ShouldBeEqual()
        {
            // Act
            var refreshToken = await localTokenService.GetReshreshTokenAsync();

            // Assert
            Assert.Equal(token.RefreshJwtToken, refreshToken);
        }
    }
}