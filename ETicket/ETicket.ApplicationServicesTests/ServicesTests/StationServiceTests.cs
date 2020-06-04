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
    public class StationServiceTests
    {
        #region Private members

        private readonly StationService stationService;
        private readonly IList<Station> fakeStations;
        private readonly StationDto stationDto;
        private Station station;

        #endregion

        #region Constructor

        public StationServiceTests()
        {
            var mockRepository = new Mock<IRepository<Station, int>>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            fakeStations = new List<Station>
            {
                new Station {Id = 3, Area= null, AreaId = 0, Latitude = 3.2, Longitude = 2.3, Name = "Test1", RouteStations = null},
                new Station {Id = 6, Area= null, AreaId = 0, Latitude = 3.2, Longitude = 2.3, Name = "Test2", RouteStations = null},
                new Station {Id = 1, Area= null, AreaId = 0, Latitude = 3.2, Longitude = 2.3, Name = "Test3", RouteStations = null},
            };

            stationDto = new StationDto
            {
                Id = 3,
                AreaId = 0,
                Latitude = 3.2,
                Longitude = 2.3,
                Name = "TestDto",
                RouteStations = null,
                AreaName = null
            };

            mockRepository.Setup(m => m.GetAll()).Returns(fakeStations.AsQueryable);
            mockRepository.Setup(m => m.Get(It.IsAny<int>()))
                .Returns<int>(id => fakeStations.Single(s => s.Id == id));
            mockRepository.Setup(r => r.Create(It.IsAny<Station>()))
                .Callback<Station>(s => fakeStations.Add(station = s));
            mockRepository.Setup(r => r.Update(It.IsAny<Station>()))
                .Callback<Station>(s => station = s);
            mockRepository.Setup(x => x.Delete(It.IsAny<int>()))
                .Callback<int>(id => fakeStations.Remove(fakeStations.Single(s => s.Id == id)));

            mockUnitOfWork.Setup(m => m.Stations).Returns(mockRepository.Object);

            stationService = new StationService(mockUnitOfWork.Object);
        }

        #endregion

        #region GetStations facts

        [Fact]
        public void GetAll_CheckNull_ShouldBeNotNull()
        {
            var actual = stationService.GetAll();

            Assert.NotNull(actual);
        }

        [Fact]
        public void GetAll_CompareCount_ShouldBeEqual()
        {
            var stations = stationService.GetAll();

            var expected = fakeStations.Count;
            var actual = stations.Count();

            Assert.Equal(expected, actual);
        }

        #endregion

        #region GetStationById facts

        [Theory]
        [InlineData(1)]
        [InlineData(6)]
        public void Get_CheckNameInReceivedObject_ShouldBeTheSameAsInFake(int id)
        {
            var expected = fakeStations.Single(t => t.Id == id).Name;
            var actual = stationService.Get(id).Name;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Get_Id_ShouldBeGreaterZero()
        {
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => stationService.Get(-9));

            var expectedMessage = "id should be greater than zero (Parameter 'id')";
            Assert.Equal(expectedMessage, exception.Message);
        }

        #endregion

        #region CreateStation facts

        [Fact]
        public void Create_Station_ShouldBeNotNull()
        {
            stationService.Create(stationDto);

            var actual = station;

            Assert.NotNull(actual);
        }

        [Fact]
        public void Create_CheckNameInNewObject_ShouldBeTheSameAsInFake()
        {
            stationService.Create(stationDto);

            var expected = stationDto.Name;
            var actual = station.Name;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Create_AddNewObject_CountShouldIncrease()
        {
            var expected = fakeStations.Count + 1;

            stationService.Create(stationDto);

            var actual = fakeStations.Count;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(" \r \t \n")]
        public void Create_Station_ShouldFailNameIsEmpty(string name)
        {
            stationDto.Name = name;

            var exception = Assert.Throws<ArgumentException>(() => stationService.Create(stationDto));

            var expectedMessage = "Name is empty";
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Theory]
        [InlineData("hhhhhhhhhhhhhhhhhhhhhhhhhhhhhh")]
        public void Create_Station_ShouldFailNameIsInvalid(string name)
        {
            stationDto.Name = name;

            var exception = Assert.Throws<ArgumentException>(() => stationService.Create(stationDto));

            var expectedMessage = $"Length {name.Length} of Name is invalid";
            Assert.Equal(expectedMessage, exception.Message);
        }

        #endregion

        #region UpdateStation facts

        [Theory]
        [InlineData("     ")]
        [InlineData("\r  \t \n ")]
        public void Update_Station_ShouldFailNameIsInvalid(string name)
        {
            stationDto.Name = name;

            Assert.Throws<ArgumentException>(() => stationService.Update(stationDto));
        }

        [Fact]
        public void Update_Station_NameShouldBeEqualDTOsName()
        {
            stationDto.Name = "NewName";
            var expected = stationDto.Name;

            stationService.Update(stationDto);

            var actual = station.Name;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region DeleteStation facts

        [Fact]
        public void Delete_Station_CountShouldDecrease()
        {
            var expected = fakeStations.Count - 1;

            stationService.Delete(fakeStations.First().Id);

            var actual = fakeStations.Count;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-100)]
        public void Delete_Station_IdShouldBeGreaterZero(int id)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => stationService.Delete(id));
        }

        #endregion
    }
}
