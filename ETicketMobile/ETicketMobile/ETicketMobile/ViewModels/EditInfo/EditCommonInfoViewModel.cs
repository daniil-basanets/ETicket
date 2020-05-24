using Android.Icu.Lang;
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
        #region Fields

        protected INavigationService navigationService;

       // private readonly ILocalApi localApi;

        private readonly HttpClientService httpClient;

        private ICommand navigateToSuccessfullySavedView;

        private string firstName;

        private string lastName;

        private string phoneNumber;

        private int age;
                
        #endregion

        #region Properties

        public ICommand NavigateToSuccessfullySavedView => navigateToSuccessfullySavedView
            ?? (navigateToSuccessfullySavedView = new Command(OnNavigateToSuccessfullySavedView));

        public string FirstName
        {
            get => firstName;
            set => SetProperty(ref firstName, value);
        }

        public string LastName
        {
            get => lastName;
            set => SetProperty(ref lastName, value);
        }

        public string PhoneNumber
        { 
            get =>  phoneNumber;
            set => SetProperty(ref phoneNumber, value);
        }

        public int Age
        {
            get => age;
            set => SetProperty(ref age, value);
        }

        #endregion

        private void OnNavigateToSuccessfullySavedView(object obj)
        {
            //navigationService.NavigateAsync(nameof(SuccessfullySavedView));
            navigationService.NavigateAsync(nameof(EditPrivateInfoView));
        }

        public EditCommonInfoViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            this.navigationService = navigationService
                ?? throw new ArgumentNullException(nameof(navigationService));

            httpClient = new HttpClientService();
        }
    }
}
