using System.Windows.Input;
using ETicketMobile.Views.UserActions;
using Prism.Navigation;
using Xamarin.Forms;

namespace ETicketMobile.ViewModels.Payment
{
    public class TransactionCompletedViewModel : ViewModelBase
    {
        #region Fields

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

        private async void OnNavigateToMainMenu()
        {
            await NavigationService.NavigateAsync(nameof(MainMenuView));
        }
    }
}