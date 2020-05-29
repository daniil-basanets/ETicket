using System;
using System.Collections.Generic;
using ETicketMobile.Business.Mapping;
using ETicketMobile.Business.Model.Tickets;
using ETicketMobile.WebAccess;
using ETicketMobile.WebAccess.DTO;
using Xunit;

namespace ETicketMobile.UnitTests.Business.Mapping
{
    public class TicketMapperProfileTests
    {
        [Fact]
        public void MapTicketTypeDtoToTicketType_Positive()
        {
            // Arrange
            var ticketTypeDto = new TicketTypeDto
            {
                Id = 1,
                Name = "Name",
                Coefficient = 2M,
                DurationHours = 3
            };

            // Act
            var tickeType = AutoMapperConfiguration.Mapper.Map<TicketTypeDto, TicketType>(ticketTypeDto);

            // Assert
            Assert.Equal(ticketTypeDto.Id, tickeType.Id);
            Assert.Equal(ticketTypeDto.Name, tickeType.Name);
            Assert.Equal(ticketTypeDto.Coefficient, tickeType.Coefficient);
            Assert.Equal(ticketTypeDto.DurationHours, tickeType.DurationHours);
        }

        [Fact]
        public void MapTicketTypeToTicketTypeDto()
        {
            // Arrange
            var ticketType = new TicketType
            {
                Id = 1,
                Name = "Name",
                Coefficient = 2M,
                DurationHours = 3
            };

            // Act
            var tickeTypeDto = AutoMapperConfiguration.Mapper.Map<TicketType, TicketTypeDto>(ticketType);

            // Assert
            Assert.Equal(ticketType.Id, tickeTypeDto.Id);
            Assert.Equal(ticketType.Name, tickeTypeDto.Name);
            Assert.Equal(ticketType.Coefficient, tickeTypeDto.Coefficient);
            Assert.Equal(ticketType.DurationHours, tickeTypeDto.DurationHours);
        }

        [Fact]
        public void MapTicketDtoToTicket()
        {
            // Arrange
            var ticket = new Ticket
            {
                TicketType = "ticketType",
                ReferenceNumber = "rrn",
                TicketAreas = new List<string> { "A", "B", "C" },
                CreatedAt = DateTime.Now,
                ActivatedAt = DateTime.Now,
                ExpiredAt = DateTime.Now
            };

            // Act
            var tickeDto = AutoMapperConfiguration.Mapper.Map<Ticket, TicketDto>(ticket);

            // Assert
            Assert.Equal(ticket.TicketType, tickeDto.TicketType);
            Assert.Equal(ticket.ReferenceNumber, tickeDto.ReferenceNumber);
            Assert.Equal(ticket.TicketAreas, tickeDto.TicketAreas);
            Assert.Equal(ticket.CreatedAt, tickeDto.CreatedAt);
            Assert.Equal(ticket.ActivatedAt, tickeDto.ActivatedAt);
            Assert.Equal(ticket.ExpiredAt, tickeDto.ExpiredAt);
        }

        [Fact]
        public void MapTicketToTicketDto()
        {
            // Arrange
            var ticketDto = new TicketDto
            {
                TicketType = "ticketType",
                ReferenceNumber = "rrn",
                TicketAreas = new List<string> { "A", "B", "C" },
                CreatedAt = DateTime.Now,
                ActivatedAt = DateTime.Now,
                ExpiredAt = DateTime.Now
            };

            // Act
            var ticket = AutoMapperConfiguration.Mapper.Map<TicketDto, Ticket>(ticketDto);

            // Assert
            Assert.Equal(ticketDto.TicketType, ticket.TicketType);
            Assert.Equal(ticketDto.ReferenceNumber, ticket.ReferenceNumber);
            Assert.Equal(ticketDto.TicketAreas, ticket.TicketAreas);
            Assert.Equal(ticketDto.CreatedAt, ticket.CreatedAt);
            Assert.Equal(ticketDto.ActivatedAt, ticket.ActivatedAt);
            Assert.Equal(ticketDto.ExpiredAt, ticket.ExpiredAt);
        }
    }
}