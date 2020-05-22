using System;
using System.Linq;
using System.Windows.Input;
using ETicketMobile.Resources;
using ETicketMobile.Views.Registration;
using Prism.Navigation;
using Xamarin.Forms;

namespace ETicketMobile.ViewModels.Registration
{
    public class PasswordRegistrationViewModel : ViewModelBase
    {
        #region Constants

        private const int PasswordMinLength = 8;
        private const int PasswordMaxLength = 100;

        #endregion

        #region Fields

        private readonly INavigationService navigationService;
        private INavigationParameters navigationParameters;

        private ICommand navigateToBirthDateRegistrationView;

        private string passwordWarning;

        private string confirmPasswordWarning;

        private string confirmPassword;

        #endregion

        #region Properties

        public ICommand NavigateToBirthDateRegistrationView => navigateToBirthDateRegistrationView 
            ??= new Command<string>(OnNavigateToBirthDateRegistrationView);

        public string PasswordWarning
        {
            get => passwordWarning;
            set => SetProperty(ref passwordWarning, value);
        }

        public string ConfirmPasswordWarning
        {
            get => confirmPasswordWarning;
            set => SetProperty(ref confirmPasswordWarning, value);
        }

        public string ConfirmPassword
        {
            get => confirmPassword;
            set => SetProperty(ref confirmPassword, value);
        }

        #endregion

        public PasswordRegistrationViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            this.navigationService = navigationService
                ?? throw new ArgumentNullException(nameof(navigationService));
        }

        public override void OnNavigatedTo(INavigationParameters navigationParameters)
        {
            this.navigationParameters = navigationParameters;
        }

        private async void OnNavigateToBirthDateRegistrationView(string password)
        {
            if (!IsValid(password))
                return;

            navigationParameters.Add("password", password);
            await navigationService.NavigateAsync(nameof(BirthdayRegistrationView), navigationParameters);
        }

        #region Validation

        private bool IsValid(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                PasswordWarning = AppResource.PasswordEmpty;

                return false;
            }

            if (!IsPasswordShort(password))
            {
                PasswordWarning = AppResource.PasswordShort;

                return false;
            }

            if (!IsPasswordLong(password))
            {
                PasswordWarning = AppResource.PasswordLong;

                return false;
            }

            if (IsPasswordWeak(password))
            {
                PasswordWarning = AppResource.PasswordStrong;

                return false;
            }

            if (!PasswordsMatched(password))
            {
                ConfirmPasswordWarning = AppResource.PasswordsMatch;

                return false;
            }

            return true;
        }

        private bool IsPasswordShort(string password)
        {
            return password.Length >= PasswordMinLength;
        }

        private bool IsPasswordLong(string password)
        {
            return password.Length <= PasswordMaxLength;
        }

        private bool IsPasswordWeak(string password)
        {
            return password.All(ch => char.IsDigit(ch));
        }

        private bool PasswordsMatched(string password)
        {
            return string.Equals(password, ConfirmPassword);
        }

        #endregion
    }
}