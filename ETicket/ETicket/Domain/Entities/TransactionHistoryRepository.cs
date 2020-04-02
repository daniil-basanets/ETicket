using System.Collections.Generic;
using ETicket.Domain.Interfaces;

namespace ETicket.Domain.Entities
{
    public class TransactionHistoryRepository : IRepository<TransactionHistory>
    {
        private readonly ETicketDataContext eTicketDataContext;
        public TransactionHistoryRepository(ETicketDataContext eTicketDataContext)
        {
            this.eTicketDataContext = eTicketDataContext;
        }

        public void Create(TransactionHistory item)
        {
            // TODO
        }

        public void Delete(int id)
        {
            // TODO
        }

        public TransactionHistory Get(int id)
        {
            // TODO

            return null;
        }

        public IEnumerable<TransactionHistory> GetAll()
        {
            return eTicketDataContext.TransactionHistory;
        }

        public void Update(TransactionHistory item)
        {
            // TODO
        }
    }
}