using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using ETicketMobile.Business.Model.UserAccount;
using ETicketMobile.UnitTests.Comparers;
using ETicketMobile.ViewModels.Settings;
using ETicketMobile.Views.Login;
using ETicketMobile.Views.Settings;
using Xunit;

namespace ETicketMobile.UnitTests.Portable.ViewModels.Settings
{
    public class SettingsViewModelTests
    {
        #region Fields

        private readonly SettingsViewModel settingsViewModel;

        private readonly IEnumerable<UserAction> settings;

        #endregion

        public SettingsViewModelTests()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

            settings = new List<UserAction>
            {
                new UserAction { Name = "Localizations", View = nameof(LocalizationView) },
                new UserAction { Name = "Log out", View = nameof(LoginView) }
            };

            settingsViewModel = new SettingsViewModel(null);
        }

        [Fact]
        public void OnAppearing_CheckSettings_ShouldBeEqual()
        {
            // Arrange
            var userActionEqualityComparer = new UserActionEqualityComparer();

            // Act
            settingsViewModel.OnAppearing();

            // Assert
            Assert.Equal(settings, settingsViewModel.Settings, userActionEqualityComparer);
        }
    }
}