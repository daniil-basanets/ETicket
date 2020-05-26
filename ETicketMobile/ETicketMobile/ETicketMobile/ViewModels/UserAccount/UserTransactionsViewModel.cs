using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ETicketMobile.Business.Mapping;
using ETicketMobile.Business.Model.Transactions;
using ETicketMobile.WebAccess.DTO;
using ETicketMobile.WebAccess.Network.Endpoints;
using ETicketMobile.WebAccess.Network.WebServices.Interfaces;
using Prism.Navigation;
using Prism.Services;

namespace ETicketMobile.ViewModels.UserAccount
{
    public class UserTransactionsViewModel : ViewModelBase
    {
        #region Fields

        private readonly IPageDialogService dialogService;

        private readonly IHttpService httpService;

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
            INavigationService navigationService,
            IPageDialogService dialogService,
            IHttpService httpService
        ) : base(navigationService)
        {
            this.dialogService = dialogService
                ?? throw new ArgumentNullException(nameof(dialogService));

            this.httpService = httpService
                ?? throw new ArgumentNullException(nameof(httpService));
        }

        public override async void OnNavigatedTo(INavigationParameters navigationParameters)
        {
            var email = navigationParameters.GetValue<string>("email");

            try
            {
                Transactions = await GetTransactionsAsync(email);
            }
            catch (WebException)
            {
                await dialogService.DisplayAlertAsync("Alert", "Check connection with server", "OK");

                return;
            }
        }

        private async Task<IEnumerable<Transaction>> GetTransactionsAsync(string email)
        {
            var getTransactionsRequestDto = new GetTransactionsRequestDto { Email = email };

            var transacationsDto = await httpService.PostAsync<GetTransactionsRequestDto, IEnumerable<TransactionDto>>(
                    TransactionsEndpoint.GetTransactionsByEmail, getTransactionsRequestDto);

            var transactions = AutoMapperConfiguration.Mapper.Map<IEnumerable<Transaction>>(transacationsDto);

            return transactions;
        }
    }
}