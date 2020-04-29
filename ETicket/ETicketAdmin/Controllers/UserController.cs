using System;
using ETicket.DataAccess.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services.Interfaces;
using log4net;
using System.Reflection;

namespace ETicket.Admin.Controllers
{
    [Authorize(Roles = "Admin, SuperUser")]
    public class UserController : Controller
    {
        private readonly IUserService service;
        private readonly IPrivilegeService PService;
        private readonly IDocumentService DService;
        private readonly IDocumentTypesService DTService;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public UserController(IPrivilegeService PService, IUserService service, IDocumentTypesService DTService, IDocumentService DService)
        {
            this.service = service;
            this.PService = PService;
            this.DService = DService;
            this.DTService = DTService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                ViewData["PrivilegeId"] = new SelectList(PService.GetPrivileges(), "Id", "Name");
                return View(service.GetUsers());
            }
            catch (Exception e)
            {
                log.Error(e);
                throw;
            }
        }

        [HttpGet]
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                log.Warn("User was not found.");
                return NotFound();
            }

            try
            {
                var user = service.GetUserById(id.Value);

                if (user == null)
                {
                    log.Warn("User was not found.");
                    return NotFound();
                }
                else
                {
                    return View(user);
                }
            }
            catch (Exception e)
            {
                log.Error(e);
                throw;
            }
        }

        [HttpGet]
        public IActionResult CreateUserWithDocument(UserDto userDto)
        {
            try
            {
                ViewData["DocumentTypeId"] = new SelectList(DTService.GetDocumentTypes(), "Id", "Name");

                return View();
            }
            catch (Exception e)
            {
                log.Error(e);
                throw;
            }  
        }

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
                catch (Exception e)
                {
                    log.Error(e);
                    throw;
                }
            }

            ViewData["DocumentTypeId"] = new SelectList(DTService.GetDocumentTypes(), "Id", "Name", documentDto.DocumentTypeId);

            return View(documentDto);
        }

        [HttpGet]
        public IActionResult Create()
        {

            ViewData["DocumentId"] = new SelectList(DService.GetDocuments(), "Id", "Number");
            ViewData["PrivilegeId"] = new SelectList(PService.GetPrivileges(), "Id", "Name");

            return View();
        }

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
                catch (Exception e)
                {
                    log.Error(e);
                    throw;
                }
            }

            ViewData["DocumentId"] = new SelectList(DService.GetDocuments(), "Id", "Number", userDto.DocumentId);
            ViewData["PrivilegeId"] = new SelectList(PService.GetPrivileges(), "Id", "Name", userDto.PrivilegeId);

            return View(userDto);
        }

        [HttpGet]
        public IActionResult SendMessage(Guid? id)
        {
            if (id == null)
            {
                log.Warn("User was not found.");
                return NotFound();
            }
            else
            {
                return View();
            }
        }

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
                catch (Exception e)
                {
                    log.Error(e);
                    throw;
                }
            }

            return View(message);
        }

        [HttpGet]
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                log.Warn("User was not found.");
                return NotFound();
            }

            try
            {
                var user = service.GetUserById(id.Value);

                if (user == null)
                {
                    log.Warn("User was not found.");
                    return NotFound();
                }
                else
                {
                    ViewData["DocumentId"] = new SelectList(DService.GetDocuments(), "Id", "Number", user.DocumentId);
                    ViewData["PrivilegeId"] = new SelectList(PService.GetPrivileges(), "Id", "Name", user.PrivilegeId);

                    return View(user);
                }
            }
            catch (Exception e)
            {
                log.Error(e);
                throw;
            }
        }

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
                catch (Exception e)
                {
                    log.Error(e);
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["DocumentId"] = new SelectList(DService.GetDocuments(), "Id", "Number", userDto.DocumentId);
            ViewData["PrivilegeId"] = new SelectList(PService.GetPrivileges(), "Id", "Name", userDto.PrivilegeId);

            return View(userDto);
        }

        [HttpGet]
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                log.Warn("User was not found.");
                return NotFound();
            }

            try
            {
                var user = service.GetUserById(id.Value);

                if (user == null)
                {
                    log.Warn("User was not found.");
                    return NotFound();
                }
                else
                {
                    return View(user);
                }
            }
            catch (Exception e)
            {
                log.Error(e);
                throw;
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            try
            {
                service.Delete(id);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                log.Error(e);
                throw;
            }
        }
    }
}