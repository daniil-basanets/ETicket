using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Android.Util;
using ETicketMobile.Business.Mapping;
using ETicketMobile.Data.Entities;
using ETicketMobile.DataAccess.LocalAPI.Interfaces;
using ETicketMobile.Resources;
using ETicketMobile.Views.ForgotPassword;
using ETicketMobile.Views.Registration;
using ETicketMobile.Views.UserActions;
using ETicketMobile.WebAccess.DTO;
using ETicketMobile.WebAccess.Network;
using ETicketMobile.WebAccess.Network.WebService;
using Prism.Navigation;
using Xamarin.Forms;

namespace ETicketMobile.ViewModels.Login
{
    public class LoginViewModel : ViewModelBase
    {
        #region Fields

        private readonly INavigationService navigationService;
        private readonly ILocalApi localApi;

        private readonly HttpClientService httpClientService;

        private ICommand navigateToRegistrationView;
        private ICommand navigateToForgetPasswordView;
        private ICommand navigateToLoginView;

        private string emailWarning;

        private string email;

        private string password;
        private string passwordPlaceHolder;
        private Color passwordPlaceHolderColor;

        #endregion

        #region Properties

        public ICommand NavigateToForgetPasswordView => navigateToForgetPasswordView 
            ??= new Command(OnNavigateToForgetPasswordView);

        public ICommand NavigateToRegistrationView => navigateToRegistrationView 
            ??= new Command(OnNavigateToRegistrationView);

        public ICommand NavigateToLoginView => navigateToLoginView 
            ??= new Command(OnNavigateToLoginView);

        public string EmailWarning
        {
            get => emailWarning;
            set => SetProperty(ref emailWarning, value);
        }

        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        public string PasswordPlaceHolder
        {
            get => passwordPlaceHolder;
            set => SetProperty(ref passwordPlaceHolder, value);
        }

        public Color PasswordPlaceHolderColor
        {
            get => passwordPlaceHolderColor;
            set => SetProperty(ref passwordPlaceHolderColor, value);
        }

        #endregion

        public LoginViewModel(INavigationService navigationService, ILocalApi localApi)
            : base(navigationService)
        {
            this.navigationService = navigationService
                ?? throw new ArgumentNullException(nameof(navigationService));

            this.localApi = localApi
                ?? throw new ArgumentNullException(nameof(localApi));

            httpClientService = new HttpClientService(ServerConfig.Address);
        }

        public override void OnAppearing()
        {
            Init();
        }

        private void Init()
        {
            PasswordPlaceHolder = AppResource.PasswordPlaceHolderDefault;
        }

        private async void OnNavigateToForgetPasswordView()
        {
            await navigationService.NavigateAsync(nameof(ForgotPasswordView));
        }

        private async void OnNavigateToRegistrationView()
        {
            await navigationService.NavigateAsync(nameof(EmailRegistrationView));
        }

        private async void OnNavigateToLoginView()
        {
            if (!IsValid(email))
                return;

            var token = await GetTokenAsync();
            if (token.RefreshJwtToken == null)
            {
                //TODO UserDoesnExists
                EmailWarning = AppResource.EmailWarning;

                Password = string.Empty;
                PasswordPlaceHolder = AppResource.PasswordPlaceHolderWrong;
                PasswordPlaceHolderColor = Color.Red;

                return;
            }

            await localApi.AddAsync(token);

            var navigationParameters = new NavigationParameters { { "email", Email } };

            await navigationService.NavigateAsync(nameof(MainMenuView), navigationParameters);
        }

        private async Task<Token> GetTokenAsync()
        {
            var userSignIn = new UserSignInRequestDto
            {
                Email = email,
                Password =  password
            };

            var tokenDto = await httpClientService.PostAsync<UserSignInRequestDto, TokenDto>(
                AuthorizeEndpoint.Login, userSignIn);

            var token = AutoMapperConfiguration.Mapper.Map<Token>(tokenDto);

            return token;
        }

        #region Validation

        private bool IsValid(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                EmailWarning = AppResource.EmailEmpty;

                return false;
            }

            if (string.IsNullOrEmpty(password))
            {
                Password = string.Empty;
                PasswordPlaceHolder = AppResource.PasswordEmpty;
                PasswordPlaceHolderColor = Color.Red;

                return false;
            }

            if (!IsEmailValid(email))
            {
                EmailWarning = AppResource.EmailInvalid;

                return false;
            }

            return true;
        }

        private bool IsEmailValid(string email)
        {
            return Patterns.EmailAddress.Matcher(email).Matches();
        }

        #endregion
    }
}