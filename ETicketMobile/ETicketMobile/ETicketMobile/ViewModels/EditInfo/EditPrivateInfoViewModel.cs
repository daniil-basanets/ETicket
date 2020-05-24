using ETicketMobile.Views.EditInfoView;
using ETicketMobile.WebAccess.Network.WebService;
using Prism.Navigation;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace ETicketMobile.ViewModels.EditInfo
{
    class EditPrivateInfoViewModel : ViewModelBase
    {
        #region Fields

        protected INavigationService navigationService;

        // private readonly ILocalApi localApi;

        private readonly HttpClientService httpClient;

        private ICommand navigateToSuccessfullySavedView;

        private string document;

        private string privilege;

        #endregion

        #region Properties

        public ICommand NavigateToSuccessfullySavedView => navigateToSuccessfullySavedView
            ?? (navigateToSuccessfullySavedView = new Command(OnNavigateToSuccessfullySavedView));

        public string Document
        {
            get => document;
            set => SetProperty(ref document, value);
        }

        public string Pivilege
        {
            get => privilege;
            set => SetProperty(ref privilege, value);
        }

        #endregion

        private void OnNavigateToSuccessfullySavedView(object obj)
        {
            navigationService.NavigateAsync(nameof(SuccessfullySavedView));
        }

        public EditPrivateInfoViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            this.navigationService = navigationService
                ?? throw new ArgumentNullException(nameof(navigationService));

            httpClient = new HttpClientService();
        }
    }
}
