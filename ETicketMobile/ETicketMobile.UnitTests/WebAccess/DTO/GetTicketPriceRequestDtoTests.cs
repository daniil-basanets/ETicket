using System.Collections.Generic;
using ETicketMobile.WebAccess.DTO;
using Xunit;

namespace ETicketMobile.UnitTests.WebAccess.DTO
{
    public class GetTicketPriceRequestDtoTests
    {
        [Fact]
        public void CheckInstanceProperties()
        {
            // Arrange
            var ticketTypeId = 1;
            var areasId = new List<int> { 1, 2, 3 };

            // Act
            var getTicketPriceRequestDto = new GetTicketPriceRequestDto
            {
                TicketTypeId = ticketTypeId,
                AreasId = areasId
            };

            // Assert
            Assert.Equal(ticketTypeId, getTicketPriceRequestDto.TicketTypeId);
            Assert.Equal(areasId, getTicketPriceRequestDto.AreasId);
        }
    }
}