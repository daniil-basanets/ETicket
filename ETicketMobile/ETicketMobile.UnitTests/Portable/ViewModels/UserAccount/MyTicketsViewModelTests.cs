using System;
using System.Collections.Generic;
using System.Linq;
using ETicketMobile.Business.Model.Tickets;
using ETicketMobile.Business.Services.Interfaces;
using ETicketMobile.Data.Entities;
using ETicketMobile.DataAccess.Services.Interfaces;
using ETicketMobile.UnitTests.Comparers;
using ETicketMobile.ViewModels.BoughtTickets;
using Moq;
using Prism.Navigation;
using Prism.Services;
using Xunit;

namespace ETicketMobile.UnitTests.Portable.ViewModels.UserAccount
{
    public class MyTicketsViewModelTests
    {
        #region Fields

        private readonly MyTicketsViewModel myTicketsViewModel;

        private readonly TicketsEqualityComparer ticketsEqualityComparer;

        private readonly INavigationParameters navigationParameters;

        private readonly Mock<ILocalTokenService> localTokenServiceMock;
        private readonly Mock<IPageDialogService> dialogServiceMock;
        private readonly Mock<ITicketsService> ticketsServiceMock;

        private readonly IEnumerable<Ticket> tickets;
        private readonly IEnumerable<Ticket> unusedTickets;
        private readonly IEnumerable<Ticket> activatedTickets;
        private readonly IEnumerable<Ticket> expiredTickets;

        private readonly Token token;

        #endregion

        public MyTicketsViewModelTests()
        {
            dialogServiceMock = new Mock<IPageDialogService>();

            tickets = new List<Ticket>
            {
                new Ticket
                {
                    TicketType = "Unused",
                    ReferenceNumber = "ReferenceNumber1",
                    TicketAreas = new List<string> { "A" },
                    CreatedAt = DateTime.Parse("02/01/20 12:00:00")
                },
                new Ticket
                {
                    TicketType = "Activated",
                    ReferenceNumber = "ReferenceNumber2",
                    TicketAreas = new List<string> { "A", "B" },
                    CreatedAt = DateTime.Parse("02/02/20 13:00:00"),
                    ActivatedAt = DateTime.Parse("02/02/20 13:00:00"),
                    ExpiredAt = DateTime.Parse("02/07/20 14:00:00")
                },
                new Ticket
                {
                    TicketType = "Expired",
                    ReferenceNumber = "ReferenceNumber3",
                    TicketAreas = new List<string> { "A", "B", "C" },
                    CreatedAt = DateTime.Parse("02/03/20 14:00:00"),
                    ActivatedAt = DateTime.Parse("02/03/20 14:00:00"),
                    ExpiredAt = DateTime.Parse("02/06/20 15:00:00")
                }
            };

            unusedTickets = new List<Ticket> { tickets.ElementAt(0) };

            activatedTickets = new List<Ticket> { tickets.ElementAt(1) };

            expiredTickets = new List<Ticket> { tickets.ElementAt(2) };

            token = new Token
            {
                AcessJwtToken = "AccessToken",
                RefreshJwtToken = "RefreshToken"
            };

            localTokenServiceMock = new Mock<ILocalTokenService>();
            localTokenServiceMock
                    .Setup(lts => lts.GetAccessTokenAsync())
                    .ReturnsAsync(token.AcessJwtToken);

            ticketsServiceMock = new Mock<ITicketsService>();
            ticketsServiceMock
                    .Setup(ts => ts.GetTicketsAsync(It.IsAny<string>(), It.IsAny<string>()))
                    .ReturnsAsync(tickets);

            ticketsEqualityComparer = new TicketsEqualityComparer();

            var email = "email";
            navigationParameters = new NavigationParameters { { email, "email" } };

            myTicketsViewModel = new MyTicketsViewModel(null, localTokenServiceMock.Object, dialogServiceMock.Object, ticketsServiceMock.Object);
        }

        [Fact]
        public void CheckConstructorWithParameters_CheckNullableLocalTokenServiceService_ShouldThrowException()
        {
            // Arrange
            ILocalTokenService localTokenService = null;

            // Assert
            Assert.Throws<ArgumentNullException>(
                () => new MyTicketsViewModel(null, localTokenService, dialogServiceMock.Object, ticketsServiceMock.Object));
        }

        [Fact]
        public void CheckConstructorWithParameters_CheckNullableDialogService_ShouldThrowException()
        {
            // Arrange
            IPageDialogService dialogService = null;

            // Assert
            Assert.Throws<ArgumentNullException>(
                () => new MyTicketsViewModel(null, localTokenServiceMock.Object, dialogService, ticketsServiceMock.Object));
        }

        [Fact]
        public void CheckConstructorWithParameters_CheckNullableTicketsService_ShouldThrowException()
        {
            // Arrange
            ITicketsService ticketsService = null;

            // Assert
            Assert.Throws<ArgumentNullException>(
                () => new MyTicketsViewModel(null, localTokenServiceMock.Object, dialogServiceMock.Object, ticketsService));
        }

        [Fact]
        public void OnNavigatedTo_CheckUnusedTickets_ShouldBeEqual()
        {
            // Act
            myTicketsViewModel.OnNavigatedTo(navigationParameters);

            // Assert
            Assert.Equal(unusedTickets, myTicketsViewModel.UnusedTickets, ticketsEqualityComparer);
        }

        [Fact]
        public void OnNavigatedTo_CheckActivatedTickets_ShouldBeEqual()
        {
            // Act
            myTicketsViewModel.OnNavigatedTo(navigationParameters);

            // Assert
            Assert.Equal(activatedTickets, myTicketsViewModel.ActivatedTickets, ticketsEqualityComparer);
        }

        [Fact]
        public void OnNavigatedTo_CheckExpiredTickets_ShouldBeEqual()
        {
            // Act
            myTicketsViewModel.OnNavigatedTo(navigationParameters);

            // Assert
            Assert.Equal(expiredTickets, myTicketsViewModel.ExpiredTickets, ticketsEqualityComparer);
        }
    }
}