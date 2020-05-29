using ETicketMobile.Business.Model.Tickets;
using Xunit;

namespace ETicketMobile.UnitTests.Business.Model.Tickets
{
    public class TicketTypeTests
    {
        [Fact]
        public void CreateInstance()
        {
            // Arrange
            var id = 1;
            var name = "Name";
            var coefficient = 2M;
            var durationHours = 3;
            var amount = 100;

            // Act
            var ticketType = new TicketType
            {
                Id = id,
                Name = name,
                Coefficient = coefficient,
                DurationHours = durationHours,
                Amount = amount
            };

            // Assert
            Assert.Equal(id, ticketType.Id);
            Assert.Equal(name, ticketType.Name);
            Assert.Equal(coefficient, ticketType.Coefficient);
            Assert.Equal(durationHours, ticketType.DurationHours);
            Assert.Equal(amount, ticketType.Amount);
        }
    }
}