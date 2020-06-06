using System;
using System.Threading.Tasks;
using ETicketMobile.Data.Entities;
using ETicketMobile.DataAccess.Interfaces;
using ETicketMobile.DataAccess.Repositories.Interfaces;
using Newtonsoft.Json;

namespace ETicketMobile.DataAccess.Repositories
{
    public class LocalizationRepository : ILocalizationRepository
    {
        #region Fields

        private readonly ISettingsRepository settingsRepository;

        #endregion

        public LocalizationRepository()
        {
            settingsRepository = new SettingsRepository();
        }

        public LocalizationRepository(ISettingsRepository settingsRepository)
        {
            this.settingsRepository = settingsRepository
                ?? throw new ArgumentNullException(nameof(settingsRepository));
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