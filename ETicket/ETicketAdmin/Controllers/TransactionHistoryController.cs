using System;
using System.Linq;
using ETicket.ApplicationServices.Services;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ETicket.Admin.Controllers
{
    [Authorize(Roles = "Admin, SuperUser")]
    public class TransactionHistoryController : Controller
    {
        #region Private Members

        private readonly IUnitOfWork unitOfWork;
        private DatabaseServices services;

        #endregion

        public TransactionHistoryController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            //services =new IntegratedServices(unitOfWork, )
        }

        // GET: TransactionHistories
        public IActionResult Index()
        {
            var ticketTypes = unitOfWork.TicketTypes.GetAll();
            ViewData["TicketTypeId"] = new SelectList(ticketTypes, "Id", "TypeName");

            IQueryable<TransactionHistory> eTicketDataContext = unitOfWork
                    .TransactionHistory
                    .GetAll()
                    .AsNoTracking()
                    .Include(t => t.TicketType);

            return View(eTicketDataContext);
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