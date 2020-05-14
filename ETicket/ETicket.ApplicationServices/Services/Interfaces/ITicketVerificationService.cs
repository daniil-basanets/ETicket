using System;
using System.Collections.Generic;

using ETicket.ApplicationServices.DTOs;
using ETicket.DataAccess.Domain.Entities;

namespace ETicket.ApplicationServices.Services.Interfaces
{
    public interface ITicketVerificationService
    {
        public IEnumerable<TicketVerification> GetTicketVerifications();

        public TicketVerification GetTicketVerificationById(Guid id);

        public void Create(TicketVerificationDto ticketVerificationDtoDto);
    }
}
