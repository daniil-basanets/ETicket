using System;
using System.Linq;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;

namespace ETicket.DataAccess.Domain.Repositories
{
    public class TicketVerificationRepository : IRepository<TicketVerification, Guid>
    {
        private readonly ETicketDataContext context;

        public TicketVerificationRepository(ETicketDataContext context)
        {
            this.context = context;
        }

        public void Create(TicketVerification ticketVerification)
        {
            context.Add(ticketVerification);
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public TicketVerification Get(Guid id)
        {
            return context.TicketVerifications.Find(id);
        }

        public IQueryable<TicketVerification> GetAll()
        {
            return context.TicketVerifications;
        }

        public void Update(TicketVerification item)
        {
            throw new NotImplementedException();
        }
    }
}
