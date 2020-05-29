using ETicketMobile.Data.Entities;
using Xunit;

namespace ETicketMobile.UnitTests.Data.Entities
{
    public class TokenTests
    {
        [Fact]
        public void CreateInstance()
        {
            // Arrange
            var accessToken = "AccessToken";
            var refreshToken = "RefreshToken";

            // Act
            var token = new Token
            {
                AcessJwtToken = accessToken,
                RefreshJwtToken = refreshToken
            };

            // Assert
            Assert.Equal(accessToken, token.AcessJwtToken);
            Assert.Equal(refreshToken, token.RefreshJwtToken);
        }
    }
}