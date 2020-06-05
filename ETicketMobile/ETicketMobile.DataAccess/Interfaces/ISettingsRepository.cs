using System.Threading.Tasks;

namespace ETicketMobile.DataAccess.Interfaces
{
    public interface ISettingsRepository
    {
        Task<string> GetByNameAsync(string name);

        Task SaveAsync(string name, string value);
    }
}