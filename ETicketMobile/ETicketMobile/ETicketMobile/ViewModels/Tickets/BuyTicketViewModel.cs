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

            httpClient = new HttpClientService(ServerConfig.Address);
        }

        public override void OnNavigatedTo(INavigationParameters navigationParameters)
        {
            this.navigationParameters = navigationParameters;
            base.OnNavigatedTo(navigationParameters);
        }

        private void OnAddTicket()
        {
            Amount++;
        }

        private void OnRemoveTicket()
        {
            if (Amount > 1)
                Amount--;
        }

        private async void OnBuyTicket()
        {
            var id = navigationParameters.GetValue<int>("id");
            var name = navigationParameters.GetValue<string>("name");
            var price = navigationParameters.GetValue<decimal>("price");

            var ticketTypeDto = new TicketTypeDto
            {
                Id = id,
                Name = name,
                Coefficient = price
            };

            var response = await httpClient.PostAsync<TicketTypeDto, string>(TicketsEndpoint.BuyTicket, ticketTypeDto, accessToken);

            if (string.IsNullOrEmpty(response))
            {
                var token = RefreshTokenAsync().Result;

                await localApi.AddAsync(token);

                response = await httpClient.PostAsync<TicketTypeDto, string>(TicketsEndpoint.BuyTicket, ticketTypeDto, token.AcessJwtToken);
            }
        }

        private async Task<Token> RefreshTokenAsync()
        {
            var refreshToken = await localApi.GetTokenAsync();
            var token = await httpClient.PostAsync<Token, Token>(AuthorizeEndpoint.RefreshToken, refreshToken);

            return token;
        }
    }
}