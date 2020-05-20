using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ETicketMobile.Business.Mapping;
using ETicketMobile.Business.Model.Transactions;
using ETicketMobile.WebAccess.DTO;
using ETicketMobile.WebAccess.Network;
using ETicketMobile.WebAccess.Network.WebService;
using Prism.Navigation;

namespace ETicketMobile.ViewModels.UserAccount
{
    public class UserTransactionsViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private INavigationParameters navigationParameters;

        private readonly HttpClientService httpClient;

        private IEnumerable<Transaction> transactions;

        public IEnumerable<Transaction> Transactions
        {
            get => transactions;
            set => SetProperty(ref transactions, value);
        }        

        public UserTransactionsViewModel(INavigationService navigationService) 
            : base(navigationService)
        {
            this.navigationService = navigationService
                ?? throw new ArgumentNullException(nameof(navigationService));

            httpClient = new HttpClientService();
        }

        public override async void OnNavigatedTo(INavigationParameters navigationParameters)
        {
            this.navigationParameters = navigationParameters;

            var email = navigationParameters.GetValue<string>("email");

            Transactions = await GetTransactions(email);
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            
        }

        private async Task<IEnumerable<Transaction>> GetTransactions(string email)
        {
            var getTransactionsRequestDto = new GetTransactionsRequestDto { Email = email };

            var transacationsDto = await httpClient.PostAsync<GetTransactionsRequestDto, IEnumerable<TransactionDto>>(
                    TransactionsEndpoint.Post, getTransactionsRequestDto);

            var transactions = AutoMapperConfiguration.Mapper.Map<IEnumerable<Transaction>>(transacationsDto);

            return transactions;
        }
    }
}