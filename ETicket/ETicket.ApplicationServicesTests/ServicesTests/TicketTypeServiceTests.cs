using System;
using System.Collections.Generic;
using System.Linq;
using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;
using Moq;
using Xunit;

namespace ETicket.ApplicationServicesTests.ServicesTests
{
    public class TicketTypeServiceTests
    {
        #region Private fields

        private readonly TicketTypeService ticketTypeService;
        private readonly IList<TicketType> fakeTicketTypes;
        private readonly TicketTypeDto ticketTypeDto;
        private TicketType ticketType;

        #endregion

        #region Constructor

        public TicketTypeServiceTests()
        {
            var mockRepository = new Mock<IRepository<TicketType, int>>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            fakeTicketTypes = new List<TicketType>
            {
                new TicketType {Id = 5, TypeName = "Test1", Coefficient = 2.3M, DurationHours = 12, IsPersonal = true},
                new TicketType {Id = 7, TypeName = "Test2", Coefficient = 2.4M, DurationHours = 13, IsPersonal = false},
                new TicketType {Id = int.MaxValue, TypeName = "Test2", Coefficient = 2.4M, DurationHours = 13, IsPersonal = false}
            };

            ticketTypeDto = new TicketTypeDto
            {
                Id = 3,
                TypeName = "TestDTO",
                Coefficient = 2.6M,
                DurationHours = 16,
                IsPersonal = false
            };

            mockRepository.Setup(m => m.GetAll()).Returns(fakeTicketTypes.AsQueryable);
            mockRepository.Setup(m => m.Get(It.IsAny<int>()))
                .Returns<int>(id => fakeTicketTypes.Single(t => t.Id == id));
            mockRepository.Setup(r => r.Create(It.IsAny<TicketType>()))
                .Callback<TicketType>(t => fakeTicketTypes.Add(ticketType = t));
            mockRepository.Setup(r => r.Update(It.IsAny<TicketType>()))
                .Callback<TicketType>(t => ticketType = t);
            mockRepository.Setup(x => x.Delete(It.IsAny<int>()))
                .Callback<int>(id => fakeTicketTypes.Remove(fakeTicketTypes.Single(t => t.Id == id)));

            mockUnitOfWork.Setup(m => m.TicketTypes).Returns(mockRepository.Object);

            ticketTypeService = new TicketTypeService(mockUnitOfWork.Object);
        }

        #endregion

        #region GetTicketTypes

        [Fact]
        public void GetTicketTypes_CheckNull_ShouldBeNotNull()
        {
            var actual = ticketTypeService.GetTicketTypes();
            
            Assert.NotNull(actual);
        }
        
        [Fact]
        public void GetTicketTypes_CompareCount_ShouldBeEqual()
        {
            var ticketTypes = ticketTypeService.GetTicketTypes();

            var expected = fakeTicketTypes.Count;
            var actual = ticketTypes.Count();
            
            Assert.Equal(expected,actual);
        }

        #endregion

        #region GetByIdTicketType

        [Theory]
        [InlineData(5)]
        [InlineData(7)]
        public void GetByIdTicketType_CheckDurationHoursInReceivedObject_ShouldBeTheSameAsInFake(int id)
        {
            var expected = fakeTicketTypes.Single(t => t.Id == id).DurationHours;
            var actual = ticketTypeService.GetTicketTypeById(id).DurationHours;
            
            Assert.Equal(expected,actual);
        }

        [Fact]
        public void GetByIdTicketType_Id_ShouldBeGreaterZero()
        {
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => ticketTypeService.GetTicketTypeById(int.MinValue));
            
            Assert.Equal("id should be greater than zero (Parameter 'id')",exception.Message);
        }

        #endregion

        #region Delete

