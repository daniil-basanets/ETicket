using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.ApplicationServices.Services.Users.Interfaces;
using ETicket.DataAccess.Domain.Interfaces;
using ETicket.Admin.Models.DataTables;
using ETicket.DataAccess.Domain.Entities;
using ETicket.Admin.Services;
using ETicket.ApplicationServices.Services.DataTable.Interfaces;
using ETicket.Admin.Services.Interfaces;

namespace ETicket.Admin.Controllers
{
    [Authorize(Roles = "Admin, SuperUser")]
    public class TicketController : Controller
    {
        private readonly IUnitOfWork uow;   //TODO change to service (remove this)
        private readonly ITicketService ticketService;
        private readonly ITicketTypeService ticketTypeService;
        private readonly IUserService userService;
        private readonly IDataTableService dataTableService;

        private void InitViewDataForSelectList(TicketDto ticketDto = null)
        {
            ViewData["TicketTypeId"] = new SelectList(ticketTypeService.GetAll(), "Id", "TypeName", ticketDto?.Id);
            ViewData["TransactionHistoryId"] = new SelectList(uow.TransactionHistory.GetAll(), "Id", "ReferenceNumber", ticketDto?.TransactionHistoryId);   //TODO change to service (remove this)
            ViewData["UserId"] = new SelectList(userService.GetAll().Select(s => new { s.Id, Name = $"{s.LastName} {s.FirstName}" }), "Id", "Name", ticketDto?.UserId);
        }

        public TicketController(IUnitOfWork uow, ITicketService ticketService, ITicketTypeService ticketTypeService, IUserService userService, IDataTablePagingService<Ticket> dataTablePaging)
        {
            this.uow = uow;

            this.ticketService = ticketService;
            this.ticketTypeService = ticketTypeService;
            this.userService = userService;
            dataTableService = new DataTableService<Ticket>(dataTablePaging);
        }

        [HttpGet]
        public IActionResult GetCurrentPage([FromQuery]DataTablePagingInfo pagingInfo)
        {
            return Json(dataTableService.GetDataTablePage(pagingInfo));
        }

        // GET: Tickets
        public IActionResult Index()
        {
            ViewData["TicketTypeId"] = new SelectList(ticketTypeService.GetAll(), "Id", "TypeName"); //TODO change to service 
            //var tickets = ticketService.GetAll();

            return View(/*tickets.ToList()*/);
        }

        // GET: Tickets/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = ticketService.Get((Guid)id);

            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            InitViewDataForSelectList();

            return View();
        }

        // POST: Tickets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TicketDto ticketDto)
        {
            var ticketType = ticketTypeService.Get(ticketDto.TicketTypeId);  //TODO change to service

            if (ticketType.IsPersonal && ticketDto.UserId == null)
            {
                ModelState.AddModelError("", "User is not specified for personal ticket type");
                InitViewDataForSelectList();

                return View(ticketDto);
            }

            if (ModelState.IsValid)
            {
                ticketService.Create(ticketDto);

                return RedirectToAction(nameof(Index));
            }

            InitViewDataForSelectList(ticketDto);

            return View(ticketDto);
        }

        // GET: Tickets/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketDto = ticketService.GetDto((Guid)id);

            if (ticketDto == null)
            {
                return NotFound();
            }

            InitViewDataForSelectList(ticketDto);

            return View(ticketDto);
        }

        // POST: Tickets/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, TicketDto ticketDto)
        {
            if (id != ticketDto.Id)
            {
                return NotFound();
            }

            var ticketType = uow.TicketTypes.Get(ticketDto.TicketTypeId);  //TODO change to service

            if (ticketType.IsPersonal && ticketDto.UserId == null)
            {
                ModelState.AddModelError("", "User is not specified for personal ticket type");
                InitViewDataForSelectList();

                return View(ticketDto);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ticketService.Update(ticketDto);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ticketService.Exists(ticketDto.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            InitViewDataForSelectList(ticketDto);

            return View(ticketDto);
        }

        // GET: Tickets/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = ticketService.Get((Guid)id);

            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            ticketService.Delete(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
