using System;
using System.Text.RegularExpressions;
using System.Windows.Input;
using ETicketMobile.Views.Registration;
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

        private string phoneNumber;

        #endregion

        #region Properties

        public ICommand NavigateToNameRegistrationView => navigateToNameRegistrationView
            ?? (navigateToNameRegistrationView = new Command(OnNavigateToNameRegistrationView));

        public string PhoneWarning
        {
            get => phoneWarning;
            set => SetProperty(ref phoneWarning, value);
        }

        public string PhoneNumber
        {
            get => phoneNumber;
            set => SetProperty(ref phoneNumber, value);
        }

        #endregion

        public PhoneRegistrationViewModel(INavigationService navigationService) 
            : base(navigationService)
        {
            this.navigationService = navigationService
                ?? throw new ArgumentNullException(nameof(navigationService));

            httpClient = new HttpClientService();

            Init();
        }

        private void Init()
        {
            PhoneNumber = "+380 ";
        }

        public override void OnNavigatedTo(INavigationParameters navigationParameters)
        {
            this.navigationParameters = navigationParameters;
        }

        private void OnNavigateToNameRegistrationView(object obj)
        {
            var phone = GetPhoneNumber();

            if (!IsValid(phone))
                return;

            navigationParameters.Add("phone", phone);
            navigationService.NavigateAsync(nameof(NameRegistrationView), navigationParameters);
        }

        #region Validation

        private bool IsValid(string phone)
        {
            if (!IsPhoneCorrectLong(phone))
            {
                PhoneWarning = ErrorMessage.PhoneFormat;

                return false;
            }

            return true;
        }

        private bool IsPhoneCorrectLong(string phone)
        {
            return phone.Length == PhoneMaxLength;
        }

        #endregion

        private string GetPhoneNumber()
        {
            var phone = Regex.Replace(PhoneNumber, @"[^\d]", string.Empty);

            return "+" + phone;
        }
    }
}