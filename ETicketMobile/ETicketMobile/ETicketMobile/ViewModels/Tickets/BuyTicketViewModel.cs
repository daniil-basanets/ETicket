using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using ETicketMobile.Data.Entities;
using ETicketMobile.DataAccess.LocalAPI.Interfaces;
using ETicketMobile.WebAccess;
using ETicketMobile.WebAccess.Network;
using ETicketMobile.WebAccess.Network.WebService;
using Prism.Navigation;
using Xamarin.Forms;

namespace ETicketMobile.ViewModels.Tickets
{
    public class BuyTicketViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Fields

        private readonly INavigationService navigationService;
        private INavigationParameters navigationParameters;

        private readonly ILocalApi localApi;

        private readonly HttpClientService httpClient;

        private readonly string accessToken;

        private int amount;

        private ICommand removeTicket;
        private ICommand addTicket;
        private ICommand buyTicket;

        #endregion

        #region Properties

        public ICommand RemoveTicket => removeTicket
            ?? (removeTicket = new Command(OnAddTicket));

        public ICommand AddTicket => addTicket
            ?? (addTicket = new Command(OnRemoveTicket));

        public ICommand BuyTicket => buyTicket
            ?? (buyTicket = new Command(OnBuyTicket));

        public int Amount
        {
            get => amount;
            set => SetProperty(ref amount, value);
        }

        #endregion

        public BuyTicketViewModel(INavigationService navigationService, ILocalApi localApi)
            : base(navigationService)
        {
            this.navigationService = navigationService
                ?? throw new ArgumentNullException(nameof(navigationService));

            this.localApi = localApi
                ?? throw new ArgumentNullException(nameof(localApi));

            httpClient = new HttpClientService();
        }

        public override void OnNavigatedTo(INavigationParameters navigationParameters)
        {
            this.navigationParameters = navigationParameters;
            base.OnNavigatedTo(navigationParameters);
        }

        private void OnAddTicket(object obj)
        {
            Amount++;
        }

        private void OnRemoveTicket(object obj)
        {
            if (Amount > 1)
                Amount--;
        }

        private async void OnBuyTicket(object obj)
        {
            var id = navigationParameters.GetValue<int>("id");
            var name = navigationParameters.GetValue<string>("name");
            var price = navigationParameters.GetValue<decimal>("price");

            var ticket = new TicketDto
            {
                Id = id,
                Name = name,
                Coefficient = price
            };

            var response = await httpClient.PostAsync<TicketDto, string>(TicketsEndpoint.BuyTicket, ticket, accessToken);

            if (string.IsNullOrEmpty(response))
            {
                var token = RefreshTokenAsync().Result;

                await localApi.AddAsync(token);

                response = await httpClient.PostAsync<TicketDto, string>(TicketsEndpoint.BuyTicket, ticket, token.AcessJwtToken);
            }
        }

        private async Task<Token> RefreshTokenAsync()
        {
            var refreshToken = await localApi.GetTokenAsync();
            var token = await httpClient.PostAsync<Token, Token>(TicketsEndpoint.RefreshToken, refreshToken);

            return token;
        }
    }
}