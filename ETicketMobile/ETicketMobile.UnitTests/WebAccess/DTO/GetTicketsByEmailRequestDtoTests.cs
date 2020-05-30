using ETicketMobile.WebAccess.DTO;
using Xunit;

namespace ETicketMobile.UnitTests.WebAccess.DTO
{
    public class GetTicketsByEmailRequestDtoTests
    {
        [Fact]
        public void CheckInstanceProperties()
        {
            // Arrange
            var email = "Email";

            // Act
            var getTicketPriceRequestDto = new GetTicketsByEmailRequestDto
            {
                Email = email
            };

            // Assert
            Assert.Equal(email, getTicketPriceRequestDto.Email);
        }
    }
}