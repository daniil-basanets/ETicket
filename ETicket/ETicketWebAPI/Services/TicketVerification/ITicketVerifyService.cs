using ETicket.WebAPI.Models.TicketVerification;
using System;

namespace ETicket.WebAPI.Services.TicketsService
{
    public interface ITicketVerifyService
    {
        VerifyTicketResponse VerifyTicket(Guid ticketId, VerifyTicketInfo request);
    }
}
