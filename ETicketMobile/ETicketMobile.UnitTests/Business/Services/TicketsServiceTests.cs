using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ETicketMobile.Business.Exceptions;
using ETicketMobile.Business.Model.Tickets;
using ETicketMobile.Business.Services;
using ETicketMobile.Business.Services.Interfaces;
using ETicketMobile.UnitTests.Comparers;
using ETicketMobile.WebAccess;
using ETicketMobile.WebAccess.DTO;
using ETicketMobile.WebAccess.Network.WebServices.Interfaces;
using Moq;
using Xunit;

namespace ETicketMobile.UnitTests.Business.Services
{
    public class TicketsServiceTests
    {
        #region Fields

        private readonly ITicketsService ticketsService;

        private readonly Mock<ITokenService> tokenServiceMock;
        private readonly Mock<IHttpService> httpServiceMock;

        private readonly IEnumerable<TicketDto> ticketsDto;
        private readonly IEnumerable<Ticket> tickets;

        private readonly IList<TicketTypeDto> ticketTypesDto;
        private readonly IEnumerable<TicketType> ticketTypes;

        private readonly IList<AreaDto> areasDto;

        private readonly GetTicketPriceResponseDto getTicketPriceResponseDto;

        private readonly BuyTicketRequestDto buyTicketRequestDto;
        private readonly BuyTicketResponseDto buyTicketResponseDto;

        private readonly string acessToken;

        private readonly string email;

        private readonly int ticketTypeId;
        private readonly IEnumerable<int> areasId;

        #endregion

