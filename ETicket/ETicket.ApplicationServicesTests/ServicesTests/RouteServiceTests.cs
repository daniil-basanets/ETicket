using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace ETicket.ApplicationServicesTests.ServicesTests
{
    public class RouteServiceTests
    {

        #region Private members

        private readonly RouteService routeService;
        private readonly IList<Route> fakeRoutes;
        private readonly RouteDto routeDto;
        private readonly RouteStation fakeRouteStation;
        private Route route;

        #endregion

        #region Constructor

        public RouteServiceTests()
        {
            var mockRepository = new Mock<IRepository<Route, int>>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            var mockRouteStation = new Mock<IRepository<RouteStation, int>>();
            fakeRouteStation = new RouteStation();

            fakeRoutes = new List<Route>
            {
                new Route {Id = 3, FirstStation= new Station {Id = 3, Area= null, AreaId = 0, Latitude = 3.2, Longitude = 2.3, Name = "Test1", RouteStations = null}, LastStation = new Station {Id = 6, Area= null, AreaId = 0, Latitude = 3.2, Longitude = 2.3, Name = "Test2", RouteStations = null}, FirstStationId = 0, LastStationId = 0, Number = "Test1", RouteStations = null},
                new Route {Id = 6, FirstStation= new Station {Id = 3, Area= null, AreaId = 0, Latitude = 3.2, Longitude = 2.3, Name = "Test1", RouteStations = null}, LastStation = new Station {Id = 6, Area= null, AreaId = 0, Latitude = 3.2, Longitude = 2.3, Name = "Test2", RouteStations = null}, FirstStationId = 0, LastStationId = 0, Number = "Test2", RouteStations = null},
                new Route {Id = 1, FirstStation= new Station {Id = 3, Area= null, AreaId = 0, Latitude = 3.2, Longitude = 2.3, Name = "Test1", RouteStations = null}, LastStation = new Station {Id = 6, Area= null, AreaId = 0, Latitude = 3.2, Longitude = 2.3, Name = "Test2", RouteStations = null}, FirstStationId = 0, LastStationId = 0, Number = "Test3", RouteStations = null},
            };

            routeDto = new RouteDto
            {
                Id = 3,
                FirstStationName = "FirstStation",
                LastStationName = "LastStation",
                FirstStationId = 0,
                LastStationId = 0,
                Number = "TestDto",
                StationNames = null,
                StationIds = new List<int>()
            };

            mockRepository.Setup(m => m.GetAll()).Returns(fakeRoutes.AsQueryable);
            mockRepository.Setup(m => m.Get(It.IsAny<int>()))
                .Returns<int>(id => fakeRoutes.Single(s => s.Id == id));
            mockRepository.Setup(r => r.Create(It.IsAny<Route>()))
                .Callback<Route>(s => fakeRoutes.Add(route = s));
            mockRepository.Setup(r => r.Update(It.IsAny<Route>()))
                .Callback<Route>(s => route = s);
            mockRepository.Setup(x => x.Delete(It.IsAny<int>()))
                .Callback<int>(id => fakeRoutes.Remove(fakeRoutes.Single(s => s.Id == id)));

            mockUnitOfWork.Setup(m => m.Routes).Returns(mockRepository.Object);

            mockRouteStation.Setup(r => r.Delete(It.IsAny<int>()));

            mockUnitOfWork.Setup(m => m.RouteStation).Returns(mockRouteStation.Object);

            routeService = new RouteService(mockUnitOfWork.Object);
        }

        #endregion

        #region GetRoute facts

        [Fact]
        public void GetAll_CheckNull_ShouldBeNotNull()
        {
            var actual = routeService.GetRoutes();

            Assert.NotNull(actual);
        }

        [Fact]
        public void GetAll_CompareCount_ShouldBeEqual()
        {
            var routes = routeService.GetRoutes();

            var expected = fakeRoutes.Count;
            var actual = routes.Count();

            Assert.Equal(expected, actual);
        }

        #endregion

        #region GetStationById facts

        [Theory]
        [InlineData(3)]
        [InlineData(6)]
        public void Get_CheckNameInReceivedObject_ShouldBeTheSameAsInFake(int id)
        {
            var expected = fakeRoutes.Single(t => t.Id == id).Number;
            var actual = routeService.GetRouteById(id).Number;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Get_Id_ShouldBeGreaterZero()
        {
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => routeService.GetRouteById(-9));

            var expectedMessage = "id should be greater than zero (Parameter 'id')";

            Assert.Equal(expectedMessage, exception.Message);
        }

        #endregion

        #region CreateRoute facts

        [Fact]
        public void Create_Route_ShouldBeNotNull()
        {
            routeService.Create(routeDto);

            var actual = route;

            Assert.NotNull(actual);
        }

        [Fact]
        public void Create_CheckNameInNewObject_ShouldBeTheSameAsInFake()
        {
            routeService.Create(routeDto);

            var expected = routeDto.Number;
            var actual = route.Number;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Create_AddNewObject_CountShouldIncrease()
        {
            var expected = fakeRoutes.Count + 1;

            routeService.Create(routeDto);

            var actual = fakeRoutes.Count;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(" \r \t \n")]
        public void Create_Route_ShouldFailNumberIsEmpty(string number)
        {
            routeDto.Number = number;

            var exception = Assert.Throws<ArgumentException>(() => routeService.Create(routeDto));

            var expectedMessage = "Route number is empty";

            Assert.Equal(expectedMessage, exception.Message);
        }

        [Theory]
        [InlineData("hhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhh")]
        public void Create_Route_ShouldFailNumberIsInvalid(string number)
        {
            routeDto.Number = number;

            var exception = Assert.Throws<ArgumentException>(() => routeService.Create(routeDto));

            var expectedMessage = $"Length {number.Length} of Route number is invalid";

            Assert.Equal(expectedMessage, exception.Message);
        }

        #endregion

        #region UpdateRoute facts

        [Theory]
        [InlineData("     ")]
        [InlineData("\r  \t \n ")]
        public void Update_Route_ShouldFailNumberIsInvalid(string number)
        {
            routeDto.Number = number;

            Assert.Throws<ArgumentException>(() => routeService.Update(routeDto));
        }

        [Fact]
        public void Update_Route_NameShouldBeEqualDTOsNumber()
        {
            routeDto.Number = "NewNumber";

            var expected = routeDto.Number;

            routeService.Update(routeDto);

            var actual = route.Number;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region DeleteRoute facts

        [Fact]
        public void Delete_Route_CountShouldDecrease()
        {
            var expected = fakeRoutes.Count - 1;

            routeService.Delete(fakeRoutes.First().Id);

            var actual = fakeRoutes.Count;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-100)]
        public void Delete_Route_IdShouldBeGreaterZero(int id)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => routeService.Delete(id));
        }

        #endregion
    }
}
