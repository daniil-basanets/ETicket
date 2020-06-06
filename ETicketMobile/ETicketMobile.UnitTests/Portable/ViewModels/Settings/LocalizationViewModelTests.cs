using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using ETicketMobile.Business.Model.UserAccount;
using ETicketMobile.DataAccess.LocalAPI.Interfaces;
using ETicketMobile.UnitTests.Comparers;
using ETicketMobile.UserInterface.Localization.Interfaces;
using ETicketMobile.ViewModels.Settings;
using Moq;
using Xunit;

namespace ETicketMobile.UnitTests.Portable.ViewModels.Settings
{
    public class LocalizationViewModelTests
    {
        #region Fields

        private readonly LocalizationViewModel localizationViewModel;

        private readonly Mock<ILocalApi> localApiMock;
        private readonly Mock<ILocalize> localizeMock;

        private readonly IEnumerable<LocalizationItemViewModel> localizations;

        #endregion

        public LocalizationViewModelTests()
        {
            localApiMock = new Mock<ILocalApi>();
            localizeMock = new Mock<ILocalize>();

            localizations = new List<LocalizationItemViewModel>
            {
                new LocalizationItemViewModel(new Localization { Language = "Українська", Culture = "uk-UA" }),
                new LocalizationItemViewModel(new Localization { Language = "Русский", Culture = "ru-RU" }),
                new LocalizationItemViewModel(new Localization { Language = "English", Culture = "en-US" }),
            };

            localizeMock.Object.CurrentCulture = new CultureInfo(localizations.ElementAt(2).Culture);

            localizationViewModel = new LocalizationViewModel(null, localApiMock.Object, localizeMock.Object);
        }

        [Fact]
        public void CheckConstructorWithParameters_CheckNullableLocalApi_ShouldThrowException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => new LocalizationViewModel(null, null, localizeMock.Object));
        }

        [Fact]
        public void CheckConstructorWithParameters_CheckNullableLocalize_ShouldThrowException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => new LocalizationViewModel(null, localApiMock.Object, null));
        }

        [Fact]
        public void OnAppearing_CompareLocalizations_ShouldBeEqual()
        {
            // Arrange
            var localizationItemViewModelEqualityComparer = new LocalizationItemViewModelEqualityComparer();

            // Act
            localizationViewModel.OnAppearing();

            // Assert
            Assert.Equal(localizations, localizationViewModel.Localizations, localizationItemViewModelEqualityComparer);
        }
    }
}