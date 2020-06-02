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

        private readonly CreateNewPasswordViewModel confirmForgotPasswordViewModel;

        private readonly Mock<IHttpService> httpServiceMock;
        private readonly Mock<IPageDialogService> dialogServiceMock;

        #endregion

        public CreateNewPasswordViewModelTests()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

            httpServiceMock = new Mock<IHttpService>();
            dialogServiceMock = new Mock<IPageDialogService>();

            confirmForgotPasswordViewModel = new CreateNewPasswordViewModel(null, dialogServiceMock.Object, httpServiceMock.Object);
        }

        [Fact]
        public void CtorWithParameters_NullHttpService()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => new CreateNewPasswordViewModel(null, dialogServiceMock.Object, null));
        }

        [Fact]
        public void CtorWithParameters_NullDialogService()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => new CreateNewPasswordViewModel(null, null, httpServiceMock.Object));
        }

        [Fact]
        public void CtorWithParameters()
        {
            // Act
            var exception = Record.Exception(
                () => new CreateNewPasswordViewModel(null, dialogServiceMock.Object, httpServiceMock.Object));

            // Assert
            Assert.Null(exception);
        }

        //[Theory]
        //[InlineData("qwerty12", "qwerty12")]
        //[InlineData("1234567A", "1234567A")]
        //public void OnNavigateToSignInView_IsValid(string password, string confirmPassword)
        //{
        //    // Arrange
        //    confirmForgotPasswordViewModel.ConfirmPassword = confirmPassword;

        //    // Act
        //    confirmForgotPasswordViewModel.NavigateToSignInView.Execute(password);

        //    // Assert
        //    Assert.Null(confirmForgotPasswordViewModel.PasswordWarning);
        //    Assert.Null(confirmForgotPasswordViewModel.ConfirmPasswordWarning);
        //}

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void OnNavigateToSignInView_IsValid_IsNullOrEmpty(string password)
        {
            // Arrange
            var passwordWarning = "Enter a password";

            // Act
            confirmForgotPasswordViewModel.NavigateToSignInView.Execute(password);

            // Assert
            Assert.Equal(passwordWarning, confirmForgotPasswordViewModel.PasswordWarning);
        }

        [Theory]
        [InlineData("1")]
        [InlineData("1234567")]
        public void OnNavigateToSignInView_IsValid_IsPasswordShort(string password)
        {
            // Arrange
            var passwordWarning = "Use 8 characters or more for your password";

            // Act
            confirmForgotPasswordViewModel.NavigateToSignInView.Execute(password);

            // Assert
            Assert.Equal(passwordWarning, confirmForgotPasswordViewModel.PasswordWarning);
        }

        [Theory]
        [InlineData("asdasdasdasdasdasdasdasddasdasdasdasasdasdasd" +
                    "asdasdasdasdasddasdasdasdasasdasdasdasdasdasdasdasdaasss")]
        public void OnNavigateToSignInView_IsValid_IsPasswordLong(string password)
        {
            // Arrange
            var passwordWarning = "Use 100 characters or fewer for your password";

            // Act
            confirmForgotPasswordViewModel.NavigateToSignInView.Execute(password);

            // Assert
            Assert.Equal(passwordWarning, confirmForgotPasswordViewModel.PasswordWarning);
        }

        [Theory]
        [InlineData("12345678")]
        [InlineData("12345678123123123")]
        public void OnNavigateToSignInView_IsValid_IsPasswordWeak(string password)
        {
            // Arrange
            var passwordWarning = "Please, choose a stronger password. Try a mix of letters, numbers, symbols.";

            // Act
            confirmForgotPasswordViewModel.NavigateToSignInView.Execute(password);

            // Assert
            Assert.Equal(passwordWarning, confirmForgotPasswordViewModel.PasswordWarning);
        }

        [Theory]
        [InlineData("qwerty12", "qwerty21")]
        public void OnNavigateToSignInView_IsValid_PasswordsMatched(string password, string confirmPassword)
        {
            // Arrange
            var passwordWarning = "Please, make sure your passwords match";
            confirmForgotPasswordViewModel.ConfirmPassword = confirmPassword;

            // Act
            confirmForgotPasswordViewModel.NavigateToSignInView.Execute(password);

            // Assert
            Assert.Equal(passwordWarning, confirmForgotPasswordViewModel.ConfirmPasswordWarning);
        }
    }
}