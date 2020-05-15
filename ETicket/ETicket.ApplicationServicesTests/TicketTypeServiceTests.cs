using System.Collections.Generic;
using System.Linq;
using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;
using ETicket.DataAccess.Domain.Repositories;
using Moq;
using Xunit;

namespace ETicket.ApplicationServicesTests
{
    public class TicketTypeServiceTests
    {
        private readonly TicketTypeService ticketTypeService;
        private readonly IQueryable<TicketType> fakeTicketTypes;
        private readonly TicketTypeDto ticketTypeDto;
        private TicketType ticketType;
        
        public TicketTypeServiceTests()
        {
            var ticketTypeRepository = new Mock<TicketTypeRepository>(null);
            var unitOfWork = new Mock<IUnitOfWork>();
            
            fakeTicketTypes = new List<TicketType>
                {
                    new TicketType {Id = 1, TypeName = "Test1", Price = 2.3M, DurationHours = 12, IsPersonal = true},
                    new TicketType {Id = 2, TypeName = "Test2", Price = 2.4M, DurationHours = 13, IsPersonal = false}
                }.AsQueryable();
            ticketTypeDto = new TicketTypeDto
            {
                Id = 3,
                TypeName = "TestDTO",
                Price = 2.6M,
                DurationHours = 16,
                IsPersonal = false
            };
            
            ticketTypeRepository.Setup(m => m.GetAll()).Returns(fakeTicketTypes);
            ticketTypeRepository.Setup(m => m.Get(fakeTicketTypes.First().Id)).Returns(fakeTicketTypes.First());
            ticketTypeRepository.Setup(r => r.Create(It.IsAny<TicketType>()))
                .Callback<TicketType>(x => ticketType = x);
            ticketTypeRepository.Setup(r => r.Update(It.IsAny<TicketType>()))
                .Callback<TicketType>(x => ticketType = x);
            
            ticketTypeRepository.Setup(r => r.Delete(It.IsAny<int>()));
            ticketTypeRepository.Object.Delete(3);

            unitOfWork.Setup(x => x.TicketTypes).Returns(ticketTypeRepository.Object);

            ticketTypeService = new TicketTypeService(unitOfWork.Object);
        }
        
        [Fact]
        public void GetAll_TicketTypes_ShouldCorrect()
        {
            var ticketTypes = ticketTypeService.GetAll();
            
            Assert.NotNull(ticketTypes);
            Assert.Equal((uint)12,fakeTicketTypes.First().DurationHours);
            Assert.Equal(2,fakeTicketTypes.Count());
        }

        [Fact]
        public void GetByIdTicketType_ShouldCorrect()
        {
            Assert.Equal(fakeTicketTypes.First().DurationHours, ticketTypeService.Get(1).DurationHours);
        }

        [Fact]
        public void Delete_TicketType_ShouldCorrect() 
        {
            
        }
        
        [Fact]
        public void Create_TicketType_ShouldCorrect()
        {
            ticketTypeService.Create(ticketTypeDto);

            //ticketTypeRepository.Verify(x => x.Create(It.IsAny<TicketType>()), Times.Once);

            Assert.NotNull(ticketType);
            Assert.Equal(ticketTypeDto.TypeName, ticketType.TypeName);
        }

        [Fact]
        public void Update_TicketType_ShouldCorrect()
        {
            ticketTypeService.Update(ticketTypeDto);
            
            //ticketTypeRepository.Verify(x => x.Update(It.IsAny<TicketType>()), Times.Once);
            
            Assert.Equal("TestDTO",ticketType.TypeName);
        }
    }
}