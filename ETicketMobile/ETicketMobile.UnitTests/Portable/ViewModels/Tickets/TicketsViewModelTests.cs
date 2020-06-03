using System;
using System.Collections.Generic;
using System.Linq;
using ETicketMobile.Business.Model.Tickets;
using ETicketMobile.Business.Services.Interfaces;
using ETicketMobile.DataAccess.LocalAPI.Interfaces;
using ETicketMobile.UnitTests.Portable.Comparer;
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

        private readonly Mock<IPageDialogService> dialogServiceMock;
        private readonly Mock<ITokenService> tokenServiceMock;
        private readonly Mock<IHttpService> httpServiceMock;
        private readonly Mock<ILocalApi> localApiMock;

        #endregion

        public TicketsViewModelTests()
        {
            dialogServiceMock = new Mock<IPageDialogService>();
            tokenServiceMock = new Mock<ITokenService>();
            httpServiceMock = new Mock<IHttpService>();
            localApiMock = new Mock<ILocalApi>();

            var accessToken = "AccessToken";
            var ticketTypesDto = new List<TicketTypeDto>
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

            tokenServiceMock
                    .Setup(ts => ts.GetAccessTokenAsync())
                    .ReturnsAsync(accessToken);

            httpServiceMock
                    .Setup(hs => hs.GetAsync<IEnumerable<TicketTypeDto>>(It.IsAny<Uri>(), It.IsAny<string>()))
                    .ReturnsAsync(ticketTypesDto);

            httpServiceMock
                    .Setup(hs => hs.GetAsync<IList<AreaDto>>(It.IsAny<Uri>(), It.IsAny<string>()))
                    .ReturnsAsync(areasDto);

            ticketsViewModel = new TicketsViewModel(null, dialogServiceMock.Object, tokenServiceMock.Object, httpServiceMock.Object, localApiMock.Object);
        }

        [Fact]
        public void CtorWithParameters()
        {
            // Act
            var exception = Record.Exception(
                () => new TicketsViewModel(null, dialogServiceMock.Object, tokenServiceMock.Object, httpServiceMock.Object, localApiMock.Object));

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public void CtorWithParameters_NullDialogService()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(
                () => new TicketsViewModel(null, null, tokenServiceMock.Object, httpServiceMock.Object, localApiMock.Object));
        }

        [Fact]
        public void CtorWithParameters_NullTokenService()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(
                () => new TicketsViewModel(null, dialogServiceMock.Object, null, httpServiceMock.Object, localApiMock.Object));
        }

        [Fact]
        public void CtorWithParameters_NullHttpService()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(
                () => new TicketsViewModel(null, dialogServiceMock.Object, tokenServiceMock.Object, null, localApiMock.Object));
        }

        [Fact]
        public void CtorWithParameters_NullLocalApi()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(
                () => new TicketsViewModel(null, dialogServiceMock.Object, tokenServiceMock.Object, httpServiceMock.Object, null));
        }

        [Fact]
        public void OnAppearing_Tickets()
        {
            // Arrange
            var ticketTypesEqualityComparer = new TicketTypesEqualityComparer();

            var ticketTypes = new List<TicketType>
            {
                new TicketType
                {
                    Id = 1,
                    Name = "TickeType",
                    Coefficient = 1,
                    DurationHours = 10
                }
            };

            // Act
            ticketsViewModel.OnAppearing();

            // Assert
            Assert.Equal(ticketTypes.First(), ticketsViewModel.Tickets.First(), ticketTypesEqualityComparer);
        }

        [Fact]
        public void OnAppearing_Areas()
        {
            // Arrange
            var areasViewModelEqualityComparer = new AreasViewModelEqualityComparer();

            var areas = new List<AreaViewModel>
            {
                new AreaViewModel
                {
                    Id = 1,
                    Name = "Area",
                    Description = "Description"
                }
            };

            // Act
            ticketsViewModel.OnAppearing();

            // Assert
            Assert.Equal(areas.First(), ticketsViewModel.Areas.First(), areasViewModelEqualityComparer);
        }

        [Fact]
        public void OnNavigatedTo_RefreshTokenAsync()
        {
            httpServiceMock
                    .Setup(hs => hs.GetAsync<IEnumerable<TicketTypeDto>>(It.IsAny<Uri>(), It.IsAny<string>()))
                    .ReturnsAsync(() => null);
        }
    }
}