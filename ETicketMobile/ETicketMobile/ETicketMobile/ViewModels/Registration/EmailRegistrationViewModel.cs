using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Android.Util;
using ETicketMobile.Resources;
using ETicketMobile.Views.Login;
using ETicketMobile.Views.Registration;
using ETicketMobile.WebAccess.DTO;
using ETicketMobile.WebAccess.Network;
using ETicketMobile.WebAccess.Network.WebService;
using Prism.Navigation;
using Xamarin.Forms;

namespace ETicketMobile.ViewModels.Registration
{
    public class EmailRegistrationViewModel : ViewModelBase
    {
        #region Constants

        private const int EmailMaxLength = 50;

        #endregion

        #region Fields

        protected INavigationService navigationService;

        private readonly HttpClientService httpClient;

        private ICommand navigateToPhoneRegistrationView;
        private ICommand navigateToSignInView;

        private string emailWarning;

        #endregion

        #region Properties

        public ICommand NavigateToPhoneRegistrationView => navigateToPhoneRegistrationView 
            ?? (navigateToPhoneRegistrationView = new Command<string>(OnMoveToPhoneRegistrationView));

        public ICommand NavigateToSignInView => navigateToSignInView
            ?? (navigateToSignInView = new Command(OnNavigateToSignInView));

        public string EmailWarning
        {
            get => emailWarning;
            set => SetProperty(ref emailWarning, value);
        }

        #endregion

        public EmailRegistrationViewModel(INavigationService navigationService) 
            : base(navigationService)
        {
            this.navigationService = navigationService
                ?? throw new ArgumentNullException(nameof(navigationService));

            httpClient = new HttpClientService(ServerConfig.Address);
        }

        private async void OnMoveToPhoneRegistrationView(string email)
        {
            if (!IsValid(email))
                return;

            if (await CheckUserExistsAsync(email))
                return;

            var navigationParams = new NavigationParameters { { "email", email } };
            await navigationService.NavigateAsync(nameof(PhoneRegistrationView), navigationParams);
        }

        private async void OnNavigateToSignInView()
        {
            await navigationService.NavigateAsync(nameof(LoginView));
        }

        private async Task<bool> RequestUserExistsAsync(string email)
        {
            var signUpRequestDto = new SignUpRequestDto { Email = email };

            var isUserExists = await httpClient.PostAsync<SignUpRequestDto, SignUpResponseDto>(AuthorizeEndpoint.CheckEmail, signUpRequestDto);

            return isUserExists.Succeeded;
        }

        #region Validation

        private bool IsValid(string email)
        {
            if (IsEmailEmpty(email))
            {
                EmailWarning = AppResource.EmailCorrect;

                return false;
            }

            if (!IsEmailValid(email))
            {
                EmailWarning = AppResource.EmailInvalid;

                return false;
            }

            if (!IsEmailConstainsCorrectLong(email))
            {
                EmailWarning = AppResource.EmailCorrectLong;

                return false;
            }

            return true;
        }

        private async Task<bool> CheckUserExistsAsync(string email)
        {
            var isUserExists = await RequestUserExistsAsync(email);

            if (isUserExists)
            {
                EmailWarning = AppResource.EmailTaken;

                return true;
            }

            return false;
        }

        private bool IsEmailEmpty(string email)
        {
            return string.IsNullOrEmpty(email);
        }

        private bool IsEmailValid(string email)
        {
            return Patterns.EmailAddress.Matcher(email).Matches();
        }

        private bool IsEmailConstainsCorrectLong(string email)
        {
            return email.Length <= EmailMaxLength;
        }

        #endregion
    }
}