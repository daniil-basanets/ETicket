using System;
using System.Threading.Tasks;
using ETicketMobile.Business.Exceptions;
using ETicketMobile.Business.Services;
using ETicketMobile.Data.Entities;
using ETicketMobile.DataAccess.Services.Interfaces;
using ETicketMobile.WebAccess.DTO;
using ETicketMobile.WebAccess.Network.WebServices.Interfaces;
using Moq;
using Xunit;

namespace ETicketMobile.UnitTests.Business.Services
{
    public class TokenServiceTests
    {
        #region Fields

        private readonly Mock<ILocalTokenService> localTokenServiceMock;
        private readonly Mock<IHttpService> httpServiceMock;

        private readonly TokenService tokenService;

        private readonly TokenDto tokenDto;
        private readonly Token token;

        private readonly string email;
        private readonly string password;

        #endregion

        public TokenServiceTests()
        {
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

            localTokenServiceMock = new Mock<ILocalTokenService>();
            localTokenServiceMock
                    .Setup(l => l.GetReshreshTokenAsync())
                    .ReturnsAsync(token.RefreshJwtToken);

            localTokenServiceMock.Setup(l => l.AddAsync(It.IsAny<Token>()));

            httpServiceMock = new Mock<IHttpService>();
            httpServiceMock
                    .SetupSequence(hs => hs.PostAsync<UserSignInRequestDto, TokenDto>(
                        It.IsAny<Uri>(), It.IsAny<UserSignInRequestDto>(), It.IsAny<string>()))
                    .ReturnsAsync(tokenDto)
                    .ThrowsAsync(new System.Net.WebException());

            httpServiceMock
                    .SetupSequence(hs => hs.PostAsync<string, TokenDto>(
                        It.IsAny<Uri>(), It.IsAny<string>(), It.IsAny<string>()))
                    .ReturnsAsync(tokenDto)
                    .ThrowsAsync(new System.Net.WebException());

            tokenService = new TokenService(localTokenServiceMock.Object, httpServiceMock.Object);
        }

        [Fact]
        public void CheckConstructorWithParameters_CheckNullableLocalTokenService_ShouldThrowException()
        {
            // Arrange
            ILocalTokenService localTokenService = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => new TokenService(localTokenService, httpServiceMock.Object));
        }

        [Fact]
        public void CheckConstructorWithParameters_CheckNullableHttpService_ShouldThrowException()
        {
            // Arrange
            IHttpService httpService = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => new TokenService(localTokenServiceMock.Object, httpService));
        }

        [Fact]
        public async Task GetToken_CheckAccessTokens_ShouldBeEqual()
        {
            // Act
            var token = await tokenService.GetTokenAsync(email, password);

            // Assert
            Assert.Equal(tokenDto.AcessJwtToken, token.AcessJwtToken);
        }

        [Fact]
        public async Task GetToken_ShouldThrowException()
        {
            // Act
            await tokenService.GetTokenAsync(email, password);

            // Assert
            await Assert.ThrowsAsync<WebException>(() => tokenService.GetTokenAsync(email, password));
        }

        [Fact]
        public async Task GetToken_CheckRefreshTokens_ShouldBeEqual()
        {
            // Act
            var accessToken = await tokenService.RefreshTokenAsync();

            // Assert
            Assert.Equal(tokenDto.AcessJwtToken, accessToken);
        }

        [Fact]
        public async Task GetToken_CheckRefreshToken_ShouldThrowException()
        {
            // Act
            await tokenService.RefreshTokenAsync();

            // Assert
            await Assert.ThrowsAsync<WebException>(() => tokenService.RefreshTokenAsync());
        }

        [Fact]
        public async Task GetToken_CheckRefreshToken_VerifyAddToken()
        {
            // Act
            await tokenService.RefreshTokenAsync();

            // Assert
            localTokenServiceMock.Verify(lts => lts.AddAsync(It.IsAny<Token>()));
        }
    }
}