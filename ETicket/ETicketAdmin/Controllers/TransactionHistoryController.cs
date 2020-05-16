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

        #endregion

        public TransactionHistoryController(
            ITransactionService transactionAppService,
            IDataTableService<TransactionHistory> dataTableService)
        {
            this.transactionAppService = transactionAppService;
            this.dataTableService = dataTableService;
        }

        // GET: TransactionHistories
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetCurrentPage([FromQuery]DataTablePagingInfo pagingInfo)
        {
            return Json(dataTableService.GetDataTablePage(pagingInfo));
        }

        // GET: TransactionHistories/Details/5
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