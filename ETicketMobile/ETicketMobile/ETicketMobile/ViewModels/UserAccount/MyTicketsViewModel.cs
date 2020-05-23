using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ETicketMobile.Business.Model.Tickets;
using ETicketMobile.DataAccess.LocalAPI.Interfaces;
using ETicketMobile.WebAccess.Network;
using ETicketMobile.WebAccess.Network.WebService;
using Prism.Navigation;

namespace ETicketMobile.ViewModels.BoughtTickets
{
    public class MyTicketsViewModel : ViewModelBase
    {
        #region Fields

        private readonly INavigationService navigationService;
        private readonly ILocalApi localApi;

        private readonly HttpClientService httpClient;

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

        public MyTicketsViewModel(INavigationService navigationService, ILocalApi localApi)
            : base(navigationService)
        {
            this.navigationService = navigationService
                ?? throw new ArgumentNullException(nameof(navigationService));

            this.localApi = localApi
                ?? throw new ArgumentNullException(nameof(localApi));

            httpClient = new HttpClientService(ServerConfig.Address);
        }

        public override void OnAppearing()
        {
            //accessToken = await GetAccessToken();

            Tickets = GetTickets();
            UnusedTickets = GetUnusedTickets();
            ActivatedTickets = GetActivatedTickets();
            ExpiredTickets = GetExpiredTickets();
        }

        private async Task<string> GetAccessTokenAsync()
        {
            var token = await localApi.GetTokenAsync().ConfigureAwait(false);

            return token.AcessJwtToken;
        }

        private IEnumerable<Ticket> GetTickets()
        {
            return new List<Ticket>
            {
                new Ticket
                {
                    TicketType = "1 hour",
                    ReferenceNumber = $"{Guid.NewGuid()}",
                    TicketAreas = new List<string>{ "A", "S"},
                    CreatedAt = DateTime.Now.AddDays(-3),
                    ActivatedAt = DateTime.Now.AddDays(-3),
                    ExpiredAt = DateTime.Now.AddDays(-3).AddHours(1)
                },
                new Ticket
                {
                    TicketType = "5 hour",
                    ReferenceNumber = $"{Guid.NewGuid()}",
                    TicketAreas = new List<string>{ "A", "B", "C"},
                    CreatedAt = DateTime.Now.AddDays(-2),
                    ActivatedAt = DateTime.Now.AddDays(-2),
                    ExpiredAt = DateTime.Now.AddDays(-2).AddHours(5)
                },
                new Ticket
                {
                    TicketType = "1 Day",
                    ReferenceNumber = $"{Guid.NewGuid()}",
                    TicketAreas = new List<string>{ "P" },
                    CreatedAt = DateTime.Now.AddDays(-2),
                    ActivatedAt = DateTime.Now.AddDays(-2),
                    ExpiredAt = DateTime.Now.AddDays(-1)
                },
                new Ticket
                {
                    TicketType = "1 Day",
                    ReferenceNumber = $"{Guid.NewGuid()}",
                    TicketAreas = new List<string>{ "P" },
                    CreatedAt = DateTime.Now.AddDays(-2),
                    ActivatedAt = DateTime.Now.AddDays(-2),
                    ExpiredAt = DateTime.Now.AddDays(-1)
                },
                new Ticket
                {
                    TicketType = "1 Day",
                    ReferenceNumber = $"{Guid.NewGuid()}",
                    TicketAreas = new List<string>{ "P" },
                    CreatedAt = DateTime.Now.AddDays(-2),
                    ActivatedAt = DateTime.Now.AddDays(-2),
                    ExpiredAt = DateTime.Now.AddDays(-1)
                },
                new Ticket
                {
                    TicketType = "1 Week",
                    ReferenceNumber = $"{Guid.NewGuid()}",
                    TicketAreas = new List<string>{ "U", "I" },
                    CreatedAt = DateTime.Now.AddDays(-7)
                },
                new Ticket
                {
                    TicketType = "1 Month",
                    ReferenceNumber = $"{Guid.NewGuid()}",
                    TicketAreas = new List<string>{ "P", "M" },
                    CreatedAt = DateTime.Now.AddDays(-30)
                },
                new Ticket
                {
                    TicketType = "3 Days",
                    ReferenceNumber = $"{Guid.NewGuid()}",
                    TicketAreas = new List<string>{ "M", "N" },
                    CreatedAt = DateTime.Now.AddDays(-1),
                    ActivatedAt = DateTime.Now.AddDays(-1).AddHours(1),
                    ExpiredAt = DateTime.Now.AddDays(3).AddHours(1)
                },
                new Ticket
                {
                    TicketType = "5 Days",
                    ReferenceNumber = $"{Guid.NewGuid()}",
                    TicketAreas = new List<string>{ "H", "R" },
                    CreatedAt = DateTime.Now.AddDays(-2),
                    ActivatedAt = DateTime.Now.AddDays(-1).AddHours(2).AddMinutes(3),
                    ExpiredAt = DateTime.Now.AddDays(5).AddHours(2).AddMinutes(3)
                },
                new Ticket
                {
                    TicketType = "6 Days",
                    ReferenceNumber = $"{Guid.NewGuid()}",
                    TicketAreas = new List<string>{ "H", "M" },
                    CreatedAt = DateTime.Now.AddDays(-10),
                    ActivatedAt = DateTime.Now.AddDays(-1).AddHours(2).AddMinutes(3),
                    ExpiredAt = DateTime.Now.AddDays(6).AddHours(2).AddMinutes(3)
                },
            };
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