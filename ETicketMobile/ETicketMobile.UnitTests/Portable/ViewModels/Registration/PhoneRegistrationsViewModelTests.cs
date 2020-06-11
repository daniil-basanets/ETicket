using System.Globalization;
using System.Threading;
using ETicketMobile.ViewModels.Registration;
using Moq;
using Prism.Navigation;
using Xunit;

namespace ETicketMobile.UnitTests.Portable.ViewModels.Registration
{
    public class PhoneRegistrationsViewModelTests
    {
        #region Fiels

        private readonly PhoneRegistrationViewModel phoneRegistrationViewModel;

        private readonly Mock<INavigationParameters> navigationParametersMock;

        private readonly string phoneWarning;
        private string phone;

        #endregion

        public PhoneRegistrationsViewModelTests()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

            phone = "+38 (067)-995-33-65";
            phoneWarning = "Phone number must be in format +380 (XX)-XXX-XX-XX";

            navigationParametersMock = new Mock<INavigationParameters>();
            navigationParametersMock.Setup(np => np.Add(It.IsAny<string>(), It.IsAny<object>()));

            phoneRegistrationViewModel = new PhoneRegistrationViewModel(null);
        }

        [Fact]
        public void NavigateToNameRegistrationViewCommand_VerifyNavigationParameters()
        {
            // Arrange
            phoneRegistrationViewModel.OnNavigatedTo(navigationParametersMock.Object);

            // Act
            phoneRegistrationViewModel.NavigateToNameRegistrationView.Execute(phone);

            // Assert
            navigationParametersMock.Verify();
        }

        [Fact]
        public void NavigateToNameRegistrationViewCommand_ValidThatPhoneEmpty()
        {
            // Arrange
            phone = string.Empty;

            // Act
            phoneRegistrationViewModel.NavigateToNameRegistrationView.Execute(phone);

            // Assert
            Assert.Equal(phoneWarning, phoneRegistrationViewModel.PhoneWarning);
        }
    }
}