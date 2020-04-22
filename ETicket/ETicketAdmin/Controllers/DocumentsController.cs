using System;
using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

//TODO move common to another common project
//TODO Try to rename projects like ETicket.WebAPI.Admin...
//TODO (nice ot have) Remove submit use ajax instead
//TODO add logger for controllers (log4NET)
//TODO Unit TESTS (coverage: in Services work must be mocked throw UnitOfWork, UOW must return mock instead of real DB data)
//TODO move Create button from table header
//TODO (nice ot have) move filter to column header columns

namespace ETicket.Admin.Controllers
{
    [Authorize(Roles = "Admin, SuperUser")]
    public class DocumentsController : Controller
    {
        private readonly DatabaseServices services;

        public DocumentsController(IUnitOfWork unitOfWork)
        {
            services = new DatabaseServices(unitOfWork);
        }

        public IActionResult Index()
        {
            var documentsTypes = services.Read<DocumentType>();

            ViewData["DocumentTypeId"] = new SelectList(documentsTypes, "Id", "Name");

            var documents = services.Read<Document>();

            return View(documents);
        }

        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = services.Read<Document>(id.Value);

            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        public IActionResult Create()
        {
            var documentsTypes = services.Read<DocumentType>();

            ViewData["DocumentTypeId"] = new SelectList(documentsTypes, "Id", "Name");
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DocumentDto documentDto)
        {
            if (ModelState.IsValid)
            {
                services.Create(documentDto);
                services.Save();

                return RedirectToAction(nameof(Index));
            }

            var documentsTypes = services.Read<DocumentType>();

            ViewData["DocumentTypeId"] = new SelectList(documentsTypes, "Id", "Name", documentDto.DocumentTypeId);

            return View(documentDto);
        }

        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = services.Read<Document>(id.Value);

            if (document == null)
            {
                return NotFound();
            }

            var documentsTypes = services.Read<DocumentType>();

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
                    services.Update(documentDto);
                    services.Save();
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

            var documentsTypes = services.Read<DocumentType>();

            ViewData["DocumentTypeId"] = new SelectList(documentsTypes, "Id", "Name", documentDto.DocumentTypeId);
            return View(documentDto);
        }

        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = services.Read<Document>(id.Value);

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
            services.Delete<Document>(id);
            services.Save();

            return RedirectToAction(nameof(Index));
        }

        private bool DocumentExists(Guid id)
        {
            return services.Read<Document>(id) != null;
        }
    }
}
