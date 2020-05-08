using System;
using System.Linq;
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
        #region Private members

        private readonly IUserService userService;
        private readonly IPrivilegeService privilegeService;
        private readonly IDocumentService documentService;
        private readonly IDocumentTypesService documentTypeService;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        public UserController(IPrivilegeService PService, IUserService UService, IDocumentTypesService DTService, IDocumentService DService)
        {
            userService = UService;
            privilegeService = PService;
            documentService = DService;
            documentTypeService = DTService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                ViewData["PrivilegeId"] = new SelectList(privilegeService.GetPrivileges(), "Id", "Name");

                return View(userService.GetUsers());
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                log.Warn(nameof(UserController.Details) + " id is null");

                return NotFound();
            }

            try
            {
                var user = userService.GetUserById(id.Value);

                if (user == null)
                {
                    log.Warn(nameof(UserController.Details) + " user is null");

                    return NotFound();
                }

                return View(user);
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult CreateUserWithDocument()
        {
            try
            {
                ViewData["DocumentTypeId"] = new SelectList(documentTypeService.GetDocumentTypes(), "Id", "Name");

                return View();
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateUserWithDocument(DocumentDto documentDto, UserDto userDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    userService.CreateUserWithDocument(documentDto, userDto);

                    return RedirectToAction(nameof(Index));
                }

                ViewData["DocumentTypeId"] = new SelectList(documentTypeService.GetDocumentTypes(), "Id", "Name", documentDto.DocumentTypeId);

                return View(documentDto);
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            try
            {
                ViewData["DocumentId"] = new SelectList(documentService.GetDocuments(), "Id", "Number");
                ViewData["PrivilegeId"] = new SelectList(privilegeService.GetPrivileges(), "Id", "Name");

                return View();
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(UserDto userDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (userDto.PrivilegeId != null)
                    {
                        return RedirectToAction(nameof(CreateUserWithDocument), userDto);
                    }
                    else
                    {
                        userService.CreateUser(userDto);

                        return RedirectToAction(nameof(Index));
                    }
                }

                ViewData["DocumentId"] = new SelectList(documentService.GetDocuments(), "Id", "Number", userDto.DocumentId);
                ViewData["PrivilegeId"] = new SelectList(privilegeService.GetPrivileges(), "Id", "Name", userDto.PrivilegeId);

                return View(userDto);
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult SendMessage(Guid? id)
        {
            if (id == null)
            {
                log.Warn(nameof(UserController.Create) + " id is null");

                return NotFound();
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SendMessage(Guid id, string message)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    userService.SendMessage(id, message);

                    return RedirectToAction(nameof(Index));
                }

                return View(message);
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                log.Warn(nameof(UserController.Edit) + " id is null");

                return NotFound();
            }

            try
            {
                var user = userService.GetUserById(id.Value);

                if (user == null)
                {
                    log.Warn(nameof(UserController.Edit) + " user is null");

                    return NotFound();
                }
                else
                {
                    ViewData["DocumentId"] = new SelectList(documentService.GetDocuments(), "Id", "Number", user.DocumentId);
                    ViewData["PrivilegeId"] = new SelectList(privilegeService.GetPrivileges(), "Id", "Name", user.PrivilegeId);

                    return View(user);
                }
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, UserDto userDto)
        {
            if (id != userDto.Id)
            {
                log.Warn(nameof(UserController.Edit) + " id is not equal to userDto.Id");

                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    userService.Update(userDto);

                    return RedirectToAction(nameof(Index));
                }

                ViewData["DocumentId"] = new SelectList(documentService.GetDocuments(), "Id", "Number", userDto.DocumentId);
                ViewData["PrivilegeId"] = new SelectList(privilegeService.GetPrivileges(), "Id", "Name", userDto.PrivilegeId);

                return View(userDto);
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                log.Warn(nameof(UserController.Edit) + " id is null");

                return NotFound();
            }

            try
            {
                var user = userService.GetUserById(id.Value);

                if (user == null)
                {
                    log.Warn(nameof(UserController.Edit) + " user is null");

                    return NotFound();
                }

                return View(user);
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            try
            {
                userService.Delete(id);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }
    }
}