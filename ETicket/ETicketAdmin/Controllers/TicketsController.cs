using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ETicketDataAccess.Domain.Entities;
using ETicketDataAccess.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace ETicketAdmin.Controllers
{
    [Authorize(Roles = "Admin, SuperUser")]
    public class TicketController : Controller
    {
        private readonly IUnitOfWork uow;

        public TicketController(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        // GET: Tickets
        public IActionResult Index(string sortOrder)
        {
            ViewData["TicketTypeId"] = new SelectList(uow.TicketTypes.GetAll(), "Id", "TypeName");
            ViewData["TicketTypeSortParm"] = sortOrder == "ticket_type" ? "ticket_type_desc" : "ticket_type";
            ViewData["CreatedSortParm"] = String.IsNullOrEmpty(sortOrder) ? "created_date" : "";
            ViewData["ActivatedSortParm"] = sortOrder == "activated_date" ? "activated_date_desc" : "activated_date";
            ViewData["ExpirationSortParm"] = sortOrder == "expiration_date" ? "expiration_date_desc" : "expiration_date";
            ViewData["UserSortParm"] = sortOrder == "user" ? "user_desc" : "user";
            ViewData["TransactionSortParm"] = sortOrder == "transaction" ? "transaction_desc" : "transaction";

            IQueryable<Ticket> eTicketDataContext = uow.Tickets.GetAll();

            switch (sortOrder)
            {
                case "created_date":
                    eTicketDataContext = eTicketDataContext.OrderBy(s => s.CreatedUTCDate);
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
                    eTicketDataContext = eTicketDataContext.OrderByDescending(s => s.CreatedUTCDate);
                    break;
            }

            return View(eTicketDataContext.ToList());
        }

        // GET: Tickets/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = uow.Tickets.Get((Guid)id);

            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            ViewData["TicketTypeId"] = new SelectList(uow.TicketTypes.GetAll(), "Id", "TypeName");
            ViewData["TransactionHistoryId"] = new SelectList(uow.TransactionHistory.GetAll(), "Id", "ReferenceNumber");
            ViewData["UserId"] = new SelectList(uow.Users.GetAll(), "Id", "FirstName");

            return View();
        }

        // POST: Tickets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,TicketTypeId,CreatedUTCDate,ActivatedUTCDate," +
            "ExpirationUTCDate,UserId,TransactionHistoryId")] Ticket ticket)
        {
            ticket.CreatedUTCDate = DateTime.UtcNow;
            ticket.TicketType = uow.TicketTypes.Get(ticket.TicketTypeId);

            if (ticket.TicketType.IsPersonal && ticket.UserId == null)
            {
                ModelState.AddModelError("", "User is not specified for personal ticket type");
                ViewData["TicketTypeId"] = new SelectList(uow.TicketTypes.GetAll(), "Id", "TypeName");
                ViewData["TransactionHistoryId"] = new SelectList(uow.TransactionHistory.GetAll(), "Id", "ReferenceNumber");
                ViewData["UserId"] = new SelectList(uow.Users.GetAll(), "Id", "FirstName");

                return View(ticket);
            }

            if (ModelState.IsValid)
            {
                ticket.Id = Guid.NewGuid();
                if (ticket.ActivatedUTCDate != null)
                {
                    ticket.ExpirationUTCDate = ticket.ActivatedUTCDate?.AddHours(ticket.TicketType.DurationHours);
                }
                uow.Tickets.Create(ticket);
                uow.Save();

                return RedirectToAction(nameof(Index));
            }

            ViewData["TicketTypeId"] = new SelectList(uow.TicketTypes.GetAll(), "Id", "TypeName", ticket.TicketTypeId);
            ViewData["TransactionHistoryId"] = new SelectList(uow.TransactionHistory.GetAll(), "Id", "ReferenceNumber", ticket.TransactionHistoryId);
            ViewData["UserId"] = new SelectList(uow.Users.GetAll(), "Id", "FirstName", ticket.UserId);

            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = uow.Tickets.Get((Guid)id);

            if (ticket == null)
            {
                return NotFound();
            }

            ViewData["TicketTypeId"] = new SelectList(uow.TicketTypes.GetAll(), "Id", "TypeName", ticket.TicketTypeId);
            ViewData["TransactionHistoryId"] = new SelectList(uow.TransactionHistory.GetAll(), "Id", "ReferenceNumber", ticket.TransactionHistoryId);
            ViewData["UserId"] = new SelectList(uow.Users.GetAll(), "Id", "FirstName", ticket.UserId);

            return View(ticket);
        }

        // POST: Tickets/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Id,TicketTypeId,CreatedUTCDate,ActivatedUTCDate,ExpirationUTCDate,UserId,TransactionHistoryId")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    uow.Tickets.Update(ticket);
                    uow.Save();
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

            ViewData["TicketTypeId"] = new SelectList(uow.TicketTypes.GetAll(), "Id", "TypeName", ticket.TicketTypeId);
            ViewData["TransactionHistoryId"] = new SelectList(uow.TransactionHistory.GetAll(), "Id", "ReferenceNumber", ticket.TransactionHistoryId);
            ViewData["UserId"] = new SelectList(uow.Users.GetAll(), "Id", "FirstName", ticket.UserId);

            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = uow.Tickets.Get((Guid)id);

            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            uow.Tickets.Delete(id);
            uow.Save();

            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(Guid id)
        {
            return uow.Tickets.Get(id) != null;
        }
    }
}
