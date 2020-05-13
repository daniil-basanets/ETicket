using System;
using System.Collections.Generic;
using System.Windows.Input;
using ETicketMobile.Business.Model.UserAccount;
using ETicketMobile.Resources;
using ETicketMobile.Views.Settings;
using Prism.Navigation;
using Xamarin.Forms;

namespace ETicketMobile.ViewModels.UserAccount
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;

        private ICommand navigateToAction;

        private IEnumerable<UserAction> settings;

        public IEnumerable<UserAction> Settings
        {
            get => settings;
            set => SetProperty(ref settings, value);
        }

        public ICommand NavigateToAction => navigateToAction
            ?? (navigateToAction = new Command<UserAction>(OnNavigateToAction));

        public SettingsViewModel(INavigationService navigationService) 
            : base(navigationService)
        {
            this.navigationService = navigationService
                ?? throw new ArgumentNullException(nameof(navigationService));
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
                new UserAction { Name = AppResource.Info, View = nameof(LocalizationView) }
            };
        }

        private async void OnNavigateToAction(UserAction action)
        {
            await navigationService.NavigateAsync(action.View);
        }
    }
}