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
        public async Task<IActionResult> Index()
        {
            var eTicketDataContext = _context.Tickets.Include(t => t.TicketType).Include(t => t.TransactionHistory).Include(t => t.User);
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
