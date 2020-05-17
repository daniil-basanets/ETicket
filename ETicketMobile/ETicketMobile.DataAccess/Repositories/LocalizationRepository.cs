using System.Threading.Tasks;
using ETicketMobile.Data.Entities;
using ETicketMobile.DataAccess.Interfaces;
using Newtonsoft.Json;

namespace ETicketMobile.DataAccess.Repositories
{
    public class LocalizationRepository : ILocalizationRepository
    {
        private readonly SettingsRepository settingsRepository;

        public LocalizationRepository()
        {
            settingsRepository = new SettingsRepository();
            settingsRepository.Connect();
        }

        public async Task<Localization> GetLocalizationAsync()
        {
            var serializedLocalization = await settingsRepository.GetByNameAsync("Localization").ConfigureAwait(false);

            if (serializedLocalization == null)
                return null;

            var localization = JsonConvert.DeserializeObject<Localization>(serializedLocalization);

            return localization;
        }

        public async Task SaveLocalizationAsync(Localization localization)
        {
            var serializedLocalization = JsonConvert.SerializeObject(localization);

            await settingsRepository.SaveAsync("Localization", serializedLocalization);
        }
    }
}