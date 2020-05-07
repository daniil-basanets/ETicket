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
using ETicket.Admin.Services;
using ETicket.DataAccess.Domain;
using ETicket.DataAccess.Domain.Entities;
using ETicketAdmin.DTOs;


namespace ETicket.Admin.Controllers
{
    [Authorize(Roles = "Admin, SuperUser")]
    public class UserController : Controller
    {
        private readonly ETicketDataContext context;
        private readonly UnitOfWork repository;
        private readonly IMapper mapper;

        public UserController(ETicketDataContext context, IMapper mapper)
        {
            this.context = context;
            repository = new UnitOfWork(context);
            this.mapper = mapper;
        }

        // GET: User
        public IActionResult Index(string sortOrder)
        {
            ViewData["PrivilegeId"] = new SelectList(repository.Privileges.GetAll(), "Id", "Name");

            return View();
        }
        
        private JsonResult GetCurrentPage(
            IQueryable<User> users,
            int drawStep,
            int countRecords,
            int countFiltered
        )
        {
            return Json(new
            {
                draw = ++drawStep,
                recordsTotal = countRecords,
                recordsFiltered = countFiltered,
                data = users
            });
        }

        // GET: User/Details/5
        public IActionResult Details(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = repository.Users.Get(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: User/Create
        public IActionResult Create()
        {

            ViewData["DocumentId"] = new SelectList(repository.Documents.GetAll(), "Id", "Number");
            ViewData["PrivilegeId"] = new SelectList(repository.Privileges.GetAll(), "Id", "Name");

            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(UserDto userDto)
        {
            if (ModelState.IsValid)
            {
                var user = mapper.Map<User>(userDto);

                user.Id = Guid.NewGuid();
                repository.Users.Create(user);
                repository.Save();

                return RedirectToAction(nameof(Index));
            }

            ViewData["DocumentId"] = new SelectList(context.Documents, "Id", "Number", userDto.DocumentId);
            ViewData["PrivilegeId"] = new SelectList(context.Privileges, "Id", "Name", userDto.PrivilegeId);

            return View(userDto);
        }

        // GET: User/SendMessage/5
        public IActionResult SendMessage(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = repository.Users.Get(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: User/SendMessage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SendMessage(Guid id, string message)
        {
            if (ModelState.IsValid)
            {
                var user = repository.Users.Get(id);
                if (user == null)
                {
                    return NotFound();
                }

                MailService emailService = new MailService();
                emailService.SendEmail(user.Email, message);

                return RedirectToAction(nameof(Index));
            }
            return View(message);
        }

        // GET: User/Edit/5
        public IActionResult Edit(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = repository.Users.Get(id);
            if (user == null)
            {
                return NotFound();
            }

            ViewData["DocumentId"] = new SelectList(context.Documents, "Id", "Number", user.DocumentId);
            ViewData["PrivilegeId"] = new SelectList(context.Privileges, "Id", "Name", user.PrivilegeId);

            return View(user);
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, UserDto userDto)
        {
            if (id != userDto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = mapper.Map<User>(userDto);

                    repository.Users.Update(user);
                    repository.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!repository.Users.UserExists(userDto.Id))
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

            ViewData["DocumentId"] = new SelectList(context.Documents, "Id", "Number", userDto.DocumentId);
            ViewData["PrivilegeId"] = new SelectList(context.Privileges, "Id", "Name", userDto.PrivilegeId);

            return View(userDto);
        }

        // GET: User/Delete/5
        public IActionResult Delete(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = repository.Users.Get(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            repository.Users.Delete(id);
            repository.Save();

            return RedirectToAction(nameof(Index));
        }

    }
}
