using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using ETicketMobile.Business.Mapping;
using ETicketMobile.Business.Model.Tickets;
using ETicketMobile.Business.Services.Interfaces;
using ETicketMobile.Business.Validators;
using ETicketMobile.DataAccess.LocalAPI.Interfaces;
using ETicketMobile.Views.Payment;
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

        private INavigationParameters navigationParameters;

        private readonly IPageDialogService dialogService;
        private readonly ITokenService tokenService;
        private readonly IHttpService httpService;

        private readonly ILocalApi localApi;

        private IList<TicketType> tickets;
        private IList<AreaViewModel> areas;

        private TicketType ticketSelected;
        private string selectedTicket;

        private string selectedArea;

        private string accessToken;

        private decimal totalPrice;

        private ICommand chooseTicket;

        #endregion

        #region Properties

        public ICommand ChooseTicket => chooseTicket
            ??= new Command(OnChooseTicket);

        public IList<TicketType> Tickets
        {
            get => tickets;
            set => SetProperty(ref tickets, value);
        }

        public IList<AreaViewModel> Areas
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

                var selectedAreas = Areas.Where(x => x.Selected);
                if (selectedAreas.Count() == 0)
                    return;

                CountTotalPrice(selectedAreas.Select(a => a.Id)).ConfigureAwait(false);
            }
        }

        public string SelectedTicket
        {
            get => selectedTicket;
            set => SetProperty(ref selectedTicket, value);
        }

        public string SelectedAreas
        {
            get => selectedArea;
            set => SetProperty(ref selectedArea, value);
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
            this.navigationParameters = navigationParameters
                ?? throw new ArgumentNullException(nameof(navigationParameters));
        }

        private async Task<IList<TicketType>> GetTicketsAsync()
        {
            var ticketsDto = await httpService.GetAsync<IEnumerable<TicketTypeDto>>(
                    TicketTypesEndpoint.GetTicketTypes, accessToken);

            if (ticketsDto == null)
            {
                accessToken = await tokenService.RefreshTokenAsync();

                ticketsDto = await httpService.GetAsync<IList<TicketTypeDto>>(
                    TicketTypesEndpoint.GetTicketTypes, accessToken);
            }

            var tickets = AutoMapperConfiguration.Mapper.Map<IList<TicketType>>(ticketsDto);

            return tickets;
        }

        private async Task<IList<AreaViewModel>> GetAreasAsync()
        {
            var areasDto = await httpService.GetAsync<IList<AreaDto>>(AreasEndpoint.GetAreas, accessToken);

            if (areasDto == null)
            {
                accessToken = await tokenService.RefreshTokenAsync();

                areasDto = await httpService.GetAsync<IList<AreaDto>>(AreasEndpoint.GetAreas, accessToken);
            }

            var areas = areasDto
                .Select(
                    a => new AreaViewModel()
                    {
                        Id = a.Id,
                        Name = a.Name,
                        Description = a.Description,
                        SelectionChanged = UpdateAreaInfo
                    }
                )
                .ToList();

            return areas;
        }

        private async void UpdateAreaInfo()
        {
            var selectedAreas = Areas.Where(a => a.Selected);

            SelectedAreas = $"({string.Join(", ", selectedAreas.Select(a => a.Name))})";

            await CountTotalPrice(selectedAreas.Select(a => a.Id));
        }

        private async Task CountTotalPrice(IEnumerable<int> selectedAreas)
        {
            if (ticketSelected == null
             || selectedArea.Length == 0)
            {
                return;
            }

            var price = await RequestGetTicketPriceAsync(selectedAreas, TicketSelected.Id);
            TotalPrice = Math.Round(price.TotalPrice, 2);
        }

        private async Task<GetTicketPriceResponseDto> RequestGetTicketPriceAsync(IEnumerable<int> areasId, int ticketTypeId)
        {
            var getTicketPriceRequestDto = new GetTicketPriceRequestDto
            {
                AreasId = areasId,
                TicketTypeId = ticketTypeId
            };

            var response = await httpService.PostAsync<GetTicketPriceRequestDto, GetTicketPriceResponseDto>(
                    TicketsEndpoint.GetTicketPrice, getTicketPriceRequestDto);

            return response;
        }

        private async void OnChooseTicket()
        {
            if (!await IsValid())
                return;

            navigationParameters.Add("ticketId", TicketSelected.Id);
            navigationParameters.Add("ticketName", TicketSelected.Name);
            navigationParameters.Add("areas", Areas.Where(a => a.Selected).Select(a => a.Id));
            navigationParameters.Add("totalPrice", TotalPrice);

            await NavigationService.NavigateAsync(nameof(LiqPayView), navigationParameters);
        }

        #region Validation

        private async Task<bool> IsValid()
        {
            if (!Validator.TicketChoosed(Tickets.Count))
            {
                await dialogService.DisplayAlertAsync("Warning", "Choose ticket", "OK");

                return false;
            }

            if (!Validator.AreaChoosed(Areas.Where(a => a.Selected).Count()))
            {
                await dialogService.DisplayAlertAsync("Warning", "Choose Areas", "OK");

                return false;
            }

            return true;
        }

        #endregion
    }
}