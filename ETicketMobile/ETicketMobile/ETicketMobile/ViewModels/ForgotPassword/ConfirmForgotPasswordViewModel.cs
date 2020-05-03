using System;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using ETicketMobile.Views.ForgotPassword;
using ETicketMobile.WebAccess.DTO;
using ETicketMobile.WebAccess.Network;
using ETicketMobile.WebAccess.Network.WebService;
using Prism.Navigation;
using Xamarin.Forms;

namespace ETicketMobile.ViewModels.ForgotPassword
{
    public class ConfirmForgotPasswordViewModel : ViewModelBase
    {
        #region Fields

        private readonly INavigationService navigationService;
        private INavigationParameters navigationParameters;

        private readonly HttpClientService httpClient;

        private Timer timer;

        private ICommand navigateToCreateNewPasswordView;
        private ICommand sendActivationCode;

        private string confirmEmailWarning;
        private int activationCodeTimer;

        #endregion

        #region Properties

        public ICommand NavigateToCreateNewPasswordView => navigateToCreateNewPasswordView
            ?? (navigateToCreateNewPasswordView = new Command<string>(OnNavigateToCreateNewPasswordView));

        public ICommand SendActivationCode => sendActivationCode
            ?? (sendActivationCode = new Command(OnSendActivationCode));

        public string ConfirmEmailWarning
        {
            get => confirmEmailWarning;
            set => SetProperty(ref confirmEmailWarning, value);
        }

        public int ActivationCodeTimer
        {
            get => activationCodeTimer;
            set => SetProperty(ref activationCodeTimer, value);
        }

        #endregion

        public ConfirmForgotPasswordViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            this.navigationService = navigationService
                ?? throw new ArgumentNullException(nameof(navigationService));

            httpClient = new HttpClientService();

            InitActivationCodeTimer();
        }

        #region Timer

        private void InitActivationCodeTimer()
        {
            timer = new Timer { Interval = 1000 };
            timer.Elapsed += TimerElapsed;

            ActivationCodeTimer = 0;
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e) 
        {
            ActivationCodeTimer--;

            if (ActivationCodeTimer <= 0)
                timer.Stop();
        }

        #endregion

        public override void OnNavigatedTo(INavigationParameters navigationParameters)
        {
            this.navigationParameters = navigationParameters;
        }

        private void OnSendActivationCode()
        {
            if (ActivationCodeTimer > 0)
                return;

            var email = navigationParameters.GetValue<string>("email");
            RequestActivationCode(email);

            ActivationCodeTimer = 60;

            timer.Start();
        }

        private async void RequestActivationCode(string email)
        {
            await httpClient.PostAsync<string, string>(TicketsEndpoint.RequestActivationCode, email);
        }

        private async void OnNavigateToCreateNewPasswordView(string code)
        {
            if (! await IsValid(code))
                return;

            await navigationService.NavigateAsync(nameof(CreateNewPasswordView), navigationParameters);
        }

        #region Validation

        private async Task<bool> IsValid(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                ConfirmEmailWarning = ErrorMessage.ConfirmEmailEmpty;

                return false;
            }

            var confirmEmailIsSucceeded = await ConfirmEmail(code);
            if (!confirmEmailIsSucceeded)
            {
                ConfirmEmailWarning = ErrorMessage.ConfirmEmailWrong;

                return false;
            }

            return true;
        }

        private async Task<bool> ConfirmEmail(string activationCode)
        {
            var response = await httpClient.PostAsync<string, ConfirmEmailResponseDto>(
                TicketsEndpoint.ConfirmEmail,
                activationCode
            );

            return response.Succeeded;
        }

        #endregion
    }
}