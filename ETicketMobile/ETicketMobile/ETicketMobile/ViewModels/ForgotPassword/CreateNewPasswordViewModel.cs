using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
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
        #region Constants

        private const int PasswordMinLength = 8;
        private const int PasswordMaxLength = 100;

        #endregion

        #region Fields

        private readonly INavigationService navigationService;
        private INavigationParameters navigationParameters;

        private readonly IPageDialogService dialogService;
        private readonly IHttpService httpService;

        private ICommand navigateToSignInView;

        private string passwordWarning;

        private string confirmPasswordWarning;

        private string confirmPassword;

        #endregion

        #region Properties

        public ICommand NavigateToSignInView => navigateToSignInView 
            ??= new Command<string>(OnNavigateToSignInView);

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

        public CreateNewPasswordViewModel(
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
                if (!await RequestChangePasswordAsync(password))
                    return;
            }
            catch (WebException)
            {
                await dialogService.DisplayAlertAsync("Alert", "Check connection with server", "OK");

                return;
            }


            await navigationService.NavigateAsync(nameof(LoginView));
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

            if (IsPasswordShort(password))
            {
                PasswordWarning = AppResource.PasswordShort;

                return false;
            }

            if (IsPasswordLong(password))
            {
                PasswordWarning = AppResource.PasswordLong;

                return false;
            }

            if (IsPasswordWeak(password))
            {
                PasswordWarning = AppResource.PasswordStrong;

                return false;
            }

            if (!PasswordsMatched(password))
            {
                ConfirmPasswordWarning = AppResource.PasswordsMatch;

                return false;
            }

            return true;
        }

        private bool IsPasswordShort(string password)
        {
            return password.Length < PasswordMinLength;
        }

        private bool IsPasswordLong(string password)
        {
            return password.Length > PasswordMaxLength;
        }

        private bool IsPasswordWeak(string password)
        {
            return password.All(ch => char.IsDigit(ch));
        }

        private bool PasswordsMatched(string password)
        {
            return string.Equals(password, ConfirmPassword);
        }

        #endregion
    }
}