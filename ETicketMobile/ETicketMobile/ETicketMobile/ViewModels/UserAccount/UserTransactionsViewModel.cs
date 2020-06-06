using System;
using System.Collections.Generic;
using ETicketMobile.Business.Exceptions;
using ETicketMobile.Business.Model.Transactions;
using ETicketMobile.Business.Services.Interfaces;
using ETicketMobile.Resources;
using Prism.Navigation;
using Prism.Services;

namespace ETicketMobile.ViewModels.UserAccount
{
    public class UserTransactionsViewModel : ViewModelBase
    {
        #region Fields

        private readonly ITransactionService transactionService;
        private readonly IPageDialogService dialogService;

        private IEnumerable<Transaction> transactions;

        #endregion

        #region Properties

        public IEnumerable<Transaction> Transactions
        {
            get => transactions;
            set => SetProperty(ref transactions, value);
        }

        #endregion

        public UserTransactionsViewModel(
            ITransactionService transactionService,
            INavigationService navigationService,
            IPageDialogService dialogService
        ) : base(navigationService)
        {
            this.transactionService = transactionService
                ?? throw new ArgumentNullException(nameof(transactionService));

            this.dialogService = dialogService
                ?? throw new ArgumentNullException(nameof(dialogService));
        }

        public override async void OnNavigatedTo(INavigationParameters navigationParameters)
        {
            var email = navigationParameters.GetValue<string>("email")
                ?? throw new NullReferenceException(nameof(navigationParameters));

            try
            {
                Transactions = await transactionService.GetTransactionsAsync(email);
            }
            catch (WebException)
            {
                await dialogService.DisplayAlertAsync(AppResource.Error, AppResource.ErrorConnection, AppResource.Ok);

                return;
            }
        }
    }
}