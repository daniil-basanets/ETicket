using System;
using System.Reflection;
using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services.Interfaces;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ETicket.Admin.Controllers
{
    [Authorize(Roles = "Admin, SuperUser")]
    public class DocumentsController : Controller
    {
        private readonly IDocumentService documentService;
        private readonly IDocumentTypesService documentTypesService;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public DocumentsController(IDocumentService documentService, IDocumentTypesService documentTypesService)
        {
            this.documentService = documentService;
            this.documentTypesService = documentTypesService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                var documentsTypes = documentTypesService.GetDocumentTypes();
                ViewData["DocumentTypeId"] = new SelectList(documentsTypes, "Id", "Name");
                var documents = documentService.GetDocuments();                

                return View(documents);
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
                log.Warn(nameof(DocumentsController.Details) + " id is null");

                return NotFound();
            }

            try
            {
                var document = documentService.GetDocumentById(id.Value);

                if (document == null)
                {
                    log.Warn(nameof(DocumentsController.Details) + " document is null");

                    return NotFound();
                }

                return View(document);
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
                var documentsTypes = documentTypesService.GetDocumentTypes();

                ViewData["DocumentTypeId"] = new SelectList(documentsTypes, "Id", "Name");

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
        public IActionResult Create(DocumentDto documentDto)
        {
            try
            {
                if (ModelState.IsValid)
                {      
                    documentService.Create(documentDto);

                    return RedirectToAction(nameof(Index));
                }

                var documentsTypes = documentTypesService.GetDocumentTypes();

                ViewData["DocumentTypeId"] = new SelectList(documentsTypes, "Id", "Name", documentDto.DocumentTypeId);

                return View(documentDto);
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
                log.Warn(nameof(DocumentsController.Edit) + " id is null");

                return NotFound();
            }

            try
            {
                var document = documentService.GetDocumentById(id.Value);

                if (document == null)
                {
                    log.Warn(nameof(DocumentsController.Edit) + " document is null");

                    return NotFound();
                }

                var documentsTypes = documentTypesService.GetDocumentTypes();

                ViewData["DocumentTypeId"] = new SelectList(documentsTypes, "Id", "Name", document.DocumentTypeId);

                return View(document);
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }         
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, DocumentDto documentDto)
        {
            if (id != documentDto.Id)
            {
                log.Warn(nameof(DocumentsController.Edit) + " id isn't equal to documentDto.Id");

                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    documentService.Update(documentDto);

                    return RedirectToAction(nameof(Index));
                }

                var documentsTypes = documentTypesService.GetDocumentTypes();

                ViewData["DocumentTypeId"] = new SelectList(documentsTypes, "Id", "Name", documentDto.DocumentTypeId);

                return View(documentDto);
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
                log.Warn(nameof(DocumentsController.Delete) + " id is null");

                return NotFound();
            }

            try
            {
                var document = documentService.GetDocumentById(id.Value);

                if (document == null)
                {
                    log.Warn(nameof(DocumentsController.Delete) + " document is null");

                    return NotFound();
                }

                return View(document);
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
                documentService.Delete(id);

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
