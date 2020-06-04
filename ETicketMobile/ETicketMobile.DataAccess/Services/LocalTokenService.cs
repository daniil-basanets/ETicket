using System;
using System.Threading.Tasks;
using ETicketMobile.Data.Entities;
using ETicketMobile.DataAccess.LocalAPI.Interfaces;
using ETicketMobile.DataAccess.Services.Interfaces;

namespace ETicketMobile.DataAccess.Services
{
    public class LocalTokenService : ILocalTokenService
    {
        #region Fields

        private readonly ILocalApi localApi;

        #endregion

        public LocalTokenService(ILocalApi localApi)
        {
            this.localApi = localApi
                ?? throw new ArgumentNullException(nameof(localApi));
        }

        public Task AddAsync(Token token)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetAccessTokenAsync()
        {
            var token = await localApi.GetTokenAsync();

            return token.AcessJwtToken;
        }

        public async Task<string> GetReshreshTokenAsync()
        {
            var token = await localApi.GetTokenAsync();

            return token.RefreshJwtToken;
        }
    }
}