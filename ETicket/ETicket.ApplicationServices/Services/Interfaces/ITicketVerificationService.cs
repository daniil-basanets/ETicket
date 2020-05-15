using System;
using System.Collections.Generic;

using ETicket.ApplicationServices.DTOs;
using ETicket.DataAccess.Domain.Entities;

namespace ETicket.ApplicationServices.Services.Interfaces
{
    public interface ITicketVerificationService
    {
        IEnumerable<TicketVerification> GetTicketVerifications();

        TicketVerificationDto GetTicketVerificationById(Guid id);

        IEnumerable<TicketVerification> GetVerificationHistoryByTicketId(Guid ticketId);

        void Create(TicketVerificationDto ticketVerificationDtoDto);

        VerifyTicketResponceDto VerifyTicket(Guid ticketId, long transportId, float longitude, float latitude);
    }
}
