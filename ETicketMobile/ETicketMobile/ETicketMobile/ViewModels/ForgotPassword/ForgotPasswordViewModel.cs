using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ETicketMobile.Business.Exceptions;
using ETicketMobile.Business.Services.Interfaces;
using ETicketMobile.Business.Validators;
using ETicketMobile.Business.Validators.Interfaces;
using ETicketMobile.Resources;
using ETicketMobile.Views.ForgotPassword;
using ETicketMobile.Views.Login;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;

namespace ETicketMobile.ViewModels.ForgotPassword
{
    public class ForgotPasswordViewModel : ViewModelBase
    {
        #region Fields

        private readonly IEmailActivationService emailActivationService;
        private readonly IPageDialogService dialogService;

        private readonly IUserValidator userValidator;

        private ICommand navigateToConfirmForgotPasswordView;
        private ICommand cancelCommand;

        private string emailWarning;

        private bool isDataLoad;

        #endregion

        #region Properties

        public ICommand NavigateToConfirmForgotPasswordView => navigateToConfirmForgotPasswordView
            ??= new Command<string>(OnNavigateToConfirmForgotPasswordView);

        public ICommand CancelCommand => cancelCommand
            ??= new Command(OnCancelCommand);

        public string EmailWarning
        {
            get => emailWarning;
            set => SetProperty(ref emailWarning, value);
        }

        public bool IsDataLoad
        {
            get => isDataLoad;
            set => SetProperty(ref isDataLoad, value);
        }

        #endregion

        public ForgotPasswordViewModel(
            IEmailActivationService emailActivationService,
            INavigationService navigationService,
            IPageDialogService dialogService,
            IUserValidator userValidator
        ) : base(navigationService)
        {
            this.emailActivationService = emailActivationService
                ?? throw new ArgumentNullException(nameof(emailActivationService));

            this.dialogService = dialogService
                ?? throw new ArgumentNullException(nameof(dialogService));

            this.userValidator = userValidator
                ?? throw new ArgumentNullException(nameof(userValidator));
        }

        private async void OnNavigateToConfirmForgotPasswordView(string email)
        {
            await NavigateToConfirmForgotPasswordViewAsync(email);
        }

        private async Task NavigateToConfirmForgotPasswordViewAsync(string email)
        {
            try
            {
                if (!await IsValidAsync(email))
                    return;

                IsDataLoad = true;

                await emailActivationService.RequestActivationCodeAsync(email);
            }
            catch (WebException)
            {
                IsDataLoad = false;

                await dialogService.DisplayAlertAsync(AppResource.Error, AppResource.ErrorConnection, AppResource.Ok);

                return;
            }

            var navigationParameters = new NavigationParameters { { "email", email } };
            await NavigationService.NavigateAsync(nameof(ConfirmForgotPasswordView), navigationParameters);

            IsDataLoad = false;
        }

        private async void OnCancelCommand()
        {
            await NavigationService.NavigateAsync(nameof(LoginView));
        }

        #region Validation

        private async Task<bool> IsValidAsync(string email)
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

            if (!await userValidator.UserExistsAsync(email))
            {
                EmailWarning = AppResource.EmailWrong;

                return false;
            }

            return true;
        }

        #endregion

    }
}