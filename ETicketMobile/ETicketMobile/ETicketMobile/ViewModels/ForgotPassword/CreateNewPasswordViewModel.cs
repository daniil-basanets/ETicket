using System;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using ETicketMobile.Business.Validators;
using ETicketMobile.Resources;
using ETicketMobile.Views.Login;
using ETicketMobile.WebAccess.DTO;
using ETicketMobile.WebAccess.Network.Endpoints;
using ETicketMobile.WebAccess.Network.WebServices.Interfaces;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;

namespace ETicketMobile.ViewModels.ForgotPassword
{
    public class CreateNewPasswordViewModel : ViewModelBase
    {
        #region Fields

        private INavigationParameters navigationParameters;

        private readonly IPageDialogService dialogService;
        private readonly IHttpService httpService;

        private ICommand navigateToSignInView;

        private string passwordWarning;

        private string confirmPassword;
        private string confirmPasswordWarning;

        private bool isDataLoad;

        #endregion

        #region Properties

        public ICommand NavigateToSignInView => navigateToSignInView 
            ??= new Command<string>(OnNavigateToSignInView);

        public string PasswordWarning
        {
            get => passwordWarning;
            set => SetProperty(ref passwordWarning, value);
        }

        public string ConfirmPassword
        {
            get => confirmPassword;
            set => SetProperty(ref confirmPassword, value);
        }

        public string ConfirmPasswordWarning
        {
            get => confirmPasswordWarning;
            set => SetProperty(ref confirmPasswordWarning, value);
        }

        public bool IsDataLoad
        {
            get => isDataLoad;
            set => SetProperty(ref isDataLoad, value);
        }

        #endregion

        public CreateNewPasswordViewModel(
            INavigationService navigationService,
            IPageDialogService dialogService,
            IHttpService httpService
        ) : base(navigationService)
        {
            this.dialogService = dialogService
                ?? throw new ArgumentNullException(nameof(dialogService));

            this.httpService = httpService
                ?? throw new ArgumentNullException(nameof(httpService));
        }

        public override void OnNavigatedTo(INavigationParameters navigationParameters)
        {
            this.navigationParameters = navigationParameters;
        }

        private async void OnNavigateToSignInView(string password)
        {
            await NavigateToSignInViewAsync(password);
        }

        private async Task NavigateToSignInViewAsync(string password)
        {
            if (!IsValid(password))
                return;

            try
            {
                IsDataLoad = true;

                if (!await RequestChangePasswordAsync(password))
                    return;
            }
            catch (WebException)
            {
                IsDataLoad = false;

                await dialogService.DisplayAlertAsync("Error", "Check connection with server", "OK");

                return;
            }

            await NavigationService.NavigateAsync(nameof(LoginView));

            IsDataLoad = false;
        }

        private async Task<bool> RequestChangePasswordAsync(string password)
        {
            var email = navigationParameters.GetValue<string>("email");

            var createNewPasswordDto = CreateNewPasswordDto(email, password);
            var response = await httpService.PostAsync<CreateNewPasswordRequestDto, CreateNewPasswordResponseDto>(
                    AuthorizeEndpoint.ResetPassword,
                    createNewPasswordDto
            );

            return response.Succeeded;
        }

        private CreateNewPasswordRequestDto CreateNewPasswordDto(string email, string password)
        {
            return new CreateNewPasswordRequestDto
            {
                Email = email,
                NewPassword = password
            };
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

            if (!Validator.PasswordsMatched(password, ConfirmPassword))
            {
                ConfirmPasswordWarning = AppResource.PasswordsMatch;

                return false;
            }

            return true;
        }

        #endregion
    }
}