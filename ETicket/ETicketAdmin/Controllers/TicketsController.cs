using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services.Interfaces;
using log4net;
using System.Reflection;
using ETicket.DataAccess.Domain.Entities;

namespace ETicket.Admin.Controllers
{
    [Authorize(Roles = "Admin, SuperUser")]
    public class TicketController : Controller
    {
        #region Private members

        private readonly ITicketService ticketService;
        private readonly ITicketTypeService ticketTypeService;
        private readonly IUserService userService;
        private readonly ITransactionAppService transactionService;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private void InitViewDataForSelectList(TicketDto ticketDto = null)
        {
            try
            {
                ViewData["TicketTypeId"] = new SelectList(ticketTypeService.GetTicketType(), "Id", "TypeName", ticketDto?.Id);
                ViewData["TransactionHistoryId"] = new SelectList(transactionService.GetTransactions(), "Id", "ReferenceNumber", ticketDto?.TransactionHistoryId);
                ViewData["UserId"] = new SelectList(userService.GetUsers().Select(s => new { s.Id, Name = $"{s.LastName} {s.FirstName}" }), "Id", "Name", ticketDto?.UserId);
            }
            catch (Exception e)
            {
                log.Error(e);
            }
        }

        #endregion

        public TicketController(ITransactionAppService transactionAppService, ITicketService ticketService, ITicketTypeService ticketTypeService, IUserService userService)
        {
            this.ticketService = ticketService;
            this.ticketTypeService = ticketTypeService;
            this.userService = userService;
            transactionService = transactionAppService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                ViewData["TicketTypeId"] = new SelectList(ticketTypeService.GetTicketType(), "Id", "TypeName");
                var tickets = ticketService.GetTickets();

                return View(tickets.ToList());
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
                return NotFound();
            }

            Ticket ticket;
            try
            {
                ticket = ticketService.GetTicketById((Guid)id);
            }
            catch (Exception e)
            {
                log.Error(e);
                return BadRequest();
            }

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
            TicketType ticketType = null;
            try
            {
                ticketType = ticketTypeService.GetTicketTypeById(ticketDto.TicketTypeId);
            }
            catch (Exception e)
            {
                log.Error(e);
                return BadRequest();
            }

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
                    ticketService.Create(ticketDto);
                }
                catch (Exception e)
                {
                    log.Error(e);
                    return BadRequest();
                }

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

            TicketDto ticketDto;
            try
            {
                ticketDto = ticketService.GetDto((Guid)id);
            }
            catch (Exception e)
            {
                log.Error(e);
                return BadRequest();
            }

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
            TicketType ticketType = null;

            if (id != ticketDto.Id)
            {
                return NotFound();
            }

            try
            {
                ticketType = ticketTypeService.GetTicketTypeById(ticketDto.TicketTypeId);
            }
            catch (Exception e)
            {
                log.Error(e);
                return BadRequest();
            }

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
                catch (Exception e)
                {
                    log.Error(e);
                    return BadRequest();
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

            Ticket ticket;
            try
            {
                ticket = ticketService.GetTicketById((Guid)id);
            }
            catch (Exception e)
            {
                log.Error(e);
                return BadRequest();
            }

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
            try
            {
                ticketService.Delete(id);
            }
            catch (Exception e)
            {
                log.Error(e);
                return BadRequest();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
