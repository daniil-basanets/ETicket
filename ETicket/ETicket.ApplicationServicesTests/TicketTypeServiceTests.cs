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
        private readonly Mock<TicketTypeRepository> ticketTypeRepository;
        private readonly Mock<IUnitOfWork> unitOfWork;
        private TicketTypeService ticketTypeService;
        private readonly IQueryable<TicketType> fakeTicketTypes;
        private readonly TicketTypeDto ticketTypeDto;

        public TicketTypeServiceTests()
        {
            ticketTypeRepository = new Mock<TicketTypeRepository>(null);
            unitOfWork = new Mock<IUnitOfWork>();
            ticketTypeDto = new TicketTypeDto
            {
                Id = 5,
                TypeName = "TestDto",
                DurationHours = 22
            };
            fakeTicketTypes = new List<TicketType>
                {
                    new TicketType {Id = 1, TypeName = "Test1", Price = 2.3M, DurationHours = 12, IsPersonal = true},
                    new TicketType {Id = 2, TypeName = "Test2", Price = 2.4M, DurationHours = 13, IsPersonal = false}
                }.AsQueryable();
        }
        
        [Fact]
        public void TicketTypeGetAll()
        {
            ticketTypeRepository.Setup(m => m.GetAll()).Returns(fakeTicketTypes);
            unitOfWork.Setup(x => x.TicketTypes).Returns(ticketTypeRepository.Object);
            
            ticketTypeService = new TicketTypeService(unitOfWork.Object);
            var ticketTypes = ticketTypeService.GetAll();
            
            Assert.NotNull(ticketTypes);
            Assert.Equal((uint)13,fakeTicketTypes.Last().DurationHours);
        }

        [Fact]
        public void GetByIdTicketType()
        {
            ticketTypeRepository.Setup(m => m.Get(1)).Returns(fakeTicketTypes.First);
            unitOfWork.Setup(m => m.TicketTypes).Returns(ticketTypeRepository.Object);
            
            ticketTypeService = new TicketTypeService(unitOfWork.Object);
            var ticketType = ticketTypeService.Get(1);
            
            Assert.Equal(fakeTicketTypes.First().DurationHours,ticketType.DurationHours);
        }

        [Fact]
        public void DeleteTicketType() 
        {
            unitOfWork.Setup(m => m.TicketTypes).Returns(ticketTypeRepository.Object);
            
            ticketTypeService = new TicketTypeService(unitOfWork.Object);
            ticketTypeService.Delete(1);
            
            ticketTypeRepository.Verify(m=>m.Delete(It.IsAny<int>()));
            
        }

        [Fact]
        public void DeleteTest()
        {
            var fakeTicketType = new List<TicketType>
            {
                new TicketType {Id = 1, TypeName = "Test1", Price = 2.3M, DurationHours = 12, IsPersonal = true},
                new TicketType {Id = 2, TypeName = "Test2", Price = 2.4M, DurationHours = 13, IsPersonal = false}
            }.AsQueryable();
            
            ticketTypeRepository.Setup(m => m.GetAll()).Returns(fakeTicketTypes);
            unitOfWork.Setup(m => m.TicketTypes).Returns(ticketTypeRepository.Object);
            
            ticketTypeService = new TicketTypeService(unitOfWork.Object);
            ticketTypeService.Delete(1);
            
            Assert.Null(fakeTicketType.First());
        }

        [Fact]
        public void CreateTicketType()
        {
            unitOfWork.Setup(m => m.TicketTypes).Returns(ticketTypeRepository.Object);
            
            ticketTypeService = new TicketTypeService(unitOfWork.Object);
            ticketTypeService.Create(ticketTypeDto);
            
            ticketTypeRepository.Verify(m=>m.Create(It.IsAny<TicketType>()));
        }
        
        [Fact]
        public void UpdateTicketType()
        {
            unitOfWork.Setup(m => m.TicketTypes).Returns(ticketTypeRepository.Object);
            
            ticketTypeService = new TicketTypeService(unitOfWork.Object);
            ticketTypeService.Update(ticketTypeDto);
            
            ticketTypeRepository.Verify(m=>m.Update(It.IsAny<TicketType>()));
        }
    }
}
