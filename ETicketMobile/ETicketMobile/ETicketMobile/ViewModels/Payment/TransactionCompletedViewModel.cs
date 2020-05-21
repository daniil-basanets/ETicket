using System;
using System.Windows.Input;
using ETicketMobile.Views.UserActions;
using Prism.Navigation;
using Xamarin.Forms;

namespace ETicketMobile.ViewModels.Payment
{
    public class TransactionCompletedViewModel : ViewModelBase
    {
        #region Fields

        private readonly INavigationService navigationService;

        private ICommand navigateToMainMenu;

        #endregion

        #region Properties

        public ICommand NavigateToMainMenu => navigateToMainMenu
            ?? (navigateToMainMenu = new Command(OnNavigateToMainMenu));

        #endregion

        public TransactionCompletedViewModel(INavigationService navigationService) 
            : base(navigationService)
        {
            this.navigationService = navigationService
                ?? throw new ArgumentNullException(nameof(navigationService));
        }

        private async void OnNavigateToMainMenu()
        {
            await navigationService.NavigateAsync(nameof(MainMenuView));
        }
    }
}