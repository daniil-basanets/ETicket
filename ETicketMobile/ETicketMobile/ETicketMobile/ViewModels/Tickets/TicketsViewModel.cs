using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ETicketMobile.Business.Exceptions;
using ETicketMobile.Business.Model.Tickets;
using ETicketMobile.Business.Services.Interfaces;
using ETicketMobile.Business.Validators;
using ETicketMobile.DataAccess.Services.Interfaces;
using ETicketMobile.Resources;
using ETicketMobile.Views.Payment;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;

namespace ETicketMobile.ViewModels.Tickets
{
    public class TicketsViewModel : ViewModelBase
    {
        #region Fields

        private INavigationParameters navigationParameters;

        private readonly ILocalTokenService localTokenService;
        private readonly IPageDialogService dialogService;
        private readonly ITicketsService ticketsService;
        private readonly ITokenService tokenService;

        private IList<TicketType> tickets;
        private IList<AreaViewModel> areas;

        private TicketType ticketSelected;
        private string selectedTicket;

        private string selectedArea;

        private decimal totalPrice;

        private ICommand chooseTicket;

        #endregion

        #region Properties

        public ICommand ChooseTicket => chooseTicket
            ??= new Command(OnGoToPayment);

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
            ILocalTokenService localTokenService,
            IPageDialogService dialogService,
            ITicketsService ticketsService,
            ITokenService tokenService
        ) : base(navigationService)
        {
            this.localTokenService = localTokenService
                ?? throw new ArgumentNullException(nameof(localTokenService));

            this.dialogService = dialogService
                ?? throw new ArgumentNullException(nameof(dialogService));

            this.ticketsService = ticketsService
                ?? throw new ArgumentNullException(nameof(ticketsService));

            this.tokenService = tokenService
                ?? throw new ArgumentNullException(nameof(tokenService));
        }

        public async override void OnAppearing()
        {
            Init();

            try
            {
                var accessToken = await localTokenService.GetAccessTokenAsync();

                Tickets = await ticketsService.GetTicketTypesAsync(accessToken);
                Areas = await GetAreasAsync(accessToken);
            }
            catch (WebException)
            {
                await dialogService.DisplayAlertAsync(AppResource.Error, AppResource.ErrorConnection, AppResource.Ok);

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

        private async void UpdateAreaInfo()
        {
            var selectedAreas = Areas.Where(x => x.Selected);

            SelectedAreas = $"({string.Join(", ", selectedAreas.Select(x => x.Name))})";

            await CountTotalPrice(selectedAreas.Select(a => a.Id));
        }

        private async Task CountTotalPrice(IEnumerable<int> selectedAreas)
        {
            if (ticketSelected == null
             || selectedArea.Length == 0)
            {
                return;
            }

            var price = await ticketsService.RequestGetTicketPriceAsync(selectedAreas, TicketSelected.Id);

            TotalPrice = Math.Round(price.TotalPrice, 2);
        }

        private async Task<IList<AreaViewModel>> GetAreasAsync(string accessToken)
        {
            var areasDto = await ticketsService.GetAreasDtoAsync(accessToken);

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

        private async void OnGoToPayment()
        {
            if (!await IsValid())
                return;

            var areasId = Areas
                    .Where(a => a.Selected)
                    .Select(a => a.Id)
                    .ToArray();

            navigationParameters.Add("ticketId", TicketSelected.Id);
            navigationParameters.Add("ticketName", TicketSelected.Name);
            navigationParameters.Add("areas", areasId);
            navigationParameters.Add("totalPrice", TotalPrice);

            await NavigationService.NavigateAsync(nameof(LiqPayView), navigationParameters);
        }

        #region Validation

        private async Task<bool> IsValid()
        {
            if (!Validator.TicketChoosed(Tickets.Count))
            {
                await dialogService.DisplayAlertAsync("Warning", "Choose ticket", AppResource.Ok);

                return false;
            }

            if (!Validator.AreaChoosed(Areas.Where(a => a.Selected).Count()))
            {
                await dialogService.DisplayAlertAsync("Warning", "Choose Areas", AppResource.Ok);

                return false;
            }

            return true;
        }

        #endregion
    }
}