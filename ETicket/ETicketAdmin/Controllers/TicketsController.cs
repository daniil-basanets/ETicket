using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using ETicket.Admin.Models.DataTables;
using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services.Interfaces;

namespace ETicket.Admin.Controllers
{
    [Authorize(Roles = "Admin, SuperUser")]
    public class TicketController : BaseMvcController
    {
        #region Private members

        private readonly ITicketService ticketService;
        private readonly ITicketTypeService ticketTypeService;
        private readonly IUserService userService;
        private readonly ITransactionService transactionService;
        private readonly IAreaService areaService;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        public TicketController(ITransactionService transactionAppService, ITicketService ticketService, ITicketTypeService ticketTypeService, IUserService userService, IAreaService areaService)
        {
            this.ticketService = ticketService;
            this.ticketTypeService = ticketTypeService;
            this.userService = userService;
            this.transactionService = transactionAppService;
            this.areaService = areaService;
        }

        [HttpGet]
        public IActionResult GetCurrentPage([FromQuery]DataTablePagingInfo pagingInfo)
        {
            return Json(ticketService.GetTicketsPage(pagingInfo));    
        }

        private void InitViewDataForSelectList(TicketDto ticketDto = null)
        {
            log.Info(nameof(TicketController.InitViewDataForSelectList));

            try
            {
                ViewData["TicketTypeId"] = new SelectList(ticketTypeService.GetTicketTypes(), "Id", "TypeName", ticketDto?.Id);
                ViewData["TransactionHistoryId"] = new SelectList(transactionService.GetTransactions(), "Id", "ReferenceNumber", ticketDto?.TransactionHistoryId);
                ViewData["UserId"] = new SelectList(userService.GetUsers().Select(s => new { s.Id, Name = $"{s.LastName} {s.FirstName}" }), "Id", "Name", ticketDto?.UserId);
                ViewData["AreaId"] = new MultiSelectList(areaService.GetAreas().Select(a => new { a.Id, a.Name }), "Id", "Name", ticketDto?.Areas);
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
                ViewData["TicketTypeId"] = new SelectList(ticketTypeService.GetTicketTypes(), "Id", "TypeName");
                
                return View();
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

            TicketDto ticket;

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

            TicketDto ticketDto = new TicketDto();
            ticketDto.SelectedAreaIds = new List<int>();
            InitViewDataForSelectList();

            return View(ticketDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TicketDto ticketDto)
        {
            log.Info(nameof(TicketController.Create) + ":Post");

            try
            {
                TicketTypeDto ticketType = null;

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
                ticketDto = ticketService.GetTicketById((Guid)id);
                ticketDto.SelectedAreaIds = new List<int>();
                foreach (var ticketArea in ticketDto.Areas)
                {
                    ticketDto.SelectedAreaIds.Add(ticketArea.Key);
                }             
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

            TicketTypeDto ticketType = null;

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

            TicketDto ticket;

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
