using System;
using System.Globalization;
using System.Threading;
using ETicketMobile.Business.Services.Interfaces;
using ETicketMobile.DataAccess.Services.Interfaces;
using ETicketMobile.ViewModels.Login;
using Moq;
using Prism.Navigation;
using Prism.Services;
using Xunit;

namespace ETicketMobile.UnitTests.Portable.ViewModels.Login
{
    public class LoginViewModelTests
    {
        #region Fields

        private readonly LoginViewModel loginViewModel;

        private readonly Mock<INavigationService> navigationServiceMock;
        private readonly Mock<ILocalTokenService> localTokenServiceMock;
        private readonly Mock<IPageDialogService> dialogServiceMock;
        private readonly Mock<ITokenService> tokenServiceMock;

        #endregion

        public LoginViewModelTests()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

            navigationServiceMock = new Mock<INavigationService>();
            localTokenServiceMock = new Mock<ILocalTokenService>();
            dialogServiceMock = new Mock<IPageDialogService>();
            tokenServiceMock = new Mock<ITokenService>();

            loginViewModel = new LoginViewModel(navigationServiceMock.Object, localTokenServiceMock.Object,
                dialogServiceMock.Object, tokenServiceMock.Object);
        }

        [Fact]
        public void CheckConstructorWithParameters_CheckNullableLocalTokenService_ShouldThrowException()
        {
            // Arrange
            ILocalTokenService localTokenService = null;

            // Assert
            Assert.Throws<ArgumentNullException>(
                () => new LoginViewModel(navigationServiceMock.Object, localTokenService, dialogServiceMock.Object, tokenServiceMock.Object));
        }

        [Fact]
        public void CheckConstructorWithParameters_CheckNullableDialogService_ShouldThrowException()
        {
            // Arrange
            IPageDialogService dialogService = null;

            // Assert
            Assert.Throws<ArgumentNullException>(
                () => new LoginViewModel(navigationServiceMock.Object, localTokenServiceMock.Object, dialogService, tokenServiceMock.Object));
        }

        [Fact]
        public void CheckConstructorWithParameters_CheckNullableTokenService_ShouldThrowException()
        {
            // Arrange
            ITokenService tokenService = null;

            // Assert
            Assert.Throws<ArgumentNullException>(
                () => new LoginViewModel(navigationServiceMock.Object, localTokenServiceMock.Object, dialogServiceMock.Object, tokenService));
        }

        [Fact]
        public void OnAppearing_CheckPasswordWatermarks_ShouldBeEqual()
        {
            // Arrange
            var passwordWatermark = "Password";

            // Act
            loginViewModel.OnAppearing();

            // Assert
            Assert.Equal(passwordWatermark, loginViewModel.PasswordWatermark);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void NavigateToLoginView_ValidThatEmptyEmail(string email)
        {
            // Arrange
            var emailWarning = "Enter an email";

            // Act
            loginViewModel.NavigateToLoginView.Execute(email);

            // Assert
            Assert.Equal(emailWarning, loginViewModel.EmailWarning);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void NavigateToLoginView_ValidThatEmptyPassword(string password)
        {
            // Arrange
            var email = "email";

            loginViewModel.Password = password;
            var passwordWarning = "Enter a password";

            // Act
            loginViewModel.NavigateToLoginView.Execute(email);

            // Assert
            Assert.Equal(passwordWarning, loginViewModel.PasswordWatermark);
        }
    }
}