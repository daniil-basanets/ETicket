using System;
using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services;
using ETicket.ApplicationServices.Services.DocumentTypes;
using ETicket.DataAccess.Domain.Entities;
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
        private readonly IDataTableService<Document> dataTableService;

        public DocumentsController(IUnitOfWork unitOfWork, IDataTableService<Document> dataTableService)
        {
            documentService = new DocumentService(unitOfWork);
            documentTypesService = new DocumentTypesService(unitOfWork);
        }

        public IActionResult Index()
        {
            var documentsTypes = documentTypesService.GetAll();

            ViewData["DocumentTypeId"] = new SelectList(documentsTypes, "Id", "Name");


            return View();
        }

        [HttpGet]
        public IActionResult GetCurrentPage([FromQuery]DataTablePagingInfo pagingInfo)
        {
            return Json(dataTableService.GetDataTablePage(pagingInfo));
        }
        
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = documentService.Read(id.Value);

            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        public IActionResult Create()
        {
            var documentsTypes = documentTypesService.GetAll();

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
                documentService.Save();

                return RedirectToAction(nameof(Index));
            }

            var documentsTypes = documentTypesService.GetAll();

            ViewData["DocumentTypeId"] = new SelectList(documentsTypes, "Id", "Name", documentDto.DocumentTypeId);

            return View(documentDto);
        }

        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = documentService.Read(id.Value);

            if (document == null)
            {
                return NotFound();
            }

            var documentsTypes = documentTypesService.GetAll();

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
                    documentService.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocumentExists(documentDto.Id))
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

            var documentsTypes = documentTypesService.GetAll();

            ViewData["DocumentTypeId"] = new SelectList(documentsTypes, "Id", "Name", documentDto.DocumentTypeId);

            return View(documentDto);
        }

        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = documentService.Read(id.Value);

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
            documentService.Save();

            return RedirectToAction(nameof(Index));
        }

        private bool DocumentExists(Guid id)
        {
            return documentService.Read(id) != null;
        }
    }
}
