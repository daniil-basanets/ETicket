using System.Threading.Tasks;
using ETicketMobile.Data.Entities;
using ETicketMobile.DataAccess.Interfaces;
using Newtonsoft.Json;

namespace ETicketMobile.DataAccess.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        #region Fields

        private readonly SettingsRepository settingsRepository;

        #endregion

        public TokenRepository()
        {
            settingsRepository = new SettingsRepository();
            settingsRepository.Connect();
        }

        public async Task<Token> GetTokenAsync()
        {
            var serializedToken = await settingsRepository.GetByNameAsync("Token").ConfigureAwait(false);

            if (serializedToken == null)
                return null;

            var token = JsonConvert.DeserializeObject<Token>(serializedToken);

            return token;
        }

        public async Task SaveTokenAsync(Token token)
        {
            var serializedToken = JsonConvert.SerializeObject(token);

            await settingsRepository.SaveAsync("Token", serializedToken);
        }
    }
}