using AutoMapper;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;
using ETicketAdmin.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ETicket.Admin.Controllers
{
    [Authorize(Roles = "Admin, SuperUser")]
    public class TicketTypeController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public TicketTypeController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        
        public IActionResult Index()
        {
            return View(unitOfWork.TicketTypes.GetAll());
        }
        
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketType = unitOfWork.TicketTypes.Get((int)id);
            
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

            var ticketType = mapper.Map<TicketType>(ticketTypeDto);

            unitOfWork.TicketTypes.Create(ticketType);
            unitOfWork.Save();
            
            return RedirectToAction(nameof(Index));
        }
        
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketType = unitOfWork.TicketTypes.Get((int)id);
            
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
                var ticketType = mapper.Map<TicketType>(ticketTypeDto);

                unitOfWork.TicketTypes.Update(ticketType);
                unitOfWork.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketTypeExists(ticketTypeDto.Id))
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

            var ticketType = unitOfWork.TicketTypes.Get((int)id);
            
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
            unitOfWork.TicketTypes.Delete(id);
            unitOfWork.Save();
            
            return RedirectToAction(nameof(Index));
        }
        
        
        private bool TicketTypeExists(int id)
        {
            return unitOfWork.TicketTypes.Get(id) != null;
        }
    }
}