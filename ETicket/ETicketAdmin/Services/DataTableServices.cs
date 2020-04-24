using ETicket.Admin.Extensions;
using ETicket.Admin.Models.DataTables;
using ETicket.Admin.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ETicket.Admin.Services
{
    public class DataTableServices<T>
    {
        private readonly IDataTablePaging<T> service;
        public DataTableServices(IDataTablePaging<T> service)
        {
            this.service = service;
        }

        public object GetDataTablePage(DataTableParameters dataTableParameters)
        {
            var data = service.GetAll();
            var drawStep = dataTableParameters.Draw;
            var countRecords = dataTableParameters.TotalEntries;

            //For single count query
            if (countRecords == -1)
            {
                countRecords = data.Count();
            }

            data = GetSortedQuery(data,
                    dataTableParameters.Order, service.GetSortExpression());

            var countFiltered = countRecords;
            var searchString = dataTableParameters.Search.Value;

            if (!string.IsNullOrEmpty(searchString))
            {
                data = GetSearchedQuery(data, service.GetSearchExpression(searchString));
                countFiltered = data.Count();
            }

            data = data
                    .Skip((dataTableParameters.PageNumber - 1) * dataTableParameters.Length)
                    .Take(dataTableParameters.Length);

            return GetJsonDataTable(data, drawStep, countRecords, countFiltered);
        }

        private object GetJsonDataTable(IQueryable<T> data, int drawStep
            , int countRecords, int countFiltered)
        {
            return new
            {
                draw = ++drawStep,
                recordsTotal = countRecords,
                recordsFiltered = countFiltered,
                data = data
            };
        }

        private IQueryable<T> GetSortedQuery(IQueryable<T> query
            , List<DataOrder> orders, List<Expression<Func<T, string>>> expressions)
        {
            var order = orders.First();
            return query.ApplySortBy(expressions[order.Column], order.Dir);
        }

        private IQueryable<T> GetSearchedQuery(IQueryable<T> query
            , Expression<Func<T, bool>> expression)
        {
            return query.Where(expression);
        }

        private IQueryable<T> GetSearchedQuery(IQueryable<T> query
            , List<Expression<Func<T, bool>>> expressions
        )
        {
            var exp = expressions.Combine();

            return query.Where(exp);
        }
    }
}
