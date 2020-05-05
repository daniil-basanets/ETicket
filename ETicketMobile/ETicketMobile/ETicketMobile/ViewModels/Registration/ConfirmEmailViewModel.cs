using System;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using ETicketMobile.Views.Login;
using ETicketMobile.WebAccess.DTO;
using ETicketMobile.WebAccess.Network;
using ETicketMobile.WebAccess.Network.WebService;
using Prism.Navigation;
using Xamarin.Forms;

namespace ETicketMobile.ViewModels.Registration
{
    public class ConfirmEmailViewModel : ViewModelBase
    {
        #region Fields

        private readonly INavigationService navigationService;
        private INavigationParameters navigationParameters;
        
        private readonly HttpClientService httpClient;

        private Timer timer;

        private ICommand navigateToSignInView;
        private ICommand sendActivationCode;

        private string confirmEmailWarning;

        private bool timerActivated;
        private int activationCodeTimer;

        #endregion

        #region Properties

        public ICommand NavigateToSignInView => navigateToSignInView
            ?? (navigateToSignInView = new Command<string>(OnNavigateToSignInView));

        public ICommand SendActivationCode => sendActivationCode
            ?? (sendActivationCode = new Command(OnSendActivationCode));

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

        #endregion

        public ConfirmEmailViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            this.navigationService = navigationService
                ?? throw new ArgumentNullException(nameof(navigationService));

            httpClient = new HttpClientService();

            Init();

            InitActivationCodeTimer();
        }

        public override void OnNavigatedTo(INavigationParameters navigationParameters)
        {
            this.navigationParameters = navigationParameters;
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

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            ActivationCodeTimer--;

            if (ActivationCodeTimer <= 0)
                timer.Stop();
        }

        private void OnSendActivationCode()
        {
            if (ActivationCodeTimer > 0)
                return;

            var email = navigationParameters.GetValue<string>("email");
            RequestActivationCode(email);

            ActivationCodeTimer = 60;

            timer.Start();

            TimerActivated = true;
        }

        private async void RequestActivationCode(string email)
        {
            await httpClient.PostAsync<string, string>(TicketsEndpoint.RequestActivationCode, email);
        }

        private async void OnNavigateToSignInView(string code)
        {
            if (!IsValid(code))
                return;

            var userCreated = await ConfirmAndCreateUser(code);

            if (userCreated)
                await navigationService.NavigateAsync(nameof(LoginView));
        }

        private async Task<bool> ConfirmAndCreateUser(string code)
        {
            var confirmEmailIsSucceeded = await ConfirmEmail(code);
            if (!confirmEmailIsSucceeded)
            {
                ConfirmEmailWarning = "Wrong activation code";

                return false;
            }

            var userCreated = await CreateNewUser();

            return userCreated;
        }

        #region Validation

        private bool IsValid(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                ConfirmEmailWarning = "Enter activation code";

                return false;
            }

            return true;
        }

        #endregion

        private async Task<bool> ConfirmEmail(string activationCode)
        {
            var confirmEmailRequestDto = new ConfirmEmailRequestDto
            {
                Email = navigationParameters.GetValue<string>("email"),
                ActivationCode = activationCode
            };

            var response = await httpClient.PostAsync<ConfirmEmailRequestDto, ConfirmEmailResponseDto>(
                TicketsEndpoint.ConfirmEmail,
                confirmEmailRequestDto
            );

            return response.Succeeded;
        }

        private UserSignUpRequestDto CreateUserSignUpRequest()
        {
            return new UserSignUpRequestDto
            {
                Email = navigationParameters.GetValue<string>("email"),
                Password = navigationParameters.GetValue<string>("password"),
                Phone = navigationParameters.GetValue<string>("phone"),
                FirstName = navigationParameters.GetValue<string>("firstName"),
                LastName = navigationParameters.GetValue<string>("lastName"),
                DateOfBirth = navigationParameters.GetValue<DateTime>("birth")
            };
        }

        private async Task<bool> CreateNewUser()
        {
            var user = CreateUserSignUpRequest();

            var response = await httpClient
                .PostAsync<UserSignUpRequestDto, UserSignUpResponseDto>(
                    TicketsEndpoint.Registration,
                    user
            );

            return response.Succeeded;
        }
    }
}