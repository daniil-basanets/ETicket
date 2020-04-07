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
    public class PrivilegesController : Controller
    {
        #region

        private readonly ETicketDataContext _context;

        #endregion

        public PrivilegesController(ETicketDataContext context)
        {
            _context = context;
        }

        // GET: Privileges
        public async Task<IActionResult> Index()
        {
            return View(await _context.Privileges.ToListAsync());
        }

        // GET: Privileges/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var privilege = await _context.Privileges
                .FirstOrDefaultAsync(m => m.Id == id);
            if (privilege == null)
            {
                return NotFound();
            }

            return View(privilege);
        }

        // GET: Privileges/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Privileges/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Coefficient")] Privilege privilege)
        {
            if (ModelState.IsValid)
            {
                _context.Add(privilege);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(privilege);
        }

        // GET: Privileges/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var privilege = await _context.Privileges.FindAsync(id);
            if (privilege == null)
            {
                return NotFound();
            }
            return View(privilege);
        }

        // POST: Privileges/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Coefficient")] Privilege privilege)
        {
            if (id != privilege.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(privilege);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrivilegeExists(privilege.Id))
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
            return View(privilege);
        }

        // GET: Privileges/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var privilege = await _context.Privileges
                .FirstOrDefaultAsync(m => m.Id == id);
            if (privilege == null)
            {
                return NotFound();
            }

            return View(privilege);
        }

        // POST: Privileges/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var privilege = await _context.Privileges.FindAsync(id);
            _context.Privileges.Remove(privilege);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PrivilegeExists(int id)
        {
            return _context.Privileges.Any(e => e.Id == id);
        }
    }
}
