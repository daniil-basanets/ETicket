using System.Threading.Tasks;

namespace ETicketMobile.Business.Validators.Interfaces
{
    public interface IUserValidator
    {
        Task<bool> UserExistsAsync(string email);
    }
}