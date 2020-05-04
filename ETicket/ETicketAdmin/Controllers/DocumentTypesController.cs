using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services.Interfaces;
using log4net;
using System.Reflection;
using System;

namespace ETicket.Admin.Controllers
{
    [Authorize(Roles = "Admin, SuperUser")]
    public class DocumentTypesController : Controller
    {
        private readonly IDocumentTypesService service;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public DocumentTypesController(IDocumentTypesService service)
        {
            this.service = service;
        }
        
        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                var documentTypes = service.GetDocumentTypes();

                return View(documentTypes);
            }
            catch(Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DocumentTypeDto documentTypeDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    service.Create(documentTypeDto);

                    return RedirectToAction(nameof(Index));
                }
                catch(Exception e)
                {
                    log.Error(e);

                    return BadRequest();
                }
            }

            return View(documentTypeDto);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                var documentType = service.GetDocumentTypeById(id.Value);

                if (documentType == null)
                {
                    return NotFound();
                }

                return View(documentType);
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, DocumentTypeDto documentTypeDto)
        {
            if (id != documentTypeDto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    service.Update(documentTypeDto);
                }
                catch (Exception e)
                {
                    log.Error(e);

                    return BadRequest();
                }

                return RedirectToAction(nameof(Index));
            }

            return View(documentTypeDto);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                var documentType = service.GetDocumentTypeById(id.Value);
                if (documentType == null)
                {
                    return NotFound();
                }

                return View(documentType);
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                service.Delete(id);

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
