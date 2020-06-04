using System;
using System.Net;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using ETicketMobile.Resources;
using ETicketMobile.Views.ForgotPassword;
using ETicketMobile.WebAccess.DTO;
using ETicketMobile.WebAccess.Network.Endpoints;
using ETicketMobile.WebAccess.Network.WebServices.Interfaces;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;

namespace ETicketMobile.ViewModels.ForgotPassword
{
    public class ConfirmForgotPasswordViewModel : ViewModelBase
    {
        #region Fields

        private INavigationParameters navigationParameters;

        private readonly IPageDialogService dialogService;
        private readonly IHttpService httpService;

        private string email;

        private Timer timer;

        private ICommand navigateToCreateNewPasswordView;
        private ICommand sendActivationCode;

        private string confirmEmailWarning;
        private int activationCodeTimer;

        private bool timerActivated;

        #endregion

        #region Properties

        public ICommand NavigateToCreateNewPasswordView => navigateToCreateNewPasswordView
            ??= new Command<string>(OnNavigateToCreateNewPasswordView);

        public ICommand SendActivationCode => sendActivationCode
            ??= new Command(OnSendActivationCode);

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

        public bool TimerActivated
        {
            get => timerActivated;
            set => SetProperty(ref timerActivated, value);
        }

        #endregion

        public ConfirmForgotPasswordViewModel(
            INavigationService navigationService,
            IPageDialogService dialogService,
            IHttpService httpService
        ) : base(navigationService)
        {
            this.httpService = httpService
                ?? throw new ArgumentNullException(nameof(httpService));

            this.dialogService = dialogService
                ?? throw new ArgumentNullException(nameof(dialogService));
        }

        public override void OnAppearing()
        {
            Init();
            InitActivationCodeTimer();
        }

        private void Init()
        {
            TimerActivated = false;
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

            email = navigationParameters.GetValue<string>("email");
        }

        private async void OnSendActivationCode()
        {
            if (ActivationCodeTimer != 0)
                return;

            await SendActivationCodeAsync();

            ActivationCodeTimer = 60;

            timer.Start();

            TimerActivated = true;
        }

        private async Task SendActivationCodeAsync()
        {
            try
            {
                await RequestActivationCodeAsync(email);
            }
            catch (WebException)
            {
                await dialogService.DisplayAlertAsync("Error", "Check connection with server", "OK");

                return;
            }
        }

        private async Task RequestActivationCodeAsync(string email)
        {
            await httpService.PostAsync<string, string>(AuthorizeEndpoint.RequestActivationCode, email);
        }

        private async void OnNavigateToCreateNewPasswordView(string code)
        {
            await NavigateToCreateNewPasswordViewAsync(code);
        }

        private async Task NavigateToCreateNewPasswordViewAsync(string code)
        {
            try
            {
                if (!await IsValidAsync(code))
                    return;
            }
            catch (WebException)
            {
                await dialogService.DisplayAlertAsync("Error", "Check connection with server", "OK");

                return;
            }

            await NavigationService.NavigateAsync(nameof(CreateNewPasswordView), navigationParameters);
        }

        #region Validation

        private async Task<bool> IsValidAsync(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                ConfirmEmailWarning = AppResource.ConfirmEmailEmpty;

                return false;
            }

            var resetPasswordRequestDto = CreateConfirmEmailRequestDto(code);

            var confirmEmailIsSucceeded = await ConfirmEmailAsync(resetPasswordRequestDto);

            if (!confirmEmailIsSucceeded)
            {
                ConfirmEmailWarning = AppResource.ConfirmEmailWrong;

                return false;
            }

            return true;
        }

        private ConfirmEmailRequestDto CreateConfirmEmailRequestDto(string code)
        {
            return new ConfirmEmailRequestDto
            {
                Email = email,
                ActivationCode = code
            };
        }

        private async Task<bool> ConfirmEmailAsync(ConfirmEmailRequestDto confirmEmailRequestDto)
        {
            var response = await httpService.PostAsync<ConfirmEmailRequestDto, ConfirmEmailResponseDto>(
                AuthorizeEndpoint.CheckCode,
                confirmEmailRequestDto
            );

            return response.Succeeded;
        }

        #endregion
    }
}