using System.Threading.Tasks;
using ETicketMobile.Data.Entities;

namespace ETicketMobile.Business.Services.Interfaces
{
    public interface ITokenService
    {
        Task<Token> GetTokenAsync(string email, string password);

        Task<string> RefreshTokenAsync();
    }
}