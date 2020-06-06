using System;
using System.Collections.Generic;
using ETicketMobile.Business.Model.Tickets;
using ETicketMobile.Business.Services.Interfaces;
using ETicketMobile.DataAccess.Services.Interfaces;
using ETicketMobile.ViewModels.Tickets;
using ETicketMobile.WebAccess;
using ETicketMobile.WebAccess.DTO;
using ETicketMobile.WebAccess.Network.WebServices.Interfaces;
using Moq;
using Prism.Services;
using Xunit;

namespace ETicketMobile.UnitTests.Portable.ViewModels.Tickets
{
    public class TicketsViewModelTests
    {
        #region Fields

        private readonly TicketsViewModel ticketsViewModel;

        private readonly Mock<ILocalTokenService> localTokenServiceMock;
        private readonly Mock<IPageDialogService> dialogServiceMock;
        private readonly Mock<ITicketsService> ticketsServiceMock;
        private readonly Mock<ITokenService> tokenServiceMock;
        private readonly Mock<IHttpService> httpServiceMock;

        private readonly IEnumerable<TicketTypeDto> ticketTypesDto;

        private readonly IEnumerable<TicketType> ticketTypes;
        private readonly IEnumerable<AreaViewModel> areas;

        #endregion

        public TicketsViewModelTests()
        {
            localTokenServiceMock = new Mock<ILocalTokenService>();
            dialogServiceMock = new Mock<IPageDialogService>();
            ticketsServiceMock = new Mock<ITicketsService>();
            tokenServiceMock = new Mock<ITokenService>();
            httpServiceMock = new Mock<IHttpService>();

            var accessToken = "AccessToken";

            ticketTypesDto = new List<TicketTypeDto>
            {
                new TicketTypeDto
                {
                    Id = 1,
                    Name = "TickeType",
                    Coefficient = 1,
                    DurationHours = 10
                }
            };

            var areasDto = new List<AreaDto>
            {
                new AreaDto
                {
                    Id = 1,
                    Name = "Area",
                    Description = "Description"
                }
            };

            areas = new List<AreaViewModel>
            {
                new AreaViewModel
                {
                    Id = 1,
                    Name = "Area",
                    Description = "Description"
                }
            };

            ticketTypes = new List<TicketType>
            {
                new TicketType
                {
                    Id = 1,
                    Name = "TickeType",
                    Coefficient = 1,
                    DurationHours = 10
                }
            };

            tokenServiceMock.Setup(ts => ts.RefreshTokenAsync());

            localTokenServiceMock
                    .Setup(lts => lts.GetAccessTokenAsync())
                    .ReturnsAsync(accessToken);

            httpServiceMock
                    .Setup(hs => hs.GetAsync<IEnumerable<TicketTypeDto>>(It.IsAny<Uri>(), It.IsAny<string>()))
                    .ReturnsAsync(ticketTypesDto);

            httpServiceMock
                    .Setup(hs => hs.GetAsync<IList<AreaDto>>(It.IsAny<Uri>(), It.IsAny<string>()))
                    .ReturnsAsync(areasDto);

            ticketsViewModel = new TicketsViewModel(null, localTokenServiceMock.Object, dialogServiceMock.Object, ticketsServiceMock.Object, tokenServiceMock.Object);
        }

        [Fact]
        public void CheckConstructorWithParameters_CheckNullableLocalTokenService_ShouldThrowException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(
                () => new TicketsViewModel(null, null, dialogServiceMock.Object, ticketsServiceMock.Object, tokenServiceMock.Object));
        }

        [Fact]
        public void CheckConstructorWithParameters_CheckNullableDialogService_ShouldThrowException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(
                () => new TicketsViewModel(null, localTokenServiceMock.Object, null, ticketsServiceMock.Object, tokenServiceMock.Object));
        }

        [Fact]
        public void CheckConstructorWithParameters_CheckNullableTokenService_ShouldThrowException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(
                () => new TicketsViewModel(null, localTokenServiceMock.Object, dialogServiceMock.Object, ticketsServiceMock.Object, null));
        }

        [Fact]
        public void CheckConstructorWithParameters_CheckNullableTicketsService_ShouldThrowException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(
                () => new TicketsViewModel(null, localTokenServiceMock.Object, dialogServiceMock.Object, null, tokenServiceMock.Object));
        }

        //[Fact]
        //public void OnAppearing_CompareTickets_ShouldBeEqual()
        //{
        //    // Arrange
        //    var ticketTypesEqualityComparer = new TicketTypesEqualityComparer();

        //    // Act
        //    ticketsViewModel.OnAppearing();

        //    // Assert
        //    Assert.Equal(ticketTypes, ticketsViewModel.Tickets, ticketTypesEqualityComparer);
        //}

        //[Fact]
        //public void OnAppearing_CompareAreas_ShouldBeEqual()
        //{
        //    // Arrange
        //    var areasViewModelEqualityComparer = new AreasViewModelEqualityComparer();

        //    // Act
        //    ticketsViewModel.OnAppearing();

        //    // Assert
        //    Assert.Equal(areas, ticketsViewModel.Areas, areasViewModelEqualityComparer);
        //}

        //[Fact]
        //public void OnNavigatedTo_Tickets_CheckRefreshTokenAsyncWhenAccessTokenShouldBeNull()
        //{
        //    // Arrange
        //    httpServiceMock
        //            .Setup(hs => hs.GetAsync<IEnumerable<TicketTypeDto>>(It.IsAny<Uri>(), It.IsAny<string>()))
        //            .ReturnsAsync(() => null);

        //    // Act
        //    ticketsViewModel.OnAppearing();

        //    // Assert
        //    httpServiceMock.Verify(hs => hs.GetAsync<IEnumerable<TicketTypeDto>>(It.IsAny<Uri>(), It.IsAny<string>()), Times.Exactly(2));
        //}

        [Fact]
        public void OnNavigatedTo_NullNavigationParameters_ThrowArgumentNullException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => ticketsViewModel.OnNavigatedTo(null));
        }
    }
}