using System;
using System.Windows.Input;
using ETicketMobile.Resources;
using ETicketMobile.Views.Registration;
using ETicketMobile.WebAccess.Network;
using ETicketMobile.WebAccess.Network.WebService;
using Prism.Navigation;
using Xamarin.Forms;

namespace ETicketMobile.ViewModels.Registration
{
    public class PhoneRegistrationViewModel : ViewModelBase
    {
        #region Constants

        private const int PhoneMaxLength = 13;

        #endregion

        #region Fields

        protected readonly INavigationService navigationService;
        protected INavigationParameters navigationParameters;

        private readonly HttpClientService httpClient;

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
            this.navigationService = navigationService
                ?? throw new ArgumentNullException(nameof(navigationService));

            httpClient = new HttpClientService(ServerConfig.Address);
        }

        public override void OnNavigatedTo(INavigationParameters navigationParameters)
        {
            this.navigationParameters = navigationParameters;
        }

        private async void OnNavigateToNameRegistrationView(string phone)
        {
            var phoneNumber = GetPhoneNumber(phone);

            if (!IsValid(phoneNumber))
                return;

            navigationParameters.Add("phone", phoneNumber);
            await navigationService.NavigateAsync(nameof(NameRegistrationView), navigationParameters);
        }

        #region Validation

        private bool IsValid(string phone)
        {
            if (!IsPhoneCorrectLong(phone))
            {
                PhoneWarning = AppResource.PhoneFormat;

                return false;
            }

            return true;
        }

        private bool IsPhoneCorrectLong(string phone)
        {
            return phone.Length == PhoneMaxLength;
        }

        #endregion

        private string GetPhoneNumber(string phoneNumber)
        {
            return "+38" + phoneNumber;
        }
    }
}