        public TicketsServiceTests()
        {
            email = "email";

            acessToken = "AccessToken";

            ticketsDto = new List<TicketDto>
            {
                new TicketDto
                {
                    TicketType = "TicketDto",
                    ReferenceNumber = "ReferenceNumber1",
                    TicketAreas = new List<string> { "A" },
                    CreatedAt = DateTime.Parse("06/06/20 18:00:00"),
                    ActivatedAt = DateTime.Parse("06/06/20 18:01:00"),
                    ExpiredAt = DateTime.Parse("06/06/20 19:01:00")
                },
                new TicketDto
                {
                    TicketType = "TicketDto",
                    ReferenceNumber = "ReferenceNumber2",
                    TicketAreas = new List<string> { "A", "B" },
                    CreatedAt = DateTime.Parse("06/06/20 19:00:00"),
                    ActivatedAt = DateTime.Parse("06/06/20 19:02:00"),
                    ExpiredAt = DateTime.Parse("06/06/20 20:02:00")
                },
                new TicketDto
                {
                    TicketType = "TicketDto",
                    ReferenceNumber = "ReferenceNumber3",
                    TicketAreas = new List<string> { "A", "B", "C" },
                    CreatedAt = DateTime.Parse("06/06/20 20:00:00"),
                    ActivatedAt = DateTime.Parse("06/06/20 20:03:00"),
                    ExpiredAt = DateTime.Parse("06/06/20 11:03:00")
                }
            };

            tickets = new List<Ticket>
            {
                new Ticket
                {
                    TicketType = "TicketDto",
                    ReferenceNumber = "ReferenceNumber1",
                    TicketAreas = new List<string> { "A" },
                    CreatedAt = DateTime.Parse("06/06/20 18:00:00"),
                    ActivatedAt = DateTime.Parse("06/06/20 18:01:00"),
                    ExpiredAt = DateTime.Parse("06/06/20 19:01:00")
                },
                new Ticket
                {
                    TicketType = "TicketDto",
                    ReferenceNumber = "ReferenceNumber2",
                    TicketAreas = new List<string> { "A", "B" },
                    CreatedAt = DateTime.Parse("06/06/20 19:00:00"),
                    ActivatedAt = DateTime.Parse("06/06/20 19:02:00"),
                    ExpiredAt = DateTime.Parse("06/06/20 20:02:00")
                },
                new Ticket
                {
                    TicketType = "TicketDto",
                    ReferenceNumber = "ReferenceNumber3",
                    TicketAreas = new List<string> { "A", "B", "C" },
                    CreatedAt = DateTime.Parse("06/06/20 20:00:00"),
                    ActivatedAt = DateTime.Parse("06/06/20 20:03:00"),
                    ExpiredAt = DateTime.Parse("06/06/20 11:03:00")
                }
            };

            ticketTypesDto = new List<TicketTypeDto>
            {
                new TicketTypeDto
                {
                    Id = 1,
                    Name = "TickeType1",
                    Coefficient = 1,
                    DurationHours = 11
                },
                new TicketTypeDto
                {
                    Id = 2,
                    Name = "TickeType2",
                    Coefficient = 2,
                    DurationHours = 12
                },
                new TicketTypeDto
                {
                    Id = 3,
                    Name = "TickeType3",
                    Coefficient = 3,
                    DurationHours = 13
                }
            };

            ticketTypes = new List<TicketType>
            {
                new TicketType
                {
                    Id = 1,
                    Name = "TickeType1",
                    Coefficient = 1,
                    DurationHours = 11
                },
                new TicketType
                {
                    Id = 2,
                    Name = "TickeType2",
                    Coefficient = 2,
                    DurationHours = 12
                },
                new TicketType
                {
                    Id = 3,
                    Name = "TickeType3",
                    Coefficient = 3,
                    DurationHours = 13
                }
            };

            areasDto = new List<AreaDto>
            {
                new AreaDto
                {
                    Id = 1,
                    Name = "Area1",
                    Description = "Description1"
                },
                new AreaDto
                {
                    Id = 2,
                    Name = "Area2",
                    Description = "Description2"
                },
                new AreaDto
                {
                    Id = 3,
                    Name = "Area3",
                    Description = "Description3"
                }
            };

            ticketTypeId = 1;
            areasId = new List<int> { 1 };

            getTicketPriceResponseDto = new GetTicketPriceResponseDto { TotalPrice = 100 };

            buyTicketRequestDto = new BuyTicketRequestDto
            {
                TicketTypeId = 1,
                Email = "email",
                Price = 100,
                Description = "Description",
                CardNumber = "CardNumber",
                ExpirationMonth = "ExpirationMonth",
                ExpirationYear = "ExpirationYear",
                CVV2 = "CVV2"
            };

            buyTicketResponseDto = new BuyTicketResponseDto
            {
                PayResult = "PayResult",
                TotalPrice = 100,
                BoughtAt = DateTime.Parse("06/06/20 19:00:01")
            };

            tokenServiceMock = new Mock<ITokenService>();
            tokenServiceMock
                    .Setup(ts => ts.RefreshTokenAsync())
                    .ReturnsAsync(acessToken);

            httpServiceMock = new Mock<IHttpService>();
            httpServiceMock
                    .SetupSequence(hs => hs.GetAsync<IEnumerable<TicketDto>>(It.IsAny<Uri>(), It.IsAny<string>()))
                    .ReturnsAsync(ticketsDto)
                    .ThrowsAsync(new System.Net.WebException());

            httpServiceMock
                    .SetupSequence(hs => hs.GetAsync<IList<TicketTypeDto>>(It.IsAny<Uri>(), It.IsAny<string>()))
                    .ReturnsAsync(ticketTypesDto)
                    .ThrowsAsync(new System.Net.WebException());

            httpServiceMock
                    .SetupSequence(hs => hs.GetAsync<IList<AreaDto>>(It.IsAny<Uri>(), It.IsAny<string>()))
                    .ReturnsAsync(areasDto)
                    .ThrowsAsync(new System.Net.WebException());

            httpServiceMock
                    .SetupSequence(hs => hs.PostAsync<GetTicketPriceRequestDto, GetTicketPriceResponseDto>(
                        It.IsAny<Uri>(), It.IsAny<GetTicketPriceRequestDto>(), It.IsAny<string>()))
                    .ReturnsAsync(getTicketPriceResponseDto)
                    .ThrowsAsync(new System.Net.WebException());

            httpServiceMock
                    .SetupSequence(hs => hs.PostAsync<BuyTicketRequestDto, BuyTicketResponseDto>(
                        It.IsAny<Uri>(), It.IsAny<BuyTicketRequestDto>(), It.IsAny<string>()))
                    .ReturnsAsync(buyTicketResponseDto)
                    .ThrowsAsync(new System.Net.WebException());

            ticketsService = new TicketsService(tokenServiceMock.Object, httpServiceMock.Object);
        }

        [Fact]
        public void CheckConstructorWithParameters_CheckNullableTokenService_ShouldThrowException()
        {
            // Arrange
            ITokenService tokenService = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => new TicketsService(tokenService, httpServiceMock.Object));
        }

