using System;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using ETicketMobile.Business.Exceptions;
using ETicketMobile.Business.Services.Interfaces;
using ETicketMobile.DataAccess.Services.Interfaces;
using ETicketMobile.Resources;
using ETicketMobile.Views.UserActions;
using ETicketMobile.WebAccess.DTO;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;

namespace ETicketMobile.ViewModels.Registration
{
    public class ConfirmEmailViewModel : ViewModelBase
    {
        #region Fields

        private INavigationParameters navigationParameters;

        private readonly IEmailActivationService emailActivationService;
        private readonly ILocalTokenService localTokenService;
        private readonly IPageDialogService dialogService;
        private readonly ITokenService tokenService;
        private readonly IUserService userService;

        private Timer timer;

        private ICommand navigateToSignInView;
        private ICommand sendActivationCode;

        private string confirmEmailWarning;

        private bool timerActivated;
        private int activationCodeTimer;

        private string email;
        private string password;

        private bool isDataLoad;

        #endregion

        #region Properties

        public ICommand NavigateToSignInView => navigateToSignInView
            ??= new Command<string>(OnNavigateToSignInView);

        public ICommand SendActivationCode => sendActivationCode
            ??= new Command(OnSendActivationCode);

        public string ConfirmEmailWarning
        {
            get => confirmEmailWarning;
            set => SetProperty(ref confirmEmailWarning, value);
        }

        public bool TimerActivated
        {
            get => timerActivated;
            set => SetProperty(ref timerActivated, value);
        }

        public int ActivationCodeTimer
        {
            get => activationCodeTimer;
            set => SetProperty(ref activationCodeTimer, value);
        }

        public bool IsDataLoad
        {
            get => isDataLoad;
            set => SetProperty(ref isDataLoad, value);
        }

        #endregion

        public ConfirmEmailViewModel(
            IEmailActivationService emailActivationService,
            INavigationService navigationService,
            ILocalTokenService localTokenService,
            IPageDialogService dialogService,
            ITokenService tokenService,
            IUserService userService
        ) : base(navigationService)
        {
            this.emailActivationService = emailActivationService
                ?? throw new ArgumentNullException(nameof(emailActivationService));

            this.localTokenService = localTokenService
                ?? throw new ArgumentNullException(nameof(localTokenService));

            this.dialogService = dialogService
                ?? throw new ArgumentNullException(nameof(dialogService));

            this.tokenService = tokenService
                ?? throw new ArgumentNullException(nameof(tokenService));

            this.userService = userService
                ?? throw new ArgumentNullException(nameof(userService));
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

        private void InitActivationCodeTimer()
        {
            timer = new Timer { Interval = 1000 };
            timer.Elapsed += TimerElapsed;

            ActivationCodeTimer = 0;
        }

        public override void OnNavigatedTo(INavigationParameters navigationParameters)
        {
            this.navigationParameters = navigationParameters;

            email = navigationParameters.GetValue<string>("email");
            password = navigationParameters.GetValue<string>("password");
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            ActivationCodeTimer--;

            if (ActivationCodeTimer <= 0)
                timer.Stop();
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
            var email = navigationParameters.GetValue<string>("email");

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

        private async void OnNavigateToSignInView(string code)
        {
            await NavigateToSignInViewAsync(code);
        }

        private async Task NavigateToSignInViewAsync(string code)
        {
            if (!IsValid(code))
                return;

            try
            {
                var userCreated = await ConfirmAndCreateUserAsync(code);

                if (!userCreated)
                    return;

                IsDataLoad = true;

                var token = await tokenService.GetTokenAsync(email, password);
                await localTokenService.AddAsync(token);
            }
            catch (WebException)
            {
                IsDataLoad = false;

                await dialogService.DisplayAlertAsync(AppResource.Error, AppResource.ErrorConnection, AppResource.Ok);

                return;
            }

            await NavigationService.NavigateAsync(nameof(MainMenuView), navigationParameters);

            IsDataLoad = false;
        }

        private async Task<bool> ConfirmAndCreateUserAsync(string code)
        {
            var emailActivated = await emailActivationService.ActivateEmailAsync(email, code);
            if (!emailActivated)
            {
                ConfirmEmailWarning = AppResource.ConfirmEmailWrong;

                return false;
            }

            await CreateNewUserAsync();

            return true;
        }

        #region Validation

        private bool IsValid(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                ConfirmEmailWarning = AppResource.ConfirmEmailEmpty;

                return false;
            }

            return true;
        }

        #endregion

        private UserSignUpRequestDto CreateUserSignUpRequest()
        {
            return new UserSignUpRequestDto
            {
                Email = email,
                Password = password,
                Phone = navigationParameters.GetValue<string>("phone"),
                FirstName = navigationParameters.GetValue<string>("firstName"),
                LastName = navigationParameters.GetValue<string>("lastName"),
                DateOfBirth = navigationParameters.GetValue<DateTime>("birth")
            };
        }

        private async Task CreateNewUserAsync()
        {
            var user = CreateUserSignUpRequest();

            await userService.CreateNewUserAsync(user);
        }
    }
}