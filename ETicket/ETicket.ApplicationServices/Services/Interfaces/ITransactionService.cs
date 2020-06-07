using System;
using System.Collections.Generic;
using ETicket.Admin.Models.DataTables;
using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services.PagingServices.Models;
using ETicket.DataAccess.Domain.Entities;

namespace ETicket.ApplicationServices.Services.Interfaces
{
    public interface ITransactionService
    {
        void AddTransaction(TransactionHistoryDto transactionDto);

        IEnumerable<TransactionHistoryDto> GetTransactions();

        TransactionHistoryDto GetTransactionById(Guid id);

        IEnumerable<TransactionHistoryDto> GetTransactionsByUserId(Guid id);
        
        public DataTablePage<TransactionHistoryDto> GetTransactionsPage(DataTablePagingInfo pagingInfo);
    }
}