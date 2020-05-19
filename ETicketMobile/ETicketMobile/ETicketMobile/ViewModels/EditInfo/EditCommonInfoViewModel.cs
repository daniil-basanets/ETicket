using ETicketMobile.DataAccess.LocalAPI.Interfaces;
using ETicketMobile.Resources;
using ETicketMobile.Views.EditInfoView;
using ETicketMobile.WebAccess.Network.WebService;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ETicketMobile.ViewModels.EditInfo
{
    public class EditCommonInfoViewModel : ViewModelBase
    {
        protected INavigationService navigationService;

        //private readonly HttpClientService httpClient;

        private ICommand navigateToSuccessfullySavedView;

        public ICommand NavigateToSuccessfullySavedView => navigateToSuccessfullySavedView
            ?? (navigateToSuccessfullySavedView = new Command(OnNavigateToSuccessfullySavedView));

        private void OnNavigateToSuccessfullySavedView(object obj)
        {
            navigationService.NavigateAsync(nameof(SuccessfullySavedView));
        }

        public EditCommonInfoViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            this.navigationService = navigationService
                ?? throw new ArgumentNullException(nameof(navigationService));

            //httpClient = new HttpClientService();
        }

        public override void OnAppearing()
        {
            //FillProperties();
        }

        private void FillProperties()
        {
            //NamePlaceHolder = AppResource.PasswordPlaceHolderDefault;
        }



    }
}
