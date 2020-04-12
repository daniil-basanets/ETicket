using DBContextLibrary.Domain.Entities;
using DBContextLibrary.Domain.Interfaces;
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
        public IActionResult Index(string sortBy, string sortDirection)
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

            eTicketDataContext = sortBy switch
            {
                "totalprice" => eTicketDataContext.ApplySortBy(x => x.TotalPrice, sortDirection),
                "date" => eTicketDataContext.ApplySortBy(x => x.Date, sortDirection),
                "tickettype" => eTicketDataContext.ApplySortBy(x => x.TicketType, sortDirection),
                "count" => eTicketDataContext.ApplySortBy(x => x.Count, sortDirection),
                _ => eTicketDataContext
            };

            return View(eTicketDataContext);
        }

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