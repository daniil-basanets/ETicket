using System.Threading.Tasks;
using ETicketMobile.Data.Entities;
using ETicketMobile.DataAccess.LocalAPI.Interfaces;
using ETicketMobile.DataAccess.Repositories;

namespace ETicketMobile.DataAccess.LocalAPI
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

        private static LocalizationRepository localizationRepository;

        public static LocalizationRepository LocalizationRepository
        {
            get
            {
                if (localizationRepository == null)
                {
                    localizationRepository = new LocalizationRepository();
                }

                return localizationRepository;
            }
        }

        public static ILocalApi GetInstance()
        {
            if (localApi == null)
            {
                localApi = new LocalApi();
                tokensRepository = new TokenRepository();
                localizationRepository = new LocalizationRepository();
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

        public Task AddAsync(Localization localization)
        {
            return localizationRepository.SaveLocalizationAsync(localization);
        }

        public Task<Localization> GetLocalizationAsync()
        {
            return localizationRepository.GetLocalizationAsync();
        }
    }
}