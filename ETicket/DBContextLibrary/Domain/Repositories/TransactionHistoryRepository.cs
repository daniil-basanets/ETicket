using System.Collections.Generic;
using DBContextLibrary.Domain.Entities;
using DBContextLibrary.Domain.Interfaces;

namespace DBContextLibrary.Domain.Repositories
{
    public class TransactionHistoryRepository : IRepository<TransactionHistory>
    {
        #region Private Members

        private readonly ETicketDataContext eTicketDataContext;

        #endregion

        public TransactionHistoryRepository(ETicketDataContext eTicketDataContext)
        {
            this.eTicketDataContext = eTicketDataContext;
        }

        public void Create(TransactionHistory transaction)
        {
            // Stopper
        }

        public void Delete(int id)
        {
            // Stopper
        }

        public TransactionHistory Get(int id)
        {
            // TODO

            var transaction = eTicketDataContext.TransactionHistory.Find(id);

            return transaction;
        }

        public IEnumerable<TransactionHistory> GetAll()
        {
            return eTicketDataContext.TransactionHistory;
        }

        public void Update(TransactionHistory transaction)
        {
            // Stopper
        }
    }
}