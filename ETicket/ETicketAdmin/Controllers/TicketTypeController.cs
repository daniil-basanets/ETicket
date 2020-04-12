using System.Linq;
using System.Threading.Tasks;
using DBContextLibrary.Domain;
using DBContextLibrary.Domain.Entities;
using ETicketAdmin.Common;
using ETicketAdmin.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace ETicketAdmin.Controllers
{
    [Authorize]
    public class TicketTypeController : Controller
    {
        private readonly ETicketDataContext context;

        public TicketTypeController(ETicketDataContext context)
        {
            this.context = context;
        }
        
        public async Task<IActionResult> Index(string sortBy, string sortDirection, string searchString, int pageNumber = 1)
        {
            IQueryable<TicketType> eTicketDataContext = context.TicketTypes;
            
            if (!(string.IsNullOrEmpty(sortBy) || string.IsNullOrEmpty(sortDirection)))
            {
                sortBy = sortBy.ToLower();
                sortDirection = sortDirection.ToLower();
            }
            
            if (!string.IsNullOrEmpty(searchString))
            {
                eTicketDataContext = eTicketDataContext.Where(t => t.TypeName.Contains(searchString)
                || t.Price.ToString().Contains(searchString) || t.DurationHours.ToString().Contains(searchString));
            }

            ViewBag.SortDirection = sortDirection == "desc" ? "asc" : "desc";

            switch (sortBy)
            {
                case "price":
                    eTicketDataContext = eTicketDataContext.ApplySortBy(t => t.Price, sortDirection);
                    break;
                
                case "ispersonal":
                    eTicketDataContext = eTicketDataContext.ApplySortBy(t => t.IsPersonal, sortDirection);
                    break;
                
                case "durationhours":
                    eTicketDataContext = eTicketDataContext.ApplySortBy(t => t.DurationHours, sortDirection);
                    break;
                default:
                    eTicketDataContext = eTicketDataContext.OrderBy(t => t.Id);
                    break;
            }
            
            var pageSize = CommonSettings.DefaultPageSize;
            
            return View(await PaginatedList<TicketType>.CreateAsync(eTicketDataContext, pageNumber, pageSize));
        }
        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketType = await context.TicketTypes.FirstOrDefaultAsync(m => m.Id == id);
            
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
        public async Task<IActionResult> Create([Bind("Id,TypeName,DurationHours,IsPersonal,Price")] TicketType ticketType)
        {
            if (!ModelState.IsValid) return View(ticketType);
            context.Add(ticketType);
            await context.SaveChangesAsync();
                
            return RedirectToAction(nameof(Index));

        }
        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketType = await context.TicketTypes.FindAsync(id);
            
            if (ticketType == null)
            {
                return NotFound();
            }
            
            return View(ticketType);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TypeName,DurationHours,IsPersonal,Price")] TicketType ticketType)
        {
            if (id != ticketType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    context.Update(ticketType);
                    await context.SaveChangesAsync();
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
            
            return View(ticketType);
        }
        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketType = await context.TicketTypes.FirstOrDefaultAsync(m => m.Id == id);
            
            if (ticketType == null)
            {
                return NotFound();
            }

            return View(ticketType);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticketType = await context.TicketTypes.FindAsync(id);
            
            context.TicketTypes.Remove(ticketType);
            await context.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }
        
        
        private bool TicketTypeExists(int id)
        {
            return context.TicketTypes.Any(e => e.Id == id);
        }
    }
}