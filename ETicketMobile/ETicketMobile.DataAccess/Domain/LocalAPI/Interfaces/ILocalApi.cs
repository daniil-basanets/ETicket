using System.Threading.Tasks;
using ETicketMobile.Data.Domain.Entities;

namespace ETicketMobile.DataAccess.Domain.LocalAPI.Interfaces
{
    public interface ILocalApi
    {
        Task<Token> GetTokenAsync();

        Task AddAsync(Token token);
    }
}