using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using ETicketMobile.Business.Model.Registration;
using ETicketMobile.Views.Payment;
using ETicketMobile.WebAccess.DTO;
using ETicketMobile.WebAccess.Network;
using ETicketMobile.WebAccess.Network.WebService;
using Prism.Navigation;
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

        private IEnumerable<int> areasId;
        private int ticketTypeId;
        private string email;

        private readonly HttpClientService httpClient;

        private ICommand pay;

        private decimal amount;
        private string description;

        private string cardNumber = string.Empty;
        private string expirationDate = string.Empty;
        private string cvv2 = string.Empty;

        private bool cardNumberWarningIsVisible;
        private bool expirationDateWarningIsVisible;
        private bool cvv2WarningIsVisible;

        #endregion

        #region Properties

        public ICommand Pay => pay
            ?? (pay = new Command(OnPay));

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

        public LiqPayViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            this.navigationService = navigationService
                ?? throw new ArgumentNullException(nameof(navigationService));

            httpClient = new HttpClientService();
        }

        public override void OnAppearing()
        {
            Init();
        }

        private void Init()
        {
            CardNumber = string.Empty;
            ExpirationDate = string.Empty;
            CVV2 = string.Empty;
        }

        public override async void OnNavigatedTo(INavigationParameters navigationParameters)
        {
            Description = navigationParameters.GetValue<string>("name");
            ticketTypeId = navigationParameters.GetValue<int>("ticketId");
            areasId = navigationParameters["areas"] as IEnumerable<int>;
            email = navigationParameters.GetValue<string>("email");

            var price = await RequestGetTicketPrice();
            Amount = Math.Round(price.TotalPrice, 2);
        }

        private async Task<GetTicketPriceResponseDto> RequestGetTicketPrice()
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
            var cardNumber = GetStringWithoutMask(CardNumber);
            if (!IsCardNumberCorrectLength(cardNumber))
            {
                CardNumberWarningIsVisible = true;

                return;
            }

            if (!IsExpirationDateCorrectLength(this.expirationDate))
            {
                CardNumberWarningIsVisible = false;

                ExpirationDateWarningIsVisible = true;

                return;
            }

            var expirationDate = GetExpirationDate();

            if (!IsCvvValid())
                return;

            var buyTicketRequestDto = CreateBuyTicketRequestDto(
                    cardNumber,
                    expirationDate.ExpirationMonth,
                    expirationDate.ExpirationYear,
                    cvv2);

            var response = await RequestBuyTicket(buyTicketRequestDto);

            await navigationService.NavigateAsync(nameof(TransactionCompletedView));
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

        private async Task<BuyTicketResponseDto> RequestBuyTicket(BuyTicketRequestDto buyTicketRequestDto)
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

        #endregion

        private ExpirationDate GetExpirationDate()
        {
            var expirationDate = ExpirationDate.Split('/');

            return new ExpirationDate
            {
                ExpirationMonth = expirationDate[0],
                ExpirationYear = expirationDate[1]
            };
        }
    }
}