using System;
using System.Linq;
using ETicket.ApplicationServices.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ETicket.Admin.Controllers
{
    [Authorize(Roles = "Admin, SuperUser")]
    public class TransactionHistoryController : Controller
    {
        #region Private Members

        private readonly ITransactionAppService transactionAppService;
        private readonly ITicketTypeService ticketTypeService;

        #endregion

        public TransactionHistoryController(
            ITransactionAppService transactionAppService,
            ITicketTypeService ticketTypeService)
        {
            this.transactionAppService = transactionAppService;
            this.ticketTypeService = ticketTypeService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var transactions = transactionAppService.GetTransactions();
            var ticketTypes = ticketTypeService.GetTicketType()
                    .OrderBy(t => t.TypeName)
                    .Select(t => new { t.Id, t.TypeName });

            ViewData["TicketTypeId"] = new SelectList(ticketTypes, "Id", "TypeName");

            return View(transactions);
        }

        [HttpGet]
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = transactionAppService.GetTransactionById(id.Value);

            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }
    }
}