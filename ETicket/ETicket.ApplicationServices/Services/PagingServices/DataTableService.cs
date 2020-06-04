using ETicket.ApplicationServices.Extensions;
using ETicket.Admin.Models.DataTables;
using ETicket.ApplicationServices.Services.DataTable.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ETicket.ApplicationServices.Services.PagingServices.Models;

namespace ETicket.ApplicationServices.Services.DataTable
{
    public class DataTableService<T> : IDataTableService<T>
    {
        private readonly IDataTablePagingService<T> service;
        public DataTableService(IDataTablePagingService<T> service)
        {
            this.service = service;
        }

        public DataTablePage<T> GetDataTablePage(DataTablePagingInfo pagingInfo)
        {
            var data = service.GetAll();
            var drawStep = pagingInfo.DrawCounter;
            var countRecords = pagingInfo.TotalEntries;

            //For single count query
            if (countRecords == -1)
            {
                countRecords = data.Count();
            }

            var sortExpression = service.GetSortExpressions()[pagingInfo.SortColumnName];

            data = GetSortedQuery(data, pagingInfo.SortColumnName
                , pagingInfo.SortColumnDirection, sortExpression);

            var countFiltered = countRecords;

            var whereExpression = MakeWhereExpression(pagingInfo.SearchValue
                    , pagingInfo.FilterColumnNames, pagingInfo.FilterValues);

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

        private Expression<Func<T, bool>> MakeWhereExpression(string globalSearchString, string[] filterColumnNames, string[] filterValues)
        {
            Expression<Func<T, bool>> expression = null;

            if (!string.IsNullOrEmpty(globalSearchString))
            {
                expression = service
                        .GetGlobalSearchExpressions(globalSearchString)
                        .CombineByOrElse();
            }
            if (filterColumnNames != null && filterColumnNames.Length != 0)
            {
                var filterExpression = service
                        .GetFilterExpressions(filterColumnNames, filterValues)
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

        private DataTablePage<T> GetJsonDataTable(IQueryable<T> data, int drawCounter, int countRecords, int countFiltered)
        {
            return new DataTablePage<T>
            {
                DrawCounter = ++drawCounter,
                CountRecords = countRecords,
                CountFiltered = countFiltered,
                PageData = data
            };
        }

        private IQueryable<T> GetSortedQuery(IQueryable<T> query, string columnName, string columnDirection, Expression<Func<T, object>> expression)
        {
            return query.ApplySortBy(expression, columnDirection);
        }

        private IQueryable<T> GetSearchedQuery(IQueryable<T> query, Expression<Func<T, bool>> expression)
        {
            return query.Where(expression);
        }
    }
}