        [Fact]
        public void Delete_TicketType_CountShouldDecrease()
        {
            var expected = fakeTicketTypes.Count - 1;
            
            ticketTypeService.Delete(fakeTicketTypes.First().Id);

            var actual = fakeTicketTypes.Count;
            
            Assert.Equal(expected,actual);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-100)]
        public void Delete_TicketType_IdShouldBeGreaterZero(int id)
        {
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => ticketTypeService.Delete(id));
            
            Assert.Equal("id",exception.ParamName);
        }

        #endregion

        #region Create

        [Fact]
        public void Create_TicketType_ShouldBeNotNull()
        {
            ticketTypeService.Create(ticketTypeDto);

            var actual = ticketType;
            
            Assert.NotNull(actual);
        }
        
        [Fact]
        public void Create_CheckNameInNewObject_ShouldBeTheSameAsInFake()
        {
            ticketTypeService.Create(ticketTypeDto);

            var expected = ticketTypeDto.TypeName;
            var actual = ticketType.TypeName;
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void Create_AddNewObject_CountShouldIncrease()
        {
            var expected = fakeTicketTypes.Count + 1;
            
            ticketTypeService.Create(ticketTypeDto);

            var actual = fakeTicketTypes.Count;
            
            Assert.Equal(expected,actual);
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(" \r \t \n" )]
        public void Create_TicketType_ShouldFailTypeNameIsEmpty(string name)
        {
            ticketTypeDto.TypeName = name;
            Action action = () => ticketTypeService.Create(ticketTypeDto);

            var exception = Assert.Throws<ArgumentException>(action);
            
            Assert.Equal("Type name is empty",exception.Message);
        }

        [Theory]
        [InlineData("q")]
        [InlineData("qwertyuioplkjhgfdsazxcvbnmetretertretertertsadasdsadsa")]
        public void Create_TicketType_ShouldFailNameIsInvalid(string typeName)
        {
            ticketTypeDto.TypeName = typeName;
            Action action = () => ticketTypeService.Create(ticketTypeDto);

            var exception = Assert.Throws<ArgumentException>(action);
            
            Assert.Equal($"Length {typeName.Length} of Type name is invalid",exception.Message);
        }

        [Fact]
        public void Create_TicketType_ShouldFailDurationHoursShouldBeGreaterThanZero()
        {
            ticketTypeDto.DurationHours = 0U;
            Action action = () => ticketTypeService.Create(ticketTypeDto);
            
            Assert.Throws<ArgumentException>(action);
        }
        
        [Fact]
        public void Create_TicketType_ShouldFailDtoShouldNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => ticketTypeService.Create(null));
        }
        
        [Fact]
        public void Create_TicketType_ShouldFailCoefficientShouldBeGreaterOrEqualZero()
        {
            ticketTypeDto.Coefficient = decimal.MinValue;
            Action action = () => ticketTypeService.Create(ticketTypeDto);
            
            var exception = Assert.Throws<ArgumentException>(action);
            
            Assert.Equal("Coefficient should be greater or equal than 0",exception.Message);
        }

        #endregion

        #region Update

        [Theory]
        [InlineData("  f   ")]
        [InlineData("\r f \t \n ")]
        public void Update_TicketType_ShouldFailNameIsInvalid(string typeName)
        {
            ticketTypeDto.TypeName = typeName;
            Action action = () => ticketTypeService.Update(ticketTypeDto);

            Assert.Throws<ArgumentException>(action);
        }
        
        [Fact]
        public void Update_TicketType_NameShouldBeEqualDTOsName()
        {
            ticketTypeDto.TypeName = "UpdatedName";
            var expected = ticketTypeDto.TypeName;
            
            ticketTypeService.Update(ticketTypeDto);

            var actual = ticketType.TypeName;

            Assert.Equal(expected,actual);
        }
        
        [Fact]
        public void Update_TicketType_ShouldFailCoefficientShouldBeGreaterOrEqualZero()
        {
            ticketTypeDto.Coefficient = decimal.MinusOne;
            Action action = () => ticketTypeService.Update(ticketTypeDto);
            
            var exception = Assert.Throws<ArgumentException>(action);
            
            Assert.Equal("Coefficient should be greater or equal than 0",exception.Message);
        }
        

        #endregion
    }
}