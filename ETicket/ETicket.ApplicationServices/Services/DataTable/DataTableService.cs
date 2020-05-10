using ETicket.Admin.Extensions;
using ETicket.Admin.Models.DataTables;
using ETicket.ApplicationServices.Services.DataTable.Interfaces;
using ETicket.ApplicationServices.Services.DataTable.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ETicket.ApplicationServices.Services.DataTable
{
    public class DataTableService<T> : IDataTableService
    {
        private readonly IDataTablePagingService<T> service;
        public DataTableService(IDataTablePagingService<T> service)
        {
            this.service = service;
        }

        public object GetDataTablePage(DataTablePagingInfo pagingInfo)
        {
            var data = service.GetAll();
            var drawStep = pagingInfo.DrawCounter;
            var countRecords = pagingInfo.TotalEntries;

            //For single count query
            if (countRecords == -1)
            {
                countRecords = data.Count();
            }

            data = GetSortedQuery(data, pagingInfo.SortColumnNumber
                , pagingInfo.SortColumnDirection, service.GetSortExpressions());

            var countFiltered = countRecords;

            var whereExpression = MakeWhereExpression(pagingInfo.SearchValue
                    , pagingInfo.FilterColumnNumbers, pagingInfo.FilterValues);

            if(whereExpression != null)
            {
                data = GetSearchedQuery(data, whereExpression);
                countFiltered = data.Count();
            }

            data = data
                    .Skip((pagingInfo.PageNumber - 1) * pagingInfo.PageSize)
                    .Take(pagingInfo.PageSize);

            return GetJsonDataTable(data, drawStep, countRecords, countFiltered);
        }

        private Expression<Func<T, bool>> MakeWhereExpression(string globalSearchString, int[] filterColumnNumbers, string[] filterValues)
        {
            Expression<Func<T, bool>> expression = null;

            if (!string.IsNullOrEmpty(globalSearchString))
            {
                expression = service
                        .GetSearchExpressions(globalSearchString)
                        .CombineByOrElse();
            }
            if (filterColumnNumbers != null && filterColumnNumbers.Length != 0)
            {
                var filterExpression = service
                        .GetFilterExpressions(filterColumnNumbers, filterValues)
                        .CombineByAndAlso();

                if (expression != null)
                {                    
                    expression = expression.AndAlso(filterExpression);
                }
                else
                {
                    expression = filterExpression;
                }
            }

            return expression;
        }

        private object GetJsonDataTable(IQueryable<T> data, int drawCounter, int countRecords, int countFiltered)
        {
            return new
            {
                draw = ++drawCounter,
                recordsTotal = countRecords,
                recordsFiltered = countFiltered,
                data = data
            };
        }

        private IQueryable<T> GetSortedQuery(IQueryable<T> query, int columnNumber, string columnDirection, IList<Expression<Func<T, string>>> expressions)
        {
            return query.ApplySortBy(expressions[columnNumber], columnDirection);
        }

        private IQueryable<T> GetSearchedQuery(IQueryable<T> query, Expression<Func<T, bool>> expression)
        {
            return query.Where(expression);
        }
    }
}
