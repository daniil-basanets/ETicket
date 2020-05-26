using System;
using System.Net;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using ETicketMobile.Business.Mapping;
using ETicketMobile.Data.Entities;
using ETicketMobile.DataAccess.LocalAPI.Interfaces;
using ETicketMobile.Resources;
using ETicketMobile.Views.UserActions;
using ETicketMobile.WebAccess.DTO;
using ETicketMobile.WebAccess.Network;
using ETicketMobile.WebAccess.Network.Endpoints;
using ETicketMobile.WebAccess.Network.WebService;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;

namespace ETicketMobile.ViewModels.Registration
{
    public class ConfirmEmailViewModel : ViewModelBase
    {
        #region Fields

        private readonly INavigationService navigationService;
        private INavigationParameters navigationParameters;

        private readonly IPageDialogService dialogService;

        private readonly ILocalApi localApi;
        
        private readonly HttpClientService httpClient;

        private Timer timer;

        private ICommand navigateToSignInView;
        private ICommand sendActivationCode;

        private string confirmEmailWarning;

        private bool timerActivated;
        private int activationCodeTimer;

        private string email;
        private string password;

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

        #endregion

        public ConfirmEmailViewModel(INavigationService navigationService, IPageDialogService dialogService, ILocalApi localApi) 
            : base(navigationService)
        {
            this.navigationService = navigationService
                ?? throw new ArgumentNullException(nameof(navigationService));

            this.dialogService = dialogService
                ?? throw new ArgumentNullException(nameof(dialogService));

            this.localApi = localApi
                ?? throw new ArgumentNullException(nameof(localApi));

            httpClient = new HttpClientService(ServerConfig.Address);
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
                await RequestActivationCodeAsync(email);
            }
            catch (WebException)
            {
                await dialogService.DisplayAlertAsync("Alert", "Check connection with server", "OK");

                return;
            }
        }

        private async Task RequestActivationCodeAsync(string email)
        {
            await httpClient.PostAsync<string, string>(AuthorizeEndpoint.RequestActivationCode, email);
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

                var token = await GetTokenAsync();
                await localApi.AddAsync(token);
            }
            catch (WebException)
            {
                await dialogService.DisplayAlertAsync("Alert", "Check connection with server", "OK");

                return;
            }

            await navigationService.NavigateAsync(nameof(MainMenuView), navigationParameters);
        }

        private async Task<Token> GetTokenAsync()
        {
            var userSignIn = new UserSignInRequestDto
            {
                Email = email,
                Password = password
            };

            var tokenDto = await httpClient.PostAsync<UserSignInRequestDto, TokenDto>(
                AuthorizeEndpoint.Login, userSignIn);

            var token = AutoMapperConfiguration.Mapper.Map<Token>(tokenDto);

            return token;
        }

        private async Task<bool> ConfirmAndCreateUserAsync(string code)
        {
            var confirmEmailIsSucceeded = await ConfirmEmailAsync(code);
            if (!confirmEmailIsSucceeded)
            {
                ConfirmEmailWarning = AppResource.ConfirmEmailWrong;

                return false;
            }

            var userCreated = await CreateNewUserAsync();

            return userCreated;
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

        private async Task<bool> ConfirmEmailAsync(string activationCode)
        {
            var confirmEmailRequestDto = new ConfirmEmailRequestDto
            {
                Email = navigationParameters.GetValue<string>("email"),
                ActivationCode = activationCode
            };

            var response = await httpClient.PostAsync<ConfirmEmailRequestDto, ConfirmEmailResponseDto>(
                AuthorizeEndpoint.CheckCode,
                confirmEmailRequestDto
            );

            return response.Succeeded;
        }

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

        private async Task<bool> CreateNewUserAsync()
        {
            var user = CreateUserSignUpRequest();

            var response = await httpClient
                .PostAsync<UserSignUpRequestDto, UserSignUpResponseDto>(AuthorizeEndpoint.Registration, user);

            return response.Succeeded;
        }
    }
}