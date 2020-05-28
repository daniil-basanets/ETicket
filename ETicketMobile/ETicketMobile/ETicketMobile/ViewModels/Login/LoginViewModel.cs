using System;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using ETicketMobile.Business.Mapping;
using ETicketMobile.Business.Validators;
using ETicketMobile.Data.Entities;
using ETicketMobile.DataAccess.LocalAPI.Interfaces;
using ETicketMobile.Resources;
using ETicketMobile.Views.ForgotPassword;
using ETicketMobile.Views.Registration;
using ETicketMobile.Views.UserActions;
using ETicketMobile.WebAccess.DTO;
using ETicketMobile.WebAccess.Network.Endpoints;
using ETicketMobile.WebAccess.Network.WebServices.Interfaces;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;

namespace ETicketMobile.ViewModels.Login
{
    public class LoginViewModel : ViewModelBase
    {
        #region Fields

        private readonly INavigationService navigationService;
        private readonly IPageDialogService dialogService;
        private readonly IHttpService httpService;

        private readonly ILocalApi localApi;

        private ICommand navigateToRegistrationView;
        private ICommand navigateToForgetPasswordView;
        private ICommand navigateToLoginView;

        private string emailWarning;

        private string password;
        private string passwordWatermark;
        private Color passwordWatermarkColor;

        #endregion

        #region Properties

        public ICommand NavigateToForgetPasswordView => navigateToForgetPasswordView 
            ??= new Command(OnNavigateToForgetPasswordView);

        public ICommand NavigateToRegistrationView => navigateToRegistrationView 
            ??= new Command(OnNavigateToRegistrationView);

        public ICommand NavigateToLoginView => navigateToLoginView 
            ??= new Command<string>(OnNavigateToLoginView);

        public string EmailWarning
        {
            get => emailWarning;
            set => SetProperty(ref emailWarning, value);
        }

        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        public string PasswordWatermark
        {
            get => passwordWatermark;
            set => SetProperty(ref passwordWatermark, value);
        }

        public Color PasswordWatermarkColor
        {
            get => passwordWatermarkColor;
            set => SetProperty(ref passwordWatermarkColor, value);
        }

        #endregion

        public LoginViewModel(
            INavigationService navigationService,
            IPageDialogService dialogService,
            IHttpService httpService,
            ILocalApi localApi
        ) : base(navigationService)
        {
            this.navigationService = navigationService
                ?? throw new ArgumentNullException(nameof(navigationService));

            this.localApi = localApi
                ?? throw new ArgumentNullException(nameof(localApi));

            this.dialogService = dialogService
                ?? throw new ArgumentNullException(nameof(dialogService));

            this.httpService = httpService
                ?? throw new ArgumentNullException(nameof(httpService));
        }

        public override void OnAppearing()
        {
            Init();
        }

        private void Init()
        {
            PasswordWatermark = AppResource.PasswordWatermarkDefault;
        }

        private async void OnNavigateToForgetPasswordView()
        {
            await navigationService.NavigateAsync(nameof(ForgotPasswordView));
        }

        private async void OnNavigateToRegistrationView()
        {
            await navigationService.NavigateAsync(nameof(EmailRegistrationView));
        }

        private async void OnNavigateToLoginView(string email)
        {
            if (!IsValid(email))
                return;

            await NavigateToLoginViewAsync(email);
        }

        private async Task NavigateToLoginViewAsync(string email)
        {
            Token token = null;

            try
            {
                token = await GetTokenAsync(email);
            }
            catch (WebException)
            {
                await dialogService.DisplayAlertAsync("Error", "Check connection with server", "OK");

                return;
            }

            if (token.RefreshJwtToken == null)
            {
                //TODO UserDoesnExists
                EmailWarning = AppResource.EmailWarning;

                Password = string.Empty;
                PasswordWatermark = AppResource.PasswordWatermarkWrong;
                PasswordWatermarkColor = Color.Red;

                return;
            }

            await localApi.AddAsync(token);

            var navigationParameters = new NavigationParameters { { "email", email } };

            await navigationService.NavigateAsync(nameof(MainMenuView), navigationParameters);
        }

        private async Task<Token> GetTokenAsync(string email)
        {
            var userSignIn = new UserSignInRequestDto
            {
                Email = email,
                Password = password
            };

            var tokenDto = await httpService.PostAsync<UserSignInRequestDto, TokenDto>(
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
                PasswordWatermark = AppResource.PasswordEmpty;
                PasswordWatermarkColor = Color.Red;

                return false;
            }

            if (!Validator.IsEmailValid(email))
            {
                EmailWarning = AppResource.EmailInvalid;

                return false;
            }

            return true;
        }

        #endregion
    }
}