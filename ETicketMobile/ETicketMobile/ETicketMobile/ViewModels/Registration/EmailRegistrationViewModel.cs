using System;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Android.Util;
using ETicketMobile.Resources;
using ETicketMobile.Views.Login;
using ETicketMobile.Views.Registration;
using ETicketMobile.WebAccess.DTO;
using ETicketMobile.WebAccess.Network.Endpoints;
using ETicketMobile.WebAccess.Network.WebServices.Interfaces;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;

namespace ETicketMobile.ViewModels.Registration
{
    public class EmailRegistrationViewModel : ViewModelBase
    {
        #region Constants

        private const int EmailMaxLength = 50;

        #endregion

        #region Fields

        private readonly INavigationService navigationService;
        private readonly IPageDialogService dialogService;
        private readonly IHttpService httpService;

        private ICommand navigateToPhoneRegistrationView;
        private ICommand navigateToSignInView;

        private string emailWarning;

        #endregion

        #region Properties

        public ICommand NavigateToPhoneRegistrationView => navigateToPhoneRegistrationView 
            ??= new Command<string>(OnMoveToPhoneRegistrationView);

        public ICommand NavigateToSignInView => navigateToSignInView 
            ??= new Command(OnNavigateToSignInView);

        public string EmailWarning
        {
            get => emailWarning;
            set => SetProperty(ref emailWarning, value);
        }

        #endregion

        public EmailRegistrationViewModel(
            INavigationService navigationService,
            IPageDialogService dialogService,
            IHttpService httpService
        ) : base(navigationService)
        {
            this.navigationService = navigationService
                ?? throw new ArgumentNullException(nameof(navigationService));

            this.dialogService = dialogService
                ?? throw new ArgumentNullException(nameof(dialogService));

            this.httpService = httpService
                ?? throw new ArgumentNullException(nameof(httpService));
        }

        private async void OnMoveToPhoneRegistrationView(string email)
        {
            await MoveToPhoneRegistrationViewAsync(email);
        }

        private async Task MoveToPhoneRegistrationViewAsync(string email)
        {
            try
            {
                if (!await IsValid(email))
                    return;
            }
            catch (WebException)
            {
                await dialogService.DisplayAlertAsync("Alert", "Check connection with server", "OK");

                return;
            }

            var navigationParams = new NavigationParameters { { "email", email } };
            await navigationService.NavigateAsync(nameof(PhoneRegistrationView), navigationParams);
        }

        private async void OnNavigateToSignInView()
        {
            await navigationService.NavigateAsync(nameof(LoginView));
        }

        #region Validation

        private async Task<bool> IsValid(string email)
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

            var isUserExists = await RequestUserExistsAsync(email);

            if (isUserExists)
            {
                EmailWarning = AppResource.EmailTaken;

                return false;
            }

            return true;
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

        private async Task<bool> RequestUserExistsAsync(string email)
        {
            var signUpRequestDto = new SignUpRequestDto { Email = email };

            var isUserExists = await httpService.PostAsync<SignUpRequestDto, SignUpResponseDto>(AuthorizeEndpoint.CheckEmail, signUpRequestDto);

            return isUserExists.Succeeded;
        }

        #endregion
    }
}