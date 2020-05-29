using System;
using System.Collections.Generic;
using ETicketMobile.Business.Model.Tickets;
using Xunit;

namespace ETicketMobile.UnitTests.Business.Model.Tickets
{
    public class TicketTests
    {
        [Fact]
        public void CreateInstance()
        {
            // Arrange
            var ticketType = "ticketType";
            var referenceNumber = "rrn";
            var ticketAreas = new List<string> { "A", "B", "C" };
            var createdAt = DateTime.Now;
            var activatedAt = DateTime.Now;
            var expiredAt = DateTime.Now;

            // Act
            var ticket = new Ticket
            {
                TicketType = ticketType,
                ReferenceNumber = referenceNumber,
                TicketAreas = ticketAreas,
                CreatedAt = createdAt,
                ActivatedAt = activatedAt,
                ExpiredAt = expiredAt
            };

            // Assert
            Assert.Equal(ticketType, ticket.TicketType);
            Assert.Equal(referenceNumber, ticket.ReferenceNumber);
            Assert.Equal(ticketAreas, ticket.TicketAreas);
            Assert.Equal(createdAt, ticket.CreatedAt);
            Assert.Equal(activatedAt, ticket.ActivatedAt);
            Assert.Equal(expiredAt, ticket.ExpiredAt);
        }
    }
}