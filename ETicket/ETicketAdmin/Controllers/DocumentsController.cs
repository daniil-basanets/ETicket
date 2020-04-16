using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ETicketDataAccess.Domain;
using ETicketDataAccess.Domain.Entities;
using ETicketDataAccess.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;

//TODO move common to another common project
//TODO Try to rename projects like ETicketWebAPI.Admin...
//TODO (nice ot have) Remove submit use ajax instead
//TODO add logger for controllers (log4NET)
//TODO Unit TESTS (coverage: in Services work must be mocked throw UnitOfWork, UOW must return mock instead of real DB data)
//TODO move Create button from table header
//TODO (nice ot have) move filter to column header columns

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

        public IActionResult Index()
        {
            ViewData["DocumentTypeId"] = new SelectList(unitOfWork.DocumentTypes.GetAll(), "Id", "Name");

            return View(unitOfWork.Documents.GetAll());
        }

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

        public IActionResult Create()
        {
            ViewData["DocumentTypeId"] = new SelectList(unitOfWork.DocumentTypes.GetAll(), "Id", "Name");
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,DocumentTypeId,Number,ExpirationDate,IsValid")] Document document)
        {//TODO remove Bind (base controller)
            if (ModelState.IsValid)
            {
                document.Id = Guid.NewGuid();
                unitOfWork.Documents.Create(document);  //TODO move business logic to Services in another project(each Service has own folder)
                unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }

            ViewData["DocumentTypeId"] = new SelectList(unitOfWork.DocumentTypes.GetAll(), "Id", "Name", document.DocumentTypeId);

            return View(document);
        }

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
