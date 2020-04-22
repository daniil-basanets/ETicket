using System;
using System.Linq;
using AutoMapper;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;
using ETicketAdmin.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ETicket.Admin.Services;

namespace ETicket.Admin.Controllers
{
    [Authorize(Roles = "Admin, SuperUser")]
    public class UserController : Controller
    {
        private readonly IUnitOfWork repository;
        private readonly IMapper mapper;

        public UserController(IUnitOfWork repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        // GET: User
        public IActionResult Index()
        {
            ViewData["PrivilegeId"] = new SelectList(repository.Privileges.GetAll(), "Id", "Name");
            var users = repository.Users.GetAll();

            return View(users.ToList());
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

        // GET: User/CreateDocument
        public IActionResult CreateUserWithDocument(User user)
        {
            ViewData["DocumentTypeId"] = new SelectList(repository.DocumentTypes.GetAll(), "Id", "Name");

            return View();
        }

        // POST: User/CreateDocument
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateUserWithDocument(DocumentDto documentDto, User user)
        {
            if (ModelState.IsValid)
            {
                var document = mapper.Map<Document>(documentDto);

                document.Id = Guid.NewGuid();
                repository.Documents.Create(document);
                repository.Save();

                user.DocumentId = document.Id;
                repository.Users.Create(user);
                repository.Save();

                return RedirectToAction(nameof(Index));
            }

            ViewData["DocumentTypeId"] = new SelectList(repository.DocumentTypes.GetAll(), "Id", "Name", documentDto.DocumentTypeId);

            return View(documentDto);
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

                if (user.PrivilegeId != null)
                {
                    return RedirectToAction(nameof(CreateUserWithDocument), user);
                }
                else
                {
                    user.Id = Guid.NewGuid();
                    repository.Users.Create(user);
                    repository.Save();

                    return RedirectToAction(nameof(Index));
                }
            }

            ViewData["DocumentId"] = new SelectList(repository.Documents.GetAll(), "Id", "Number", userDto.DocumentId);
            ViewData["PrivilegeId"] = new SelectList(repository.Privileges.GetAll(), "Id", "Name", userDto.PrivilegeId);

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

            ViewData["DocumentId"] = new SelectList(repository.Documents.GetAll(), "Id", "Number", user.DocumentId);
            ViewData["PrivilegeId"] = new SelectList(repository.Privileges.GetAll(), "Id", "Name", user.PrivilegeId);

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

            ViewData["DocumentId"] = new SelectList(repository.Documents.GetAll(), "Id", "Number", userDto.DocumentId);
            ViewData["PrivilegeId"] = new SelectList(repository.Privileges.GetAll(), "Id", "Name", userDto.PrivilegeId);

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