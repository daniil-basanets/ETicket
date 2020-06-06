using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ETicketMobile.Business.Exceptions;
using ETicketMobile.Business.Services.Interfaces;
using ETicketMobile.Business.Validators;
using ETicketMobile.Data.Entities;
using ETicketMobile.DataAccess.Services.Interfaces;
using ETicketMobile.Resources;
using ETicketMobile.Views.ForgotPassword;
using ETicketMobile.Views.Registration;
using ETicketMobile.Views.UserActions;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;

namespace ETicketMobile.ViewModels.Login
{
    public class LoginViewModel : ViewModelBase
    {
        #region Fields

        private readonly ILocalTokenService localTokenService;
        private readonly IPageDialogService dialogService;
        private readonly ITokenService tokenService;

        private ICommand navigateToRegistrationView;
        private ICommand navigateToForgetPasswordView;
        private ICommand navigateToLoginView;

        private string emailWarning;

        private string password;
        private string passwordWatermark;
        private Color passwordWatermarkColor;

        private bool isDataLoad;

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

        public bool IsDataLoad
        {
            get => isDataLoad;
            set => SetProperty(ref isDataLoad, value);
        }

        #endregion

        public LoginViewModel(
            INavigationService navigationService,
            ILocalTokenService localTokenService,
            IPageDialogService dialogService,
            ITokenService tokenService
        ) : base(navigationService)
        {
            this.localTokenService = localTokenService
                ?? throw new ArgumentNullException(nameof(localTokenService));

            this.dialogService = dialogService
                ?? throw new ArgumentNullException(nameof(dialogService));

            this.tokenService = tokenService
                ?? throw new ArgumentNullException(nameof(tokenService));
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
            await NavigationService.NavigateAsync(nameof(ForgotPasswordView));
        }

        private async void OnNavigateToRegistrationView()
        {
            await NavigationService.NavigateAsync(nameof(EmailRegistrationView));
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
                IsDataLoad = true;

                token = await tokenService.GetTokenAsync(email, password);
            }
            catch (WebException)
            {
                IsDataLoad = false;

                await dialogService.DisplayAlertAsync(AppResource.Error, AppResource.ErrorConnection, AppResource.Ok);

                return;
            }

            if (token.RefreshJwtToken == null)
            {
                IsDataLoad = false;

                EmailWarning = AppResource.EmailWarning;

                Password = string.Empty;
                PasswordWatermark = AppResource.PasswordWatermarkWrong;
                PasswordWatermarkColor = Color.Red;

                return;
            }

            await localTokenService.AddAsync(token);

            var navigationParameters = new NavigationParameters { { "email", email } };
            await NavigationService.NavigateAsync(nameof(MainMenuView), navigationParameters);

            IsDataLoad = false;
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