using ETicketMobile.Business.Mapping;
using ETicketMobile.Data.Entities;
using ETicketMobile.WebAccess.DTO;
using Xunit;

namespace ETicketMobile.UnitTests.Business.Mapping
{
    public class TokenMapperProfileTests
    {
        [Fact]
        public void MapTokenDtoToToken()
        {
            // Arrange
            var tokenDto = new TokenDto
            {
                AcessJwtToken = "AccessToken",
                RefreshJwtToken = "RefreshToken"
            };

            // Act
            var token = AutoMapperConfiguration.Mapper.Map<TokenDto, Token>(tokenDto);

            // Assert
            Assert.Equal(tokenDto.AcessJwtToken, token.AcessJwtToken);
            Assert.Equal(tokenDto.RefreshJwtToken, token.RefreshJwtToken);
        }

        [Fact]
        public void MapTokenToTokenDto()
        {
            // Arrange
            var token = new Token
            {
                AcessJwtToken = "AccessToken",
                RefreshJwtToken = "RefreshToken"
            };

            // Act
            var tokenDto = AutoMapperConfiguration.Mapper.Map<Token, TokenDto>(token);

            // Assert
            Assert.Equal(token.AcessJwtToken, tokenDto.AcessJwtToken);
            Assert.Equal(token.RefreshJwtToken, tokenDto.RefreshJwtToken);
        }
    }
}