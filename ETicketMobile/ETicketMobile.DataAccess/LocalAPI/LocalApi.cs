using System;
using System.Threading.Tasks;
using ETicketMobile.Data.Entities;
using ETicketMobile.DataAccess.LocalAPI.Interfaces;
using ETicketMobile.DataAccess.Repositories;
using ETicketMobile.DataAccess.Repositories.Interfaces;

namespace ETicketMobile.DataAccess.LocalAPI
{
    public class LocalApi : ILocalApi
    {
        private static ILocalApi localApi;

        private static ITokenRepository tokenRepository;
        public static ITokenRepository TokenRepository
        {
            get
            {
                if (tokenRepository == null)
                {
                    tokenRepository = new TokenRepository();
                }

                return tokenRepository;
            }
        }

        private static ILocalizationRepository localizationRepository;

        public static ILocalizationRepository LocalizationRepository
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

        public LocalApi()
        {
        }

        public LocalApi(ITokenRepository tokenRepository, ILocalizationRepository localizationRepository)
        {
            LocalApi.localApi = this;

            LocalApi.tokenRepository = tokenRepository
                ?? throw new ArgumentNullException(nameof(tokenRepository));

            LocalApi.localizationRepository = localizationRepository
                ?? throw new ArgumentNullException(nameof(localizationRepository));
        }

        public static ILocalApi GetInstance()
        {
            if (localApi == null)
            {
                localApi = new LocalApi();
                tokenRepository = new TokenRepository();
                localizationRepository = new LocalizationRepository();
            }

            return localApi;
        }

        public Task AddAsync(Token token)
        {
            return tokenRepository.SaveTokenAsync(token);
        }

        public Task<Token> GetTokenAsync()
        {
            return tokenRepository.GetTokenAsync();
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