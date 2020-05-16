using System;
using System.Collections.Generic;
using System.Linq;
using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;

namespace ETicket.ApplicationServices.Services.Transaction
{
    public class TransactionService : ITransactionService
    {
        #region Private Members

        private readonly IUnitOfWork unitOfWork;
        private readonly MapperService mapperService;

        #endregion

        public TransactionService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;

            mapperService = new MapperService();
        }

        public void AddTransaction(TransactionHistoryDto transactionDto)
        {
            var transaction = mapperService.Map<TransactionHistoryDto, TransactionHistory>(transactionDto);

            unitOfWork.TransactionHistory.Create(transaction);
            unitOfWork.Save();
        }

        public IEnumerable<TransactionHistory> GetTransactions()
        {
            return unitOfWork
                    .TransactionHistory
                    .GetAll()
                    .ToList();
        }

        public TransactionHistoryDto GetTransactionById(Guid id)
        {
            return mapperService.Map<TransactionHistory, TransactionHistoryDto>(unitOfWork.TransactionHistory.Get(id));
        }
    }
}