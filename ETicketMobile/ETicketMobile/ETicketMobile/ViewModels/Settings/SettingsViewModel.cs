using System.Collections.Generic;
using System.Windows.Input;
using ETicketMobile.Business.Model.UserAccount;
using ETicketMobile.Resources;
using ETicketMobile.Views.Login;
using ETicketMobile.Views.Settings;
using Prism.Navigation;
using Xamarin.Forms;

namespace ETicketMobile.ViewModels.Settings
{
    public class SettingsViewModel : ViewModelBase
    {
        #region Fields

        private IEnumerable<UserAction> settings;

        private ICommand navigateToAction;

        #endregion

        #region Properties

        public ICommand NavigateToAction => navigateToAction
            ??= new Command<UserAction>(OnNavigateToAction);

        public IEnumerable<UserAction> Settings
        {
            get => settings;
            set => SetProperty(ref settings, value);
        }

        #endregion

        public SettingsViewModel(INavigationService navigationService)
            : base(navigationService)
        {
        }

        public override void OnAppearing()
        {
            Init();
        }

        private void Init()
        {
            Settings = new List<UserAction>
            {
                new UserAction { Name = AppResource.Localization, View = nameof(LocalizationView) },
                new UserAction { Name = AppResource.Logout, View = nameof(LoginView) }
            };
        }

        private async void OnNavigateToAction(UserAction action)
        {
            await NavigationService.NavigateAsync(action.View);
        }
    }
}