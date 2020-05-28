using System.Globalization;
using System.Threading.Tasks;
using ETicketMobile.DataAccess.Interfaces;
using ETicketMobile.DataAccess.LocalAPI;
using ETicketMobile.DataAccess.LocalAPI.Interfaces;
using ETicketMobile.DataAccess.Repositories;
using ETicketMobile.Resources;
using ETicketMobile.UserInterface.Localization.Interfaces;
using ETicketMobile.ViewModels;
using ETicketMobile.ViewModels.BoughtTickets;
using ETicketMobile.ViewModels.ForgotPassword;
using ETicketMobile.ViewModels.Login;
using ETicketMobile.ViewModels.Payment;
using ETicketMobile.ViewModels.Registration;
using ETicketMobile.ViewModels.Settings;
using ETicketMobile.ViewModels.Tickets;
using ETicketMobile.ViewModels.UserAccount;
using ETicketMobile.Views;
using ETicketMobile.Views.ForgotPassword;
using ETicketMobile.Views.Login;
using ETicketMobile.Views.Payment;
using ETicketMobile.Views.Registration;
using ETicketMobile.Views.Settings;
using ETicketMobile.Views.Tickets;
using ETicketMobile.Views.UserAccount;
using ETicketMobile.Views.UserActions;
using ETicketMobile.WebAccess.Network.Configs;
using ETicketMobile.WebAccess.Network.WebServices.Interfaces;
using ETicketMobile.WebAccess.Network.WebServices;
using Prism;
using Prism.Ioc;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ETicketMobile.Business.Validators;
using ETicketMobile.Business.Validators.Interfaces;
using ETicketMobile.Business.Services;
using ETicketMobile.Business.Services.Interfaces;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace ETicketMobile
{
    public partial class App
    {
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync(nameof(NavigationPage) + "/" + nameof(LoginView));
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance<ITokenRepository>(new TokenRepository());

            var localApi = LocalApi.GetInstance();
            var localize = DependencyService.Get<ILocalize>();

            InitCultureAsync(localApi, localize).Wait();

            var httpService = new HttpService(ServerConfig.Address);
            containerRegistry.RegisterInstance<IHttpService>(httpService);

            containerRegistry.RegisterInstance<ILocalApi>(localApi);
            containerRegistry.RegisterInstance<ILocalize>(localize);

            var userValidator = new UserValidator(httpService);
            containerRegistry.RegisterInstance<IUserValidator>(userValidator);

            var tokenService = new TokenService(httpService, localApi);
            containerRegistry.RegisterInstance<ITokenService>(tokenService);

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainView, MainViewModel>();
            containerRegistry.RegisterForNavigation<LoginView, LoginViewModel>();
            containerRegistry.RegisterForNavigation<EmailRegistrationView, EmailRegistrationViewModel>();
            containerRegistry.RegisterForNavigation<NameRegistrationView, NameRegistrationViewModel>();
            containerRegistry.RegisterForNavigation<PasswordRegistrationView, PasswordRegistrationViewModel>();
            containerRegistry.RegisterForNavigation<BirthdayRegistrationView, BirthdayRegistrationViewModel>();
            containerRegistry.RegisterForNavigation<ForgotPasswordView, ForgotPasswordViewModel>();
            containerRegistry.RegisterForNavigation<TicketsView, TicketsViewModel>();
            containerRegistry.RegisterForNavigation<PhoneRegistrationView, PhoneRegistrationViewModel>();
            containerRegistry.RegisterForNavigation<ConfirmEmailView, ConfirmEmailViewModel>();
            containerRegistry.RegisterForNavigation<ConfirmForgotPasswordView, ConfirmForgotPasswordViewModel>();
            containerRegistry.RegisterForNavigation<CreateNewPasswordView, CreateNewPasswordViewModel>();
            containerRegistry.RegisterForNavigation<AreasView, AreasViewModel>();
            containerRegistry.RegisterForNavigation<LiqPayView, LiqPayViewModel>();
            containerRegistry.RegisterForNavigation<TransactionCompletedView, TransactionCompletedViewModel>();
            containerRegistry.RegisterForNavigation<MainMenuView, MainMenuViewModel>();
            containerRegistry.RegisterForNavigation<UserAccountView, UserAccountViewModel>();
            containerRegistry.RegisterForNavigation<SettingsView, SettingsViewModel>();
            containerRegistry.RegisterForNavigation<UserTransactionsView, UserTransactionsViewModel>();
            containerRegistry.RegisterForNavigation<LocalizationView, LocalizationViewModel>();
            containerRegistry.RegisterForNavigation<MyTicketsView, MyTicketsViewModel>();
        }

        private async Task InitCultureAsync(ILocalApi localApi, ILocalize localize)
        {
            var localization = await localApi.GetLocalizationAsync().ConfigureAwait(false);

            if (localization != null)
            {
                var currentCulture = new CultureInfo(localization.Culture);

                localize.CurrentCulture = currentCulture;
                AppResource.Culture = currentCulture;
            }
            else
            {
                localize.CurrentCulture = CultureInfo.CurrentCulture;
                AppResource.Culture = CultureInfo.CurrentCulture;
            }
        }
    }
}