using System.Threading.Tasks;
using System.Windows.Input;
using ETicketMobile.Business.Validators;
using ETicketMobile.Resources;
using ETicketMobile.Views.Registration;
using Prism.Navigation;
using Xamarin.Forms;

namespace ETicketMobile.ViewModels.Registration
{
    public class PhoneRegistrationViewModel : ViewModelBase
    {
        #region Fields

        protected INavigationParameters navigationParameters;

        private ICommand navigateToNameRegistrationView;

        private string phoneWarning;

        #endregion

        #region Properties

        public ICommand NavigateToNameRegistrationView => navigateToNameRegistrationView
            ??= new Command<string>(OnNavigateToNameRegistrationView);

        public string PhoneWarning
        {
            get => phoneWarning;
            set => SetProperty(ref phoneWarning, value);
        }

        #endregion

        public PhoneRegistrationViewModel(INavigationService navigationService)
            : base(navigationService)
        {
        }

        public override void OnNavigatedTo(INavigationParameters navigationParameters)
        {
            this.navigationParameters = navigationParameters;
        }

        private async void OnNavigateToNameRegistrationView(string phone)
        {
            await NavigateToNameRegistrationViewAsync(phone);
        }

        private async Task NavigateToNameRegistrationViewAsync(string phone)
        {
            var phoneNumber = GetPhoneNumber(phone);

            if (!IsValid(phoneNumber))
                return;

            navigationParameters.Add("phone", phoneNumber);
            await NavigationService.NavigateAsync(nameof(NameRegistrationView), navigationParameters);
        }

        #region Validation

        private bool IsValid(string phone)
        {
            if (!Validator.HasPhoneCorrectLength(phone))
            {
                PhoneWarning = AppResource.PhoneFormat;

                return false;
            }

            return true;
        }

        #endregion

        private string GetPhoneNumber(string phoneNumber)
        {
            return "+38" + phoneNumber;
        }
    }
}