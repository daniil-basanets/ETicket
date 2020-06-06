using System;
using System.Collections.Generic;
using System.Linq;
using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;
using Moq;
using Xunit;

namespace ETicket.ApplicationServicesTests.ServicesTests
{
    public class TicketVerificationServiceTests
    {
        #region Private fields

        private readonly TicketVerificationService ticketVerificationService;
        private readonly IList<TicketVerification> fakeTicketVerifications;
        private readonly TicketVerificationDto ticketVerificationDto;
        private TicketVerification ticketVerification;

        private readonly IList<Ticket> fakeTickets;
        private readonly IList<Station> fakeStations;
        private readonly IList<Transport> fakeTransports;
        private readonly IList<Route> fakeRoutes;
        private readonly IList<RouteStation> fakeRouteStations;
        private readonly IList<Area> fakeAreas;

        #endregion

        #region Constructor

        public TicketVerificationServiceTests()
        {
            var mockTicketVerificationRepository = new Mock<IRepository<TicketVerification, Guid>>();
            var mockTicketRepository = new Mock<IRepository<Ticket, Guid>>();
            var mockTransportRepository = new Mock<IRepository<Transport, int>>();
            var mockTicketService = new Mock<ITicketService>();
            var mockStationRepository = new Mock<IRepository<Station, int>>();
            var mockRouteStationsRepository = new Mock<IRepository<RouteStation, int>>();

            var mockUnitOfWork = new Mock<IUnitOfWork>();

            fakeAreas = new List<Area>
            {
                new Area{Id = 1, Name = "A" },
                new Area{Id = 2, Name = "B" }
            };

            fakeTickets = new List<Ticket>
            {
                new Ticket //Normal ticket
                {
                    Id = Guid.Parse("196d28f2-c6f0-4d70-a872-7a29d5dc79d3"),
                    TicketArea = new TicketArea[] { new TicketArea{ AreaId = fakeAreas[0].Id} } 
                },
                new Ticket //Expired ticket
                {
                    Id = Guid.Parse("396d28f2-c6f0-4d70-a872-7a29d5dc79d3"),
                    ActivatedUTCDate = DateTime.Now.AddDays(-2),
                    ExpirationUTCDate = DateTime.Now.AddDays(-1),
                    TicketArea = new TicketArea[] { new TicketArea{ AreaId = fakeAreas[1].Id} }
                }
            };

            fakeStations = new List<Station>
            {
                new Station {Id = 1, Latitude = 40.0, Longitude = 40.0, Name = "Station40:40", AreaId = fakeAreas[0].Id, Area = fakeAreas[0]}, //Station in zone A
                new Station {Id = 2, Latitude = 44.0, Longitude = 36.0, Name = "Station44:36", AreaId = fakeAreas[1].Id, Area = fakeAreas[1]}  //Station in zone B
            };

            fakeRoutes = new List<Route>()
            {
                new Route {Id = 1, Number = "157A"},
            };

            fakeRouteStations = new List<RouteStation>()
            {
                new RouteStation { Route = fakeRoutes[0], Station = fakeStations[0]},
                new RouteStation { Route = fakeRoutes[0], Station = fakeStations[1]},
            };

            fakeTransports = new List<Transport>
            {
                new Transport {Id = 1, CarriersId = 1, VehicleNumber = "TE1111ST", RouteId = fakeRoutes[0].Id, Route = fakeRoutes[0]}, //Normal transport
                new Transport {Id = 2, CarriersId = 2, VehicleNumber = "TE3333ST", RouteId = Int32.MaxValue} //With wrong route
            };

            fakeTicketVerifications = new List<TicketVerification>
            {
                new TicketVerification 
                {
                    Id = Guid.Parse("696d28f2-c6f0-4d70-a872-7a29d5dc79d1"),
                    VerificationUTCDate = new DateTime(2020, 06, 03, 13, 36, 05),
                    TicketId = fakeTickets[0].Id,
                    StationId = fakeStations[0].Id,
                    TransportId = fakeTransports[0].Id
                },
                new TicketVerification 
                {
                    Id = Guid.Parse("696d28f2-c6f0-4d70-a872-7a29d5dc79d3"),
                    VerificationUTCDate = new DateTime(2020, 02, 01, 07, 22, 55),
                    TicketId = fakeTickets[1].Id,
                    StationId = fakeStations[1].Id,
                    TransportId = fakeTransports[0].Id
                }
            };

            ticketVerificationDto = new TicketVerificationDto
            {
                Id = Guid.Parse("696d28f2-c6f0-4d70-a872-7a29d5dc79d4"),
                TicketId = Guid.Parse("696d28f2-c6f0-4d70-a872-7a29d5dc79d7"),
                TransportId = 4,
                StationId = 6,
                IsVerified = false
            };

            //mock TicketVerification
            mockTicketVerificationRepository.Setup(m => m.GetAll()).Returns(fakeTicketVerifications.AsQueryable);
            mockTicketVerificationRepository.Setup(m => m.Get(It.IsAny<Guid>()))
                .Returns<Guid>(id => fakeTicketVerifications.Single(t => t.Id == id));
            mockTicketVerificationRepository.Setup(r => r.Create(It.IsAny<TicketVerification>()))
                .Callback<TicketVerification>(t => fakeTicketVerifications.Add(ticketVerification = t));
            mockUnitOfWork.Setup(m => m.TicketVerifications).Returns(mockTicketVerificationRepository.Object);

            //mock Tickets
            mockTicketRepository.Setup(m => m.GetAll()).Returns(fakeTickets.AsQueryable);
            mockUnitOfWork.Setup(m => m.Tickets).Returns(mockTicketRepository.Object);

            //mock Transports
            mockTransportRepository.Setup(m => m.Get(It.IsAny<int>()))
                .Returns<int>(id => fakeTransports.FirstOrDefault(t => t.Id == id));
            mockUnitOfWork.Setup(m => m.Transports).Returns(mockTransportRepository.Object);

            //mock Stations
            mockStationRepository.Setup(m => m.GetAll()).Returns(fakeStations.AsQueryable);
            mockUnitOfWork.Setup(m => m.Stations).Returns(mockStationRepository.Object);

            //mock RouteStation
            mockRouteStationsRepository.Setup(m => m.GetAll()).Returns(fakeRouteStations.AsQueryable);
            mockUnitOfWork.Setup(m => m.RouteStation).Returns(mockRouteStationsRepository.Object);

            mockTicketService.Setup(x => x.Activate(It.IsAny<Guid>()))
                .Callback<Guid>(id => {
                    fakeTickets.Single(t => t.Id == id).ActivatedUTCDate = DateTime.Now;
                    fakeTickets.Single(t => t.Id == id).ExpirationUTCDate = DateTime.Now.AddHours(1);
                });

            ticketVerificationService = new TicketVerificationService(mockUnitOfWork.Object, mockTicketService.Object);
        }

