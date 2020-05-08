using System.Threading.Tasks;
using ETicketMobile.Data.Domain.Entities;
using ETicketMobile.DataAccess.Domain.LocalAPI.Interfaces;
using ETicketMobile.DataAccess.Domain.Repositories;

namespace ETicketMobile.DataAccess.Domain.LocalAPI
{
    public class LocalApi : ILocalApi
    {
        private static ILocalApi localApi;

        private static TokenRepository tokensRepository;

        public static TokenRepository TokensRepository
        {
            get
            {
                if (tokensRepository == null)
                {
                    tokensRepository = new TokenRepository();
                }

                return tokensRepository;
            }
        }

        public static ILocalApi GetInstance()
        {
            if (localApi == null)
            {
                localApi = new LocalApi();
                tokensRepository = new TokenRepository();
            }

            return localApi;
        }

        public Task AddAsync(Token token)
        {
            return tokensRepository.SaveTokenAsync(token);
        }

        public Task<Token> GetTokenAsync()
        {
            return tokensRepository.GetTokenAsync();
        }
    }
}