using System.Windows.Input;
using ETicketMobile.Business.Validators;
using ETicketMobile.Resources;
using ETicketMobile.Views.Registration;
using Prism.Navigation;
using Xamarin.Forms;

namespace ETicketMobile.ViewModels.Registration
{
    public class PasswordRegistrationViewModel : ViewModelBase
    {
        #region Fields

        private INavigationParameters navigationParameters;

        private ICommand navigateToBirthDateRegistrationView;

        private string passwordWarning;

        private string confirmPassword;
        private string confirmPasswordWarning;

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
            await NavigationService.NavigateAsync(nameof(BirthdayRegistrationView), navigationParameters);
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

            if (!Validator.PasswordsMatched(password, confirmPassword))
            {
                ConfirmPasswordWarning = AppResource.PasswordsMatch;

                return false;
            }

            return true;
        }

        #endregion

    }
}