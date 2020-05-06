using ETicket.Admin.Extensions;
using ETicket.Admin.Models.DataTables;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ETicket.Admin.Controllers
{
    [Authorize(Roles = "Admin, SuperUser")]
    public class TransactionHistoryController : Controller
    {
        #region Private Members

        private readonly IUnitOfWork unitOfWork;

        #endregion

        public TransactionHistoryController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // GET: TransactionHistories
        public IActionResult Index()
        {
            var ticketTypes = unitOfWork
                    .TicketTypes
                    .GetAll()
                    .AsNoTracking();

            ViewData["TicketTypeId"] = new SelectList(ticketTypes, "Id", "TypeName");

            return View();
        }

        //[HttpPost]
        //public IActionResult GetCurrentPage(DataTableParameters dataTableParameters)
        //{
        //    var drawStep = int.Parse(Request.Form["draw"]);

        //    var countRecords = unitOfWork
        //            .TransactionHistory
        //            .GetAll()
        //            .AsNoTracking()
        //            .Count();

        //    IQueryable<TransactionHistory> eTicketDataContext = unitOfWork
        //           .TransactionHistory
        //           .GetAll()
        //           .AsNoTracking()
        //           .Include(t => t.TicketType);

        //    SortDataTable(ref eTicketDataContext, dataTableParameters.Order);
        //    SearchInDataTable(ref eTicketDataContext, dataTableParameters.Search.Value);

        //    var countFiltered = eTicketDataContext.Count();

        //    eTicketDataContext = eTicketDataContext
        //            .Skip(dataTableParameters.Start)
        //            .Take(dataTableParameters.PageSize);

        //    return GetCurrentPage(eTicketDataContext, drawStep, countRecords, countFiltered);
        //}

        //private void SearchInDataTable(
        //    ref IQueryable<TransactionHistory> eTicketDataContext,
        //    string searchString
        //)
        //{
        //    //if (!string.IsNullOrEmpty(searchString))
        //    //{
        //    //    eTicketDataContext = eTicketDataContext.ApplySearchBy(
        //    //        t =>
        //    //        t.TicketType.TypeName.StartsWith(searchString)
        //    //         || t.ReferenceNumber.StartsWith(searchString)
        //    //         || t.Date.ToString().Contains(searchString)
        //    //         || t.Count.ToString().StartsWith(searchString)
        //    //         || t.TotalPrice.ToString().StartsWith(searchString)
        //    //         );
        //    //}
        //}

        //private void SortDataTable(
        //    ref IQueryable<TransactionHistory> eTicketDataContext,
        //    List<SortingOrder> orders
        //)
        //{
        //    foreach (var order in orders)
        //    {
        //        eTicketDataContext = order.ColumnNumber switch
        //        {
        //            0 => eTicketDataContext.ApplySortBy(t => t.TotalPrice, order.Direction),
        //            1 => eTicketDataContext.ApplySortBy(t => t.Date, order.Direction),
        //            2 => eTicketDataContext.ApplySortBy(t => t.TicketType, order.Direction),
        //            3 => eTicketDataContext.ApplySortBy(t => t.Count, order.Direction),
        //            _ => eTicketDataContext.ApplySortBy(t => t.Date, "desc")
        //        };
        //    }
        //}

        private JsonResult GetCurrentPage(
            IQueryable<TransactionHistory> transactionHistory,
            int drawStep,
            int countRecords,
            int countFiltered
        )
        {
            return Json(new
            {
                draw = ++drawStep,
                recordsTotal = countRecords,
                recordsFiltered = countFiltered,
                data = transactionHistory
            });
        }

        // GET: TransactionHistories/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transactionHistory = unitOfWork
                    .TransactionHistory
                    .GetAll()
                    .AsNoTracking()
                    .Include(t => t.TicketType)
                    .FirstOrDefault(m => m.Id == id);

            if (transactionHistory == null)
            {
                return NotFound();
            }

            return View(transactionHistory);
        }
    }
}