using System;
using System.Linq;
using System.Reflection;
using ETicket.ApplicationServices.Services.Interfaces;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

using ETicket.ApplicationServices.Services.DataTable.Interfaces;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.DataAccess.Domain.Entities;
using ETicket.Admin.Models.DataTables;

namespace ETicket.Admin.Controllers
{
    [Authorize(Roles = "Admin, SuperUser")]
    public class TransactionHistoryController : Controller
    {
        #region Private Members

        private readonly ITransactionService transactionAppService;
        private readonly IDataTableService<TransactionHistory> dataTableService;

        private readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        public TransactionHistoryController(
            ITransactionService transactionAppService,
            IDataTableService<TransactionHistory> dataTableService)
        {
            this.transactionAppService = transactionAppService;
            this.dataTableService = dataTableService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                var transactions = transactionAppService.GetTransactions();
                var ticketTypes = ticketTypeService.GetTicketType()
                        .OrderBy(t => t.TypeName)
                        .Select(t => new { t.Id, t.TypeName });

                ViewData["TicketTypeId"] = new SelectList(ticketTypes, "Id", "TypeName");

                return View(transactions);
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                log.Warn(nameof(TransactionHistoryController.Details) + " id is null");

                return NotFound();
            }

            try
            {
                var transaction = transactionAppService.GetTransactionById(id.Value);

                if (transaction == null)
                {
                    log.Warn(nameof(TransactionHistoryController.Details) + " transaction is null");

                    return NotFound();
                }

                return View(transaction);
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }            
        }
    }
}