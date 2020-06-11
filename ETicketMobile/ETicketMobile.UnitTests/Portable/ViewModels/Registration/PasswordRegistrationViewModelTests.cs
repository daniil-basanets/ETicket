using System.Globalization;
using System.Threading;
using ETicketMobile.ViewModels.Registration;
using Moq;
using Prism.Navigation;
using Xunit;

namespace ETicketMobile.UnitTests.Portable.ViewModels.Registration
{
    public class PasswordRegistrationViewModelTests
    {
        #region Fiels

        private readonly PasswordRegistrationViewModel passwordRegistrationViewModel;

        private readonly Mock<INavigationParameters> navigationParametersMock;
        private readonly Mock<INavigationService> navigationServiceMock;

        #endregion

        public PasswordRegistrationViewModelTests()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

            navigationServiceMock = new Mock<INavigationService>();

            navigationParametersMock = new Mock<INavigationParameters>();
            navigationParametersMock.Setup(np => np.Add(It.IsAny<string>(), It.IsAny<object>()));

            passwordRegistrationViewModel = new PasswordRegistrationViewModel(navigationServiceMock.Object);
        }

        [Fact]
        public void NavigateToBirthDateRegistrationView_VerifyNavigationParameters()
        {
            // Arrange
            var password = "qwerty12";
            passwordRegistrationViewModel.ConfirmPassword = "qwerty12";

            passwordRegistrationViewModel.OnNavigatedTo(navigationParametersMock.Object);

            // Act
            passwordRegistrationViewModel.NavigateToBirthDateRegistrationView.Execute(password);

            // Assert
            navigationParametersMock.Verify();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void NavigateToBirthDateRegistrationView_ValidThatPasswordEmpty(string password)
        {
            // Arrange
            var passwordWarning = "Enter a password";

            // Act
            passwordRegistrationViewModel.NavigateToBirthDateRegistrationView.Execute(password);

            // Assert
            Assert.Equal(passwordWarning, passwordRegistrationViewModel.PasswordWarning);
        }

        [Theory]
        [InlineData("1")]
        [InlineData("1234567")]
        [InlineData("abcdefg")]
        public void NavigateToBirthDateRegistrationView_ValidThatPasswordShort(string password)
        {
            // Arrange
            var passwordWarning = "Use 8 characters or more for your password";

            // Act
            passwordRegistrationViewModel.NavigateToBirthDateRegistrationView.Execute(password);

            // Assert
            Assert.Equal(passwordWarning, passwordRegistrationViewModel.PasswordWarning);
        }

        [Fact]
        public void NavigateToBirthDateRegistrationView_ValidThatPasswordLong()
        {
            // Arrange
            var password = "asdasdasdasdasdasdasdasddasdasdasdasasdasdasds1345" +
                           "asdasdasdasdasddasdasdasdasasdasdasdasdasdasdasdasd";

            var passwordWarning = "Use 100 characters or fewer for your password";

            // Act
            passwordRegistrationViewModel.NavigateToBirthDateRegistrationView.Execute(password);

            // Assert
            Assert.Equal(passwordWarning, passwordRegistrationViewModel.PasswordWarning);
        }

        [Theory]
        [InlineData("12345678")]
        [InlineData("12311231231223")]
        public void NavigateToBirthDateRegistrationView_ValidThatPasswordIsNotEnoughStrong(string password)
        {
            // Arrange
            var passwordWarning = "Please, choose a stronger password. Try a mix of letters, numbers, symbols.";

            // Act
            passwordRegistrationViewModel.NavigateToBirthDateRegistrationView.Execute(password);

            // Assert
            Assert.Equal(passwordWarning, passwordRegistrationViewModel.PasswordWarning);
        }

        [Theory]
        [InlineData("qwerty12", "qwerty21")]
        [InlineData("1234567A", "A1234567")]
        public void NavigateToBirthDateRegistrationView_CheckConfirmPasswordsValidation_DoesntMatch(string password, string confirmPassword)
        {
            // Arrange
            passwordRegistrationViewModel.ConfirmPassword = confirmPassword;
            var passwordWarning = "Please, make sure your passwords match";

            // Act
            passwordRegistrationViewModel.NavigateToBirthDateRegistrationView.Execute(password);

            // Assert
            Assert.Equal(passwordWarning, passwordRegistrationViewModel.ConfirmPasswordWarning);
        }
    }
}