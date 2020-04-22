using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using AutoMapper;
using ETicket.Admin.Extensions;
using ETicket.Admin.Models.DataTables;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;
using ETicketAdmin.DTOs;
using ETicket.Admin.Services;
using System.Linq.Expressions;

namespace ETicket.Admin.Controllers
{
    [Authorize(Roles = "Admin, SuperUser")]
    public class TicketController : Controller
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;
        private readonly DataTableServices dataTableServices; 

        public TicketController(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
            dataTableServices = new DataTableServices();
        }

        // GET: Tickets
        public IActionResult Index(string sortOrder)
        {
            ViewData["TicketTypeId"] = new SelectList(uow.TicketTypes.GetAll(), "Id", "TypeName");

            return View();
        }


        [HttpPost]
        public IActionResult GetCurrentPage(DataTableParameters dataTableParameters)
        {
            var drawStep = dataTableParameters.Draw;
            var countRecords = dataTableParameters.TotalEntries;

            IQueryable<Ticket> tickets = uow
                    .Tickets
                    .GetAll()
                    .AsNoTracking()
                    .Include(t => t.TicketType)
                    .Include(t => t.User);

            //For single count query
            if (countRecords == -1)
            {
                countRecords = tickets.Count();
            }

            var sortableExpression = new List<Expression<Func<Ticket, string>>> 
            {
                (t => t.TicketType.TypeName),
                (t => t.CreatedUTCDate.ToString()),
                (t => t.ActivatedUTCDate.ToString()),
                (t => t.ExpirationUTCDate.ToString()),
                (t => t.User.LastName)
            };

            tickets = dataTableServices.GetSortedQuery<Ticket>(tickets, dataTableParameters.Order, sortableExpression);

            var countFiltered = countRecords;
            var searchString = dataTableParameters.Search.Value;

            Expression<Func<Ticket, bool>> searchableExpression = (t => t.TicketType.TypeName.StartsWith(dataTableParameters.Search.Value)
                            || t.CreatedUTCDate.ToString().Contains(searchString)
                            || t.ActivatedUTCDate.ToString().Contains(searchString)
                            || t.ExpirationUTCDate.ToString().Contains(searchString)
                            || t.User.FirstName.StartsWith(searchString)
                            || t.User.LastName.StartsWith(searchString));

            if (!string.IsNullOrEmpty(dataTableParameters.Search.Value))
            {
                tickets = dataTableServices.GetSearchedQuery<Ticket>(tickets, searchableExpression);
                countFiltered = tickets.Count();
            }

            tickets = tickets
                    .Skip((dataTableParameters.PageNumber - 1) * dataTableParameters.Length)
                    .Take(dataTableParameters.Length);

            return Json(dataTableServices.GetJsonDataTable<Ticket>(tickets, drawStep, countRecords, countFiltered));
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
        public IActionResult Create(TicketDto ticketDto)
        {
            var ticket = mapper.Map<Ticket>(ticketDto);

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
        public IActionResult Edit(Guid id, TicketDto ticketDto)
        {
            if (id != ticketDto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var ticket = mapper.Map<Ticket>(ticketDto);

                    uow.Tickets.Update(ticket);
                    uow.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticketDto.Id))
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

            ViewData["TicketTypeId"] = new SelectList(uow.TicketTypes.GetAll(), "Id", "TypeName", ticketDto.TicketTypeId);
            ViewData["TransactionHistoryId"] = new SelectList(uow.TransactionHistory.GetAll(), "Id", "ReferenceNumber", ticketDto.TransactionHistoryId);
            ViewData["UserId"] = new SelectList(uow.Users.GetAll(), "Id", "FirstName", ticketDto.UserId);

            return View(ticketDto);
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
