using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using ETicket.ApplicationServices.Extensions;
using ETicket.ApplicationServices.Services.DataTable.Interfaces;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;


namespace ETicket.ApplicationServices.Services.PagingServices
{
    public class TransactionHistoryPagingService : IDataTablePagingService<TransactionHistory>
    {
        private readonly IUnitOfWork unitOfWork;

        public TransactionHistoryPagingService(IUnitOfWork uow)
        {
            this.unitOfWork = uow;
        }

        public IQueryable<TransactionHistory> GetAll()
        {
            return unitOfWork.TransactionHistory.GetAll();
        }

        public Expression<Func<TransactionHistory, bool>> GetSingleFilterExpression(string columnName, string filterValue)
        {
            return columnName switch
            {
                "totalPrice" => (t => t.TotalPrice.ToString() == filterValue),
                "date" => (t => t.Date.Date == filterValue.ParseToDate()),
                "referenceNumber" => (t => t.ReferenceNumber.StartsWith(filterValue)),
                _ => (t => true)
            };
        }

        public IList<Expression<Func<TransactionHistory, bool>>> GetFilterExpressions(string[] columnNames, string[] filterValues)
        {
            var result = new List<Expression<Func<TransactionHistory, bool>>>();

            for (int i = 0; i < columnNames.Length; i++)
            {
                result.Add(GetSingleFilterExpression(columnNames[i], filterValues[i]));
            }

            return result;
        }

        public IList<Expression<Func<TransactionHistory, bool>>> GetGlobalSearchExpressions(string searchValue)
        {
            return new List<Expression<Func<TransactionHistory, bool>>>
            {
                (t => t.TotalPrice.ToString().StartsWith(searchValue)),
                (t => t.Date.ToString().Contains(searchValue)),
                (t => t.ReferenceNumber.StartsWith(searchValue)),
            };
        }

        public IDictionary<string, Expression<Func<TransactionHistory, object>>> GetSortExpressions()
        {
            return new Dictionary<string, Expression<Func<TransactionHistory, object>>>
            {
                { "totalPrice", (t => t.TotalPrice) },
                { "date", (t => t.Date) },
                { "referenceNumber", (t => t.ReferenceNumber) }
            };
        }
    }
}
