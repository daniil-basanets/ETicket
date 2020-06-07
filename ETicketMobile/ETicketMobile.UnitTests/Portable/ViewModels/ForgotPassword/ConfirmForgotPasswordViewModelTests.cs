using System;
using ETicketMobile.Business.Services.Interfaces;
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

        private readonly Mock<IEmailActivationService> emailActivationServiceMock;
        private readonly Mock<IPageDialogService> dialogServiceMock;
        private readonly Mock<IHttpService> httpServiceMock;

        #endregion

        public ConfirmForgotPasswordViewModelTests()
        {
            emailActivationServiceMock = new Mock<IEmailActivationService>();
            dialogServiceMock = new Mock<IPageDialogService>();
            httpServiceMock = new Mock<IHttpService>();

            confirmForgotPasswordViewModel = new ConfirmForgotPasswordViewModel(emailActivationServiceMock.Object, null, dialogServiceMock.Object);
        }

        [Fact]
        public void CheckConstructorWithParameters_CheckNullableEmailActivationService_ShouldThrowException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => new ConfirmForgotPasswordViewModel(null, null, dialogServiceMock.Object));
        }

        [Fact]
        public void CheckConstructorWithParameters_CheckNullableDialogService_ShouldThrowException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => new ConfirmForgotPasswordViewModel(emailActivationServiceMock.Object, null, null));
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