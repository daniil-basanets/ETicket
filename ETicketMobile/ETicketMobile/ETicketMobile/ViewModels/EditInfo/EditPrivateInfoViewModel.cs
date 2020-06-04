using Android.Icu.Lang;
using ETicketMobile.Business.Mapping;
using ETicketMobile.Business.Model.User;
using ETicketMobile.Business.Services.Interfaces;
using ETicketMobile.Business.Validators;
using ETicketMobile.DataAccess.LocalAPI.Interfaces;
using ETicketMobile.Resources;
using ETicketMobile.Views.EditInfoView;
using ETicketMobile.WebAccess.DTO;
using ETicketMobile.WebAccess.Network.Endpoints;
using ETicketMobile.WebAccess.Network.WebServices;
using ETicketMobile.WebAccess.Network.WebServices.Interfaces;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ETicketMobile.ViewModels.EditInfo
{
    class EditPrivateInfoViewModel : ViewModelBase
    {
        #region Fields

        private readonly IPageDialogService dialogService;

        protected INavigationService navigationService;

        private INavigationParameters navigationParameters;

        private readonly ITokenService tokenService;

        private readonly ILocalApi localApi;

        private User user;

        private User userInfo;

        private Document document;

        private Privilege privilege;

        private readonly IHttpService httpService;

        private ICommand navigateToSuccessfullySavedView;

        private string accessToken;

        private string documentWarning;

        private string documentEmptyWarning;

        private string privilegeWarning;

        private string privilegeEmptyWarning;

        private Guid documentId;

        private string privilegeId;

        #endregion

        #region Properties

        public ICommand NavigateToSuccessfullySavedView => navigateToSuccessfullySavedView
            ?? (navigateToSuccessfullySavedView = new Command(OnNavigateToSuccessfullySavedView));

        public Guid DocumentId
        {
            get => documentId;
            set => SetProperty(ref documentId, value);
        }

        public string PivilegeId
        {
            get => privilegeId;
            set => SetProperty(ref privilegeId, value);
        }

        public string DocumentWarning
        {
            get => documentWarning;
            set => SetProperty(ref documentWarning, value);
        }

        public string DocumentEmptyWarning
        {
            get => documentEmptyWarning;
            set => SetProperty(ref documentEmptyWarning, value);
        }

        public string PrivilegeEmptyWarning
        {
            get => privilegeEmptyWarning;
            set => SetProperty(ref privilegeEmptyWarning, value);
        }

        public string PrivilegeWarning
        {
            get => privilegeWarning;
            set => SetProperty(ref privilegeWarning, value);
        }

        public User User
        {
            get => user;
            set => SetProperty(ref user, value);
        }

        #endregion

        public EditPrivateInfoViewModel(
            INavigationService navigationService,
            IHttpService httpService,
            IPageDialogService dialogService,
            ITokenService tokenService,
            ILocalApi localApi
            )
            : base(navigationService)
        {
            this.localApi = localApi
                ?? throw new ArgumentNullException(nameof(localApi));

            this.navigationService = navigationService
                ?? throw new ArgumentNullException(nameof(navigationService));

            this.httpService = httpService
                 ?? throw new ArgumentNullException(nameof(httpService));

            this.tokenService = tokenService
                ?? throw new ArgumentNullException(nameof(tokenService));
        }

        public async override void OnAppearing()
        {
            try
            {
                accessToken = await tokenService.GetAccessTokenAsync();
            }
            catch (WebException)
            {
                await dialogService.DisplayAlertAsync
                    ("Error", "Check connection with server", "OK");

                return;
            }
        }

        public async override void OnNavigatedTo(INavigationParameters navigationParameters)
        {
            this.navigationParameters = navigationParameters;

            User.Email = navigationParameters.GetValue<string>("email");

            GetUserRequestDto userRequestDto = new GetUserRequestDto();

            userRequestDto.Email = User.Email;

            var userResponseDto = await httpService.PostAsync
                <GetUserRequestDto, GetUserResponseDto>
                (UsersEndpoint.GetUserByEmail, userRequestDto, accessToken);

            userInfo = AutoMapperConfiguration.Mapper.Map<User>(userResponseDto);

            Init();
        }

        private async void Init()
        {
            DocumentId = userInfo.DocumentId;

            DocumentRequestDto documentRequestDto = new DocumentRequestDto();

            documentRequestDto.DocumentId = DocumentId;

            var documentRequestInfo = await httpService.GetAsync<DocumentRequestDto>(UsersEndpoint.GetDocumentById, accessToken);

            var documentInfo = AutoMapperConfiguration.Mapper.Map<DocumentRequestDto>(documentRequestInfo);

            PrivilegeRequestDto privilegeRequestDto = new PrivilegeRequestDto();

            privilegeRequestDto.PrivilegeId = userInfo.PrivilegeId;

            var privilegeRequestInfo = await httpService.GetAsync<PrivilegeRequestDto>(UsersEndpoint.GetPrivilegeById, accessToken);

            var privilegeInfo = AutoMapperConfiguration.Mapper.Map<PrivilegeRequestDto>(privilegeRequestInfo);

            document.Number = documentInfo.Number;

            privilege.Name = privilegeInfo.Name;
        }

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

        private bool IsValid()
        {
            if (string.IsNullOrEmpty(document.Number))
            {
                DocumentEmptyWarning = AppResource.DocumentEmptyWarning;

                return false;
            }

            if (string.IsNullOrEmpty(privilege.Name))
            {
                PrivilegeEmptyWarning = AppResource.PrivilegeEmptyWarning;

                return false;
            }

            if (!Validator.IsDocumentNumberValid(document.Number))
            {
                DocumentWarning = AppResource.DocumentWarning;

                return false;
            }

            if (!Validator.IsNameValid(privilege.Name))
            {
                PrivilegeWarning = AppResource.PrivilegeWarning;

                return false;
            }           

            return true;
        }
    }
}
