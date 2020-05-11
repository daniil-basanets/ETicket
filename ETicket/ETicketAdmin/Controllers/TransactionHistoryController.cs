using ETicket.ApplicationServices.Extensions;
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
using ETicket.ApplicationServices.Services.DataTable.Interfaces;
using ETicket.ApplicationServices.Services.DataTable;

namespace ETicket.Admin.Controllers
{
    [Authorize(Roles = "Admin, SuperUser")]
    public class TransactionHistoryController : Controller
    {
        #region Private Members

        private readonly IUnitOfWork unitOfWork;
        private readonly IDataTableService dataTableService;

        #endregion

        public TransactionHistoryController(IUnitOfWork unitOfWork, IDataTablePagingService<TransactionHistory> dataTablePaging)
        {
            this.unitOfWork = unitOfWork;
            dataTableService = new DataTableService<TransactionHistory>(dataTablePaging);
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