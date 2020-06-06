using System;
using System.Threading.Tasks;
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
            localTokenServiceMock = new Mock<ILocalTokenService>();
            httpServiceMock = new Mock<IHttpService>();

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

            localTokenServiceMock
                    .Setup(l => l.GetAccessTokenAsync())
                    .ReturnsAsync(token.AcessJwtToken);

            localTokenServiceMock
                    .Setup(l => l.GetReshreshTokenAsync())
                    .ReturnsAsync(token.RefreshJwtToken);

            localTokenServiceMock.Setup(l => l.AddAsync(It.IsAny<Token>()));

            httpServiceMock
                .Setup(hs => hs.PostAsync<string, TokenDto>(
                    It.IsAny<Uri>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(tokenDto);

            httpServiceMock
                .Setup(hs => hs.PostAsync<UserSignInRequestDto, TokenDto>(
                    It.IsAny<Uri>(), It.IsAny<UserSignInRequestDto>(), It.IsAny<string>()))
                .ReturnsAsync(tokenDto);

            tokenService = new TokenService(localTokenServiceMock.Object, httpServiceMock.Object);
        }

        [Fact]
        public void CheckConstructorWithParameters_CheckNullableLocalTokenService_ShouldThrowException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => new TokenService(null, httpServiceMock.Object));
        }

        [Fact]
        public void CheckConstructorWithParameters_CheckNullableHttpService_ShouldThrowException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => new TokenService(localTokenServiceMock.Object, null));
        }

        [Fact]
        public async Task GetTokenAsync_AccessToken_CompareAccessesTokens_ShouldBeEqual()
        {
            // Act
            var token = await tokenService.GetTokenAsync(email, password);

            // Assert
            Assert.Equal(tokenDto.AcessJwtToken, token.AcessJwtToken);
        }

        [Fact]
        public async Task GetTokenAsync_RefreshToken_CompareRefreshesTokens_ShouldBeEqual()
        {
            // Act
            var token = await tokenService.GetTokenAsync(email, password);

            // Assert
            Assert.Equal(tokenDto.RefreshJwtToken, token.RefreshJwtToken);
        }
    }
}