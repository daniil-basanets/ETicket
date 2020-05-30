using System;
using System.Reflection;
using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services.Interfaces;
using log4net;
using ETicket.DataAccess.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            logger.Info(nameof(TicketController.Index));
            
            try
            {
                var ticketTypes = ticketTypeService.GetTicketTypes();
                
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
            logger.Info(nameof(TicketController.Index));
            
            if (id == null)
            {
                logger.Warn(nameof(TicketTypeController.Details) + " id is null");

                return NotFound();
            }

            try
            {
                var ticketTypeDto = ticketTypeService.GetTicketTypeById(id.Value);  
                
                if (ticketTypeDto == null)
                {
                    logger.Warn(nameof(TicketTypeController.Details) + " ticketType is null");
                
                    return NotFound();
                }
                
                return View(ticketTypeDto);
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            logger.Info(nameof(TicketTypeController.Create));
            
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TicketTypeDto ticketTypeDto)
        {
            logger.Info(nameof(TicketTypeController.Create));
            
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
            logger.Info(nameof(TicketTypeController.Edit));
            
            return Details(id);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, TicketTypeDto ticketTypeDto)
        {
            logger.Info(nameof(TicketTypeController.Edit));
            
            if (id != ticketTypeDto.Id)
            {
                logger.Warn(nameof(TicketTypeController.Edit) + " id is not equal to ticketTypeDto.Id");

                return NotFound();
            }
            
            if (!ModelState.IsValid)
            {                
                return View(ticketTypeDto);
            }
            
            try
            {
                ticketTypeService.Update(ticketTypeDto);                
            }
            catch (Exception exception)
            {
                logger.Error(exception);

                return BadRequest();
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
            logger.Info(nameof(TicketTypeController.DeleteConfirmed));
            
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