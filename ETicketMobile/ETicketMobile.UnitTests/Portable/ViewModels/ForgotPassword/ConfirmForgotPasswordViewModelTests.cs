using System;
using ETicketMobile.Business.Services.Interfaces;
using ETicketMobile.ViewModels.ForgotPassword;
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

        #endregion

        public ConfirmForgotPasswordViewModelTests()
        {
            emailActivationServiceMock = new Mock<IEmailActivationService>();
            dialogServiceMock = new Mock<IPageDialogService>();

            confirmForgotPasswordViewModel = new ConfirmForgotPasswordViewModel(emailActivationServiceMock.Object, null, dialogServiceMock.Object);
        }

        [Fact]
        public void CheckConstructorWithParameters_CheckNullableEmailActivationService_ShouldThrowException()
        {
            // Arrange
            IEmailActivationService emailActivationService = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => new ConfirmForgotPasswordViewModel(emailActivationService, null, dialogServiceMock.Object));
        }

        [Fact]
        public void CheckConstructorWithParameters_CheckNullableDialogService_ShouldThrowException()
        {
            // Arrange
            IPageDialogService dialogService = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => new ConfirmForgotPasswordViewModel(emailActivationServiceMock.Object, null, dialogService));
        }

        [Fact]
        public void OnAppearing_CheckActivationCodeTimers_ShouldBeZero()
        {
            // Arrange
            var activationCodeTimer = 0;

            // Act
            confirmForgotPasswordViewModel.OnAppearing();

            // Assert
            Assert.Equal(activationCodeTimer, confirmForgotPasswordViewModel.ActivationCodeTimer);
        }

        [Fact]
        public void OnAppearing_CheckTimersActivated_ShouldBefalse()
        {
            // Act
            confirmForgotPasswordViewModel.OnAppearing();

            // Assert
            Assert.False(confirmForgotPasswordViewModel.TimerActivated);
        }

        [Fact]
        public void OnNavigatedTo_CheckNullableNavigationParameters_ShouldThrowException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => confirmForgotPasswordViewModel.OnNavigatedTo(null));
        }
    }
}