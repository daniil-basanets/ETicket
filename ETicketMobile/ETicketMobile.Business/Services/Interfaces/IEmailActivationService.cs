using System.Threading.Tasks;

namespace ETicketMobile.Business.Services.Interfaces
{
    public interface IEmailActivationService
    {
        Task<bool> ActivateEmailAsync(string email, string code);

        Task RequestActivationCodeAsync(string email);
    }
}