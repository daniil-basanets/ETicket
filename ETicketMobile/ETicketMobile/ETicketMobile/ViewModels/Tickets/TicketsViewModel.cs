using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using ETicketMobile.Business.Mapping;
using ETicketMobile.Business.Model.Tickets;
using ETicketMobile.Business.Services.Interfaces;
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
    public class TicketsViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Fields

        private INavigationParameters navigationParameters;

        private readonly IPageDialogService dialogService;
        private readonly ITokenService tokenService;
        private readonly IHttpService httpService;

        private readonly ILocalApi localApi;

        private IEnumerable<TicketType> tickets;
        private IEnumerable<Area> areas;

        private TicketType ticketSelected;
        private string selectedTicket;

        private Area areaSelected;
        private string selectedArea;

        private string accessToken;

        private decimal totalPrice;

        private bool selected;

        private ICommand chooseTicket;

        #endregion

        #region Properties

        public ICommand ChooseTicket => chooseTicket
            ??= new Command(OnChooseTicket);

        public IEnumerable<TicketType> Tickets
        {
            get => tickets;
            set => SetProperty(ref tickets, value);
        }

        public IEnumerable<Area> Areas
        {
            get => areas;
            set => SetProperty(ref areas, value);
        }

        public TicketType TicketSelected
        {
            get => ticketSelected;
            set
            {
                SelectedTicket = value.Name;

                SetProperty(ref ticketSelected, value);
            }
        }

        public string SelectedTicket
        {
            get => selectedTicket;
            set => SetProperty(ref selectedTicket, value);
        }

        public Area AreaSelected
        {
            get => areaSelected;
            set
            {
                SelectedArea = value.Name;

                SetProperty(ref areaSelected, value);
            }
        }

        public string SelectedArea
        {
            get => selectedArea;
            set => SetProperty(ref selectedArea, value);
        }

        public bool Selected
        {
            get { return selected; }
            set
            {
                SetProperty(ref selected, value);
            }
        }

        public decimal TotalPrice
        {
            get => totalPrice;
            set => SetProperty(ref totalPrice, value);
        }

        #endregion

        public TicketsViewModel(
            INavigationService navigationService,
            IPageDialogService dialogService,
            ITokenService tokenService,
            IHttpService httpService,
            ILocalApi localApi
        ) : base(navigationService)
        {
            this.dialogService = dialogService
                ?? throw new ArgumentNullException(nameof(dialogService));

            this.tokenService = tokenService
                ?? throw new ArgumentNullException(nameof(tokenService));

            this.httpService = httpService
                ?? throw new ArgumentNullException(nameof(httpService));

            this.localApi = localApi
                ?? throw new ArgumentNullException(nameof(localApi));
        }

        public async override void OnAppearing()
        {
            Init();

            try
            {
                accessToken = await tokenService.GetAccessTokenAsync();
                Tickets = await GetTicketsAsync();
                Areas = await GetAreasAsync();
            }
            catch (WebException)
            {
                await dialogService.DisplayAlertAsync("Error", "Check connection with server", "OK");

                return;
            }
        }

        private void Init()
        {
            TotalPrice = 0.00M;
        }

        public override void OnNavigatedTo(INavigationParameters navigationParameters)
        {
            this.navigationParameters = navigationParameters;
        }

        private async Task<IEnumerable<TicketType>> GetTicketsAsync()
        {
            var ticketsDto = await httpService.GetAsync<IEnumerable<TicketTypeDto>>(
                    TicketTypesEndpoint.GetTicketTypes, accessToken);

            if (ticketsDto == null)
            {
                accessToken = await tokenService.RefreshTokenAsync();

                ticketsDto = await httpService.GetAsync<IEnumerable<TicketTypeDto>>(
                    TicketTypesEndpoint.GetTicketTypes, accessToken);
            }

            var tickets = AutoMapperConfiguration.Mapper.Map<IEnumerable<TicketType>>(ticketsDto);

            return tickets;
        }

        private async Task<IEnumerable<Area>> GetAreasAsync()
        {
            var areasDto = await httpService.GetAsync<IEnumerable<AreaDto>>(AreasEndpoint.GetAreas, accessToken);

            if (areasDto == null)
            {
                accessToken = await tokenService.RefreshTokenAsync();

                areasDto = await httpService.GetAsync<IEnumerable<AreaDto>>(AreasEndpoint.GetAreas, accessToken);
            }

            var areas = AutoMapperConfiguration.Mapper.Map<IEnumerable<Area>>(areasDto);

            return areas;
        }

        private async void OnChooseTicket()
        {
            //navigationParameters.Add("ticketId", ticket.Id);
            //navigationParameters.Add("durationHours", ticket.DurationHours);
            //navigationParameters.Add("name", ticket.Name);
            //navigationParameters.Add("coefficient", ticket.Coefficient);

            await NavigationService.NavigateAsync(nameof(AreasView), navigationParameters);
        }
    }
}