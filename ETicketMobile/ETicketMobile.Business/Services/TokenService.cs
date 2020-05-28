using System;
using System.Threading.Tasks;
using ETicketMobile.Business.Mapping;
using ETicketMobile.Business.Services.Interfaces;
using ETicketMobile.Data.Entities;
using ETicketMobile.DataAccess.LocalAPI.Interfaces;
using ETicketMobile.WebAccess.DTO;
using ETicketMobile.WebAccess.Network.Endpoints;
using ETicketMobile.WebAccess.Network.WebServices.Interfaces;

namespace ETicketMobile.Business.Services
{
    public class TokenService : ITokenService
    {
        #region Fields

        private readonly IHttpService httpService;
        private readonly ILocalApi localApi;

        #endregion

        public TokenService(IHttpService httpService, ILocalApi localApi)
        {
            this.httpService = httpService
                ?? throw new ArgumentNullException(nameof(httpService));

            this.localApi = localApi
                ?? throw new ArgumentNullException(nameof(localApi));
        }

        public async Task<Token> GetTokenAsync(string email, string password)
        {
            var userSignIn = new UserSignInRequestDto
            {
                Email = "bot@gmail.com", // email,
                Password = "qwerty12" // password
            };

            var tokenDto = await httpService.PostAsync<UserSignInRequestDto, TokenDto>(
                AuthorizeEndpoint.Login, userSignIn);

            var token = AutoMapperConfiguration.Mapper.Map<Token>(tokenDto);

            return token;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            var token = await localApi.GetTokenAsync();

            return token.AcessJwtToken;
        }

        public async Task<string> RefreshTokenAsync()
        {
            var refreshTokenTask = await localApi.GetTokenAsync();
            var refreshToken = refreshTokenTask.RefreshJwtToken;

            var tokenDto = await httpService.PostAsync<string, TokenDto>(
                AuthorizeEndpoint.RefreshToken, refreshToken);

            var token = AutoMapperConfiguration.Mapper.Map<Token>(tokenDto);

            await localApi.AddAsync(token);

            return token.AcessJwtToken;
        }
    }
}