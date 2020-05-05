using System;
using System.Reflection;
using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using log4net;
using ETicket.DataAccess.Domain.Entities;

namespace ETicket.Admin.Controllers
{
    [Authorize(Roles = "Admin, SuperUser")]
    public class TicketTypeController : Controller
    {
        private readonly ITicketTypeService ticketTypeService;
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public TicketTypeController(ITicketTypeService ticketTypeService)
        {
            this.ticketTypeService = ticketTypeService;
        }
        
        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                var ticketTypes = ticketTypeService.GetTicketType();
                
                return View(ticketTypes);
            }
            catch (Exception exception)
            {
                logger.Error(exception);

                return BadRequest();
            }
        }
        
        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                logger.Warn(nameof(TicketTypeController.Details) + " id is null");

                return NotFound();
            }

            TicketType ticketType;

            try
            {
                ticketType = ticketTypeService.GetTicketTypeById(id.Value);                
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                
                return BadRequest();
            }

            if (ticketType == null)
            {
                logger.Warn(nameof(TicketTypeController.Details) + " ticketType is null");
                
                return NotFound();
            }

            return View(ticketType);
        }
        
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TicketTypeDto ticketTypeDto)
        {
            if (!ModelState.IsValid)
            {                
                return View(ticketTypeDto);
            }

            try
            {
                ticketTypeService.Create(ticketTypeDto);                
            }
            catch (Exception exception)
            {
                logger.Error(exception);

                return BadRequest();
            }
            
            return RedirectToAction(nameof(Index));
        }
        
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                logger.Warn(nameof(TicketTypeController.Edit) + " id is null");

                return NotFound();
            }

            TicketType ticketType;

            try
            {
                ticketType = ticketTypeService.GetTicketTypeById(id.Value);
            }
            catch (Exception exception)
            {
                logger.Error(exception);

                return BadRequest();
            }

            if (ticketType == null)
            {
                logger.Warn(nameof(TicketTypeController.Edit) + " ticketType is null");

                return NotFound();
            }
            
            return View(ticketType);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, TicketTypeDto ticketTypeDto)
        {
            if (id != ticketTypeDto.Id)
            {
                logger.Warn(nameof(TicketTypeController.Edit) + " id is not equal to ticketTypeDto.Id");

                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ticketTypeService.Update(ticketTypeDto);
                }
                catch (Exception exception)
                {
                    logger.Error(exception);

                    return BadRequest();
                }
            }
            
            return RedirectToAction(nameof(Index));
        }
        
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                logger.Warn(nameof(TicketTypeController.Delete) + " id is null");

                return NotFound();
            }

            TicketType ticketType;

            try
            {
                ticketType = ticketTypeService.GetTicketTypeById(id.Value);
            }
            catch (Exception exception)
            {
                logger.Error(exception);

                return BadRequest();
            }            
            
            if (ticketType == null)
            {
                logger.Warn(nameof(TicketTypeController.Delete) + " ticketType is null");

                return NotFound();
            }

            return View(ticketType);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                ticketTypeService.Delete(id);
            }
            catch (Exception exception)
            {
                logger.Error(exception);

                return BadRequest();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}