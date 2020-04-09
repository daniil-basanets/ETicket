using System;
using System.Collections.Generic;
using System.Linq;
using DBContextLibrary.Domain.Entities;
using DBContextLibrary.Domain.Interfaces;

namespace DBContextLibrary.Domain.Repositories
{
    public class TransactionHistoryRepository : IRepository<TransactionHistory, Guid>
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
            throw new NotImplementedException();
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public TransactionHistory Get(Guid id)
        {
            return eTicketDataContext.TransactionHistory.Find(id);
        }

        public IQueryable<TransactionHistory> GetAll()
        {
            return eTicketDataContext.TransactionHistory;
        }

        public void Update(TransactionHistory transaction)
        {
            throw new NotImplementedException();
        }
    }
}