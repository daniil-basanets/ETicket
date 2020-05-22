using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ETicketMobile.Views.Registration;
using ETicketMobile.WebAccess.Network;
using ETicketMobile.WebAccess.Network.WebService;
using Prism.Navigation;
using Xamarin.Forms;

namespace ETicketMobile.ViewModels.Registration
{
    public class BirthdayRegistrationViewModel : ViewModelBase
    {
        #region Constanst

        private const int MinAge = 13;
        private const int MaxAge = 120;

        #endregion

        #region Fields

        private readonly INavigationService navigationService;
        private INavigationParameters navigationParameters;

        private ICommand navigateToConfirmEmailView;

        private readonly HttpClientService httpClient;

        private DateTime birthday;
        private DateTime minBirthday;
        private DateTime maxBirthday;

        #endregion

        #region Properties

        public ICommand NavigateToConfirmEmailView => navigateToConfirmEmailView
            ?? (navigateToConfirmEmailView = new Command(OnNavigateToConfirmEmailView));

        public DateTime Birthday
        {
            get => birthday;
            set => SetProperty(ref birthday, value);
        }

        public DateTime MinBirthday
        {
            get => minBirthday;
            set => SetProperty(ref minBirthday, value);
        }

        public DateTime MaxBirthday
        {
            get => maxBirthday;
            set => SetProperty(ref maxBirthday, value);
        }

        #endregion

        public BirthdayRegistrationViewModel(INavigationService navigationService) 
            : base(navigationService)
        {
            this.navigationService = navigationService
                ?? throw new ArgumentNullException(nameof(navigationService));

            httpClient = new HttpClientService(ServerConfig.Address);
        }

        public override void OnAppearing()
        {
            FillProperties();
        }

        private void FillProperties()
        {
            MinBirthday = DateTime.Today.AddYears(-MinAge);
            MaxBirthday = DateTime.Today.AddYears(-MaxAge);

            Birthday = DateTime.Today.Date;
        }

        public override void OnNavigatedTo(INavigationParameters navigationParameters)
        {
            this.navigationParameters = navigationParameters;
        }

        private async void OnNavigateToConfirmEmailView()
        {
            var email = navigationParameters.GetValue<string>("email");
            await RequestActivationCodeAsync(email);

            navigationParameters.Add("birth", birthday.Date);
            await navigationService.NavigateAsync(nameof(ConfirmEmailView), navigationParameters);
        }

        private async Task RequestActivationCodeAsync(string email)
        {
            await httpClient.PostAsync<string, string>(AuthorizeEndpoint.RequestActivationCode, email);
        }
    }
}