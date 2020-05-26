using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using ETicketMobile.Business.Model.Registration;
using ETicketMobile.Views.Payment;
using ETicketMobile.WebAccess.DTO;
using ETicketMobile.WebAccess.Network;
using ETicketMobile.WebAccess.Network.WebService;
using Java.Net;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;

namespace ETicketMobile.ViewModels.Payment
{
    public class LiqPayViewModel : ViewModelBase
    {
        #region Constants

        private const int CardNumberLength = 16;
        private const int ExpirationDateLength = 5;
        private const int CVV2Length = 3;

        #endregion

        #region Fields

        private readonly INavigationService navigationService;
        private readonly IPageDialogService dialogService;

        private IEnumerable<int> areasId;
        private int ticketTypeId;
        private string email;

        private readonly HttpClientService httpClient;

        private ICommand pay;

        private decimal amount;
        private string description;

        private string cardNumber;
        private string expirationDate;
        private string cvv2;

        private bool cardNumberWarningIsVisible;
        private bool expirationDateWarningIsVisible;
        private bool cvv2WarningIsVisible;

        #endregion

        #region Properties

        public ICommand Pay => pay ??= new Command(OnPay);

        public decimal Amount
        {
            get => amount;
            set => SetProperty(ref amount, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public bool CardNumberWarningIsVisible
        {
            get => cardNumberWarningIsVisible;
            set => SetProperty(ref cardNumberWarningIsVisible, value);
        }

        public bool ExpirationDateWarningIsVisible
        {
            get => expirationDateWarningIsVisible;
            set => SetProperty(ref expirationDateWarningIsVisible, value);
        }

        public bool CVV2WarningIsVisible
        {
            get => cvv2WarningIsVisible;
            set => SetProperty(ref cvv2WarningIsVisible, value);
        }

        public string CardNumber
        {
            get => cardNumber;
            set => SetProperty(ref cardNumber, value);
        }

        public string ExpirationDate
        {
            get => expirationDate;
            set => SetProperty(ref expirationDate, value);
        }

        public string CVV2
        {
            get => cvv2;
            set => SetProperty(ref cvv2, value);
        }

        #endregion

        public LiqPayViewModel(INavigationService navigationService, IPageDialogService dialogService)
            : base(navigationService)
        {
            this.navigationService = navigationService
                ?? throw new ArgumentNullException(nameof(navigationService));

            this.dialogService = dialogService
                ?? throw new ArgumentNullException(nameof(dialogService));

            httpClient = new HttpClientService(ServerConfig.Address);
        }

        public override void OnAppearing()
        {
            Init();
        }

        private void Init()
        {
            ExpirationDate = string.Empty;
            CVV2 = string.Empty;

            expirationDate = string.Empty;
            cvv2 = string.Empty;
        }

        public override async void OnNavigatedTo(INavigationParameters navigationParameters)
        {
            Description = navigationParameters.GetValue<string>("name");
            ticketTypeId = navigationParameters.GetValue<int>("ticketId");
            areasId = navigationParameters["areas"] as IEnumerable<int>;
            email = navigationParameters.GetValue<string>("email");

            try
            {
                var price = await RequestGetTicketPriceAsync();
                Amount = Math.Round(price.TotalPrice, 2);
            }
            catch (WebException)
            {
                await dialogService.DisplayAlertAsync("Alert", "Check connection with server", "OK");

                return;
            }
        }

        private async Task<GetTicketPriceResponseDto> RequestGetTicketPriceAsync()
        {
            var getTicketPriceRequestDto = new GetTicketPriceRequestDto
            {
                AreasId = areasId,
                TicketTypeId = ticketTypeId
            };

            var response = await httpClient.PostAsync<GetTicketPriceRequestDto, GetTicketPriceResponseDto>(
                    TicketsEndpoint.GetTicketPrice, getTicketPriceRequestDto);

            return response;
        }

        private async void OnPay()
        {
            await PayAsync();
        }

        private async Task PayAsync()
        {
            var cardNumber = GetStringWithoutMask(this.cardNumber);

            if (!IsValid(cardNumber))
                return;

            var expirationDateDescriptor = GetExpirationDateDescriptor();

            var buyTicketRequestDto = CreateBuyTicketRequestDto(
                    cardNumber,
                    expirationDateDescriptor.ExpirationMonth,
                    expirationDateDescriptor.ExpirationYear,
                    cvv2);

            try
            {
                var response = await RequestBuyTicketAsync(buyTicketRequestDto);
            }
            catch (WebException)
            {
                await dialogService.DisplayAlertAsync("Alert", "Check connection with server", "OK");

                return;
            }
            catch (SocketException)
            {
                await dialogService.DisplayAlertAsync("Alert", "Check connection with server", "OK");

                return;
            }

            await navigationService.NavigateAsync(nameof(TransactionCompletedView));
        }

        private async Task<BuyTicketResponseDto> RequestBuyTicketAsync(BuyTicketRequestDto buyTicketRequestDto)
        {
            var response = await httpClient.PostAsync<BuyTicketRequestDto, BuyTicketResponseDto>(
                TicketsEndpoint.BuyTicket, buyTicketRequestDto);

            return response;
        }

        private BuyTicketRequestDto CreateBuyTicketRequestDto(
            string cardNumber,
            string expirationMonth,
            string expirationYear,
            string cvv2
        )
        {
            return new BuyTicketRequestDto
            {
                TicketTypeId = ticketTypeId,
                Email = email,
                Price = Amount,
                Description = Description,
                CardNumber = cardNumber,
                ExpirationMonth = expirationMonth,
                ExpirationYear = expirationYear,
                CVV2 = cvv2
            };
        }

        private bool IsValid(string cardNumber)
        {
            if (!IsCardNumberCorrectLength(cardNumber))
            {
                CardNumberWarningIsVisible = true;

                return false;
            }

            if (!IsExpirationDateCorrectLength(expirationDate))
            {
                CardNumberWarningIsVisible = false;

                ExpirationDateWarningIsVisible = true;

                return false;
            }

            if (!IsCvvValid())
                return false;

            return true;
        }

        private string GetStringWithoutMask(string str)
        {
            return Regex.Replace(str, @"[^\d]", string.Empty);
        }

        #region Validation

        private bool IsCardNumberCorrectLength(string cardNumber)
        {
            return cardNumber.Length == CardNumberLength;
        }

        private bool IsExpirationDateCorrectLength(string expirationDate)
        {
            return expirationDate.Length == ExpirationDateLength;
        }

        private bool IsCVV2CorrectLength(string cvv2)
        {
            return cvv2.Length == CVV2Length;
        }

        private bool IsCvvValid()
        {
            var cvv2 = GetStringWithoutMask(CVV2);

            if (cvv2 != CVV2 || !IsCVV2CorrectLength(cvv2))
            {
                CardNumberWarningIsVisible = false;
                ExpirationDateWarningIsVisible = false;

                CVV2WarningIsVisible = true;

                return false;
            }

            return true;
        }

        #endregion

        private ExpirationDateDescriptor GetExpirationDateDescriptor()
        {
            var expirationDate = ExpirationDate.Split('/');

            return new ExpirationDateDescriptor
            {
                ExpirationMonth = expirationDate[0],
                ExpirationYear = expirationDate[1]
            };
        }
    }
}