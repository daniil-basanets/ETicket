using System;
using System.Globalization;
using System.Threading;
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

        private readonly Mock<IHttpService> httpServiceMock;
        private readonly Mock<IPageDialogService> dialogServiceMock;

        #endregion

        public CreateNewPasswordViewModelTests()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

            httpServiceMock = new Mock<IHttpService>();
            dialogServiceMock = new Mock<IPageDialogService>();

            createNewPasswordViewModel = new CreateNewPasswordViewModel(null, dialogServiceMock.Object, httpServiceMock.Object);
        }

        [Fact]
        public void CheckConstructorWithParameters_CheckNullableNullHttpService_ShouldThrowException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => new CreateNewPasswordViewModel(null, dialogServiceMock.Object, null));
        }

        [Fact]
        public void CheckConstructorWithParameters_CheckNullableNullDialogService_ShouldThrowException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => new CreateNewPasswordViewModel(null, null, httpServiceMock.Object));
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
        [InlineData("1")]
        [InlineData("1234567")]
        public void OnNavigateToSignInView_CheckIfIsValid_CheckIfIsPasswordShort_ReturnsFalse(string password)
        {
            // Arrange
            var passwordWarning = "Use 8 characters or more for your password";

            // Act
            createNewPasswordViewModel.NavigateToSignInView.Execute(password);

            // Assert
            Assert.Equal(passwordWarning, createNewPasswordViewModel.PasswordWarning);
        }

        [Theory]
        [InlineData("asdasdasdasdasdasdasdasddasdasdasdasasdasdasd" +
                    "asdasdasdasdasddasdasdasdasasdasdasdasdasdasdasdasdaasss")]
        public void OnNavigateToSignInView_CheckIfIsValid_CheckIfIsPasswordLong_ReturnsFalse(string password)
        {
            // Arrange
            var passwordWarning = "Use 100 characters or fewer for your password";

            // Act
            createNewPasswordViewModel.NavigateToSignInView.Execute(password);

            // Assert
            Assert.Equal(passwordWarning, createNewPasswordViewModel.PasswordWarning);
        }

        [Theory]
        [InlineData("12345678")]
        [InlineData("12345678123123123")]
        public void OnNavigateToSignInView_CheckIfIsValid_CheckIfIsPasswordWeak_ReturnsFalse(string password)
        {
            // Arrange
            var passwordWarning = "Please, choose a stronger password. Try a mix of letters, numbers, symbols.";

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