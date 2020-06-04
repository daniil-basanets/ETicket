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
    public class EditCommonInfoViewModel : ViewModelBase
    {
        #region Fields

        private readonly IPageDialogService dialogService;

        protected INavigationService navigationService;

        private INavigationParameters navigationParameters;

        private readonly ITokenService tokenService;

        private readonly ILocalApi localApi;

        private User user;

        private User userInfo;

        private readonly IHttpService httpService;

        private ICommand navigateToSuccessfullySavedView;

        private string accessToken;

        private string firstName;

        private string lastName;

        private string phoneNumber;

        private int age;

        private string firstNameWarning;

        private string lastNameWarning;

        private string phoneWarning;

        private string ageWarning;

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
        public string FirstNameWarning
        {
            get => firstNameWarning;
            set => SetProperty(ref firstNameWarning, value);
        }

        public string LastNameWarning
        {
            get => lastNameWarning;
            set => SetProperty(ref lastNameWarning, value);
        }

        public string PhoneWarning
        {
            get => phoneWarning;
            set => SetProperty(ref phoneWarning, value);
        }

        public string AgeWarning
        {
            get => ageWarning;
            set => SetProperty(ref ageWarning, value);
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

        public User User
        {
            get => user;
            set => SetProperty(ref user, value);
        }
        #endregion

        public EditCommonInfoViewModel(
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

        private void Init()
        {
            FirstName = userInfo.FirstName;
            LastName = userInfo.LastName;
            PhoneNumber = userInfo.Phone;
            Age = userInfo.Age;
        }

        private UserInfoRequestDto UserInfoRequest()
        {
            return new UserInfoRequestDto
            {
            Phone = phoneNumber,
            FirstName = firstName,
            LastName = lastName,
            Age = age
            };
        }

        private async void OnNavigateToSuccessfullySavedView(object obj)
        {
            if (!IsValid())
                return;

            await UpdateUserInfo();

            await navigationService.NavigateAsync(nameof(SuccessfullySavedView));
            //navigationService.NavigateAsync(nameof(EditPrivateInfoView));
        }

        private async Task<bool> UpdateUserInfo()
        {
            var user = UserInfoRequest();

            accessToken = await tokenService.RefreshTokenAsync();

            var response = await httpService.PutAsync
                <UserInfoRequestDto, UserInfoResponseDto>
                (AuthorizeEndpoint.Registration, user, accessToken);

            return response.Succeeded;
        }
        
        private bool IsValid() 
        {
            if (string.IsNullOrEmpty(firstName))
            {
                FirstNameWarning = AppResource.FirstNameEmpty;

                return false;
            }

            if (string.IsNullOrEmpty(lastName))
            {
                LastNameWarning = AppResource.LastNameEmpty;

                return false;
            }

            if (!Validator.IsNameValid(firstName))
            {
                FirstNameWarning = AppResource.FirstNameValid;

                return false;
            }

            if (!Validator.IsNameValid(lastName))
            {
                LastNameWarning = AppResource.LastNameValid;

                return false;
            }

            if (!Validator.HasPhoneCorrectLength(phoneNumber))
            {
                PhoneWarning = AppResource.Phone;
                return false;
            }
            
            if (!Validator.IsAgeCorrect(Age))
            {
                AgeWarning = AppResource.WrongAge;
                return false;
            }

            return true;
        }
    }
}
