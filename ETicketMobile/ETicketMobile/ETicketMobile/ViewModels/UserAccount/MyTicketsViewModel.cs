using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ETicketMobile.Business.Mapping;
using ETicketMobile.Business.Model.Tickets;
using ETicketMobile.Data.Entities;
using ETicketMobile.DataAccess.LocalAPI.Interfaces;
using ETicketMobile.WebAccess;
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

        private readonly INavigationService navigationService;
        private readonly IPageDialogService dialogService;
        private readonly IHttpService httpService;

        private readonly ILocalApi localApi;
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
            IPageDialogService dialogService,
            IHttpService httpService,
            ILocalApi localApi
        ) : base(navigationService)
        {
            this.navigationService = navigationService
                ?? throw new ArgumentNullException(nameof(navigationService));

            this.dialogService = dialogService
                ?? throw new ArgumentNullException(nameof(dialogService));

            this.localApi = localApi
                ?? throw new ArgumentNullException(nameof(localApi));

            this.httpService = httpService
                ?? throw new ArgumentNullException(nameof(httpService));
        }

        public async override void OnNavigatedTo(INavigationParameters navigationParameters)
        {
            email = navigationParameters.GetValue<string>("email");

            try
            {
                accessToken = await GetAccessTokenAsync();

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

        private async Task<string> GetAccessTokenAsync()
        {
            var token = await localApi.GetTokenAsync().ConfigureAwait(false);

            return token.AcessJwtToken;
        }

        private async Task<IEnumerable<Ticket>> GetTickets()
        {
            var getTicketsByEmailRequestDto = new GetTicketsByEmailRequestDto { Email = email };

            var ticketsDto = await httpService.PostAsync<GetTicketsByEmailRequestDto, IEnumerable<TicketDto>>(
                    TicketsEndpoint.GetTickets, getTicketsByEmailRequestDto, accessToken);

            if (ticketsDto == null)
            {
                accessToken = await RefreshTokenAsync();

                await httpService.PostAsync<GetTicketsByEmailRequestDto, IEnumerable<TicketDto>>(
                    TicketsEndpoint.GetTickets, getTicketsByEmailRequestDto, accessToken);
            }

            var tickets = AutoMapperConfiguration.Mapper.Map<IEnumerable<Ticket>>(ticketsDto);

            return tickets;


            //return new List<Ticket>
            //{
            //    new Ticket
            //    {
            //        TicketType = "1 hour",
            //        ReferenceNumber = $"{Guid.NewGuid()}",
            //        TicketAreas = new List<string>{ "A", "S"},
            //        CreatedAt = DateTime.Now.AddDays(-3),
            //        ActivatedAt = DateTime.Now.AddDays(-3),
            //        ExpiredAt = DateTime.Now.AddDays(-3).AddHours(1)
            //    },
            //    new Ticket
            //    {
            //        TicketType = "5 hour",
            //        ReferenceNumber = $"{Guid.NewGuid()}",
            //        TicketAreas = new List<string>{ "A", "B", "C"},
            //        CreatedAt = DateTime.Now.AddDays(-2),
            //        ActivatedAt = DateTime.Now.AddDays(-2),
            //        ExpiredAt = DateTime.Now.AddDays(-2).AddHours(5)
            //    },
            //    new Ticket
            //    {
            //        TicketType = "1 Day",
            //        ReferenceNumber = $"{Guid.NewGuid()}",
            //        TicketAreas = new List<string>{ "P" },
            //        CreatedAt = DateTime.Now.AddDays(-2),
            //        ActivatedAt = DateTime.Now.AddDays(-2),
            //        ExpiredAt = DateTime.Now.AddDays(-1)
            //    },
            //    new Ticket
            //    {
            //        TicketType = "1 Day",
            //        ReferenceNumber = $"{Guid.NewGuid()}",
            //        TicketAreas = new List<string>{ "P" },
            //        CreatedAt = DateTime.Now.AddDays(-2),
            //        ActivatedAt = DateTime.Now.AddDays(-2),
            //        ExpiredAt = DateTime.Now.AddDays(-1)
            //    },
            //    new Ticket
            //    {
            //        TicketType = "1 Day",
            //        ReferenceNumber = $"{Guid.NewGuid()}",
            //        TicketAreas = new List<string>{ "P" },
            //        CreatedAt = DateTime.Now.AddDays(-2),
            //        ActivatedAt = DateTime.Now.AddDays(-2),
            //        ExpiredAt = DateTime.Now.AddDays(-1)
            //    },
            //    new Ticket
            //    {
            //        TicketType = "1 Week",
            //        ReferenceNumber = $"{Guid.NewGuid()}",
            //        TicketAreas = new List<string>{ "U", "I" },
            //        CreatedAt = DateTime.Now.AddDays(-7)
            //    },
            //    new Ticket
            //    {
            //        TicketType = "1 Month",
            //        ReferenceNumber = $"{Guid.NewGuid()}",
            //        TicketAreas = new List<string>{ "P", "M" },
            //        CreatedAt = DateTime.Now.AddDays(-30)
            //    },
            //    new Ticket
            //    {
            //        TicketType = "3 Days",
            //        ReferenceNumber = $"{Guid.NewGuid()}",
            //        TicketAreas = new List<string>{ "M", "N" },
            //        CreatedAt = DateTime.Now.AddDays(-1),
            //        ActivatedAt = DateTime.Now.AddDays(-1).AddHours(1),
            //        ExpiredAt = DateTime.Now.AddDays(3).AddHours(1)
            //    },
            //    new Ticket
            //    {
            //        TicketType = "5 Days",
            //        ReferenceNumber = $"{Guid.NewGuid()}",
            //        TicketAreas = new List<string>{ "H", "R" },
            //        CreatedAt = DateTime.Now.AddDays(-2),
            //        ActivatedAt = DateTime.Now.AddDays(-1).AddHours(2).AddMinutes(3),
            //        ExpiredAt = DateTime.Now.AddDays(5).AddHours(2).AddMinutes(3)
            //    },
            //    new Ticket
            //    {
            //        TicketType = "6 Days",
            //        ReferenceNumber = $"{Guid.NewGuid()}",
            //        TicketAreas = new List<string>{ "H", "M" },
            //        CreatedAt = DateTime.Now.AddDays(-10),
            //        ActivatedAt = DateTime.Now.AddDays(-1).AddHours(2).AddMinutes(3),
            //        ExpiredAt = DateTime.Now.AddDays(6).AddHours(2).AddMinutes(3)
            //    },
            //};
        }

        private async Task<string> RefreshTokenAsync()
        {
            var refreshTokenTask = await localApi.GetTokenAsync();
            var refreshToken = refreshTokenTask.RefreshJwtToken;

            var tokenDto = await httpService.PostAsync<string, TokenDto>(
                AuthorizeEndpoint.RefreshToken, refreshToken);

            var token = AutoMapperConfiguration.Mapper.Map<Token>(tokenDto);

            await localApi.AddAsync(token);

            return token.AcessJwtToken;
        }

        private IEnumerable<Ticket> GetUnusedTickets()
        {
            var unusedTickets = Tickets.Where(t => t.ActivatedAt == null);

            return unusedTickets;
        }

        private IEnumerable<Ticket> GetActivatedTickets()
        {
            var unusedTickets = Tickets.Where(t => t.ExpiredAt > DateTime.Now);

            return unusedTickets;
        }

        private IEnumerable<Ticket> GetExpiredTickets()
        {
            var unusedTickets = Tickets.Where(t => t.ExpiredAt <= DateTime.Now);

            return unusedTickets;
        }
    }
}