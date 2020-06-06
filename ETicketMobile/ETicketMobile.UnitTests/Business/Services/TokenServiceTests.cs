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
                    .Setup(l => l.GetReshreshTokenAsync())
                    .ReturnsAsync(token.RefreshJwtToken);

            localTokenServiceMock.Setup(l => l.AddAsync(It.IsAny<Token>()));

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
        public void GetTokenAsync_AccessToken_ShouldThrowException()
        {
            // Assert
            Assert.ThrowsAsync<System.Net.WebException>(() => tokenService.GetTokenAsync(email, password));
        }

        [Fact]
        public async Task GetTokenAsync_RefreshToken_CompareRefreshesTokens_ShouldBeEqual()
        {
            // Act
            var accessToken = await tokenService.RefreshTokenAsync();

            localTokenServiceMock.Verify(lts => lts.AddAsync(It.IsAny<Token>()));

            // Assert
            Assert.Equal(tokenDto.AcessJwtToken, accessToken);
        }

        [Fact]
        public void GetTokenAsync_RefreshToken_ShouldThrowException()
        {
            // Assert
            Assert.ThrowsAsync<System.Net.WebException>(() => tokenService.RefreshTokenAsync());
        }

        [Fact]
        public async Task GetTokenAsync_RefreshToken_Verify_Add_Token()
        {
            // Act
            await tokenService.RefreshTokenAsync();

            // Assert
            localTokenServiceMock.Verify(lts => lts.AddAsync(It.IsAny<Token>()));
        }
    }
}