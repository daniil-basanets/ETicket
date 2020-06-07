using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ETicketMobile.Business.Exceptions;
using ETicketMobile.Business.Services.Interfaces;
using ETicketMobile.Business.Validators;
using ETicketMobile.Resources;
using ETicketMobile.Views.Login;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;

namespace ETicketMobile.ViewModels.ForgotPassword
{
    public class CreateNewPasswordViewModel : ViewModelBase
    {
        #region Fields

        private INavigationParameters navigationParameters;

        private readonly IPageDialogService dialogService;
        private readonly IUserService userService;

        private ICommand navigateToSignInView;

        private string passwordWarning;

        private string confirmPassword;
        private string confirmPasswordWarning;

        private bool isDataLoad;

        #endregion

        #region Properties

        public ICommand NavigateToSignInView => navigateToSignInView
            ??= new Command<string>(OnNavigateToSignInView);

        public string PasswordWarning
        {
            get => passwordWarning;
            set => SetProperty(ref passwordWarning, value);
        }

        public string ConfirmPassword
        {
            get => confirmPassword;
            set => SetProperty(ref confirmPassword, value);
        }

        public string ConfirmPasswordWarning
        {
            get => confirmPasswordWarning;
            set => SetProperty(ref confirmPasswordWarning, value);
        }

        public bool IsDataLoad
        {
            get => isDataLoad;
            set => SetProperty(ref isDataLoad, value);
        }

        #endregion

        public CreateNewPasswordViewModel(
            INavigationService navigationService,
            IPageDialogService dialogService,
            IUserService userService
        ) : base(navigationService)
        {
            this.dialogService = dialogService
                ?? throw new ArgumentNullException(nameof(dialogService));

            this.userService = userService
                ?? throw new ArgumentNullException(nameof(userService));
        }

        public override void OnNavigatedTo(INavigationParameters navigationParameters)
        {
            this.navigationParameters = navigationParameters
                ?? throw new ArgumentNullException(nameof(navigationParameters));
        }

        private async void OnNavigateToSignInView(string password)
        {
            await NavigateToSignInViewAsync(password);
        }

        private async Task NavigateToSignInViewAsync(string password)
        {
            if (!IsValid(password))
                return;

            try
            {
                IsDataLoad = true;

                var email = navigationParameters.GetValue<string>("email");

                if (!await userService.RequestChangePasswordAsync(email, password))
                    return;
            }
            catch (WebException)
            {
                IsDataLoad = false;

                await dialogService.DisplayAlertAsync(AppResource.Error, AppResource.ErrorConnection, AppResource.Ok);

                return;
            }

            await NavigationService.NavigateAsync(nameof(LoginView));

            IsDataLoad = false;
        }

        #region Validation

        private bool IsValid(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                PasswordWarning = AppResource.PasswordEmpty;

                return false;
            }

            if (Validator.IsPasswordShort(password))
            {
                PasswordWarning = AppResource.PasswordShort;

                return false;
            }

            if (Validator.IsPasswordLong(password))
            {
                PasswordWarning = AppResource.PasswordLong;

                return false;
            }

            if (Validator.IsPasswordWeak(password))
            {
                PasswordWarning = AppResource.PasswordStrong;

                return false;
            }

            if (!Validator.PasswordsMatched(password, ConfirmPassword))
            {
                ConfirmPasswordWarning = AppResource.PasswordsMatch;

                return false;
            }

            return true;
        }

        #endregion

    }
}