using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ETicket.Admin.Controllers
{
    [Authorize(Roles = "Admin, SuperUser")]
    public class TicketTypeController : Controller
    {
        private readonly ITicketTypeService ticketTypeService;

        public TicketTypeController(ITicketTypeService ticketTypeService)
        {
            this.ticketTypeService = ticketTypeService;
        }
        
        public IActionResult Index()
        {
            return View(ticketTypeService.GetAll());
        }
        
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketType = ticketTypeService.Get(id.Value);
            
            if (ticketType == null)
            {
                return NotFound();
            }

            return View(ticketType);
        }
        
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

            ticketTypeService.Create(ticketTypeDto);

            return RedirectToAction(nameof(Index));
        }
        
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketType = ticketTypeService.Get(id.Value);
            
            if (ticketType == null)
            {
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
                return NotFound();
            }

            if (!ModelState.IsValid) return View(ticketTypeDto);
            try
            {
                ticketTypeService.Update(ticketTypeDto);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ticketTypeService.Exists(ticketTypeDto.Id))
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
        
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketType = ticketTypeService.Get(id.Value);
            
            if (ticketType == null)
            {
                return NotFound();
            }

            return View(ticketType);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            ticketTypeService.Delete(id);
            
            return RedirectToAction(nameof(Index));
        }
    }
}