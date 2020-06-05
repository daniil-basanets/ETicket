using System.Threading.Tasks;
using ETicketMobile.WebAccess.DTO;

namespace ETicketMobile.Business.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> RequestChangePasswordAsync(string email, string password);

        Task<bool> CreateNewUserAsync(UserSignUpRequestDto userSignUpRequestDto);
    }
}