        #endregion

        #region GetTicketTypes

        [Fact]
        public void GetTicketVerifications_CheckNull_ShouldBeNotNull()
        {
            var actual = ticketVerificationService.GetTicketVerifications();

            Assert.NotNull(actual);
        }

        [Fact]
        public void GetTicketVerifications_CompareCount_ShouldBeEqual()
        {
            var ticketTypes = ticketVerificationService.GetTicketVerifications();

            var expected = fakeTicketVerifications.Count;
            var actual = ticketTypes.Count();

            Assert.Equal(expected, actual);
        }

        #endregion

        #region GetByIdTicketType

        [Theory]
        [InlineData("696d28f2-c6f0-4d70-a872-7a29d5dc79d1")]
        [InlineData("696d28f2-c6f0-4d70-a872-7a29d5dc79d3")]
        public void GetTicketVerificationById_CheckVerificationUTCDateInReceivedObject_ShouldBeTheSameAsInFake(string guidId)
        {
            var id = Guid.Parse(guidId);

            var expected = fakeTicketVerifications.Single(t => t.Id == id).VerificationUTCDate;
            var actual = ticketVerificationService.GetTicketVerificationById(id).VerificationUTCDate;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Create

        [Fact]
        public void Create_TicketVerification_ShouldBeNotNull()
        {
            ticketVerificationService.Create(ticketVerificationDto);

            var actual = ticketVerification;

            Assert.NotNull(actual);
        }

        [Fact]
        public void Create_CheckVerificationUTCDateInNewObject_ShouldBeTheSameAsInFake()
        {
            ticketVerificationService.Create(ticketVerificationDto);

            var expected = ticketVerificationDto.VerificationUTCDate;
            var actual = ticketVerification.VerificationUTCDate;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Create_AddNewObject_CountShouldIncrease()
        {
            var expected = fakeTicketVerifications.Count + 1;

            ticketVerificationService.Create(ticketVerificationDto);

            var actual = fakeTicketVerifications.Count;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region VerifyTicket

        [Fact]
        public void VerifyTicket_NotRealTicket_TicketWasNotFoundErrorMessage()
        {
            var expected = "Ticket was not found";
            var actual = ticketVerificationService.VerifyTicket(Guid.Parse("111d28f2-c6f0-4d70-a872-7a29d5dc79d1"), 1, 1, 1).ErrorMessage;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void VerifyTicket_NotActivatedTicket_HasActivatedUTCDate()
        {
            ticketVerificationService.VerifyTicket(fakeTickets[0].Id, 0, 0, 0);

            Assert.NotNull(fakeTickets[0].ActivatedUTCDate);
        }

        [Fact]
        public void VerifyTicket_NotActivatedTicket_HasExpirationUTCDate()
        {
            ticketVerificationService.VerifyTicket(fakeTickets[0].Id, 0, 0, 0);

            Assert.NotNull(fakeTickets[0].ExpirationUTCDate);
        }

        [Fact]
        public void VerifyTicket_NoTransport_TransportWasNotFoundErrorMessage()
        {
            var expected = "Transport was not found";
            var actual = ticketVerificationService.VerifyTicket(fakeTickets[0].Id, 0, 0, 0).ErrorMessage;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void VerifyTicket_TransportWithoutRoute_StationWasNotFoundErrorMessage()
        {
            var expected = "Station was not found";
            var actual = ticketVerificationService.VerifyTicket(fakeTickets[0].Id, fakeTransports[1].Id, 0, 0).ErrorMessage;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void VerifyTicket_ExpiredTicket_TicketExpiredErrorMessage()
        {
            var expected = "Ticket expired";
            var actual = ticketVerificationService.VerifyTicket(fakeTickets[1].Id, fakeTransports[0].Id, (float)fakeStations[0].Longitude, (float)fakeStations[0].Latitude).ErrorMessage;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void VerifyTicket_ExpiredTicket_CountShouldIncrease()
        {
            var expected = fakeTicketVerifications.Count + 1;

            ticketVerificationService.VerifyTicket(fakeTickets[1].Id, fakeTransports[0].Id, (float)fakeStations[0].Longitude, (float)fakeStations[0].Latitude);

            var actual = fakeTicketVerifications.Count;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void VerifyTicket_ExpiredTicket_IsVerifyFalse()
        {
            ticketVerificationService.VerifyTicket(fakeTickets[1].Id, fakeTransports[0].Id, (float)fakeStations[0].Longitude, (float)fakeStations[0].Latitude);

            var actual = fakeTicketVerifications.Last().IsVerified;

            Assert.False(actual);
        }

        [Fact]
        public void VerifyTicket_TicketWithWrongZone_TicketDoesNotContainTheZoneErrorMessage()
        {
            var expected = "Ticket does not contain the zone";
            var actual = ticketVerificationService.VerifyTicket(fakeTickets[0].Id, fakeTransports[0].Id, (float)fakeStations[1].Longitude, (float)fakeStations[1].Latitude).ErrorMessage;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void VerifyTicket_TicketWithWrongZone_CountShouldIncrease()
        {
            var expected = fakeTicketVerifications.Count + 1;

            ticketVerificationService.VerifyTicket(fakeTickets[0].Id, fakeTransports[0].Id, (float)fakeStations[1].Longitude, (float)fakeStations[1].Latitude);

            var actual = fakeTicketVerifications.Count;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void VerifyTicket_TicketWithWrongZone_IsVerifyFalse()
        {
            ticketVerificationService.VerifyTicket(fakeTickets[0].Id, fakeTransports[0].Id, (float)fakeStations[1].Longitude, (float)fakeStations[1].Latitude);

            var actual = fakeTicketVerifications.Last().IsVerified;

            Assert.False(actual);
        }

        [Fact]
        public void VerifyTicket_AllOk_CountShouldIncrease()
        {
            var expected = fakeTicketVerifications.Count + 1;

            ticketVerificationService.VerifyTicket(fakeTickets[0].Id, fakeTransports[0].Id, (float)fakeStations[0].Longitude, (float)fakeStations[0].Longitude);

            var actual = fakeTicketVerifications.Count;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void VerifyTicket_AllOk_IsVerifyFalse()
        {
            ticketVerificationService.VerifyTicket(fakeTickets[0].Id, fakeTransports[0].Id, (float)fakeStations[0].Longitude, (float)fakeStations[0].Longitude);

            var actual = fakeTicketVerifications.Last().IsVerified;

            Assert.True(actual);
        }

        [Fact]
        public void VerifyTicket_AllOk_HasCorrectStationId()
        {
            var expected = fakeStations[0].Id;

            ticketVerificationService.VerifyTicket(fakeTickets[0].Id, fakeTransports[0].Id, (float)fakeStations[0].Longitude, (float)fakeStations[0].Longitude);

            var actual = fakeTicketVerifications.Last().StationId;

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
