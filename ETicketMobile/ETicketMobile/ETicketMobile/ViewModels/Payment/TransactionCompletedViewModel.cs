using System.Windows.Input;
using ETicketMobile.Views.UserActions;
using Prism.Navigation;
using Xamarin.Forms;

namespace ETicketMobile.ViewModels.Payment
{
    public class TransactionCompletedViewModel : ViewModelBase
    {
        #region Fields

        private INavigationParameters navigationParameters;

        private ICommand navigateToMainMenu;

        #endregion

        #region Properties

        public ICommand NavigateToMainMenu => navigateToMainMenu
            ??= new Command(OnNavigateToMainMenu);

        #endregion

        public TransactionCompletedViewModel(INavigationService navigationService)
            : base(navigationService)
        {
        }

        public override void OnNavigatedTo(INavigationParameters navigationParameters)
        {
            this.navigationParameters = navigationParameters;
        }

        private async void OnNavigateToMainMenu()
        {
            await NavigationService.NavigateAsync(nameof(MainMenuView), navigationParameters);
        }
    }
}