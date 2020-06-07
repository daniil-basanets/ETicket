using System.Threading.Tasks;
using ETicketMobile.Data.Entities;

namespace ETicketMobile.DataAccess.Services.Interfaces
{
    public interface ILocalTokenService
    {
        Task<string> GetAccessTokenAsync();

        Task<string> GetReshreshTokenAsync();

        Task AddAsync(Token token);
    }
}