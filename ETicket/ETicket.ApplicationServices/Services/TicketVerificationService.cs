using System;
using System.Collections.Generic;
using System.Text;

using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ETicket.ApplicationServices.Services
{
    public class TicketVerificationService : ITicketVerificationService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly MapperService mapper;

        public TicketVerificationService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            mapper = new MapperService();
        }

        public IEnumerable<TicketVerification> GetTicketVerifications()
        {
            return unitOfWork.TicketVerifications
                    .GetAll()
                    .Include(x => x.Station)
                    .Include(x => x.Ticket)
                    .Include(x => x.Transport);
        }

        public TicketVerification GetTicketVerificationById(Guid id)
        {
            return unitOfWork.TicketVerifications.Get(id);
        }

        public void Create(TicketVerificationDto ticketVerificationDto)
        {
            var ticketVerification = mapper.Map<TicketVerificationDto, TicketVerification>(ticketVerificationDto);

            unitOfWork.TicketVerifications.Create(ticketVerification);
            unitOfWork.Save();
        }
    }
}
