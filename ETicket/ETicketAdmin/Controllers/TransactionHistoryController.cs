using DBContextLibrary.Domain.Entities;
using DBContextLibrary.Domain.Interfaces;
using ETicketAdmin.Common;
using ETicketAdmin.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ETicketAdmin.Controllers
{
    [Authorize]
    public class TransactionHistoryController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private static int pageSize = CommonSettings.DefaultPageSize;

        public TransactionHistoryController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpPost]
        public IActionResult SetPageSize([FromBody]string pageSizeValue)
        {
            pageSize = int.Parse(pageSizeValue);

            return Ok("{}");
        }

        // GET: TransactionHistories
        public async Task<IActionResult> Index(
            string sortBy,
            string sortDirection,
            int? pageNumber,
            string searchBy, // = "count",
            string searchFrom, // = "3",
            string searchTo // = "10"
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

            ViewBag.PageSize = pageSize;

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
        public async Task<IActionResult> Details(Guid? id)
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