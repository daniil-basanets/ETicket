using System;
using System.Globalization;
using System.Threading;
using ETicketMobile.Business.Services.Interfaces;
using ETicketMobile.Business.Validators.Interfaces;
using ETicketMobile.ViewModels.ForgotPassword;
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

        #endregion

        public ForgotPasswordViewModelTests()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

            emailActivationServiceMock = new Mock<IEmailActivationService>();
            dialogServiceMock = new Mock<IPageDialogService>();
            userValidatorMock = new Mock<IUserValidator>();

            forgotPasswordViewModel = new ForgotPasswordViewModel(emailActivationServiceMock.Object, null, dialogServiceMock.Object, userValidatorMock.Object);
        }

        [Fact]
        public void CheckConstructorWithParameters_CheckNullableEmailActivationService_ShouldThrowException()
        {
            // Arrange
            IEmailActivationService emailActivationService = null;

            // Assert
            Assert.Throws<ArgumentNullException>(
                () => new ForgotPasswordViewModel(emailActivationService, null, dialogServiceMock.Object, userValidatorMock.Object));
        }

        [Fact]
        public void CheckConstructorWithParameters_CheckNullableDialogService_ShouldThrowException()
        {
            // Arrange
            IPageDialogService dialogService = null;

            // Assert
            Assert.Throws<ArgumentNullException>(
                () => new ForgotPasswordViewModel(emailActivationServiceMock.Object, null, dialogService, userValidatorMock.Object));
        }

        [Fact]
        public void CheckConstructorWithParameters_CheckNullableUserValidator_ShouldThrowException()
        {
            // Arrange
            IUserValidator userValidator = null;

            // Assert
            Assert.Throws<ArgumentNullException>(
                () => new ForgotPasswordViewModel(emailActivationServiceMock.Object, null, dialogServiceMock.Object, userValidator));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void OnNavigateToConfirmForgotPasswordView_ValidThatEmailEmpty(string email)
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