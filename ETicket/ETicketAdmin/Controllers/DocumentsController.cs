using System;
using System.Reflection;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.Admin.Models.DataTables;

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
            log.Info(nameof(DocumentsController.Index));

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

        [HttpGet]
        public IActionResult GetCurrentPage([FromQuery]DataTablePagingInfo pagingInfo)
        {
            return Json(documentService.GetDocumentPage(pagingInfo));
        }
        
        public IActionResult Details(Guid? id)
        {
            log.Info(nameof(DocumentsController.Details));

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
            log.Info(nameof(DocumentsController.Create));

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
            log.Info(nameof(DocumentsController.Create));

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
            log.Info(nameof(DocumentsController.Edit));

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
            log.Info(nameof(DocumentsController.Edit));

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
            log.Info(nameof(DocumentsController.Delete));

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
            log.Info(nameof(DocumentsController.DeleteConfirmed));

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
