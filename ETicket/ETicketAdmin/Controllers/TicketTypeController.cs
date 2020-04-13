using DBContextLibrary.Domain.Entities;
using DBContextLibrary.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ETicketAdmin.Controllers
{
    [Authorize(Roles = "Admin, SuperUser")]
    public class TicketTypeController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public TicketTypeController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
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
        public IActionResult Create([Bind("Id,TypeName,DurationHours,IsPersonal,Price")] TicketType ticketType)
        {
            if (!ModelState.IsValid)
            {
                return View(ticketType);
            }
                
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
        public IActionResult Edit(int id, [Bind("Id,TypeName,DurationHours,IsPersonal,Price")] TicketType ticketType)
        {
            if (id != ticketType.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid) return View(ticketType);
            try
            {
                unitOfWork.TicketTypes.Update(ticketType);
                unitOfWork.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketTypeExists(ticketType.Id))
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