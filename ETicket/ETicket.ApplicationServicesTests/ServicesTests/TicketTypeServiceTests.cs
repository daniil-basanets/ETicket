using System.Collections.Generic;
using System.Linq;
using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services;
using ETicket.DataAccess.Domain;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;
using ETicket.DataAccess.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace ETicket.ApplicationServicesTests.ServicesTests
{
    public class TicketTypeServiceTests
    {
        private readonly TicketTypeService ticketTypeService;
        private readonly IList<TicketType> fakeTicketTypes;
        private readonly TicketTypeDto ticketTypeDto;
        private TicketType ticketType;

        public TicketTypeServiceTests()
        {
            var mockRepository = new Mock<IRepository<TicketType,int>>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockDbSet = new Mock<DbSet<TicketType>>();
            var context = new Mock<ETicketDataContext>();
            
            

            fakeTicketTypes = new List<TicketType>
            {
                new TicketType {Id = 1, TypeName = "Test1", Coefficient = 2.3M, DurationHours = 12, IsPersonal = true},
                new TicketType {Id = 2, TypeName = "Test2", Coefficient = 2.4M, DurationHours = 13, IsPersonal = false}
            };
            
            ticketTypeDto = new TicketTypeDto
            {
                Id = 3,
                TypeName = "TestDTO",
                Coefficient = 2.6M,
                DurationHours = 16,
                IsPersonal = false
            };
            
            mockDbSet.Setup(m => m.Remove(It.IsAny<TicketType>()))
                .Callback<TicketType>(entity => fakeTicketTypes.Remove(entity));

            context.Setup(c => c.TicketTypes).Returns(mockDbSet.Object);
            context.Setup(c=>c.TicketTypes.Remove(It.IsAny<TicketType>()))
                .Callback<TicketType>(entity => fakeTicketTypes.Remove(entity));
            context.Setup(c => c.TicketTypes.Find(It.IsAny<int>()))
                .Returns<TicketType>(ticketType => fakeTicketTypes[It.IsAny<int>()]);
            
            var repository = new TicketTypeRepository(context.Object);

            mockRepository.Setup(m => m.GetAll()).Returns(fakeTicketTypes.AsQueryable);
            mockRepository.Setup(m => m.Get(fakeTicketTypes.First().Id))
                .Returns(fakeTicketTypes.First);
            mockRepository.Setup(r => r.Create(It.IsAny<TicketType>()))
                .Callback<TicketType>(x => ticketType = x);
            mockRepository.Setup(r => r.Update(It.IsAny<TicketType>()))
                .Callback<TicketType>(x => ticketType = x);
            // mockRepository.Setup(x=>x.Delete(It.IsAny<int>()))
            //     .Callback<int>()

            //mockUnitOfWork.Setup(m => m.TicketTypes).Returns(mockRepository.Object);
            mockUnitOfWork.Setup(m => m.TicketTypes).Returns(repository);
            ticketTypeService = new TicketTypeService(mockUnitOfWork.Object);
        }
        
        [Fact]
        public void GetAll_TicketTypes_ShouldCorrect()
        {
            var ticketTypes = ticketTypeService.GetTicketTypes();
            
            Assert.Equal(fakeTicketTypes.Count(),ticketTypes.Count());
        }
        
        [Fact]
        public void GetByIdTicketType_ShouldCorrect()
        {
            Assert.Equal(fakeTicketTypes.First().DurationHours, ticketTypeService.GetTicketTypeById(1).DurationHours);
        }
        
        [Fact]
        public void Delete_TicketType_ShouldCorrect() 
        {
            ticketTypeService.Delete(1);
            Assert.Equal(1,fakeTicketTypes.Count);
        }
        
        [Fact]
        public void Create_TicketType_ShouldCorrect()
        {
            ticketTypeService.Create(ticketTypeDto);

            Assert.NotNull(ticketType);
            Assert.Equal(ticketTypeDto.TypeName, ticketType.TypeName);
        }
        
        [Fact]
        public void Update_TicketType_ShouldCorrect()
        {
            ticketTypeDto.TypeName = "UpdatedName";
            ticketTypeService.Update(ticketTypeDto);

            Assert.Equal(ticketTypeDto.TypeName,ticketType.TypeName);
        }
    }
}