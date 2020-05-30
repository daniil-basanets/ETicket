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
            this.unitOfWork = unitOfWork;

            mapperService = new MapperService();
            
            var dataTablePagingService = new TransactionHistoryPagingService(unitOfWork);
            dataTableService = new DataTableService<TransactionHistory>(dataTablePagingService);
        }

        public void AddTransaction(TransactionHistoryDto transactionDto)
        {
            var transaction = mapperService.Map<TransactionHistoryDto, TransactionHistory>(transactionDto);

            unitOfWork.TransactionHistory.Create(transaction);
            unitOfWork.Save();
        }

        public IEnumerable<TransactionHistoryDto> GetTransactions()
        {
            var transactions = unitOfWork.TransactionHistory.GetAll();
            
            return mapperService.Map<IQueryable<TransactionHistory>, IEnumerable<TransactionHistoryDto>>(transactions).ToList();
        }

        public IEnumerable<TransactionHistoryDto> GetTransactionsByUserId(Guid id)
        {
            var transactions = unitOfWork.Tickets
                .GetAll()
                .Where(t => t.UserId == id)
                .Select(t => t.TransactionHistory);
            
            return mapperService.Map<IQueryable<TransactionHistory>, IEnumerable<TransactionHistoryDto>>(transactions).ToList();
        }

        public DataTablePage<TransactionHistoryDto> GetTransactionsPage(DataTablePagingInfo pagingInfo)
        {
            var transactionsPage = dataTableService.GetDataTablePage(pagingInfo);

            return mapperService.Map<DataTablePage<TransactionHistory>, DataTablePage<TransactionHistoryDto>>(transactionsPage);
        }

        public TransactionHistoryDto GetTransactionById(Guid id)
        {
            return mapperService.Map<TransactionHistory, TransactionHistoryDto>(unitOfWork.TransactionHistory.Get(id));
        }
    }
}