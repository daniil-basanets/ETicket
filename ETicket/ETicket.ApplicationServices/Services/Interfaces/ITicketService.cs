using System;
using System.Collections.Generic;
using ETicket.Admin.Models.DataTables;
using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services.PagingServices.Models;
using ETicket.DataAccess.Domain.Entities;

namespace ETicket.ApplicationServices.Services.Interfaces
{
    public interface ITicketService
    {
        public IEnumerable<TicketDto> GetTickets();

        public IEnumerable<TicketDto> GetTicketsByUserId(Guid userId);

        public IEnumerable<TicketApiDto> GetTicketsByUserEmail(string userEmail);

        public TicketDto GetTicketById(Guid id);

        public void Create(TicketDto ticketDto);

        public void Update(TicketDto ticketDto);

        public void Delete(Guid id);

        public void Activate(Guid ticketId);
        
        public DataTablePage<TicketDto> GetTicketsPage(DataTablePagingInfo pagingInfo);
    }
}
