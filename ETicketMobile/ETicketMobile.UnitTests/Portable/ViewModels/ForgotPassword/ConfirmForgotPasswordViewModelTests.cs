using System;
using ETicketMobile.ViewModels.ForgotPassword;
using ETicketMobile.WebAccess.Network.WebServices.Interfaces;
using Moq;
using Prism.Navigation;
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
        public void CtorWithParameters_NullHttpService()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => new ConfirmForgotPasswordViewModel(null, dialogServiceMock.Object, null));
        }

        [Fact]
        public void CtorWithParameters_NullDialogService()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => new ConfirmForgotPasswordViewModel(null, null, httpServiceMock.Object));
        }

        [Fact]
        public void CtorWithParameters()
        {
            // Act
            var exception = Record.Exception(
                () => new ConfirmForgotPasswordViewModel(null, dialogServiceMock.Object, httpServiceMock.Object));

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public void OnAppearing_ActivationCodeTimer()
        {
            // Arrange
            var activationCodeTimer = 0;

            // Act
            confirmForgotPasswordViewModel.OnAppearing();

            // Assert
            Assert.Equal(activationCodeTimer, confirmForgotPasswordViewModel.ActivationCodeTimer);
        }

        [Fact]
        public void OnAppearing_TimerActivated()
        {
            // Arrange
            var timerActivated = false;

            // Act
            confirmForgotPasswordViewModel.OnAppearing();

            // Assert
            Assert.Equal(timerActivated, confirmForgotPasswordViewModel.TimerActivated);
        }

        [Fact]
        public void OnNavigatedTo()
        {
            // Arrange
            var email = "email";

            var navigationParameters = new NavigationParameters { { email, "email" } };

            // Act
            confirmForgotPasswordViewModel.OnNavigatedTo(navigationParameters);

            var actualEmail = navigationParameters.GetValue<string>("email");

            // Assert
            Assert.Equal(email, actualEmail);
        }
    }
}