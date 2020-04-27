using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services.Interfaces;

namespace ETicket.Admin.Controllers
{
    [Authorize(Roles = "Admin, SuperUser")]
    public class DocumentTypesController : Controller
    {
        private readonly IDocumentTypesService service;

        public DocumentTypesController(IDocumentTypesService service)
        {
            this.service = service;
        }
        
        [HttpGet]
        public IActionResult Index()
        {
            var documentTypes = service.GetDocumentTypes();

            return View(documentTypes);
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
                service.Create(documentTypeDto);

                return RedirectToAction(nameof(Index));
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

            var documentType = service.GetDocumentTypeById(id.Value);
            if (documentType == null)
            {
                return NotFound();
            }

            return View(documentType);
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
                catch (DbUpdateConcurrencyException)
                {
                    if (!service.Exists(documentTypeDto.Id))
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

            return View(documentTypeDto);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var documentType = service.GetDocumentTypeById(id.Value);
            if (documentType == null)
            {
                return NotFound();
            }

            return View(documentType);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            service.Delete(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
