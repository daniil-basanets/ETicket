using ETicketMobile.WebAccess;
using Xunit;

namespace ETicketMobile.UnitTests.WebAccess.DTO
{
    public class TicketTypeDtoTests
    {
        [Fact]
        public void CheckInstanceProperties()
        {
            // Arrange
            var id = 1;
            var name = "Name";
            var coefficient = 1.00M;
            var duration = 1;

            // Act
            var ticketTypeDto = new TicketTypeDto
            {
                Id = id,
                Name = name,
                Coefficient = coefficient,
                DurationHours = duration
            };

            // Assert
            Assert.Equal(id, ticketTypeDto.Id);
            Assert.Equal(name, ticketTypeDto.Name);
            Assert.Equal(coefficient, ticketTypeDto.Coefficient);
            Assert.Equal(duration, ticketTypeDto.DurationHours);
        }
    }
}