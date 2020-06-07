using System;
using System.Globalization;
using System.Threading;
using ETicketMobile.Business.Services.Interfaces;
using ETicketMobile.ViewModels.Registration;
using Moq;
using Prism.Navigation;
using Prism.Services;
using Xunit;

namespace ETicketMobile.UnitTests.Portable.ViewModels.Registration
{
    public class BirthdayRegistrationViewModelTests
    {
        #region Fields

        private readonly BirthdayRegistrationViewModel birthdayRegistrationViewModel;

        private readonly Mock<INavigationParameters> navigationParametersMock;
        private readonly Mock<INavigationService> navigationServiceMock;

        private readonly Mock<IEmailActivationService> emailActivationServiceMock;
        private readonly Mock<IPageDialogService> dialogServiceMock;

        #endregion

        public BirthdayRegistrationViewModelTests()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

            dialogServiceMock = new Mock<IPageDialogService>();

            navigationServiceMock = new Mock<INavigationService>();

            emailActivationServiceMock = new Mock<IEmailActivationService>();
            emailActivationServiceMock.Setup(eas => eas.RequestActivationCodeAsync(It.IsAny<string>()));

            navigationParametersMock = new Mock<INavigationParameters>();

            birthdayRegistrationViewModel = new BirthdayRegistrationViewModel(emailActivationServiceMock.Object, navigationServiceMock.Object, dialogServiceMock.Object);
        }

        [Fact]
        public void CheckConstructorWithParameters_CheckNullableEmailActivationService_ShouldThrowException()
        {
            // Arrange
            IEmailActivationService emailActivationService = null;

            // Assert
            Assert.Throws<ArgumentNullException>(
                () => new BirthdayRegistrationViewModel(emailActivationService, navigationServiceMock.Object, dialogServiceMock.Object));
        }

        [Fact]
        public void CheckConstructorWithParameters_CheckNullableDialogService_ShouldThrowException()
        {
            // Arrange
            IPageDialogService dialogService = null;

            // Assert
            Assert.Throws<ArgumentNullException>(
                () => new BirthdayRegistrationViewModel(emailActivationServiceMock.Object, navigationServiceMock.Object, dialogService));
        }

        [Fact]
        public void NavigateToConfirmEmailView_RequestActivationCodeAsync()
        {
            // Arrange
            var birthday = DateTime.Now.Date;

            // Act
            birthdayRegistrationViewModel.OnNavigatedTo(navigationParametersMock.Object);
            birthdayRegistrationViewModel.NavigateToConfirmEmailView.Execute(birthday);

            // Assert
            emailActivationServiceMock.Verify(eas => eas.RequestActivationCodeAsync(It.IsAny<string>()));
        }
    }
}