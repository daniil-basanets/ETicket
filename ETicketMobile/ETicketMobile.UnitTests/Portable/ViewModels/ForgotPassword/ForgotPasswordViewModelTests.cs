using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using ETicketMobile.Business.Validators.Interfaces;
using ETicketMobile.ViewModels.ForgotPassword;
using ETicketMobile.WebAccess.Network.WebServices.Interfaces;
using Moq;
using Prism.Services;
using Xunit;

namespace ETicketMobile.UnitTests.Portable.ViewModels.ForgotPassword
{
    public class ForgotPasswordViewModelTests
    {
        #region Fields

        private readonly ForgotPasswordViewModel forgotPasswordViewModel;

        private readonly Mock<IPageDialogService> dialogServiceMock;
        private readonly Mock<IUserValidator> userValidatorMock;
        private readonly Mock<IHttpService> httpServiceMock;

        #endregion

        public ForgotPasswordViewModelTests()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

            dialogServiceMock = new Mock<IPageDialogService>();
            userValidatorMock = new Mock<IUserValidator>();
            httpServiceMock = new Mock<IHttpService>();

            forgotPasswordViewModel = new ForgotPasswordViewModel(null, dialogServiceMock.Object, userValidatorMock.Object, httpServiceMock.Object);
        }

        [Fact]
        public void CtorWithParameters()
        {
            // Act
            var exception = Record.Exception(
                () => new ForgotPasswordViewModel(null, dialogServiceMock.Object, userValidatorMock.Object, httpServiceMock.Object));

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public void CtorWithParameters_NullDialogService()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(
                () => new ForgotPasswordViewModel(null, null, userValidatorMock.Object, httpServiceMock.Object));
        }

        [Fact]
        public void CtorWithParameters_NullUserValidator()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(
                () => new ForgotPasswordViewModel(null, dialogServiceMock.Object, null, httpServiceMock.Object));
        }

        [Fact]
        public void CtorWithParameters_NullHttpService()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(
                () => new ForgotPasswordViewModel(null, dialogServiceMock.Object, userValidatorMock.Object, null));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void OnNavigateToConfirmForgotPasswordView_IsValid_IsNullOrEmpty(string email)
        {
            // Arrange
            var emailCorrect = "Are you sure you entered your email correctly?";

            // Act
            forgotPasswordViewModel.NavigateToConfirmForgotPasswordView.Execute(email);

            // Assert
            Assert.Equal(emailCorrect, forgotPasswordViewModel.EmailWarning);
        }
    }
}