using System;
using System.Globalization;
using System.Threading;
using ETicketMobile.Business.Services.Interfaces;
using ETicketMobile.ViewModels.ForgotPassword;
using ETicketMobile.WebAccess.Network.WebServices.Interfaces;
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
        private readonly Mock<IHttpService> httpServiceMock;

        #endregion

        public CreateNewPasswordViewModelTests()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

            dialogServiceMock = new Mock<IPageDialogService>();
            userServiceMock = new Mock<IUserService>();
            httpServiceMock = new Mock<IHttpService>();

            createNewPasswordViewModel = new CreateNewPasswordViewModel(null, dialogServiceMock.Object, userServiceMock.Object);
        }

        [Fact]
        public void CheckConstructorWithParameters_CheckNullableNullDialogService_ShouldThrowException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => new CreateNewPasswordViewModel(null, null, userServiceMock.Object));
        }

        [Fact]
        public void CheckConstructorWithParameters_CheckNullableNullUserService_ShouldThrowException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => new CreateNewPasswordViewModel(null, dialogServiceMock.Object, null));
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
        public void OnNavigateToSignInView_CheckIfIsValid_CheckIfIsNullOrEmpty_ReturnsFalse(string password)
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
        public void OnNavigateToSignInView_CheckIfIsValid_CheckIfPasswordsMatched_ReturnsFalse(string password, string confirmPassword)
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