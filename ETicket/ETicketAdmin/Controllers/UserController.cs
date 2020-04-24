using System;
using ETicket.DataAccess.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.ApplicationServices.Services;

namespace ETicket.Admin.Controllers
{
    [Authorize(Roles = "Admin, SuperUser")]
    public class UserController : Controller
    {
        private readonly IUserService service;
        IUnitOfWork repository;

        public UserController(IUnitOfWork repository, IMailService mailService)
        {
            this.repository = repository;
            service = new UserService(repository, mailService);
        }

        // GET: User
        public IActionResult Index()
        {
            try
            {
                ViewData["PrivilegeId"] = new SelectList(repository.Privileges.GetAll(), "Id", "Name");

                return View(service.GetAll());
            }
            catch (Exception)
            {

                throw;
            }
        }

        // GET: User/Details/5
        public IActionResult Details(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var user = service.GetById(id);

                if (user == null)
                {

                    return NotFound();
                }
                else
                {
                    return View(user);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        // GET: User/CreateUserWithDocument
        public IActionResult CreateUserWithDocument(UserDto userDto)
        {
            ViewData["DocumentTypeId"] = new SelectList(repository.DocumentTypes.GetAll(), "Id", "Name");

            return View();
        }

        // POST: User/CreateUserWithDocument
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateUserWithDocument(DocumentDto documentDto, UserDto userDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    service.CreateUserWithDocument(documentDto, userDto);

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {

                    throw;
                }
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
                try
                {
                    if (userDto.PrivilegeId != null)
                    {
                        return RedirectToAction(nameof(CreateUserWithDocument), userDto);
                    }
                    else
                    {
                        service.CreateUser(userDto);

                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception)
                {

                    throw;
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
            else
            {
                return View();
            }
        }

        // POST: User/SendMessage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SendMessage(Guid id, string message)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    service.SendMessage(id, message);

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {

                    throw;
                }
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

            try
            {
                var user = service.GetById(id);

                if (user == null)
                {

                    return NotFound();
                }
                else
                {
                    ViewData["DocumentId"] = new SelectList(repository.Documents.GetAll(), "Id", "Number", user.DocumentId);
                    ViewData["PrivilegeId"] = new SelectList(repository.Privileges.GetAll(), "Id", "Name", user.PrivilegeId);

                    return View(user);
                }
            }
            catch (Exception)
            {

                throw;
            }
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
                    service.Update(userDto);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!service.Exists(userDto.Id))
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

            try
            {
                var user = service.GetById(id);

                if (user == null)
                {

                    return NotFound();
                }
                else
                {
                    return View(user);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            try
            {
                service.Delete(id);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}