using DBContextLibrary.Domain.Entities;
using DBContextLibrary.Domain.Interfaces;
using ETicketAdmin.Common;
using ETicketAdmin.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ETicketAdmin.Controllers
{
    public class TransactionHistoryController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public TransactionHistoryController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // GET: TransactionHistories
        public async Task<IActionResult> Index(
            string sortBy,
            string sortDirection,
            string searchBy,
            string searchFrom,
            string searchTo,
            int? pageNumber
        )
        {
            if (string.IsNullOrEmpty(sortBy)
             || string.IsNullOrEmpty(sortDirection))
            {
                sortBy = "date";
                sortDirection = "desc";
            }
            else
            {
                sortBy = sortBy.ToLower();
                sortDirection = sortDirection.ToLower();
            }

            ViewBag.SortDirection = (sortDirection == "desc") ? "asc" : "desc";

            IQueryable<TransactionHistory> eTicketDataContext = unitOfWork
                    .TransactionHistory
                    .GetAll()
                    .AsNoTracking()
                    .Include(t => t.TicketType);

            searchBy = "count";
            searchFrom = "3";
            searchTo = "10";

            if (!string.IsNullOrEmpty(searchFrom)
             && !string.IsNullOrEmpty(searchTo))
            {
                eTicketDataContext = ApplyFilterBy(eTicketDataContext, searchBy, searchFrom, searchTo);
            }

            eTicketDataContext = sortBy switch
            {
                "totalprice" => eTicketDataContext.ApplySortBy(x => x.TotalPrice, sortDirection),
                "date" => eTicketDataContext.ApplySortBy(x => x.Date, sortDirection),
                "tickettype" => eTicketDataContext.ApplySortBy(x => x.TicketType, sortDirection),
                "count" => eTicketDataContext.ApplySortBy(x => x.Count, sortDirection),
                _ => eTicketDataContext
            };

            if (!pageNumber.HasValue)
                pageNumber = 1;

            var pageSize = CommonSettings.DefaultPageSize;

            return View(await PaginatedList<TransactionHistory>.CreateAsync(eTicketDataContext, pageNumber.Value, pageSize));
        }

        private IQueryable<TransactionHistory> ApplyFilterBy(
            IQueryable<TransactionHistory> query,
            string searchBy,
            string searchFrom,
            string searchTo) => searchBy switch
            {
                "totalprice" => ((Func<IQueryable<TransactionHistory>>)(() =>
                {
                    var totalpriceFrom = int.Parse(searchFrom);
                    var totalpriceTo = int.Parse(searchTo);

                    return query.Where(t => t.TotalPrice >= totalpriceFrom
                                         && t.TotalPrice <= totalpriceTo);
                }))(),
                "date" => ((Func<IQueryable<TransactionHistory>>)(() =>
                {
                    var dateFrom = DateTime.Parse(searchFrom);
                    var dateTo = DateTime.Parse(searchTo);

                    return query.Where(t => t.Date >= dateFrom
                                         && t.Date <= dateTo);
                }))(),
                "tickettype" => ((Func<IQueryable<TransactionHistory>>)(() =>
                {
                    var ticketTypeFrom = uint.Parse(searchFrom);
                    var ticketTypeTo = uint.Parse(searchTo);

                    return query.Where(t => t.TicketType.DurationHours >= ticketTypeFrom
                                         && t.TicketType.DurationHours <= ticketTypeTo);
                }))(),
                "count" => ((Func<IQueryable<TransactionHistory>>)(() =>
                {
                    var countFrom = int.Parse(searchFrom);
                    var countTo = int.Parse(searchTo);

                    return query.Where(t => t.Count >= countFrom
                                         && t.Count <= countTo);
                }))(),

                _ => query,
            };

        // GET: TransactionHistories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transactionHistory = await unitOfWork
                    .TransactionHistory
                    .GetAll()
                    .AsNoTracking()
                    .Include(t => t.TicketType)
                    .FirstOrDefaultAsync(m => m.Id == id);

            if (transactionHistory == null)
            {
                return NotFound();
            }

            return View(transactionHistory);
        }
    }
}