using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ETicketMobile.Business.Mapping;
using ETicketMobile.Business.Model.Tickets;
using ETicketMobile.Business.Services.Interfaces;
using ETicketMobile.DataAccess.Services.Interfaces;
using ETicketMobile.WebAccess.DTO;
using ETicketMobile.WebAccess.Network.Endpoints;
using ETicketMobile.WebAccess.Network.WebServices.Interfaces;
using Java.Net;
using Prism.Navigation;
using Prism.Services;

namespace ETicketMobile.ViewModels.BoughtTickets
{
    public class MyTicketsViewModel : ViewModelBase
    {
        #region Fields

        private readonly ILocalTokenService localTokenService;
        private readonly IPageDialogService dialogService;
        private readonly ITokenService tokenService;
        private readonly IHttpService httpService;

        private string email;

        private string accessToken;

        private IEnumerable<Ticket> tickets;
        private IEnumerable<Ticket> unusedTickets;
        private IEnumerable<Ticket> activatedTickets;
        private IEnumerable<Ticket> expiredTickets;

        #endregion

        #region Properties

        public IEnumerable<Ticket> Tickets
        {
            get => tickets;
            set => SetProperty(ref tickets, value);
        }

        public IEnumerable<Ticket> UnusedTickets
        {
            get => unusedTickets;
            set => SetProperty(ref unusedTickets, value);
        }

        public IEnumerable<Ticket> ActivatedTickets
        {
            get => activatedTickets;
            set => SetProperty(ref activatedTickets, value);
        }
        public IEnumerable<Ticket> ExpiredTickets
        {
            get => expiredTickets;
            set => SetProperty(ref expiredTickets, value);
        }

        #endregion

        public MyTicketsViewModel(
            INavigationService navigationService,
            ILocalTokenService localTokenService,
            IPageDialogService dialogService,
            IHttpService httpService
        ) : base(navigationService)
        {
            this.dialogService = dialogService
                ?? throw new ArgumentNullException(nameof(dialogService));

            this.localTokenService = localTokenService
                ?? throw new ArgumentNullException(nameof(localTokenService));

            this.httpService = httpService
                ?? throw new ArgumentNullException(nameof(httpService));
        }

        public async override void OnNavigatedTo(INavigationParameters navigationParameters)
        {
            email = navigationParameters.GetValue<string>("email")
                 ?? throw new ArgumentNullException(nameof(navigationParameters));

            try
            {
                accessToken = await localTokenService.GetAccessTokenAsync();

                Tickets = await GetTickets();
                UnusedTickets = GetUnusedTickets();
                ActivatedTickets = GetActivatedTickets();
                ExpiredTickets = GetExpiredTickets();
            }
            catch (WebException)
            {
                await dialogService.DisplayAlertAsync("Error", "Check connection with server", "OK");

                return;
            }
            catch (SocketException)
            {
                await dialogService.DisplayAlertAsync("Error", "Check connection with server", "OK");

                return;
            }
        }

        private async Task<IEnumerable<Ticket>> GetTickets()
        {
            var getTicketsByEmailRequestDto = new GetTicketsByEmailRequestDto { Email = email };

            var ticketsDto = await httpService.PostAsync<GetTicketsByEmailRequestDto, IEnumerable<TicketDto>>(
                    TicketsEndpoint.GetTickets, getTicketsByEmailRequestDto, accessToken);

            if (ticketsDto == null)
            {
                accessToken = await tokenService.RefreshTokenAsync();

                await httpService.PostAsync<GetTicketsByEmailRequestDto, IEnumerable<TicketDto>>(
                    TicketsEndpoint.GetTickets, getTicketsByEmailRequestDto, accessToken);
            }

            var tickets = AutoMapperConfiguration.Mapper.Map<IEnumerable<Ticket>>(ticketsDto);

            return tickets;
        }

        private async Task<string> RefreshTokenAsync()
        {
            var refreshToken = await localTokenService.GetReshreshTokenAsync();

            var tokenDto = await httpService.PostAsync<string, TokenDto>(
                AuthorizeEndpoint.RefreshToken, refreshToken);

            var token = AutoMapperConfiguration.Mapper.Map<Token>(tokenDto);

            await localTokenService.AddAsync(token);

            return token.AcessJwtToken;
        }

        private IEnumerable<Ticket> GetUnusedTickets()
        {
            var unusedTickets = Tickets.Where(t => t.ActivatedAt == null);

            return unusedTickets;
        }

        private IEnumerable<Ticket> GetActivatedTickets()
        {
            var activatedTickets = Tickets.Where(t => t.ExpiredAt > DateTime.Now);

            return activatedTickets;
        }

        private IEnumerable<Ticket> GetExpiredTickets()
        {
            var expiredTickets = Tickets.Where(t => t.ExpiredAt <= DateTime.Now);

            return expiredTickets;
        }
    }
}