        [Fact]
        public void CheckConstructorWithParameters_CheckNullableHttpService_ShouldThrowException()
        {
            // Arrange
            IHttpService httpService = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => new TicketsService(tokenServiceMock.Object, httpService));
        }

        [Fact]
        public async Task GetTickets_CheckTickets_ShouldBeEqual()
        {
            // Arrange
            var ticketsEqualityComparer = new TicketsEqualityComparer();

            // Act
            var actualTickets = await ticketsService.GetTicketsAsync(acessToken, email);

            // Assert
            Assert.Equal(tickets, actualTickets, ticketsEqualityComparer);
        }

        [Fact]
        public async Task GetTickets_WhenSecurityTokenExpired()
        {
            // Arrange
            var ticketsEqualityComparer = new TicketsEqualityComparer();

            httpServiceMock
                    .SetupSequence(hs => hs.GetAsync<IEnumerable<TicketDto>>(It.IsAny<Uri>(), It.IsAny<string>()))
                    .ReturnsAsync(() => null)
                    .ReturnsAsync(ticketsDto);

            // Act
            var actualTickets = await ticketsService.GetTicketsAsync(acessToken, email);

            // Assert
            Assert.Equal(tickets, actualTickets, ticketsEqualityComparer);
        }

        [Fact]
        public async Task GetTickets_ShouldThrowException()
        {
            // Act
            await ticketsService.GetTicketsAsync(acessToken, email);

            // Assert
            await Assert.ThrowsAsync<WebException>(() => ticketsService.GetTicketsAsync(acessToken, email));
        }

        [Fact]
        public async Task GetTicketTypes_CheckTicketTypes_ShouldBeEqual()
        {
            // Arrange
            var ticketTypesEqualityComparer = new TicketTypesEqualityComparer();

            // Act
            var actualTicketTypes = await ticketsService.GetTicketTypesAsync(acessToken);

            // Assert
            Assert.Equal(ticketTypes, actualTicketTypes, ticketTypesEqualityComparer);
        }

        [Fact]
        public async Task GetTicketTypes_WhenSecurityTokenExpired()
        {
            // Arrange
            var ticketTypesEqualityComparer = new TicketTypesEqualityComparer();

            httpServiceMock
                    .SetupSequence(hs => hs.GetAsync<IList<TicketTypeDto>>(It.IsAny<Uri>(), It.IsAny<string>()))
                    .ReturnsAsync(() => null)
                    .ReturnsAsync(ticketTypesDto);

            // Act
            var actualTicketTypesDto = await ticketsService.GetTicketTypesAsync(acessToken);

            // Assert
            Assert.Equal(ticketTypes, actualTicketTypesDto, ticketTypesEqualityComparer);
        }

        [Fact]
        public async Task GetTicketTypes_ShouldThrowException()
        {
            // Act
            await ticketsService.GetTicketTypesAsync(acessToken);

            // Assert
            await Assert.ThrowsAsync<WebException>(() => ticketsService.GetTicketTypesAsync(acessToken));
        }

        [Fact]
        public async Task GetAreas_CheckAreas_ShouldBeEqual()
        {
            // Arrange
            var areasDtoEqualityComparer = new AreasDtoEqualityComparer();

            // Act
            var acutalAreasDto = await ticketsService.GetAreasDtoAsync(acessToken);

            // Assert
            Assert.Equal(areasDto, acutalAreasDto, areasDtoEqualityComparer);
        }

        [Fact]
        public async Task GetAreas_WhenSecurityTokenExpired()
        {
            // Arrange
            var areasDtoEqualityComparer = new AreasDtoEqualityComparer();

            httpServiceMock
                    .SetupSequence(hs => hs.GetAsync<IList<AreaDto>>(It.IsAny<Uri>(), It.IsAny<string>()))
                    .ReturnsAsync(() => null)
                    .ReturnsAsync(areasDto);

            // Act
            var actualAreasDto = await ticketsService.GetAreasDtoAsync(acessToken);

            // Assert
            Assert.Equal(areasDto, actualAreasDto, areasDtoEqualityComparer);
        }

        [Fact]
        public async Task GetAreas_ShouldThrowException()
        {
            // Act
            await ticketsService.GetAreasDtoAsync(acessToken);

            // Assert
            await Assert.ThrowsAsync<WebException>(() => ticketsService.GetAreasDtoAsync(acessToken));
        }

        [Fact]
        public async Task TryRequestGetTicketPrice_CheckTotalPrices_ShouldBeEqual()
        {
            // Arrange
            var ticketPriceEqualityComparer = new GetTicketPriceResponseDtoEqualityComparer();

            // Act
            var actualPrice = await ticketsService.RequestGetTicketPriceAsync(areasId, ticketTypeId);

            // Assert
            Assert.Equal(getTicketPriceResponseDto, actualPrice, ticketPriceEqualityComparer);
        }

        [Fact]
        public async Task TryRequestGetTicketPrice_ShouldThrowException()
        {
            // Act
            await ticketsService.RequestGetTicketPriceAsync(areasId, ticketTypeId);

            // Assert
            await Assert.ThrowsAsync<WebException>(() => ticketsService.RequestGetTicketPriceAsync(areasId, ticketTypeId));
        }

        [Fact]
        public async Task TryRequestBuyTicket_CheckBuyTicketReponses_ShouldBeEqual()
        {
            // Arrange
            var ticketReponseDtoEqualityComparer = new BuyTicketReponseDtoEqualityComparer();

            // Act
            var actualTicketResponse = await ticketsService.RequestBuyTicketAsync(buyTicketRequestDto);

            // Assert
            Assert.Equal(buyTicketResponseDto, actualTicketResponse, ticketReponseDtoEqualityComparer);
        }

        [Fact]
        public async Task TryRequestBuyTicketAsync_ShouldThrowException()
        {
            // Act
            await ticketsService.RequestBuyTicketAsync(buyTicketRequestDto);

            // Assert
            await Assert.ThrowsAsync<WebException>(() => ticketsService.RequestBuyTicketAsync(buyTicketRequestDto));
        }
    }
}