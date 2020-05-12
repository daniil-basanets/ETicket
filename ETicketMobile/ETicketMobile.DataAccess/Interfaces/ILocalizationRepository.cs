using System.Threading.Tasks;
using ETicketMobile.Data.Entities;

namespace ETicketMobile.DataAccess.Interfaces
{
    public interface ILocalizationRepository
    {
        Task<Localization> GetLocalizationAsync();

        Task SaveLocalizationAsync(Localization localization);
    }
}