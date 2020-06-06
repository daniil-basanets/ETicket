using System;
using System.Collections.Generic;
using System.Windows.Input;
using ETicketMobile.Business.Model.UserAccount;
using ETicketMobile.Resources;
using ETicketMobile.Views.Tickets;
using ETicketMobile.Views.UserAccount;
using ETicketMobile.Views.UserActions;
using Prism.Navigation;
using Xamarin.Forms;

namespace ETicketMobile.ViewModels.UserAccount
{
    public class UserAccountViewModel : ViewModelBase
    {
        #region Fields

        private INavigationParameters navigationParameters;

        private ICommand navigateToAction;

        private IEnumerable<UserAction> actions;

        private string email;

        #endregion

        #region Properties

        public IEnumerable<UserAction> UserActions
        {
            get => actions;
            set => SetProperty(ref actions, value);
        }

        public ICommand NavigateToAction => navigateToAction
            ??= new Command<UserAction>(OnNavigateToAction);

        #endregion

        public UserAccountViewModel(INavigationService navigationService)
            : base(navigationService)
        {
        }

        public override void OnAppearing()
        {
            Init();
        }

        private void Init()
        {
            UserActions = new List<UserAction>
            {
                new UserAction { Name = AppResource.BuyTicket, View = nameof(TicketsView) },
                new UserAction { Name = AppResource.TransactionHistory, View = nameof(UserTransactionsView) },
                new UserAction { Name = AppResource.MyTickets, View = nameof(MyTicketsView) }
            };
        }

        public override void OnNavigatedTo(INavigationParameters navigationParameters)
        {
            this.navigationParameters = navigationParameters
                ?? throw new ArgumentNullException(nameof(navigationParameters));

            if (string.IsNullOrEmpty(email))
            {
                email = navigationParameters.GetValue<string>("email");
            }
        }

        private async void OnNavigateToAction(UserAction action)
        {
            await NavigationService.NavigateAsync(action.View, navigationParameters);
        }
    }
}