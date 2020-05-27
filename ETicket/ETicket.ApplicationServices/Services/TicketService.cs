using System;
using System.Linq;
using System.Collections.Generic;
using ETicket.DataAccess.Domain.Interfaces;
using ETicket.DataAccess.Domain.Entities;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.ApplicationServices.DTOs;

namespace ETicket.ApplicationServices.Services
{
    public class TicketService : ITicketService
    {
        private readonly IUnitOfWork uow;
        private readonly MapperService mapper;
        private readonly ITicketTypeService ticketTypeService;

        public TicketService(IUnitOfWork uow, ITicketTypeService ticketTypeService)
        {
            this.uow = uow;
            this.ticketTypeService = ticketTypeService;
            mapper = new MapperService();
        }

        IEnumerable<Ticket> ITicketService.GetTickets()
        {
            return uow.Tickets.GetAll().ToList();
        }

        public TicketDto GetTicketById(Guid id)
        {
            var ticket = uow.Tickets.Get(id);
            var ticketDto = mapper.Map<Ticket, TicketDto>(ticket);

            return ticketDto;
        }

        public void Create(TicketDto ticketDto)
        {
            var ticket = mapper.Map<TicketDto, Ticket>(ticketDto);

            ticket.Id = Guid.NewGuid();
            ticket.CreatedUTCDate = DateTime.UtcNow;

            ticket.TicketType = uow.TicketTypes.Get(ticket.TicketTypeId);
            ticket.TransactionHistory = null;

            if (ticket.ActivatedUTCDate != null)
            {
                ticket.ExpirationUTCDate = ticket.ActivatedUTCDate?.AddHours(ticket.TicketType.DurationHours);
            }

            ticket.TicketArea = new List<TicketArea>();

            foreach (var areaId in ticketDto.SelectedAreaIds)
            {
                TicketArea ticketArea = new TicketArea() { TicketId = ticket.Id, AreaId = areaId };
                ticket.TicketArea.Add(ticketArea);
            }

            uow.Tickets.Create(ticket);
            uow.Save();
        }

        public void Update(TicketDto ticketDto)
        {
            var ticket = mapper.Map<TicketDto, Ticket>(ticketDto);

            if (ticket.TicketArea == null)
            {
                var ticketAreas = uow.TicketArea.GetAll().Where(t => t.TicketId == ticket.Id).ToList();

                foreach (var ticketArea in ticketAreas)
                {
                    uow.TicketArea.Delete(ticketArea);
                }
            }

            foreach (var areaId in ticketDto.SelectedAreaIds)
            {
                uow.TicketArea.Create(new TicketArea() { TicketId = ticket.Id, AreaId = areaId });
            }

            uow.Tickets.Update(ticket);
            uow.Save();
        }

        public void Delete(Guid id)
        {
            uow.Tickets.Delete(id);
            uow.Save();
        }

        public void Activate(Guid ticketId)
        {
            var ticket = uow.Tickets.GetAll().Where(t => t.Id == ticketId).FirstOrDefault();


            if (ticket == null)
            {
                var e = new ApplicationException(nameof(Activate) + " ticket with id = " + ticketId + " does not exists");
                e.Data.Add("ticketId", ticketId);

                throw e;
            }

            ticket.ActivatedUTCDate = DateTime.UtcNow;
            ticket.ExpirationUTCDate = ticket.ActivatedUTCDate?.AddHours(ticket.TicketType.DurationHours);
            uow.Tickets.Update(ticket);
            uow.Save();
        }

        public IEnumerable<TicketsByUserEmailDto> GetTicketsByUserEmail(string userEmail)
        {
            var query = uow.Tickets.GetAll()
                .Where(t => t.User.Email == userEmail)
                .OrderBy(t => t.CreatedUTCDate);

            return mapper.ProjectTo<TicketsByUserEmailDto>(query).ToList<TicketsByUserEmailDto>();
        }
    }
}
