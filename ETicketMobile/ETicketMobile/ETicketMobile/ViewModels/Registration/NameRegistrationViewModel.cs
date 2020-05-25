using System;
using System.Windows.Input;
using ETicketMobile.Resources;
using ETicketMobile.Views.Registration;
using Prism.Navigation;
using Xamarin.Forms;

namespace ETicketMobile.ViewModels.Registration
{
    public class NameRegistrationViewModel : ViewModelBase
    {
        #region Fields

        protected INavigationService navigationService;
        private INavigationParameters navigationParameters;

        private ICommand navigateToPasswordRegistrationView;

        private string firstNameWarning;
        private string lastNameWarning;

        private string lastName;

        #endregion

        #region Properties

        public ICommand NavigateToPasswordRegistrationView => navigateToPasswordRegistrationView 
            ??= new Command<string>(OnMoveToPasswordRegistrationView);

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
            this.navigationService = navigationService
                ?? throw new ArgumentNullException(nameof(navigationService));
        }

        public override void OnNavigatedTo(INavigationParameters navigationParameters)
        {
            this.navigationParameters = navigationParameters;
        }

        private async void OnMoveToPasswordRegistrationView(string firstName)
        {
            if (!IsValid(firstName))
                return;

            navigationParameters.Add("firstName", firstName);
            navigationParameters.Add("lastName", lastName);

            await navigationService.NavigateAsync(nameof(PasswordRegistrationView), navigationParameters);
        }

        #region Validation

        private bool IsValid(string firstName)
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

            if (!IsNameValid(firstName))
            {
                FirstNameWarning = AppResource.FirstNameValid;

                return false;
            }

            if (!IsNameValid(lastName))
            {
                LastNameWarning = AppResource.LastNameValid;

                return false;
            }

            return true;
        }

        private bool IsNameValid(string name)
        {
            name ??= string.Empty;

            return name.Length >= 2 && name.Length <= 25;
        }

        #endregion
    }
}