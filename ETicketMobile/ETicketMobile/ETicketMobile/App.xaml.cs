using ETicketMobile.DataAccess.Domain.Interfaces;
using ETicketMobile.DataAccess.Domain.LocalAPI;
using ETicketMobile.DataAccess.Domain.LocalAPI.Interfaces;
using ETicketMobile.DataAccess.Domain.Repositories;
using ETicketMobile.ViewModels;
using ETicketMobile.ViewModels.ForgotPassword;
using ETicketMobile.ViewModels.Login;
using ETicketMobile.ViewModels.Registration;
using ETicketMobile.ViewModels.Tickets;
using ETicketMobile.Views;
using ETicketMobile.Views.ForgotPassword;
using ETicketMobile.Views.Login;
using ETicketMobile.Views.Registration;
using ETicketMobile.Views.Tickets;
using Prism;
using Prism.Ioc;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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
            //await NavigationService.NavigateAsync(nameof(NavigationPage) + "/" + nameof(ConfirmForgotPasswordView));
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance<ITokenRepository>(new TokenRepository());
            containerRegistry.RegisterInstance<ILocalApi>(LocalApi.GetInstance());

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
            containerRegistry.RegisterForNavigation<BuyTicketView, BuyTicketViewModel>();
            containerRegistry.RegisterForNavigation<ConfirmEmailView, ConfirmEmailViewModel>();
            containerRegistry.RegisterForNavigation<ConfirmForgotPasswordView, ConfirmForgotPasswordViewModel>();
            containerRegistry.RegisterForNavigation<CreateNewPasswordView, CreateNewPasswordViewModel>();
            containerRegistry.RegisterForNavigation<AreasView, AreasViewModel>();
        }
    }
}