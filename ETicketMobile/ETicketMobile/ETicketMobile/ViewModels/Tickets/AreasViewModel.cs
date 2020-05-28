﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using ETicketMobile.Business.Mapping;
using ETicketMobile.Business.Model.Tickets;
using ETicketMobile.Business.Validators;
using ETicketMobile.Data.Entities;
using ETicketMobile.DataAccess.LocalAPI.Interfaces;
using ETicketMobile.Resources;
using ETicketMobile.Views.Payment;
using ETicketMobile.WebAccess.DTO;
using ETicketMobile.WebAccess.Network.Endpoints;
using ETicketMobile.WebAccess.Network.WebServices.Interfaces;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;

namespace ETicketMobile.ViewModels.Tickets
{
    public class AreasViewModel : ViewModelBase
    {
        #region Fields

        private INavigationParameters navigationParameters;

        private readonly IPageDialogService dialogService;
        private readonly IHttpService httpService;

        private readonly ILocalApi localApi;

        private string accessToken;

        private ICommand buy;

        private IEnumerable<Area> areas;

        #endregion

        #region Properties

        public ICommand Buy => buy 
            ??= new Command(OnBuy);

        public IEnumerable<Area> Areas
        {
            get => areas;
            set => SetProperty(ref areas, value);
        }

        #endregion

        public AreasViewModel(
            INavigationService navigationService,
            IPageDialogService dialogService,
            IHttpService httpService,
            ILocalApi localApi
        ) : base(navigationService)
        {
            this.dialogService = dialogService
                ?? throw new ArgumentNullException(nameof(dialogService));

            this.localApi = localApi
                ?? throw new ArgumentNullException(nameof(localApi));

            this.httpService = httpService
                ?? throw new ArgumentNullException(nameof(httpService));
        }

        public async override void OnAppearing()
        {
            try
            {
                accessToken = await GetAccessTokenAsync();
                Areas = await GetAreasAsync();
            }
            catch (WebException)
            {
                await dialogService.DisplayAlertAsync("Error", "Check connection with server", "OK");

                return;
            }
        }

        public override void OnNavigatedTo(INavigationParameters navigationParameters)
        {
            this.navigationParameters = navigationParameters;
        }

        private async Task<string> GetAccessTokenAsync()
        {
            var token = await localApi.GetTokenAsync();

            return token.AcessJwtToken;
        }

        private async Task<IEnumerable<Area>> GetAreasAsync()
        {
            var areasDto = await httpService.GetAsync<IEnumerable<AreaDto>>(AreasEndpoint.GetAreas, accessToken);

            if (areasDto == null)
            {
                accessToken = await RefreshTokenAsync();

                areasDto = await httpService.GetAsync<IEnumerable<AreaDto>>(AreasEndpoint.GetAreas, accessToken);
            }

            var areas = AutoMapperConfiguration.Mapper.Map<IEnumerable<Area>>(areasDto);

            return areas;
        }

        private async Task<string> RefreshTokenAsync()
        {
            var refreshTokenTask = await localApi.GetTokenAsync();
            var refreshToken = refreshTokenTask.RefreshJwtToken;

            var tokenDto = await httpService.PostAsync<string, TokenDto>(AuthorizeEndpoint.RefreshToken, refreshToken);

            var token = AutoMapperConfiguration.Mapper.Map<Token>(tokenDto);

            await localApi.AddAsync(token);

            return token.AcessJwtToken;
        }

        private async void OnBuy()
        {
            var selectedAreasId = Areas;
                    //.Where(a => a.Selected)
                    //.Select(a => a.Id);

            if (!IsValid(selectedAreasId.Count()))
                return;

            navigationParameters.Add("areas", selectedAreasId);
            await NavigationService.NavigateAsync(nameof(LiqPayView), navigationParameters);
        }

        private bool IsValid(int count)
        {
            if (!Validator.TicketChoosed(count))
            {
                dialogService.DisplayAlertAsync("Error", AppResource.CountTicketsWrong, "OK");

                return false;
            }

            return true;
        }
    }
}