using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.DataAccess.Domain.Interfaces;

namespace ETicket.Admin.Controllers
{
    [Authorize(Roles = "Admin, SuperUser")]
    public class TicketController : Controller
    {
        private readonly IUnitOfWork uow;   //TODO change to service (remove this)
        private readonly ITicketService ticketService;
        private readonly ITicketTypeService ticketTypeService;
        private readonly IUserService userService;

        private void InitViewDataForSelectList(TicketDto ticketDto = null)
        {
            ViewData["TicketTypeId"] = new SelectList(ticketTypeService.GetTicketType(), "Id", "TypeName", ticketDto?.Id);
            ViewData["TransactionHistoryId"] = new SelectList(uow.TransactionHistory.GetAll(), "Id", "ReferenceNumber", ticketDto?.TransactionHistoryId);   //TODO change to service (remove this)
            ViewData["UserId"] = new SelectList(userService.GetUsers().Select(s => new { s.Id, Name = $"{s.LastName} {s.FirstName}" }), "Id", "Name", ticketDto?.UserId);
        }

        public TicketController(IUnitOfWork uow, ITicketService ticketService, ITicketTypeService ticketTypeService, IUserService userService)
        {
            this.uow = uow;

            this.ticketService = ticketService;
            this.ticketTypeService = ticketTypeService;
            this.userService = userService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewData["TicketTypeId"] = new SelectList(ticketTypeService.GetTicketType(), "Id", "TypeName"); //TODO change to service 
            var tickets = ticketService.GetTickets();

            return View(tickets.ToList());
        }

        [HttpGet]
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = ticketService.GetTicketById((Guid)id);

            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        [HttpGet]
        public IActionResult Create()
        {
            InitViewDataForSelectList();

            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TicketDto ticketDto)
        {
            var ticketType = ticketTypeService.GetTicketTypeById(ticketDto.TicketTypeId);  //TODO change to service

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

        [HttpGet]
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

        [HttpGet]
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = ticketService.GetTicketById((Guid)id);

            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            ticketService.Delete(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
