using System;
using System.Collections.Generic;

using ETicket.ApplicationServices.DTOs;
using ETicket.DataAccess.Domain.Entities;

namespace ETicket.ApplicationServices.Services.Interfaces
{
    public interface ITicketVerificationService
    {
        IEnumerable<TicketVerificationDto> GetTicketVerifications();

        TicketVerificationDto GetTicketVerificationById(Guid id);

        IEnumerable<TicketVerification> GetVerificationHistoryByTicketId(Guid ticketId);

        void Create(TicketVerificationDto ticketVerificationDtoDto);

        VerifyTicketResponceDto VerifyTicket(Guid ticketId, int transportId, float longitude, float latitude);
    }
}
