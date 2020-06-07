using System;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using ETicketMobile.Business.Exceptions;
using ETicketMobile.Business.Services.Interfaces;
using ETicketMobile.Resources;
using ETicketMobile.Views.ForgotPassword;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;

namespace ETicketMobile.ViewModels.ForgotPassword
{
    public class ConfirmForgotPasswordViewModel : ViewModelBase
    {
        #region Fields

        private INavigationParameters navigationParameters;

        private readonly IEmailActivationService emailActivationService;
        private readonly IPageDialogService dialogService;

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
            IEmailActivationService emailActivationService,
            INavigationService navigationService,
            IPageDialogService dialogService
        ) : base(navigationService)
        {
            this.emailActivationService = emailActivationService
                ?? throw new ArgumentNullException(nameof(emailActivationService));

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
            this.navigationParameters = navigationParameters
                ?? throw new ArgumentNullException(nameof(navigationParameters));

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
                await emailActivationService.RequestActivationCodeAsync(email);
            }
            catch (WebException)
            {
                await dialogService.DisplayAlertAsync(AppResource.Error, AppResource.ErrorConnection, AppResource.Ok);

                return;
            }
        }

        private async void OnNavigateToCreateNewPasswordView(string code)
        {
            if (!await TryValidateUserEmailAsync(code))
                return;

            await NavigationService.NavigateAsync(nameof(CreateNewPasswordView), navigationParameters);
        }

        private async Task<bool> TryValidateUserEmailAsync(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                ConfirmEmailWarning = AppResource.ConfirmEmailEmpty;

                return false;
            }

            try
            {
                var emailActivated = await emailActivationService.ActivateEmailAsync(email, code);

                if (emailActivated)
                    return true;

                ConfirmEmailWarning = AppResource.ConfirmEmailWrong;
            }
            catch (WebException)
            {
                await dialogService.DisplayAlertAsync(AppResource.Error, AppResource.ErrorConnection, AppResource.Ok);
            }

            return false;
        }
    }
}