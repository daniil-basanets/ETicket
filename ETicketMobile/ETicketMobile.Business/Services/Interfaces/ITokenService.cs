using System.Threading.Tasks;

namespace ETicketMobile.Business.Services.Interfaces
{
    public interface ITokenService
    {
        Task<string> GetAccessTokenAsync();

        Task<string> RefreshTokenAsync();
    }
}