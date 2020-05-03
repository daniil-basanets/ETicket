using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ETicketMobile.Business.Mapping;
using ETicketMobile.Business.Model.Tickets;
using ETicketMobile.Data.Domain.Entities;
using ETicketMobile.DataAccess.Domain.LocalAPI.Interfaces;
using ETicketMobile.Views.Tickets;
using ETicketMobile.WebAccess;
using ETicketMobile.WebAccess.DTO;
using ETicketMobile.WebAccess.Network;
using ETicketMobile.WebAccess.Network.WebService;
using Prism.Navigation;
using Xamarin.Forms;

namespace ETicketMobile.ViewModels.Tickets
{
    public class TicketsViewModel : ViewModelBase
    {
        #region Fields

        private readonly INavigationService navigationService;
        private INavigationParameters navigationParameters;

        private readonly ILocalApi localApi;

        private IEnumerable<Ticket> tickets;

        private readonly HttpClientService httpClient;

        private string accessToken;

        private Ticket ticketTapped;

        private ICommand chooseTicket;

        #endregion

        #region Properties

        public IEnumerable<Ticket> Tickets
        {
            get => tickets;
            set => SetProperty(ref tickets, value);
        }

        public Ticket TicketSelected
        {
            get => ticketTapped;
            set
            {
                ticketTapped = value;

                if (ticketTapped == null)
                    return;

                ChooseTicket.Execute(ticketTapped);

                TicketSelected = null;
            }
        }

        public ICommand ChooseTicket => chooseTicket
            ?? (chooseTicket = new Command(OnChooseTicket));

        #endregion

        public TicketsViewModel(
            INavigationService navigationService,
            ILocalApi localApi
        ) : base(navigationService)
        {
            this.navigationService = navigationService
                ?? throw new ArgumentNullException(nameof(navigationService));

            this.localApi = localApi
                ?? throw new ArgumentNullException(nameof(localApi));

            httpClient = new HttpClientService();
        }

        public override async void OnNavigatedTo(INavigationParameters navigationParameters)
        {
            this.navigationParameters = navigationParameters;

            accessToken = await GetAccessToken();
            Tickets = await GetTickets();
        }

        private async Task<string> GetAccessToken()
        {
            var token = await localApi.GetTokenAsync().ConfigureAwait(false);

            return token.AcessJwtToken;
        }

        private async Task<IEnumerable<Ticket>> GetTickets()
        {
            var ticketsDto = await httpClient.GetAsync<IEnumerable<TicketDto>>(
                    TicketsEndpoint.Get, accessToken).ConfigureAwait(false);

            if (ticketsDto == null)
            {
                accessToken = await RefreshToken();

                ticketsDto = await httpClient.GetAsync<IEnumerable<TicketDto>>(
                    TicketsEndpoint.Get, accessToken).ConfigureAwait(false);
            }

            var tickets = AutoMapperConfiguration.Mapper.Map<IEnumerable<Ticket>>(ticketsDto);

            return tickets;
        }

        private async Task<string> RefreshToken()
        {
            var refreshToken = localApi.GetTokenAsync().Result.RefreshJwtToken;

            var tokenDto = await httpClient.PostAsync<string, TokenDto>(
                TicketsEndpoint.RefreshToken, refreshToken);

            var token = AutoMapperConfiguration.Mapper.Map<Token>(tokenDto);

            await localApi.AddAsync(token);

            return token.AcessJwtToken;
        }

        private void OnChooseTicket(object obj)
        {
            var navigationParameters = new NavigationParameters
            {
                { "ticket", TicketSelected }
            };

            navigationService.NavigateAsync(nameof(AreasView), navigationParameters);
        }
    }
}