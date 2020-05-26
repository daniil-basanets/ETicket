using System;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using ETicketMobile.Views.Registration;
using ETicketMobile.WebAccess.Network.Endpoints;
using ETicketMobile.WebAccess.Network.WebServices.Interfaces;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;

namespace ETicketMobile.ViewModels.Registration
{
    public class BirthdayRegistrationViewModel : ViewModelBase
    {
        #region Constants

        private const int MinAge = 13;
        private const int MaxAge = 120;

        #endregion

        #region Fields

        private readonly INavigationService navigationService;
        private INavigationParameters navigationParameters;

        private readonly IPageDialogService dialogService;
        private readonly IHttpService httpService;

        private ICommand navigateToConfirmEmailView;

        private DateTime defaultDisplayDate;
        private DateTime minBirthday;
        private DateTime maxBirthday;

        #endregion

        #region Properties

        public ICommand NavigateToConfirmEmailView => navigateToConfirmEmailView 
            ??= new Command<DateTime>(OnNavigateToConfirmEmailView);

        public DateTime DefaultDisplayDate
        {
            get => defaultDisplayDate;
            set => SetProperty(ref defaultDisplayDate, value);
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

        public BirthdayRegistrationViewModel(
            INavigationService navigationService,
            IPageDialogService dialogService,
            IHttpService httpService
        ) : base(navigationService)
        {
            this.navigationService = navigationService
                ?? throw new ArgumentNullException(nameof(navigationService));

            this.dialogService = dialogService
                ?? throw new ArgumentNullException(nameof(dialogService));

            this.httpService = httpService
                ?? throw new ArgumentNullException(nameof(httpService));
        }

        public override void OnAppearing()
        {
            FillProperties();
        }

        private void FillProperties()
        {
            DefaultDisplayDate = DateTime.Now.Date;

            MinBirthday = DateTime.Today.AddYears(-MinAge);
            MaxBirthday = DateTime.Today.AddYears(-MaxAge);
        }

        public override void OnNavigatedTo(INavigationParameters navigationParameters)
        {
            this.navigationParameters = navigationParameters;
        }

        private async void OnNavigateToConfirmEmailView(DateTime birthday)
        {
            await NavigateToConfirmEmailViewAsync(birthday.Date);
        }

        private async Task NavigateToConfirmEmailViewAsync(DateTime birthday)
        {
            var email = navigationParameters.GetValue<string>("email");

            try
            {
                await RequestActivationCodeAsync(email);
            }
            catch (WebException)
            {
                await dialogService.DisplayAlertAsync("Error", "Check connection with server", "OK");

                return;
            }

            navigationParameters.Add("birth", birthday);
            await navigationService.NavigateAsync(nameof(ConfirmEmailView), navigationParameters);
        }

        private async Task RequestActivationCodeAsync(string email)
        {
            await httpService.PostAsync<string, string>(AuthorizeEndpoint.RequestActivationCode, email);
        }
    }
}