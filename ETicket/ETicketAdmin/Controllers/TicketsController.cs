using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DBContextLibrary.Domain;
using DBContextLibrary.Domain.Entities;

namespace ETicketAdmin.Controllers
{
    public class TicketController : Controller
    {
        private readonly ETicketDataContext _context;

        public TicketController(ETicketDataContext context)
        {
            _context = context;
        }

        // GET: Tickets
        public async Task<IActionResult> Index(string sortOrder)
        {
            ViewData["TicketTypeSortParm"] = sortOrder == "ticket_type" ? "ticket_type_desc" : "ticket_type";
            ViewData["CreatedSortParm"] = String.IsNullOrEmpty(sortOrder) ? "created_date_desc" : "";
            ViewData["ActivatedSortParm"] = sortOrder == "activated_date" ? "activated_date_desc" : "activated_date";
            ViewData["ExpirationSortParm"] = sortOrder == "expiration_date" ? "expiration_date_desc" : "expiration_date";
            ViewData["UserSortParm"] = sortOrder == "user" ? "user_desc" : "user";
            ViewData["TransactionSortParm"] = sortOrder == "transaction" ? "transaction_desc" : "transaction";

            var eTicketDataContext = from s in _context.Tickets.Include(t => t.TicketType)
                .Include(t => t.TransactionHistory)
                .Include(t => t.User)
                          select s;
            switch (sortOrder)
            {
                case "created_date_desc":
                    eTicketDataContext = eTicketDataContext.OrderByDescending(s => s.CreatedUTCDate);
                    break;
                case "ticket_type":
                    eTicketDataContext = eTicketDataContext.OrderBy(s => s.TicketType);
                    break;
                case "ticket_type_desc":
                    eTicketDataContext = eTicketDataContext.OrderByDescending(s => s.TicketType);
                    break;
                case "activated_date":
                    eTicketDataContext = eTicketDataContext.OrderBy(s => s.ActivatedUTCDate);
                    break;
                case "activated_date_desc":
                    eTicketDataContext = eTicketDataContext.OrderByDescending(s => s.ActivatedUTCDate);
                    break;
                case "expiration_date":
                    eTicketDataContext = eTicketDataContext.OrderBy(s => s.ExpirationUTCDate);
                    break;
                case "expiration_date_desc":
                    eTicketDataContext = eTicketDataContext.OrderByDescending(s => s.ExpirationUTCDate);
                    break;
                case "user":
                    eTicketDataContext = eTicketDataContext.OrderBy(s => s.UserId);
                    break;
                case "user_desc":
                    eTicketDataContext = eTicketDataContext.OrderByDescending(s => s.User);
                    break;
                case "transaction":
                    eTicketDataContext = eTicketDataContext.OrderBy(s => s.TransactionHistoryId);
                    break;
                case "transaction_desc":
                    eTicketDataContext = eTicketDataContext.OrderByDescending(s => s.TransactionHistoryId);
                    break;
                default:
                    eTicketDataContext = eTicketDataContext.OrderBy(s => s.CreatedUTCDate);
                    break;
            }

            return View(await eTicketDataContext.ToListAsync());
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.TicketType)
                .Include(t => t.TransactionHistory)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "TypeName");
            ViewData["TransactionHistoryId"] = new SelectList(_context.TransactionHistory, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FirstName");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TicketTypeId,CreatedUTCDate,ActivatedUTCDate,ExpirationUTCDate,UserId,TransactionHistoryId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                ticket.Id = Guid.NewGuid();
                _context.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "TypeName", ticket.TicketTypeId);
            ViewData["TransactionHistoryId"] = new SelectList(_context.TransactionHistory, "Id", "Id", ticket.TransactionHistoryId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FirstName", ticket.UserId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "TypeName", ticket.TicketTypeId);
            ViewData["TransactionHistoryId"] = new SelectList(_context.TransactionHistory, "Id", "Id", ticket.TransactionHistoryId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FirstName", ticket.UserId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,TicketTypeId,CreatedUTCDate,ActivatedUTCDate,ExpirationUTCDate,UserId,TransactionHistoryId")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.Id))
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
            ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "TypeName", ticket.TicketTypeId);
            ViewData["TransactionHistoryId"] = new SelectList(_context.TransactionHistory, "Id", "Id", ticket.TransactionHistoryId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FirstName", ticket.UserId);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.TicketType)
                .Include(t => t.TransactionHistory)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(Guid id)
        {
            return _context.Tickets.Any(e => e.Id == id);
        }
    }
}
