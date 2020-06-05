using System;
using ETicketMobile.ViewModels.ForgotPassword;
using ETicketMobile.WebAccess.Network.WebServices.Interfaces;
using Moq;
using Prism.Services;
using Xunit;

namespace ETicketMobile.UnitTests.Portable.ViewModels.ForgotPassword
{
    public class ConfirmForgotPasswordViewModelTests
    {
        #region Fields

        private readonly ConfirmForgotPasswordViewModel confirmForgotPasswordViewModel;

        private readonly Mock<IHttpService> httpServiceMock;
        private readonly Mock<IPageDialogService> dialogServiceMock;

        #endregion

        public ConfirmForgotPasswordViewModelTests()
        {
            httpServiceMock = new Mock<IHttpService>();
            dialogServiceMock = new Mock<IPageDialogService>();

            confirmForgotPasswordViewModel = new ConfirmForgotPasswordViewModel(null, dialogServiceMock.Object, httpServiceMock.Object);
        }

        [Fact]
        public void CheckConstructorWithParameters_CheckNullableHttpService_ShouldThrowException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => new ConfirmForgotPasswordViewModel(null, dialogServiceMock.Object, null));
        }

        [Fact]
        public void CheckConstructorWithParameters_CheckNullableDialogService_ShouldThrowException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => new ConfirmForgotPasswordViewModel(null, null, httpServiceMock.Object));
        }

        [Fact]
        public void OnAppearing_CompareActivationCodeTimers_ShouldBeEqual()
        {
            // Arrange
            var activationCodeTimer = 0;

            // Act
            confirmForgotPasswordViewModel.OnAppearing();

            // Assert
            Assert.Equal(activationCodeTimer, confirmForgotPasswordViewModel.ActivationCodeTimer);
        }

        [Fact]
        public void OnAppearing_CompareTimersActivated_ShouldBeEqual()
        {
            // Arrange
            var timerActivated = false;

            // Act
            confirmForgotPasswordViewModel.OnAppearing();

            // Assert
            Assert.Equal(timerActivated, confirmForgotPasswordViewModel.TimerActivated);
        }

        [Fact]
        public void OnNavigatedTo_CheckNullableNavigationParameters_ShouldThrowException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => confirmForgotPasswordViewModel.OnNavigatedTo(null));
        }
    }
}