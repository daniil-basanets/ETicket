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

        #endregion

        public TicketController(ITransactionAppService transactionAppService, ITicketService ticketService, ITicketTypeService ticketTypeService, IUserService userService)
        {
            this.ticketService = ticketService;
            this.ticketTypeService = ticketTypeService;
            this.userService = userService;
            transactionService = transactionAppService;
        }

        private void InitViewDataForSelectList(TicketDto ticketDto = null)
        {
            log.Info(nameof(TicketController.InitViewDataForSelectList));

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

        [HttpGet]
        public IActionResult Index()
        {
            log.Info(nameof(TicketController.Index));

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
            log.Info(nameof(TicketController.Details));

            if (id == null)
            {
                log.Warn(nameof(TicketController.Details) + " id is null");

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
                log.Warn(nameof(TicketController.Details) + " ticket is null");

                return NotFound();
            }

            return View(ticket);
        }

        [HttpGet]
        public IActionResult Create()
        {
            log.Info(nameof(TicketController.Create));

            InitViewDataForSelectList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TicketDto ticketDto)
        {
            log.Info(nameof(TicketController.Create) + ":Post");

            try
            {
                TicketType ticketType = null;

                ticketType = ticketTypeService.GetTicketTypeById(ticketDto.TicketTypeId);

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
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult Edit(Guid? id)
        {
            log.Info(nameof(TicketController.Edit));

            if (id == null)
            {
                log.Warn(nameof(TicketController.Edit) + " id is null");

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
                log.Warn(nameof(TicketController.Edit) + " ticketDto is null");

                return NotFound();
            }

            InitViewDataForSelectList(ticketDto);

            return View(ticketDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, TicketDto ticketDto)
        {
            log.Info(nameof(TicketController.Edit) + ":Post");

            TicketType ticketType = null;

            if (id != ticketDto.Id)
            {
                log.Warn(nameof(TicketController.Edit) + " id is not equal to ticketDto.Id");

                return NotFound();
            }

            try
            {
                ticketType = ticketTypeService.GetTicketTypeById(ticketDto.TicketTypeId);           

                if (ticketType.IsPersonal && ticketDto.UserId == null)
                {
                    ModelState.AddModelError("", "User is not specified for personal ticket type");
                    InitViewDataForSelectList();

                    return View(ticketDto);
                }

                if (ModelState.IsValid)
                {
                    ticketService.Update(ticketDto);
                
                    return RedirectToAction(nameof(Index));
                }

                InitViewDataForSelectList(ticketDto);

                return View(ticketDto);
            }
            catch (Exception e)
            {
                log.Error(e);
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult Delete(Guid? id)
        {
            log.Info(nameof(TicketController.Delete));

            if (id == null)
            {
                log.Warn(nameof(TicketController.Delete) + " id is null");

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
                log.Warn(nameof(TicketController.Delete) + " ticket is null");

                return NotFound();
            }

            return View(ticket);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            log.Info(nameof(TicketController.DeleteConfirmed) + ":Post");

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
