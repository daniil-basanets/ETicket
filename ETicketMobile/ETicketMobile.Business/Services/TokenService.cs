using System;
using System.Threading.Tasks;
using ETicketMobile.Business.Mapping;
using ETicketMobile.Business.Services.Interfaces;
using ETicketMobile.Data.Entities;
using ETicketMobile.DataAccess.Services.Interfaces;
using ETicketMobile.WebAccess.DTO;
using ETicketMobile.WebAccess.Network.Endpoints;
using ETicketMobile.WebAccess.Network.WebServices.Interfaces;
using Java.Net;

namespace ETicketMobile.Business.Services
{
    public class TokenService : ITokenService
    {
        #region Fields

        private readonly ILocalTokenService localTokenService;
        private readonly IHttpService httpService;

        #endregion

        public TokenService(ILocalTokenService localTokenService, IHttpService httpService)
        {
            this.localTokenService = localTokenService
                ?? throw new ArgumentNullException(nameof(localTokenService));

            this.httpService = httpService
                ?? throw new ArgumentNullException(nameof(httpService));
        }

        public async Task<Token> GetTokenAsync(string email, string password)
        {
            var userSignIn = new UserSignInRequestDto
            {
                Email = email,
                Password = password
            };

            try
            {
                var tokenDto = await httpService.PostAsync<UserSignInRequestDto, TokenDto>(
                    AuthorizeEndpoint.Login, userSignIn);

                var token = AutoMapperConfiguration.Mapper.Map<Token>(tokenDto);

                return token;
            }
            catch (System.Net.WebException ex)
            {
                throw new Exceptions.WebException("Server exception", ex);
            }
            catch (SocketException ex)
            {
                throw new Exceptions.WebException("Server exception", ex);
            }
        }

        public async Task<string> RefreshTokenAsync()
        {
            try
            {
                var refreshToken = await localTokenService.GetReshreshTokenAsync();

                var tokenDto = await httpService.PostAsync<string, TokenDto>(
                    AuthorizeEndpoint.RefreshToken, refreshToken);

                var token = AutoMapperConfiguration.Mapper.Map<Token>(tokenDto);

                await localTokenService.AddAsync(token);

                return token.AcessJwtToken;
            }
            catch (System.Net.WebException ex)
            {
                throw new Exceptions.WebException("Server exception", ex);
            }
            catch (SocketException ex)
            {
                throw new Exceptions.WebException("Server exception", ex);
            }
        }
    }
}