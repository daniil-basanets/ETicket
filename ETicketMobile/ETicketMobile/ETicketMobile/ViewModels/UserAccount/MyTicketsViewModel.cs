using System;
using System.Collections.Generic;
using System.Linq;
using ETicketMobile.Business.Exceptions;
using ETicketMobile.Business.Model.Tickets;
using ETicketMobile.Business.Services.Interfaces;
using ETicketMobile.DataAccess.Services.Interfaces;
using ETicketMobile.Resources;
using Prism.Navigation;
using Prism.Services;

namespace ETicketMobile.ViewModels.BoughtTickets
{
    public class MyTicketsViewModel : ViewModelBase
    {
        #region Fields

        private readonly ILocalTokenService localTokenService;
        private readonly IPageDialogService dialogService;
        private readonly ITicketsService ticketsService;

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
            ITicketsService ticketsService
        ) : base(navigationService)
        {
            this.localTokenService = localTokenService
                ?? throw new ArgumentNullException(nameof(localTokenService));

            this.dialogService = dialogService
                ?? throw new ArgumentNullException(nameof(dialogService));

            this.ticketsService = ticketsService
                ?? throw new ArgumentNullException(nameof(ticketsService));
        }

        public async override void OnNavigatedTo(INavigationParameters navigationParameters)
        {
            var email = navigationParameters.GetValue<string>("email")
                 ?? throw new ArgumentNullException(nameof(navigationParameters));

            try
            {
                var accessToken = await localTokenService.GetAccessTokenAsync();

                Tickets = await ticketsService.GetTicketsAsync(accessToken, email);
                UnusedTickets = GetUnusedTickets();
                ActivatedTickets = GetActivatedTickets();
                ExpiredTickets = GetExpiredTickets();
            }
            catch (WebException)
            {
                await dialogService.DisplayAlertAsync(AppResource.Error, AppResource.ErrorConnection, AppResource.Ok);

                return;
            }
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