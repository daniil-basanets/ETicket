using ETicketMobile.WebAccess.DTO;
using Xunit;

namespace ETicketMobile.UnitTests.WebAccess.DTO
{
    public class GetTicketPriceResponseDtoTests
    {
        [Fact]
        public void CheckInstanceProperties()
        {
            // Arrange
            var totalPrice = 100;

            // Act
            var getTicketPriceResponsetDto = new GetTicketPriceResponseDto
            {
                TotalPrice = totalPrice
            };

            // Assert
            Assert.Equal(totalPrice, getTicketPriceResponsetDto.TotalPrice);
        }
    }
}