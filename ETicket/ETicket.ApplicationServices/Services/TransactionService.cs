using System;
using System.Collections.Generic;
using System.Linq;
using ETicket.Admin.Models.DataTables;
using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services.DataTable;
using ETicket.ApplicationServices.Services.DataTable.Interfaces;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.ApplicationServices.Services.PagingServices;
using ETicket.ApplicationServices.Services.PagingServices.Models;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;

namespace ETicket.ApplicationServices.Services.Transaction
{
    public class TransactionService : ITransactionService
    {
        #region Private Members

        private readonly IUnitOfWork unitOfWork;
        private readonly MapperService mapperService;
        private readonly IDataTableService<TransactionHistory> dataTableService;

        #endregion

        public TransactionService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork
                ?? throw new ArgumentNullException(nameof(unitOfWork));

            mapperService = new MapperService();
            
            var dataTablePagingService = new TransactionHistoryPagingService(unitOfWork);
            dataTableService = new DataTableService<TransactionHistory>(dataTablePagingService);
        }

        public void AddTransaction(TransactionHistoryDto transactionDto)
        {
            if (transactionDto == null)
                throw new ArgumentNullException(nameof(transactionDto));

            if (transactionDto.TotalPrice <= 0)
                throw new ArgumentException("Total Price should be greather than 0");

            var transaction = mapperService.Map<TransactionHistoryDto, TransactionHistory>(transactionDto);

            unitOfWork.TransactionHistory.Create(transaction);
            unitOfWork.Save();
        }

        public IEnumerable<TransactionHistoryDto> GetTransactions()
        {
            var transactions = unitOfWork.TransactionHistory.GetAll();
            
            return mapperService.Map<IQueryable<TransactionHistory>, IEnumerable<TransactionHistoryDto>>(transactions).ToList();
        }

        public IEnumerable<TransactionHistoryDto> GetTransactionsByUserId(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("UserId can't be empty");

            var transactions = unitOfWork.Tickets
                .GetAll()
                .Where(t => t.UserId == userId)
                .Select(t => t.TransactionHistory);
            
            return mapperService.Map<IQueryable<TransactionHistory>, IEnumerable<TransactionHistoryDto>>(transactions).ToList();
        }

        public DataTablePage<TransactionHistoryDto> GetTransactionsPage(DataTablePagingInfo pagingInfo)
        {
            var transactionsPage = dataTableService.GetDataTablePage(pagingInfo);

            return mapperService.Map<DataTablePage<TransactionHistory>, DataTablePage<TransactionHistoryDto>>(transactionsPage);
        }

        public TransactionHistoryDto GetTransactionById(Guid transactionId)
        {
            if (transactionId == Guid.Empty)
                throw new ArgumentException("TransactionId can't be empty");

            var transaction = unitOfWork.TransactionHistory.Get(transactionId);

            if (transaction == null)
                throw new NullReferenceException("Transaction not found.");

            return mapperService.Map<TransactionHistory, TransactionHistoryDto>(transaction);
        }
    }
}