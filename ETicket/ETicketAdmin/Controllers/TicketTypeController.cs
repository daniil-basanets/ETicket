using System;
using System.Reflection;
using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.DataAccess.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using log4net;

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
                
                logger.Info("Get Ticket types");

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
                logger.Error("Id was not passed");
                
                return NotFound();
            }

            TicketType ticketType;

            try
            {
                ticketType = ticketTypeService.GetTicketTypeById(id.Value);
                
                logger.Info("Get ticket type");
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                
                return BadRequest();
            }

            if (ticketType == null)
            {
                logger.Error($"Not found ticket type with id:{id.Value}");
                
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
                logger.Warn("Model is not valid");
                
                return View(ticketTypeDto);
            }

            try
            {
                ticketTypeService.Create(ticketTypeDto);
                
                logger.Info("New ticket type created");
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
                logger.Error("Id was not passed");
                
                return NotFound();
            }

            TicketType ticketType;

            try
            {
                ticketType = ticketTypeService.GetTicketTypeById(id.Value);
                logger.Info("Get ticket type");
            }
            catch (Exception exception)
            {
                logger.Error(exception);

                return BadRequest();
            }

            if (ticketType == null)
            {
                logger.Error($"Not found ticket type with id:{id.Value}");
                
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
                logger.Error($"Not found ticket type with id:{id}");
                
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ticketTypeService.Update(ticketTypeDto);
                    
                    logger.Info("Ticket type updated");
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
                logger.Warn("Id was not passed");
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
                logger.Error($"Not found ticket type with id:{id}");
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