using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DBContextLibrary.Domain;
using DBContextLibrary.Domain.Entities;
using DBContextLibrary.Domain.Interfaces;

namespace ETicketAdmin.Controllers
{
    public class DocumentsController : Controller
    {
        //private readonly ETicketData eTicketData;
        //private readonly ETicketDataContext _context;

        private readonly IUnitOfWork unitOfWork;

        public DocumentsController(IUnitOfWork unitOfWork)
        {
            //_context = context;
            //eTicketData = new ETicketData(_context);
            this.unitOfWork = unitOfWork;
        }

        public IActionResult Index(string sorting)
        {
            ViewBag.DocumentSorting = String.IsNullOrEmpty(sorting) ? "Documents_Desc" : "";
            ViewBag.NumberSorting = sorting == "Number" ? "Number_desc" : "Number";
            ViewBag.ExpirationDateSorting = sorting == "Expiration Date" ? "ExpirationDate_Desc" : "ExpirationDate";
            ViewBag.IsValid = sorting == "IsValid" ? "IsValid_Desc" : "IsValid";

            var eContext = unitOfWork.Documents.GetAll();
            IOrderedQueryable<Document> documents;

            switch (sorting)
            {
                case "Documents_Desc":
                    documents = eContext.OrderByDescending(s => s.DocumentType);
                    break;
                case "Number_desc":
                    documents = eContext.OrderByDescending(s => s.Number);
                    break;
                case "Number":
                    documents = eContext.OrderBy(s => s.Number);
                    break;
                case "ExpirationDate_Desc":
                    documents = eContext.OrderByDescending(s => s.ExpirationDate);
                    break;
                case "ExpirationDate":
                    documents = eContext.OrderBy(s => s.ExpirationDate);
                    break;
                case "IsValid_Desc":
                    documents = eContext.OrderByDescending(s => s.IsValid);
                    break;
                case "IsValid":
                    documents = eContext.OrderBy(s => s.IsValid);
                    break;
                default:
                    documents = eContext.OrderBy(s => s.DocumentType);
                    break;
            }
            return View(documents.ToList());
        }

        public IActionResult Details(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = unitOfWork.Documents.Get(id);

            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        public IActionResult Create()
        {
            ViewData["DocumentTypeId"] = new SelectList(unitOfWork.DocumentTypes.GetAll(), "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,DocumentType,Number,ExpirationDate,IsValid")] Document document)
        {
            if (ModelState.IsValid)
            {
                document.Id = Guid.NewGuid();
                unitOfWork.Documents.Create(document);
                unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }

            ViewData["DocumentTypeId"] = new SelectList(unitOfWork.DocumentTypes.GetAll(), "Id", "Name", document.DocumentTypeId);

            return View(document);
        }

        public IActionResult Edit(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = unitOfWork.Documents.Get(id);
            if (document == null)
            {
                return NotFound();
            }

            ViewData["DocumentTypeId"] = new SelectList(unitOfWork.Documents.GetAll(), "Id", "Name", document.DocumentTypeId);

            return View(document);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Id,DocumentType,Number,ExpirationDate,IsValid")] Document document)
        {
            if (id != document.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    unitOfWork.Documents.Update(document);
                    unitOfWork.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocumentExists(document.Id))
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

            ViewData["DocumentTypeId"] = new SelectList(unitOfWork.Documents.GetAll(), "Id", "Name", document.DocumentTypeId);

            return View(document);
        }

        public IActionResult Delete(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = unitOfWork.Documents.Get(id);
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
            unitOfWork.Documents.Delete(id);
            unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        private bool DocumentExists(Guid id)
        {
            return unitOfWork.Documents.Get(id) != null;
        }

    }
}