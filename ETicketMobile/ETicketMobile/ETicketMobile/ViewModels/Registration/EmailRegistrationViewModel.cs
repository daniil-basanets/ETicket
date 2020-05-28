using System;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using ETicketMobile.Business.Validators;
using ETicketMobile.Business.Validators.Interfaces;
using ETicketMobile.Resources;
using ETicketMobile.Views.Login;
using ETicketMobile.Views.Registration;
using ETicketMobile.WebAccess.Network.WebServices.Interfaces;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;

namespace ETicketMobile.ViewModels.Registration
{
    public class EmailRegistrationViewModel : ViewModelBase
    {
        #region Fields

        private readonly INavigationService navigationService;
        private readonly IPageDialogService dialogService;
        private readonly IHttpService httpService;

        private ICommand navigateToPhoneRegistrationView;
        private ICommand navigateToSignInView;

        private readonly IUserValidator userValidator;

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
            IHttpService httpService,
            IUserValidator userValidator
        ) : base(navigationService)
        {
            this.navigationService = navigationService
                ?? throw new ArgumentNullException(nameof(navigationService));

            this.dialogService = dialogService
                ?? throw new ArgumentNullException(nameof(dialogService));

            this.httpService = httpService
                ?? throw new ArgumentNullException(nameof(httpService));

            this.userValidator = userValidator
                ?? throw new ArgumentNullException(nameof(userValidator));
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
                await dialogService.DisplayAlertAsync("Error", "Check connection with server", "OK");

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
            if (string.IsNullOrEmpty(email))
            {
                EmailWarning = AppResource.EmailCorrect;

                return false;
            }

            if (!Validator.IsEmailValid(email))
            {
                EmailWarning = AppResource.EmailInvalid;

                return false;
            }

            if (!Validator.HasEmailCorrectLength(email))
            {
                EmailWarning = AppResource.EmailCorrectLong;

                return false;
            }

            if (await userValidator.UserExistsAsync(email))
            {
                EmailWarning = AppResource.EmailTaken;

                return false;
            }

            return true;
        }

        #endregion
    }
}