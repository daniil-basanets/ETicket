using System;
using System.Globalization;
using System.Threading;
using ETicketMobile.Business.Services.Interfaces;
using ETicketMobile.Data.Entities;
using ETicketMobile.DataAccess.Services.Interfaces;
using ETicketMobile.ViewModels.Registration;
using ETicketMobile.WebAccess.DTO;
using Moq;
using Prism.Navigation;
using Prism.Services;
using Xunit;

namespace ETicketMobile.UnitTests.Portable.ViewModels.Registration
{
    public class ConfirmEmailViewModelTests
    {
        #region Fiels

        private readonly ConfirmEmailViewModel confirmEmailViewModel;

        private readonly Mock<INavigationParameters> navigationParametersMock;
        private readonly Mock<INavigationService> navigationServiceMock;

        private readonly Mock<IEmailActivationService> emailActivationServiceMock;
        private readonly Mock<ILocalTokenService> localTokenServiceMock;
        private readonly Mock<IPageDialogService> dialogServiceMock;
        private readonly Mock<ITokenService> tokenServiceMock;
        private readonly Mock<IUserService> userServiceMock;

        private readonly string code;

        private readonly Token token;

        #endregion

        public ConfirmEmailViewModelTests()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

            code = "code";

            dialogServiceMock = new Mock<IPageDialogService>();
            navigationServiceMock = new Mock<INavigationService>();            

            emailActivationServiceMock = new Mock<IEmailActivationService>();
            emailActivationServiceMock.Setup(eas => eas.RequestActivationCodeAsync(It.IsAny<string>()));

            emailActivationServiceMock
                    .Setup(eas => eas.ActivateEmailAsync(It.IsAny<string>(), It.IsAny<string>()))
                    .ReturnsAsync(() => false);

            localTokenServiceMock = new Mock<ILocalTokenService>();
            localTokenServiceMock.Setup(lts => lts.AddAsync(It.IsAny<Token>()));

            token = new Token
            {
                AcessJwtToken = "AccessToken",
                RefreshJwtToken = "RefreshToken"
            };

            tokenServiceMock = new Mock<ITokenService>();
            tokenServiceMock
                    .Setup(ts => ts.GetTokenAsync(It.IsAny<string>(), It.IsAny<string>()))
                    .ReturnsAsync(token);

            userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(us => us.CreateNewUserAsync(It.IsAny<UserSignUpRequestDto>()));

            navigationParametersMock = new Mock<INavigationParameters>();
            navigationParametersMock.Setup(np => np.Add(It.IsAny<string>(), It.IsAny<object>()));

            confirmEmailViewModel = new ConfirmEmailViewModel(emailActivationServiceMock.Object, navigationServiceMock.Object, 
                localTokenServiceMock.Object, dialogServiceMock.Object, tokenServiceMock.Object, userServiceMock.Object);
        }

        [Fact]
        public void CheckConstructorWithParameters_CheckNullableEmailActivationService_ShouldThrowException()
        {
            // Arrange
            IEmailActivationService emailActivationService = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => new ConfirmEmailViewModel(emailActivationService, null, localTokenServiceMock.Object,
                dialogServiceMock.Object, tokenServiceMock.Object, userServiceMock.Object));
        }

        [Fact]
        public void CheckConstructorWithParameters_CheckNullableLocalTokenService_ShouldThrowException()
        {
            // Arrange
            ILocalTokenService localTokenService = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => new ConfirmEmailViewModel(emailActivationServiceMock.Object, null, localTokenService,
                dialogServiceMock.Object, tokenServiceMock.Object, userServiceMock.Object));
        }

        [Fact]
        public void CheckConstructorWithParameters_CheckNullableDialogService_ShouldThrowException()
        {
            // Arrange
            IPageDialogService dialogService = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => new ConfirmEmailViewModel(emailActivationServiceMock.Object, null, localTokenServiceMock.Object,
                dialogService, tokenServiceMock.Object, userServiceMock.Object));
        }

        [Fact]
        public void CheckConstructorWithParameters_CheckNullableTokenService_ShouldThrowException()
        {
            // Arrange
            ITokenService tokenService = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => new ConfirmEmailViewModel(emailActivationServiceMock.Object, null, localTokenServiceMock.Object,
                dialogServiceMock.Object, tokenService, userServiceMock.Object));
        }

        [Fact]
        public void CheckConstructorWithParameters_CheckNullableUserService_ShouldThrowException()
        {
            // Arrange
            IUserService userService = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => new ConfirmEmailViewModel(emailActivationServiceMock.Object, null, localTokenServiceMock.Object,
                dialogServiceMock.Object, tokenServiceMock.Object, userService));
        }

        [Fact]
        public void OnAppearing_CheckTimerActivated_ShouldBeFalse()
        {
            // Act
            confirmEmailViewModel.OnAppearing();

            // Assert
            Assert.False(confirmEmailViewModel.TimerActivated);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void NavigateToSignInView_ValidThatEmailEmpty(string code)
        {
            // Arrange
            var confirmEmailWarning = "Enter activation code";

            // Act
            confirmEmailViewModel.NavigateToSignInView.Execute(code);

            // Assert
            Assert.Equal(confirmEmailWarning, confirmEmailViewModel.ConfirmEmailWarning);
        }

        [Fact]
        public void NavigateToSignInView_CheckEmailActivated()
        {
            // Arrange
            var confirmEmailWarning = "Wrong activation code";

            // Act
            confirmEmailViewModel.NavigateToSignInView.Execute(code);

            // Assert
            Assert.Equal(confirmEmailWarning, confirmEmailViewModel.ConfirmEmailWarning);
        }
    }
}