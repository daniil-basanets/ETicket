using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ETicketMobile.Business.Exceptions;
using ETicketMobile.Business.Services.Interfaces;
using ETicketMobile.Resources;
using ETicketMobile.Views.Registration;
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

        private INavigationParameters navigationParameters;

        private readonly IEmailActivationService emailActivationService;
        private readonly IPageDialogService dialogService;

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
            IEmailActivationService emailActivationService,
            INavigationService navigationService,
            IPageDialogService dialogService
        ) : base(navigationService)
        {
            this.emailActivationService = emailActivationService
                ?? throw new ArgumentNullException(nameof(emailActivationService));

            this.dialogService = dialogService
                ?? throw new ArgumentNullException(nameof(dialogService));
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
                await emailActivationService.RequestActivationCodeAsync(email);
            }
            catch (WebException)
            {
                await dialogService.DisplayAlertAsync(AppResource.Error, AppResource.ErrorConnection, AppResource.Ok);

                return;
            }

            navigationParameters.Add("birth", birthday);
            await NavigationService.NavigateAsync(nameof(ConfirmEmailView), navigationParameters);
        }
    }
}