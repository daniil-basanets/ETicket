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
    public class TransactionHistoryController : Controller
    {
        private readonly ETicketDataContext _context;

        public TransactionHistoryController(ETicketDataContext context)
        {
            _context = context;
        }

        // GET: TransactionHistories
        public async Task<IActionResult> Index(string sortOrder)
        {
            if (string.IsNullOrEmpty(sortOrder))
                sortOrder = "DateDesc";

            ViewBag.DateSortParm = sortOrder == "DateDesc"
                ? "DateAsc"
                : "DateDesc";

            IQueryable<TransactionHistory> eTicketDataContext = _context
                .TransactionHistory
                .Include(t => t.TicketType);

            switch (sortOrder)
            {
                case "DateAsc":
                    eTicketDataContext = eTicketDataContext
                        .OrderBy(x => x.Date);
                    break;

                case "DateDesc":
                    eTicketDataContext = eTicketDataContext
                        .OrderByDescending(x => x.Date);
                    break;
            }

            return View(await eTicketDataContext.ToListAsync());
        }

        // GET: TransactionHistories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transactionHistory = await _context.TransactionHistory
                .Include(t => t.TicketType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transactionHistory == null)
            {
                return NotFound();
            }

            return View(transactionHistory);
        }

        // GET: TransactionHistories/Create
        public IActionResult Create()
        {
            ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "TypeName");
            return View();
        }

        // POST: TransactionHistories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TotalPrice,Date,TicketTypeId,Count")] TransactionHistory transactionHistory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(transactionHistory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "TypeName", transactionHistory.TicketTypeId);
            return View(transactionHistory);
        }

        // GET: TransactionHistories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transactionHistory = await _context.TransactionHistory.FindAsync(id);
            if (transactionHistory == null)
            {
                return NotFound();
            }
            ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "TypeName", transactionHistory.TicketTypeId);
            return View(transactionHistory);
        }

        // POST: TransactionHistories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,TotalPrice,Date,TicketTypeId,Count")] TransactionHistory transactionHistory)
        {
            if (id != transactionHistory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transactionHistory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionHistoryExists(transactionHistory.Id))
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
            ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "TypeName", transactionHistory.TicketTypeId);
            return View(transactionHistory);
        }

        // GET: TransactionHistories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transactionHistory = await _context.TransactionHistory
                .Include(t => t.TicketType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transactionHistory == null)
            {
                return NotFound();
            }

            return View(transactionHistory);
        }

        // POST: TransactionHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transactionHistory = await _context.TransactionHistory.FindAsync(id);
            _context.TransactionHistory.Remove(transactionHistory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionHistoryExists(int id)
        {
            return _context.TransactionHistory.Any(e => e.Id == id);
        }
    }
}
