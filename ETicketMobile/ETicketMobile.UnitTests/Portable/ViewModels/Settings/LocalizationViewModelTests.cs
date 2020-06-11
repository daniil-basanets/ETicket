using System;
using ETicketMobile.DataAccess.LocalAPI.Interfaces;
using ETicketMobile.UserInterface.Localization.Interfaces;
using ETicketMobile.ViewModels.Settings;
using Moq;
using Xunit;

namespace ETicketMobile.UnitTests.Portable.ViewModels.Settings
{
    public class LocalizationViewModelTests
    {
        #region Fields

        private readonly Mock<ILocalApi> localApiMock;
        private readonly Mock<ILocalize> localizeMock;

        #endregion

        public LocalizationViewModelTests()
        {
            localApiMock = new Mock<ILocalApi>();
            localizeMock = new Mock<ILocalize>();
        }

        [Fact]
        public void CheckConstructorWithParameters_CheckNullableLocalApi_ShouldThrowException()
        {
            // Arrange
            ILocalApi localApi = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => new LocalizationViewModel(null, localApi, localizeMock.Object));
        }

        [Fact]
        public void CheckConstructorWithParameters_CheckNullableLocalize_ShouldThrowException()
        {
            // Arrange
            ILocalize localize = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => new LocalizationViewModel(null, localApiMock.Object, localize));
        }
    }
}