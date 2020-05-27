using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using ETicketMobile.Business.Mapping;
using ETicketMobile.Business.Model.Tickets;
using ETicketMobile.Data.Entities;
using ETicketMobile.DataAccess.LocalAPI.Interfaces;
using ETicketMobile.Views.Tickets;
using ETicketMobile.WebAccess;
using ETicketMobile.WebAccess.DTO;
using ETicketMobile.WebAccess.Network.Endpoints;
using ETicketMobile.WebAccess.Network.WebServices.Interfaces;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;

namespace ETicketMobile.ViewModels.Tickets
{
    public class TicketsViewModel : ViewModelBase
    {
        #region Fields

        private readonly INavigationService navigationService;
        private INavigationParameters navigationParameters;

        private readonly IPageDialogService dialogService;
        private readonly IHttpService httpService;

        private readonly ILocalApi localApi;

        private IEnumerable<TicketType> tickets;

        private string accessToken;

        private ICommand chooseTicket;

        #endregion

        #region Properties

        public IEnumerable<TicketType> Tickets
        {
            get => tickets;
            set => SetProperty(ref tickets, value);
        }

        public ICommand ChooseTicket => chooseTicket 
            ??= new Command<TicketType>(OnChooseTicket);

        #endregion

        public TicketsViewModel(
            INavigationService navigationService,
            IPageDialogService dialogService,
            IHttpService httpService,
            ILocalApi localApi)
            : base(navigationService)
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

        public async override void OnAppearing()
        {
            try
            {
                accessToken = await GetAccessTokenAsync();
                Tickets = await GetTicketsAsync();
            }
            catch (WebException)
            {
                await dialogService.DisplayAlertAsync("Error", "Check connection with server", "OK");

                return;
            }
        }

        public override void OnNavigatedTo(INavigationParameters navigationParameters)
        {
            this.navigationParameters = navigationParameters;
        }

        private async Task<string> GetAccessTokenAsync()
        {
            var token = await localApi.GetTokenAsync();

            return token.AcessJwtToken;
        }

        private async Task<IEnumerable<TicketType>> GetTicketsAsync()
        {
            var ticketsDto = await httpService.GetAsync<IEnumerable<TicketTypeDto>>(
                    TicketTypesEndpoint.GetTicketTypes, accessToken);

            if (ticketsDto == null)
            {
                accessToken = await RefreshTokenAsync();

                ticketsDto = await httpService.GetAsync<IEnumerable<TicketTypeDto>>(
                    TicketTypesEndpoint.GetTicketTypes, accessToken);
            }

            var tickets = AutoMapperConfiguration.Mapper.Map<IEnumerable<TicketType>>(ticketsDto);

            return tickets;
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

        private async void OnChooseTicket(TicketType ticket)
        {
            navigationParameters.Add("ticketId", ticket.Id);
            navigationParameters.Add("durationHours", ticket.DurationHours);
            navigationParameters.Add("name", ticket.Name);
            navigationParameters.Add("coefficient", ticket.Coefficient);

            await navigationService.NavigateAsync(nameof(AreasView), navigationParameters);
        }
    }
}