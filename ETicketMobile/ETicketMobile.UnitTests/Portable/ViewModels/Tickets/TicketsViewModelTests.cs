using System;
using System.Collections.Generic;
using System.Linq;
using ETicketMobile.Business.Model.Tickets;
using ETicketMobile.Business.Services.Interfaces;
using ETicketMobile.DataAccess.Services.Interfaces;
using ETicketMobile.UnitTests.Comparers;
using ETicketMobile.ViewModels.Tickets;
using ETicketMobile.WebAccess.DTO;
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

        private readonly IList<TicketType> ticketTypes;

        private readonly IList<AreaViewModel> areas;
        private readonly IList<AreaDto> areasDto;

        private readonly string accessToken;

        private readonly GetTicketPriceResponseDto getTicketPriceResponseDto;

        #endregion

        public TicketsViewModelTests()
        {
            areas = new List<AreaViewModel>
            {
                new AreaViewModel
                {
                    Id = 1,
                    Name = "Area1",
                    Description = "Description1",
                    Selected = false
                },
                new AreaViewModel
                {
                    Id = 2,
                    Name = "Area2",
                    Description = "Description2",
                    Selected = false
                },
                new AreaViewModel
                {
                    Id = 3,
                    Name = "Area3",
                    Description = "Description3",
                    Selected = true
                }
            };

            accessToken = "AccessToken";

            tokenServiceMock = new Mock<ITokenService>();
            tokenServiceMock.Setup(ts => ts.RefreshTokenAsync());

            localTokenServiceMock = new Mock<ILocalTokenService>();
            localTokenServiceMock
                    .Setup(lts => lts.GetAccessTokenAsync())
                    .ReturnsAsync(accessToken);

            ticketTypes = new List<TicketType>
            {
                new TicketType
                {
                    Id = 1,
                    Name = "TickeType",
                    Coefficient = 1,
                    DurationHours = 10,
                    Amount = 100
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

            ticketsServiceMock = new Mock<ITicketsService>();
            ticketsServiceMock
                    .Setup(ts => ts.GetTicketTypesAsync(It.IsAny<string>()))
                    .ReturnsAsync(ticketTypes);

            ticketsServiceMock
                    .Setup(ts => ts.GetAreasDtoAsync(It.IsAny<string>()))
                    .ReturnsAsync(areasDto);

            getTicketPriceResponseDto = new GetTicketPriceResponseDto { TotalPrice = 100 };

            ticketsServiceMock
                    .Setup(ts => ts.RequestGetTicketPriceAsync(It.IsAny<IEnumerable<int>>(), It.IsAny<int>()))
                    .ReturnsAsync(getTicketPriceResponseDto);

            dialogServiceMock = new Mock<IPageDialogService>();

            ticketsViewModel = new TicketsViewModel(null, localTokenServiceMock.Object, dialogServiceMock.Object, ticketsServiceMock.Object, tokenServiceMock.Object);
        }

        [Fact]
        public void CheckConstructorWithParameters_CheckNullableLocalTokenService_ShouldThrowException()
        {
            // Arrange
            ILocalTokenService localTokenService = null;

            // Assert
            Assert.Throws<ArgumentNullException>(
                () => new TicketsViewModel(null, localTokenService, dialogServiceMock.Object, ticketsServiceMock.Object, tokenServiceMock.Object));
        }

        [Fact]
        public void CheckConstructorWithParameters_CheckNullableDialogService_ShouldThrowException()
        {
            // Arrange
            IPageDialogService dialogService = null;

            // Assert
            Assert.Throws<ArgumentNullException>(
                () => new TicketsViewModel(null, localTokenServiceMock.Object, dialogService, ticketsServiceMock.Object, tokenServiceMock.Object));
        }

        [Fact]
        public void CheckConstructorWithParameters_CheckNullableTokenService_ShouldThrowException()
        {
            // Arrange
            ITokenService tokenService = null;

            // Assert
            Assert.Throws<ArgumentNullException>(
                () => new TicketsViewModel(null, localTokenServiceMock.Object, dialogServiceMock.Object, ticketsServiceMock.Object, tokenService));
        }

        [Fact]
        public void CheckConstructorWithParameters_CheckNullableTicketsService_ShouldThrowException()
        {
            // Arrange
            ITicketsService ticketsService = null;

            // Assert
            Assert.Throws<ArgumentNullException>(
                () => new TicketsViewModel(null, localTokenServiceMock.Object, dialogServiceMock.Object, ticketsService, tokenServiceMock.Object));
        }

        [Fact]
        public void OnNavigatedTo_NullNavigationParameters_ThrowArgumentNullException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => ticketsViewModel.OnNavigatedTo(null));
        }

        [Fact]
        public void OnAppearing_CheckTotalPrices_ShouldBeZero()
        {
            // Arrange
            var totalPrice = 0;

            // Act
            ticketsViewModel.OnAppearing();

            // Assert
            Assert.Equal(totalPrice, ticketsViewModel.TotalPrice);
        }

        [Fact]
        public void OnAppearing_CheckTicketTypesLoading()
        {
            // Arrange
            var ticketTypesEqualityComparer = new TicketTypesEqualityComparer();

            // Act
            ticketsViewModel.OnAppearing();
            var actualTicketTypes = ticketsViewModel.Tickets;

            // Assert
            Assert.Equal(ticketTypes, actualTicketTypes, ticketTypesEqualityComparer);
        }

        [Fact]
        public void OnAppearing_CheckAreasLoading()
        {
            // Arrange
            var areasEqualityComparer = new AreasViewModelEqualityComparer();

            // Act
            ticketsViewModel.OnAppearing();

            // Assert
            Assert.Equal(areas, ticketsViewModel.Areas, areasEqualityComparer);
        }

        [Fact]
        public void TicketSelected_CheckPriceWhenNoneOfAreasSelected_TotalPriceShouldBeZero()
        {
            // Arrange
            getTicketPriceResponseDto.TotalPrice = 0;

            ticketsViewModel.Areas = areas.Where(a => a.Selected = false).ToList();

            // Act
            ticketsViewModel.TicketSelected = ticketTypes.ElementAt(0);

            // Assert
            Assert.Equal(getTicketPriceResponseDto.TotalPrice, ticketsViewModel.TotalPrice);
        }

        [Fact]
        public void TicketSelectedProperty_CheckPriceWhenNoneOfTicketsSelected_TotalPriceShouldBeZero()
        {
            // Arrange
            getTicketPriceResponseDto.TotalPrice = 0;

            ticketsViewModel.Areas = areas;
            ticketsViewModel.SelectedAreas = string.Empty;

            // Act
            ticketsViewModel.TicketSelected = ticketTypes.ElementAt(0);

            // Assert
            Assert.Equal(getTicketPriceResponseDto.TotalPrice, ticketsViewModel.TotalPrice);
        }

        [Fact]
        public void TicketSelectedProperty_CheckTotalPrice_ShouldBeEqual()
        {
            // Arrange
            ticketsViewModel.Areas = areas;
            ticketsViewModel.SelectedAreas = $"{areas}";

            // Act
            ticketsViewModel.TicketSelected = ticketTypes.ElementAt(0);

            // Assert
            Assert.Equal(getTicketPriceResponseDto.TotalPrice, ticketsViewModel.TotalPrice);
        }
    }
}