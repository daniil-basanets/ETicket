using System;
using System.Linq;
using AutoMapper;
using ETicket.DataAccess.Domain.Interfaces;
using ETicketAdmin.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ETicket.ApplicationServices.Services;
using ETicket.DataAccess.Domain.Entities;
using ETicket.ApplicationServices.Services.Interfaces;

namespace ETicket.Admin.Controllers
{
    [Authorize(Roles = "Admin, SuperUser")]
    public class TicketController : Controller
    {
        private readonly IUnitOfWork uow;   //TODO change to service (remove this)
        private readonly IMapper mapper;
        private readonly ITicketService ticketService;

        public TicketController(IUnitOfWork uow, IMapper mapper, ITicketService ticketService)
        {
            this.uow = uow;
            this.mapper = mapper;
            this.ticketService = ticketService;// new TicketService(uow);
        }

        // GET: Tickets
        public IActionResult Index(string sortOrder)
        {
            ViewData["TicketTypeId"] = new SelectList(uow.TicketTypes.GetAll(), "Id", "TypeName"); //TODO change to service 
            var tickets = ticketService.GetAll();

            return View(tickets.ToList());
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
            //TODO change to service
            ViewData["TicketTypeId"] = new SelectList(uow.TicketTypes.GetAll(), "Id", "TypeName");
            ViewData["TransactionHistoryId"] = new SelectList(uow.TransactionHistory.GetAll(), "Id", "ReferenceNumber");
            ViewData["UserId"] = new SelectList(uow.Users.GetAll(), "Id", "FirstName");

            return View();
        }

        // POST: Tickets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TicketDto ticketDto)
        {
            var ticket = mapper.Map<Ticket>(ticketDto);

            var ticketType = uow.TicketTypes.Get(ticket.TicketTypeId);  //TODO change to service

            if (ticketType.IsPersonal && ticket.UserId == null)
            {
                ModelState.AddModelError("", "User is not specified for personal ticket type");

                //TODO change to service
                ViewData["TicketTypeId"] = new SelectList(uow.TicketTypes.GetAll(), "Id", "TypeName");
                ViewData["TransactionHistoryId"] = new SelectList(uow.TransactionHistory.GetAll(), "Id", "ReferenceNumber");
                ViewData["UserId"] = new SelectList(uow.Users.GetAll(), "Id", "FirstName");

                return View(ticket);
            }

            if (ModelState.IsValid)
            {
                ticketService.Create(ticket);

                return RedirectToAction(nameof(Index));
            }

            //TODO change to service
            ViewData["TicketTypeId"] = new SelectList(uow.TicketTypes.GetAll(), "Id", "TypeName", ticket.TicketTypeId);
            ViewData["TransactionHistoryId"] = new SelectList(uow.TransactionHistory.GetAll(), "Id", "ReferenceNumber", ticket.TransactionHistoryId);
            ViewData["UserId"] = new SelectList(uow.Users.GetAll(), "Id", "FirstName", ticket.UserId);

            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public IActionResult Edit(Guid? id)
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

            //TODO change to service
            ViewData["TicketTypeId"] = new SelectList(uow.TicketTypes.GetAll(), "Id", "TypeName", ticket.TicketTypeId);
            ViewData["TransactionHistoryId"] = new SelectList(uow.TransactionHistory.GetAll(), "Id", "ReferenceNumber", ticket.TransactionHistoryId);
            ViewData["UserId"] = new SelectList(uow.Users.GetAll(), "Id", "FirstName", ticket.UserId);

            return View(ticket);
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

            var ticket = mapper.Map<Ticket>(ticketDto);
            var ticketType = uow.TicketTypes.Get(ticket.TicketTypeId);  //TODO change to service

            if (ticketType.IsPersonal && ticket.UserId == null)
            {
                ModelState.AddModelError("", "User is not specified for personal ticket type");

                //TODO change to service
                ViewData["TicketTypeId"] = new SelectList(uow.TicketTypes.GetAll(), "Id", "TypeName");
                ViewData["TransactionHistoryId"] = new SelectList(uow.TransactionHistory.GetAll(), "Id", "ReferenceNumber");
                ViewData["UserId"] = new SelectList(uow.Users.GetAll(), "Id", "FirstName");

                return View(ticket);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    
                    ticketService.Update(ticket);
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

            //TODO change to service
            ViewData["TicketTypeId"] = new SelectList(uow.TicketTypes.GetAll(), "Id", "TypeName", ticketDto.TicketTypeId);
            ViewData["TransactionHistoryId"] = new SelectList(uow.TransactionHistory.GetAll(), "Id", "ReferenceNumber", ticketDto.TransactionHistoryId);
            ViewData["UserId"] = new SelectList(uow.Users.GetAll(), "Id", "FirstName", ticketDto.UserId);

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
