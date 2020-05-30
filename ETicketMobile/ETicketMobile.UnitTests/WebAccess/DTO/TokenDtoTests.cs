using ETicketMobile.WebAccess.DTO;
using Xunit;

namespace ETicketMobile.UnitTests.WebAccess.DTO
{
    public class TokenDtoTests
    {
        [Fact]
        public void CheckInstanceProperties()
        {
            // Arrange
            var acessJwtToken = "AcessToken";
            var refreshJwtToken = "RefreshToken";
            var succeeded = "yes";

            // Act
            var tokenDto = new TokenDto
            {
                AcessJwtToken = acessJwtToken,
                RefreshJwtToken = refreshJwtToken,
                Succeeded = succeeded
            };

            // Assert
            Assert.Equal(acessJwtToken, tokenDto.AcessJwtToken);
            Assert.Equal(refreshJwtToken, tokenDto.RefreshJwtToken);
            Assert.Equal(succeeded, tokenDto.Succeeded);
        }
    }
}