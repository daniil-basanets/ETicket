using System;
using System.Threading.Tasks;
using ETicketMobile.Business.Services;
using ETicketMobile.Data.Entities;
using ETicketMobile.DataAccess.LocalAPI.Interfaces;
using ETicketMobile.WebAccess.DTO;
using ETicketMobile.WebAccess.Network.WebServices.Interfaces;
using Moq;
using Xunit;

namespace ETicketMobile.UnitTests.Business.Services
{
    public class TokenServiceTests
    {
        #region Fields

        private readonly Mock<IHttpService> httpServiceMock;
        private readonly Mock<ILocalApi> localApiMock;

        #endregion

        public TokenServiceTests()
        {
            httpServiceMock = new Mock<IHttpService>();
            localApiMock = new Mock<ILocalApi>();
        }

        [Fact]
        public void CtorWithParameters()
        {
            // Act
            var exception = Record.Exception(() => new TokenService(httpServiceMock.Object, localApiMock.Object));

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public void CtorWithParameters_NullHttpService()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => new TokenService(null, localApiMock.Object));
        }

        [Fact]
        public void CtorWithParameters_NullLocalApi()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => new TokenService(httpServiceMock.Object, null));
        }

        [Fact]
        public async Task GetTokenAsync()
        {
            // Arrange
            var email = "email";
            var password = "password";

            var tokenDto = new TokenDto
            {
                AcessJwtToken = "AccessToken",
                RefreshJwtToken = "RefreshToken"
            };

            httpServiceMock
                .Setup(hs => hs.PostAsync<UserSignInRequestDto, TokenDto>(
                    It.IsAny<Uri>(), It.IsAny<UserSignInRequestDto>(), It.IsAny<string>()))
                .ReturnsAsync(tokenDto);

            var tokenService = new TokenService(httpServiceMock.Object, localApiMock.Object);

            // Act
            var token = await tokenService.GetTokenAsync(email, password);

            // Assert
            Assert.Equal(tokenDto.AcessJwtToken, token.AcessJwtToken);
            Assert.Equal(tokenDto.RefreshJwtToken, token.RefreshJwtToken);
        }

        [Fact]
        public async Task GetAccessTokenAsync()
        {
            // Arrange
            var token = new Token
            {
                AcessJwtToken = "AccessToken"
            };

            localApiMock
                .Setup(l => l.GetTokenAsync())
                .ReturnsAsync(token);

            var tokenService = new TokenService(httpServiceMock.Object, localApiMock.Object);

            // Act
            var accessToken = await tokenService.GetAccessTokenAsync();

            // Assert
            Assert.Equal(token.AcessJwtToken, accessToken);
        }

        [Fact]
        public async Task RefreshTokenAsync()
        {
            // Arrange
            var tokenDto = new TokenDto
            {
                AcessJwtToken = "AccessToken",
                RefreshJwtToken = "RefreshToken"
            };

            var token = new Token
            {
                AcessJwtToken = "AccessToken",
                RefreshJwtToken = "RefreshToken"
            };

            httpServiceMock
                    .Setup(hs => hs.PostAsync<string, TokenDto>(
                        It.IsAny<Uri>(), It.IsAny<string>(), It.IsAny<string>()))
                    .ReturnsAsync(tokenDto);

            localApiMock
                    .Setup(l => l.GetTokenAsync())
                    .ReturnsAsync(token);

            localApiMock.Setup(l => l.AddAsync(It.IsAny<Token>()));

            var tokenService = new TokenService(httpServiceMock.Object, localApiMock.Object);

            // Act
            var accessToken = await tokenService.RefreshTokenAsync();

            localApiMock.VerifyAll();

            // Assert
            Assert.Equal(tokenDto.AcessJwtToken, accessToken);
        }
    }
}