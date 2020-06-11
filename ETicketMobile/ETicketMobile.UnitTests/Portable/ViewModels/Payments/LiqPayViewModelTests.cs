using System;
using ETicketMobile.Business.Services.Interfaces;
using ETicketMobile.ViewModels.Payment;
using ETicketMobile.WebAccess.DTO;
using Moq;
using Prism.Navigation;
using Prism.Services;
using Xunit;

namespace ETicketMobile.UnitTests.Portable.ViewModels.Payments
{
    public class LiqPayViewModelTests
    {
        #region Fields

        private readonly LiqPayViewModel liqPayViewModel;

        private readonly Mock<INavigationService> navigationServiceMock;
        private readonly Mock<IPageDialogService> dialogServiceMock;
        private readonly Mock<ITicketsService> ticketsServiceMock;

        private readonly BuyTicketResponseDto buyTicketResponseDto;

        private readonly string cardNumber;
        private readonly string expirationDate;
        private readonly string cvv2;

        #endregion

        public LiqPayViewModelTests()
        {
            navigationServiceMock = new Mock<INavigationService>();
            dialogServiceMock = new Mock<IPageDialogService>();

            cardNumber = "1234 5678 1234 5678";
            expirationDate = "12/34";
            cvv2 = "123";

            buyTicketResponseDto = new BuyTicketResponseDto();

            ticketsServiceMock = new Mock<ITicketsService>();
            ticketsServiceMock
                    .Setup(ts => ts.RequestBuyTicketAsync(It.IsAny<BuyTicketRequestDto>()))
                    .ReturnsAsync(buyTicketResponseDto);

            liqPayViewModel = new LiqPayViewModel(navigationServiceMock.Object, dialogServiceMock.Object, ticketsServiceMock.Object);
        }

        [Fact]
        public void CheckConstructorWithParameters_CheckNullableDialogService_ShouldThrowException()
        {
            // Arrange
            IPageDialogService dialogService = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => new LiqPayViewModel(navigationServiceMock.Object, dialogService, ticketsServiceMock.Object));
        }

        [Fact]
        public void CheckConstructorWithParameters_CheckNullableTicketsService_ShouldThrowException()
        {
            // Arrange
            ITicketsService ticketsService = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => new LiqPayViewModel(navigationServiceMock.Object, dialogServiceMock.Object, ticketsService));
        }

        [Fact]
        public void OnAppearing_CheckExpirationDate_ShouldBeEmpty()
        {
            // Act
            liqPayViewModel.OnAppearing();

            // Assert
            Assert.Empty(liqPayViewModel.ExpirationDate);
        }

        [Fact]
        public void OnAppearing_CheckCVV2_ShouldBeEmpty()
        {
            // Act
            liqPayViewModel.OnAppearing();

            // Assert
            Assert.Empty(liqPayViewModel.CVV2);
        }

        [Fact]
        public void Pay_ValidThatHasCardNumberCorrectLength()
        {
            // Arrange
            liqPayViewModel.CardNumber = "1234";

            // Act
            liqPayViewModel.Pay.Execute(null);

            // Assert
            Assert.True(liqPayViewModel.CardNumberWarningIsVisible);
        }

        [Fact]
        public void Pay_ValidThatHasExpirationDateCorrectLength()
        {
            // Arrange
            liqPayViewModel.CardNumber = cardNumber;
            liqPayViewModel.ExpirationDate = "12";

            // Act
            liqPayViewModel.Pay.Execute(null);

            // Assert
            Assert.True(liqPayViewModel.ExpirationDateWarningIsVisible);
        }

        [Fact]
        public void Pay_ValidThatHasCVV2CorrectLength()
        {
            // Arrange
            liqPayViewModel.CardNumber = cardNumber;
            liqPayViewModel.ExpirationDate = expirationDate;
            liqPayViewModel.CVV2 = "12";

            // Act
            liqPayViewModel.Pay.Execute(null);

            // Assert
            Assert.True(liqPayViewModel.CVV2WarningIsVisible);
        }

        [Fact]
        public void Pay_VerifyRequestBuyTicket()
        {
            // Arrange
            liqPayViewModel.CardNumber = cardNumber;
            liqPayViewModel.ExpirationDate = expirationDate;
            liqPayViewModel.CVV2 = cvv2;

            // Act
            liqPayViewModel.Pay.Execute(null);

            // Assert
            ticketsServiceMock.Verify(ts => ts.RequestBuyTicketAsync(It.IsAny<BuyTicketRequestDto>()));
        }
    }
}