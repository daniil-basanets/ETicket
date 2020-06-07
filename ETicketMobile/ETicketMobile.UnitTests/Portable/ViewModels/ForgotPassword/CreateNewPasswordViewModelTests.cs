using System;
using System.Globalization;
using System.Threading;
using ETicketMobile.Business.Services.Interfaces;
using ETicketMobile.ViewModels.ForgotPassword;
using Moq;
using Prism.Services;
using Xunit;

namespace ETicketMobile.UnitTests.Portable.ViewModels.ForgotPassword
{
    public class CreateNewPasswordViewModelTests
    {
        #region Fields

        private readonly CreateNewPasswordViewModel createNewPasswordViewModel;

        private readonly Mock<IPageDialogService> dialogServiceMock;
        private readonly Mock<IUserService> userServiceMock;

        #endregion

        public CreateNewPasswordViewModelTests()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

            dialogServiceMock = new Mock<IPageDialogService>();
            userServiceMock = new Mock<IUserService>();

            createNewPasswordViewModel = new CreateNewPasswordViewModel(null, dialogServiceMock.Object, userServiceMock.Object);
        }

        [Fact]
        public void CheckConstructorWithParameters_CheckNullableNullDialogService_ShouldThrowException()
        {
            // Arrange
            IPageDialogService dialogService = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => new CreateNewPasswordViewModel(null, dialogService, userServiceMock.Object));
        }

        [Fact]
        public void CheckConstructorWithParameters_CheckNullableNullUserService_ShouldThrowException()
        {
            // Arrange
            IUserService userService = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => new CreateNewPasswordViewModel(null, dialogServiceMock.Object, userService));
        }

        [Fact]
        public void OnNavigatedTo_CheckNullableNavigationParameters_ShouldThrowException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => createNewPasswordViewModel.OnNavigatedTo(null));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void OnNavigateToSignInView_ValidThatEmptyPassword(string password)
        {
            // Arrange
            var passwordWarning = "Enter a password";

            // Act
            createNewPasswordViewModel.NavigateToSignInView.Execute(password);

            // Assert
            Assert.Equal(passwordWarning, createNewPasswordViewModel.PasswordWarning);
        }

        [Theory]
        [InlineData("qwerty12", "qwerty21")]
        public void OnNavigateToSignInView_CheckConfirmPasswordsValidation_DoesntMatch(string password, string confirmPassword)
        {
            // Arrange
            var passwordWarning = "Please, make sure your passwords match";
            createNewPasswordViewModel.ConfirmPassword = confirmPassword;

            // Act
            createNewPasswordViewModel.NavigateToSignInView.Execute(password);

            // Assert
            Assert.Equal(passwordWarning, createNewPasswordViewModel.ConfirmPasswordWarning);
        }
    }
}