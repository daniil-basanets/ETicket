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
        private readonly IDocumentTypesService documentTypeService;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public DocumentTypesController(IDocumentTypesService documentTypeService)
        {
            this.documentTypeService = documentTypeService;
        }
        
        [HttpGet]
        public IActionResult Index()
        {
            log.Info(nameof(DocumentTypesController.Index));

            try
            {
                var documentTypes = documentTypeService.GetDocumentTypes();

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
            log.Info(nameof(DocumentTypesController.Create));

            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DocumentTypeDto documentTypeDto)
        {
            log.Info(nameof(DocumentTypesController.Create));
            
            if (ModelState.IsValid)
            {
                try
                {
                    documentTypeService.Create(documentTypeDto);

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
            log.Info(nameof(DocumentTypesController.Edit));
            
            if (id == null)
            {
                log.Warn(nameof(DocumentTypesController.Edit) + " id is null");

                return NotFound();
            }
            try
            {
                var documentType = documentTypeService.GetDocumentTypeById(id.Value);

                if (documentType == null)
                {
                    log.Warn(nameof(DocumentTypesController.Edit) + " documentType is null");

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
            log.Info(nameof(DocumentTypesController.Edit));
            
            if (id != documentTypeDto.Id)
            {
                log.Warn(nameof(DocumentTypesController.Edit) + " id is not equal to documentTypeDto.Id");

                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    documentTypeService.Update(documentTypeDto);
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
            log.Info(nameof(DocumentTypesController.Delete));
            
            if (id == null)
            {
                log.Warn(nameof(DocumentTypesController.Delete) + " id is null");

                return NotFound();
            }

            try
            {
                var documentType = documentTypeService.GetDocumentTypeById(id.Value);

                if (documentType == null)
                {
                    log.Warn(nameof(DocumentTypesController.Delete) + " documentType is null");

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
            log.Info(nameof(DocumentTypesController.DeleteConfirmed));
            
            try
            {
                documentTypeService.Delete(id);

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
