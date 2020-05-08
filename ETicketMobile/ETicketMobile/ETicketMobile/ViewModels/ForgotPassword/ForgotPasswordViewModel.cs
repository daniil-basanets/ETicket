using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Android.Util;
using ETicketMobile.Views.ForgotPassword;
using ETicketMobile.WebAccess.DTO;
using ETicketMobile.WebAccess.Network;
using ETicketMobile.WebAccess.Network.WebService;
using Prism.Navigation;
using Xamarin.Forms;

namespace ETicketMobile.ViewModels.ForgotPassword
{
    public class ForgotPasswordViewModel : ViewModelBase
    {
        #region Fields

        private readonly INavigationService navigationService;

        private ICommand navigateToConfirmForgotPasswordView;

        private readonly HttpClientService httpClient;

        private string emailWarning;

        private const int EmailMaxLength = 50;

        #endregion

        #region Properties

        public ICommand NavigateToConfirmForgotPasswordView => navigateToConfirmForgotPasswordView 
            ?? (navigateToConfirmForgotPasswordView = new Command<string>(OnNavigateToConfirmForgotPasswordView));

        public string EmailWarning
        {
            get => emailWarning;
            set => SetProperty(ref emailWarning, value);
        }

        #endregion

        public ForgotPasswordViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            this.navigationService = navigationService
                ?? throw new ArgumentNullException(nameof(navigationService));

            httpClient = new HttpClientService();
        }

        private async void OnNavigateToConfirmForgotPasswordView(string email)
        {
            if (! await IsValid(email))
                return;

            RequestActivationCode(email);

            var navigationParameters = new NavigationParameters
            {
                { "email", email }
            };

            await navigationService.NavigateAsync(nameof(ConfirmForgotPasswordView), navigationParameters);
        }

        #region Validation

        private async Task<bool> IsValid(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                EmailWarning = ErrorMessage.EmailCorrect;

                return false;
            }

            if (!IsEmailValid(email))
            {
                EmailWarning = ErrorMessage.EmailInvalid;

                return false;
            }

            if (!IsEmailConstainsCorrectLong(email))
            {
                EmailWarning = ErrorMessage.EmailCorrectLong;

                return false;
            }

            var isUserExists = await RequestUserExists(email);

            if (!isUserExists)
            {
                EmailWarning = ErrorMessage.EmailWrong;

                return false;
            }

            return true;
        }

        private async Task<bool> RequestUserExists(string email)
        {
            var signUpRequestDto = new ForgotPasswordRequestDto { Email = email };

            var isUserExists = await httpClient.PostAsync<ForgotPasswordRequestDto, ForgotPasswordResponseDto>(
                TicketsEndpoint.CheckUserExsists, 
                signUpRequestDto);

            return isUserExists.Succeeded;
        }

        private bool IsEmailValid(string email)
        {
            return Patterns.EmailAddress.Matcher(email).Matches();
        }

        private bool IsEmailConstainsCorrectLong(string email)
        {
            return email.Length <= EmailMaxLength;
        }

        #endregion

        private async void RequestActivationCode(string email)
        {
            await httpClient.PostAsync<string, string>(TicketsEndpoint.RequestActivationCode, email);
        }
    }
}