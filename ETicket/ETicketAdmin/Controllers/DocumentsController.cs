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
using Microsoft.AspNetCore.Authorization;

namespace ETicketAdmin.Controllers
{
    [Authorize(Roles = "Admin, SuperUser")]
    public class DocumentsController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public DocumentsController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // GET: Documents
        public IActionResult Index()
        {
            return View(unitOfWork.Documents.GetAll());
        }

        // GET: Documents/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            IQueryable<Document> eTicketDataContext = unitOfWork.Documents.GetAll();

            var document = eTicketDataContext
                .Include(d => d.DocumentType)
                .FirstOrDefault(m => m.Id == id);

            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // GET: Documents/Create
        public IActionResult Create()
        {
            ViewData["DocumentTypeId"] = new SelectList(unitOfWork.DocumentTypes.GetAll(), "Id", "Name");
            
            return View();
        }

        // POST: Documents/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,DocumentTypeId,Number,ExpirationDate,IsValid")] Document document)
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

        // GET: Documents/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = unitOfWork.Documents.Get((Guid)id);

            if (document == null)
            {
                return NotFound();
            }

            ViewData["DocumentTypeId"] = new SelectList(unitOfWork.DocumentTypes.GetAll(), "Id", "Name", document.DocumentTypeId);
            return View(document);
        }

        // POST: Documents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Id,DocumentTypeId,Number,ExpirationDate,IsValid")] Document document)
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

            ViewData["DocumentTypeId"] = new SelectList(unitOfWork.DocumentTypes.GetAll(), "Id", "Name", document.DocumentTypeId);
            return View(document);
        }

        // GET: Documents/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = unitOfWork.Documents.Get((Guid)id);

            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // POST: Documents/Delete/5
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
