using System;
using System.Collections.Generic;
using ETicketMobile.WebAccess.DTO;
using Xunit;

namespace ETicketMobile.UnitTests.WebAccess.DTO
{
    public class TicketDtoTests
    {
        [Fact]
        public void CheckInstanceProperties()
        {
            // Arrange
            var ticketType = "TicketType";
            var ticketAreas = new List<string> { "Ticket", "Areas" };
            var referenceNumber = "ReferenceNumber";
            var createdAt = DateTime.Now;
            var activatedAt = DateTime.Now;
            var expiredAt = DateTime.Now;

            // Act
            var ticketDto = new TicketDto
            {
                TicketType = ticketType,
                TicketAreas = ticketAreas,
                ReferenceNumber= referenceNumber,
                CreatedAt= createdAt,
                ActivatedAt= activatedAt,
                ExpiredAt= expiredAt
            };

            // Assert
            Assert.Equal(ticketType, ticketDto.TicketType);
            Assert.Equal(ticketAreas, ticketDto.TicketAreas);
            Assert.Equal(referenceNumber, ticketDto.ReferenceNumber);
            Assert.Equal(createdAt, ticketDto.CreatedAt);
            Assert.Equal(activatedAt, ticketDto.ActivatedAt);
            Assert.Equal(expiredAt, ticketDto.ExpiredAt);
        }
    }
}