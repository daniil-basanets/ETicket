using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Android.Util;
using ETicketMobile.Business.Mapping;
using ETicketMobile.Data.Domain.Entities;
using ETicketMobile.DataAccess.Domain.LocalAPI.Interfaces;
using ETicketMobile.Views.ForgotPassword;
using ETicketMobile.Views.Registration;
using ETicketMobile.Views.Tickets;
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

        protected INavigationService navigationService;
        private readonly ILocalApi localApi;

        private readonly HttpClientService httpClient;

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
            ?? (navigateToForgetPasswordView = new Command(OnNavigateToForgetPasswordView));

        public ICommand NavigateToRegistrationView => navigateToRegistrationView
            ?? (navigateToRegistrationView = new Command(OnNavigateToRegistrationView));

        public ICommand NavigateToLoginView => navigateToLoginView
            ?? (navigateToLoginView = new Command(OnNavigateToLoginView));

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

        public LoginViewModel(
            INavigationService navigationService,
            ILocalApi localApi
        ) : base(navigationService)
        {
            this.navigationService = navigationService
                ?? throw new ArgumentNullException(nameof(navigationService));

            this.localApi = localApi
                ?? throw new ArgumentNullException(nameof(localApi));

            httpClient = new HttpClientService();

            FillProperties();
        }

        private void FillProperties()
        {
            PasswordPlaceHolder = "Password";
        }

        private void OnNavigateToForgetPasswordView(object obj)
        {
            navigationService.NavigateAsync(nameof(ForgotPasswordView));
        }

        private void OnNavigateToRegistrationView(object obj)
        {
            navigationService.NavigateAsync(nameof(EmailRegistrationView));
        }

        private async void OnNavigateToLoginView(object obj)
        {
            if (!IsValid(email))
                return;

            var token = await GetTokenAsync();
            if (token == null)
            {
                EmailWarning = "User doesn't exists";

                Password = string.Empty;
                PasswordPlaceHolder = "or wrong password";
                PasswordPlaceHolderColor = Color.Red;

                return;
            }

            await localApi.AddAsync(token);
            await navigationService.NavigateAsync(nameof(TicketsView));
        }

        private async Task<Token> GetTokenAsync()
        {
            var userSignIn = new UserSignInRequestDto
            {
                Email = email,
                Password = password
            };

            var tokenDto = await httpClient.PostAsync<UserSignInRequestDto, TokenDto>(
                TicketsEndpoint.Login, userSignIn);

            var token = AutoMapperConfiguration.Mapper.Map<Token>(tokenDto);

            return token;
        }

        #region Validation

        private bool IsValid(string email)
        {
            if (IsEmpty(email))
            {
                EmailWarning = ErrorMessage.EmailEmpty;

                return false;
            }

            if (IsEmpty(password))
            {
                Password = string.Empty;
                PasswordPlaceHolder = ErrorMessage.PasswordEmpty;
                PasswordPlaceHolderColor = Color.Red;

                return false;
            }

            if (!IsEmailValid(email))
            {
                EmailWarning = ErrorMessage.EmailInvalid;

                return false;
            }

            return true;
        }

        private bool IsEmpty(string field)
        {
            return string.IsNullOrEmpty(field);
        }

        private bool IsEmailValid(string email)
        {
            return Patterns.EmailAddress.Matcher(email).Matches();
        }

        #endregion
    }
}