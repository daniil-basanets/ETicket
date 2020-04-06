using DBContextLibrary.Domain;
using DBContextLibrary.Domain.Entities;
using ETicketAdmin.Common;
using ETicketAdmin.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ETicketAdmin.Controllers
{
    public class TransactionHistoryController : Controller
    {
        private readonly ETicketDataContext _context;

        public TransactionHistoryController(ETicketDataContext context)
        {
            _context = context;
        }

        // GET: TransactionHistories
        public async Task<IActionResult> Index(
            string sortBy,
            string sortDirection,
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

            ViewBag.SortDirection = sortDirection == "desc"
                ? "asc"
                : "desc";

            IQueryable<TransactionHistory> eTicketDataContext = _context
                .TransactionHistory
                .AsNoTracking()
                .Include(t => t.TicketType);

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

        // GET: TransactionHistories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transactionHistory = await _context.TransactionHistory
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