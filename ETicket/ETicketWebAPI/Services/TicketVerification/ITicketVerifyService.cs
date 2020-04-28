using ETicket.WebAPI.Models.TicketVerification;

namespace ETicket.WebAPI.Services.TicketVerifyService
{
    public interface ITicketVerifyService
    {
        VerifyTicketResponse VerifyTicket(VerifyTicketRequest request);
    }
}
