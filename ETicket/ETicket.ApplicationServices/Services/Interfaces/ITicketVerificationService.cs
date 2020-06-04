using System;
using System.Collections.Generic;
using ETicket.Admin.Models.DataTables;
using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services.PagingServices.Models;
using ETicket.DataAccess.Domain.Entities;

namespace ETicket.ApplicationServices.Services.Interfaces
{
    public interface ITicketVerificationService
    {
        IEnumerable<TicketVerificationDto> GetTicketVerifications();

        TicketVerificationDto GetTicketVerificationById(Guid id);

        IEnumerable<TicketVerificationDto> GetVerificationHistoryByTicketId(Guid ticketId);

        void Create(TicketVerificationDto ticketVerificationDtoDto);

        VerifyTicketResponceDto VerifyTicket(Guid ticketId, int transportId, float longitude, float latitude);
        
        public DataTablePage<TicketVerificationDto> GetVerificationsPage(DataTablePagingInfo pagingInfo);
    }
}
