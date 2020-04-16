using System;
using System.Collections.Generic;
using System.Linq;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;

namespace ETicket.DataAccess.Domain.Repositories
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
            eTicketDataContext.Add(transaction);
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