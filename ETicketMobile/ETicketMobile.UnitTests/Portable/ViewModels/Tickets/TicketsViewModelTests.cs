using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using ETicketMobile.Business.Model.Tickets;
using ETicketMobile.Business.Services.Interfaces;
using ETicketMobile.DataAccess.Services.Interfaces;
using ETicketMobile.UnitTests.Comparers;
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

        private readonly IEnumerable<TicketTypeDto> ticketTypesDto;
        private readonly IList<TicketType> ticketTypes;

        private readonly IList<AreaViewModel> areas;
        private readonly IList<AreaDto> areasDto;

        private readonly string accessToken;

        private readonly GetTicketPriceResponseDto getTicketPriceResponseDto;

        #endregion

        public TicketsViewModelTests()
        {
            localTokenServiceMock = new Mock<ILocalTokenService>();
            dialogServiceMock = new Mock<IPageDialogService>();
            ticketsServiceMock = new Mock<ITicketsService>();
            tokenServiceMock = new Mock<ITokenService>();

            accessToken = "AccessToken";

            getTicketPriceResponseDto = new GetTicketPriceResponseDto { TotalPrice = 100 };

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

            tokenServiceMock.Setup(ts => ts.RefreshTokenAsync());

            localTokenServiceMock
                    .Setup(lts => lts.GetAccessTokenAsync())
                    .ReturnsAsync(accessToken);

            ticketsServiceMock
                    .Setup(ts => ts.GetTicketTypesAsync(It.IsAny<string>()))
                    .ReturnsAsync(ticketTypes);

            ticketsServiceMock
                    .Setup(ts => ts.GetAreasDtoAsync(It.IsAny<string>()))
                    .ReturnsAsync(areasDto);

            ticketsServiceMock
                    .Setup(ts => ts.RequestGetTicketPriceAsync(It.IsAny<IEnumerable<int>>(), It.IsAny<int>()))
                    .ReturnsAsync(getTicketPriceResponseDto);

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

        [Fact]
        public void OnNavigatedTo_NullNavigationParameters_ThrowArgumentNullException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => ticketsViewModel.OnNavigatedTo(null));
        }

        [Fact]
        public void OnAppearing_Init_CompareTotalPrices_ShouldBeEqual()
        {
            // Arrange
            var totalPrice = 0;

            // Act
            ticketsViewModel.OnAppearing();

            // Assert
            Assert.Equal(totalPrice, ticketsViewModel.TotalPrice);
        }

        [Fact]
        public void OnAppearing_GetTicketTypesAsync_CompareTicketTypes_ShouldBeEqual()
        {
            // Arrange
            var ticketTypesEqualityComparer = new TicketTypesEqualityComparer();

            // Act
            ticketsViewModel.OnAppearing();

            // Assert
            Assert.Equal(ticketTypes, ticketsViewModel.Tickets, ticketTypesEqualityComparer);
        }

        [Fact]
        public void OnAppearing_GetAreasAsync_CompareAreas_ShouldBeEqual()
        {
            // Arrange
            var areasEqualityComparer = new AreasViewModelEqualityComparer();

            // Act
            ticketsViewModel.OnAppearing();

            // Assert
            Assert.Equal(areas, ticketsViewModel.Areas, areasEqualityComparer);
        }

        [Fact]
        public void TicketSelectedProperty_ZeroSelectedAreas_Return()
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
        public void TicketSelectedProperty_CountTotalPrice_ZeroSelectedArea_ShouldBeEqual()
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
        public void TicketSelectedProperty_CountTotalPrice_CompareTotalPrices_ShouldBeEqual()
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