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

        private readonly TokenService tokenService;

        private readonly TokenDto tokenDto;
        private readonly Token token;

        private readonly string email;
        private readonly string password;

        #endregion

        public TokenServiceTests()
        {
            httpServiceMock = new Mock<IHttpService>();
            localApiMock = new Mock<ILocalApi>();

            email = "email";
            password = "password";

            token = new Token
            {
                AcessJwtToken = "AccessToken",
                RefreshJwtToken = "RefreshToken"
            };

            tokenDto = new TokenDto
            {
                AcessJwtToken = "AccessToken",
                RefreshJwtToken = "RefreshToken"
            };

            localApiMock
                    .Setup(l => l.GetTokenAsync())
                    .ReturnsAsync(token);

            httpServiceMock
                .Setup(hs => hs.PostAsync<UserSignInRequestDto, TokenDto>(
                    It.IsAny<Uri>(), It.IsAny<UserSignInRequestDto>(), It.IsAny<string>()))
                .ReturnsAsync(tokenDto);

            tokenService = new TokenService(httpServiceMock.Object, localApiMock.Object);
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
        public async Task GetTokenAsync_AccessToken()
        {
            // Act
            var token = await tokenService.GetTokenAsync(email, password);

            // Assert
            Assert.Equal(tokenDto.AcessJwtToken, token.AcessJwtToken);
        }

        [Fact]
        public async Task GetTokenAsync_RefreshToken()
        {
            // Act
            var token = await tokenService.GetTokenAsync(email, password);

            // Assert
            Assert.Equal(tokenDto.RefreshJwtToken, token.RefreshJwtToken);
        }

        [Fact]
        public async Task GetAccessTokenAsync()
        {
            // Act
            var accessToken = await tokenService.GetAccessTokenAsync();

            // Assert
            Assert.Equal(token.AcessJwtToken, accessToken);
        }

        [Fact]
        public async Task RefreshTokenAsync()
        {
            // Arrange
            httpServiceMock
                    .Setup(hs => hs.PostAsync<string, TokenDto>(
                        It.IsAny<Uri>(), It.IsAny<string>(), It.IsAny<string>()))
                    .ReturnsAsync(tokenDto);

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