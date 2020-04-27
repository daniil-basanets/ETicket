using System;
using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services;
using ETicket.ApplicationServices.Services.DocumentTypes;
using ETicket.DataAccess.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

//TODO move common to another common project
//TODO (nice ot have) Remove submit use ajax instead
//TODO add logger for controllers (log4NET)
//TODO Unit TESTS (coverage: in Services work must be mocked throw UnitOfWork, UOW must return mock instead of real DB data)

namespace ETicket.Admin.Controllers
{
    [Authorize(Roles = "Admin, SuperUser")]
    public class DocumentsController : Controller
    {
        private readonly DocumentService documentService;
        private readonly DocumentTypesService documentTypesService;

        public DocumentsController(IUnitOfWork unitOfWork)
        {
            documentService = new DocumentService(unitOfWork);
            documentTypesService = new DocumentTypesService(unitOfWork);
        }

        [HttpGet]
        public IActionResult Index()
        {
            var documentsTypes = documentTypesService.GetDocumentTypes();

            ViewData["DocumentTypeId"] = new SelectList(documentsTypes, "Id", "Name");

            var documents = documentService.GetDocuments();

            return View(documents);
        }

        [HttpGet]
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = documentService.GetDocumentById(id.Value);

            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var documentsTypes = documentTypesService.GetDocumentTypes();

            ViewData["DocumentTypeId"] = new SelectList(documentsTypes, "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DocumentDto documentDto)
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

        [HttpGet]
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = documentService.GetDocumentById(id.Value);

            if (document == null)
            {
                return NotFound();
            }

            var documentsTypes = documentTypesService.GetDocumentTypes();

            ViewData["DocumentTypeId"] = new SelectList(documentsTypes, "Id", "Name", document.DocumentTypeId);

            return View(document);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, DocumentDto documentDto)
        {
            if (id != documentDto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    documentService.Update(documentDto);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!documentService.Exists(documentDto.Id))
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

            var documentsTypes = documentTypesService.GetDocumentTypes();

            ViewData["DocumentTypeId"] = new SelectList(documentsTypes, "Id", "Name", documentDto.DocumentTypeId);

            return View(documentDto);
        }
        
        [HttpGet]
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = documentService.GetDocumentById(id.Value);

            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            documentService.Delete(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
