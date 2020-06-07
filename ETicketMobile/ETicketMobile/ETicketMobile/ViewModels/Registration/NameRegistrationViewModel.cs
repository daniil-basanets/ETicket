using System.Windows.Input;
using ETicketMobile.Business.Validators;
using ETicketMobile.Resources;
using ETicketMobile.Views.Registration;
using Prism.Navigation;
using Xamarin.Forms;

namespace ETicketMobile.ViewModels.Registration
{
    public class NameRegistrationViewModel : ViewModelBase
    {
        #region Fields

        private INavigationParameters navigationParameters;

        private ICommand navigateToPasswordRegistrationView;

        private string firstNameWarning;
        private string lastNameWarning;

        private string firstName;
        private string lastName;

        #endregion

        #region Properties

        public ICommand NavigateToPasswordRegistrationView => navigateToPasswordRegistrationView
            ??= new Command(OnMoveToPasswordRegistrationView);

        public string FirstName
        {
            get => firstName;
            set => SetProperty(ref firstName, value);
        }

        public string LastName
        {
            get => lastName;
            set => SetProperty(ref lastName, value);
        }

        public string FirstNameWarning
        {
            get => firstNameWarning;
            set => SetProperty(ref firstNameWarning, value);
        }

        public string LastNameWarning
        {
            get => lastNameWarning;
            set => SetProperty(ref lastNameWarning, value);
        }

        #endregion

        public NameRegistrationViewModel(INavigationService navigationService)
            : base(navigationService)
        {
        }

        public override void OnNavigatedTo(INavigationParameters navigationParameters)
        {
            this.navigationParameters = navigationParameters;
        }

        private async void OnMoveToPasswordRegistrationView()
        {
            if (!IsValid())
                return;

            navigationParameters.Add("firstName", firstName);
            navigationParameters.Add("lastName", lastName);

            await NavigationService.NavigateAsync(nameof(PasswordRegistrationView), navigationParameters);
        }

        #region Validation

        private bool IsValid()
        {
            if (string.IsNullOrEmpty(firstName))
            {
                FirstNameWarning = AppResource.FirstNameEmpty;

                return false;
            }

            if (string.IsNullOrEmpty(lastName))
            {
                LastNameWarning = AppResource.LastNameEmpty;

                return false;
            }

            if (!Validator.IsNameValid(firstName))
            {
                FirstNameWarning = AppResource.FirstNameValid;

                return false;
            }

            if (!Validator.IsNameValid(lastName))
            {
                LastNameWarning = AppResource.LastNameValid;

                return false;
            }

            return true;
        }

        #endregion

    }
}