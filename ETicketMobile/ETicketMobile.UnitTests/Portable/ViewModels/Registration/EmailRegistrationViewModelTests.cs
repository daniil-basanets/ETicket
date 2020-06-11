using System.Globalization;
using System.Threading;
using ETicketMobile.Business.Validators.Interfaces;
using ETicketMobile.ViewModels.Registration;
using Moq;
using Prism.Navigation;
using Prism.Services;
using Xunit;

namespace ETicketMobile.UnitTests.Portable.ViewModels.Registration
{
    public class EmailRegistrationViewModelTests
    {
        #region Fields

        private readonly EmailRegistrationViewModel nameRegistrationViewModel;

        private readonly Mock<INavigationParameters> navigationParametersMock;
        private readonly Mock<INavigationService> navigationServiceMock;

        private readonly Mock<IPageDialogService> dialogServiceMock;
        private readonly Mock<IUserValidator> userValidatorMock;

        #endregion

        public EmailRegistrationViewModelTests()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            
            navigationServiceMock = new Mock<INavigationService>();

            dialogServiceMock = new Mock<IPageDialogService>();

            userValidatorMock = new Mock<IUserValidator>();
            userValidatorMock
                    .Setup(uv => uv.UserExistsAsync(It.IsAny<string>()))
                    .ReturnsAsync(() => true);

            navigationParametersMock = new Mock<INavigationParameters>();
            navigationParametersMock.Setup(np => np.Add(It.IsAny<string>(), It.IsAny<object>()));

            nameRegistrationViewModel = new EmailRegistrationViewModel(navigationServiceMock.Object, dialogServiceMock.Object, userValidatorMock.Object);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void NavigateToPhoneRegistrationView_ValidThatEmptyEmail(string email)
        {
            // Arrange
            var emailWarning = "Are you sure you entered your email correctly?";

            // Act
            nameRegistrationViewModel.NavigateToPhoneRegistrationView.Execute(email);

            // Assert
            Assert.Equal(emailWarning, nameRegistrationViewModel.EmailWarning);
        }
    }
}