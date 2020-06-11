using System.Globalization;
using System.Threading;
using ETicketMobile.ViewModels.Registration;
using Moq;
using Prism.Navigation;
using Xunit;

namespace ETicketMobile.UnitTests.Portable.ViewModels.Registration
{
    public class NameRegistrationViewModelTests
    {
        #region Fields

        private readonly NameRegistrationViewModel nameRegistrationViewModel;

        private readonly Mock<INavigationParameters> navigationParametersMock;
        private readonly Mock<INavigationService> navigationServiceMock;

        private readonly string firstName;
        private string firstNameWarning;

        private readonly string lastName;
        private string lastNameWarning;

        #endregion

        public NameRegistrationViewModelTests()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

            firstName = "firstName";
            firstNameWarning = string.Empty;

            lastName = "lastName";
            lastNameWarning = string.Empty;
            
            navigationServiceMock = new Mock<INavigationService>();

            navigationParametersMock = new Mock<INavigationParameters>();
            navigationParametersMock.Setup(np => np.Add(It.IsAny<string>(), It.IsAny<object>()));

            nameRegistrationViewModel = new NameRegistrationViewModel(navigationServiceMock.Object);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void NavigateToPasswordRegistrationView_ValidThatFirstNameEmpty(string firstName)
        {
            // Arrange
            nameRegistrationViewModel.FirstName = firstName;
            firstNameWarning = "Enter first name";

            // Act
            nameRegistrationViewModel.NavigateToPasswordRegistrationView.Execute(null);

            // Assert
            Assert.Equal(firstNameWarning, nameRegistrationViewModel.FirstNameWarning);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void NavigateToPasswordRegistrationView_ValidThatLastNameEmpty(string lastName)
        {
            // Arrange
            nameRegistrationViewModel.FirstName = firstName;
            nameRegistrationViewModel.LastName = lastName;
            lastNameWarning = "Enter last name";

            // Act
            nameRegistrationViewModel.NavigateToPasswordRegistrationView.Execute(null);

            // Assert
            Assert.Equal(lastNameWarning, nameRegistrationViewModel.LastNameWarning);
        }

        [Theory]
        [InlineData("a")]
        [InlineData("Wolfe­schlegel­stein­hausen­berger­dorff")]
        public void NavigateToPasswordRegistrationView_ValidThatFirstNameValid(string firstName)
        {
            // Arrange
            nameRegistrationViewModel.FirstName = firstName;
            nameRegistrationViewModel.LastName = lastName;
            firstNameWarning = "Are you sure you entered your first name correctly?";

            // Act
            nameRegistrationViewModel.NavigateToPasswordRegistrationView.Execute(null);

            // Assert
            Assert.Equal(firstNameWarning, nameRegistrationViewModel.FirstNameWarning);
        }

        [Theory]
        [InlineData("b")]
        [InlineData("Wolfe­schlegel­stein­hausen­berger­dorff")]
        public void NavigateToPasswordRegistrationView_ValidThatLastNameValid(string lastName)
        {
            // Arrange
            nameRegistrationViewModel.FirstName = firstName;
            nameRegistrationViewModel.LastName = lastName;
            lastNameWarning = "Are you sure you entered your last name correctly?";

            // Act
            nameRegistrationViewModel.NavigateToPasswordRegistrationView.Execute(null);

            // Assert
            Assert.Equal(lastNameWarning, nameRegistrationViewModel.LastNameWarning);
        }

        [Fact]
        public void NavigateToNameRegistrationViewCommand_Verify_NavigationParameters()
        {
            // Arrange
            nameRegistrationViewModel.FirstName = firstName;
            nameRegistrationViewModel.LastName = lastName;

            // Act
            nameRegistrationViewModel.OnNavigatedTo(navigationParametersMock.Object);
            nameRegistrationViewModel.NavigateToPasswordRegistrationView.Execute(null);

            // Assert
            navigationParametersMock.Verify();
        }
    }
}