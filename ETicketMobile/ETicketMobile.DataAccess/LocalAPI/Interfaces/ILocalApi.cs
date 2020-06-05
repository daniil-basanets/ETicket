using System.Threading.Tasks;
using ETicketMobile.Data.Entities;

namespace ETicketMobile.DataAccess.LocalAPI.Interfaces
{
    public interface ILocalApi
    {
        Task<Localization> GetLocalizationAsync();

        Task<Token> GetTokenAsync();

        Task AddAsync(Token token);

        Task AddAsync(Localization localization);
    }
}