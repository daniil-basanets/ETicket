using System.Threading.Tasks;
using ETicketMobile.Data.Domain.Entities;

namespace ETicketMobile.DataAccess.Domain.Interfaces
{
    public interface ITokenRepository
    {
        Task<Token> GetTokenAsync();

        Task SaveTokenAsync(Token token);
    }
}