using System;
using System.Collections.Generic;
using System.Linq;
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
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public TransactionHistory Get(int id)
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