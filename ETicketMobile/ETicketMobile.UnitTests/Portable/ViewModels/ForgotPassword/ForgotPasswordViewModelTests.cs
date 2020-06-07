using System;
using System.Globalization;
using System.Threading;
using ETicketMobile.Business.Services.Interfaces;
using ETicketMobile.Business.Validators.Interfaces;
using ETicketMobile.ViewModels.ForgotPassword;
using ETicketMobile.WebAccess.Network.WebServices.Interfaces;
using Moq;
using Prism.Services;
using Xunit;

namespace ETicketMobile.UnitTests.Portable.ViewModels.ForgotPassword
{
    public class ForgotPasswordViewModelTests
    {
        #region Fields

        private readonly ForgotPasswordViewModel forgotPasswordViewModel;

        private readonly Mock<IEmailActivationService> emailActivationServiceMock;
        private readonly Mock<IPageDialogService> dialogServiceMock;
        private readonly Mock<IUserValidator> userValidatorMock;
        private readonly Mock<IHttpService> httpServiceMock;

        #endregion

        public ForgotPasswordViewModelTests()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

            emailActivationServiceMock = new Mock<IEmailActivationService>();
            dialogServiceMock = new Mock<IPageDialogService>();
            userValidatorMock = new Mock<IUserValidator>();
            httpServiceMock = new Mock<IHttpService>();

            forgotPasswordViewModel = new ForgotPasswordViewModel(emailActivationServiceMock.Object, null, dialogServiceMock.Object, userValidatorMock.Object);
        }

        [Fact]
        public void CheckConstructorWithParameters_CheckNullableEmailActivationService_ShouldThrowException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(
                () => new ForgotPasswordViewModel(null, null, dialogServiceMock.Object, userValidatorMock.Object));
        }

        [Fact]
        public void CheckConstructorWithParameters_CheckNullableDialogService_ShouldThrowException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(
                () => new ForgotPasswordViewModel(emailActivationServiceMock.Object, null, null, userValidatorMock.Object));
        }

        [Fact]
        public void CheckConstructorWithParameters_CheckNullableUserValidator_ShouldThrowException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(
                () => new ForgotPasswordViewModel(emailActivationServiceMock.Object, null, dialogServiceMock.Object, null));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void OnNavigateToConfirmForgotPasswordView_CheckIfIsValid_CheckIfIsNullOrEmpty_ReturnsFalse(string email)
        {
            // Arrange
            var emailCorrect = "Are you sure you entered your email correctly?";

            // Act
            forgotPasswordViewModel.NavigateToConfirmForgotPasswordView.Execute(email);

            // Assert
            Assert.Equal(emailCorrect, forgotPasswordViewModel.EmailWarning);
        }
    }
}