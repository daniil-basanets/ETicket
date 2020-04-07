using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using DBContextLibrary.Domain;
using DBContextLibrary.Domain.Entities;
using DBContextLibrary.Domain.Interfaces;

namespace ETicketAdmin.Controllers
{
    public class DocumentTypesController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public DocumentTypesController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // GET: DocumentTypes
        public IActionResult Index()
        {
            return View(unitOfWork.DocumentTypes.GetAll());
        }

        // GET: DocumentTypes/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var documentType = unitOfWork.DocumentTypes.Get((int)id);
            if (documentType == null)
            {
                return NotFound();
            }

            return View(documentType);
        }

        // GET: DocumentTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DocumentTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name")] DocumentType documentType)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.DocumentTypes.Create(documentType);
                unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }

            return View(documentType);
        }

        // GET: DocumentTypes/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var documentType = unitOfWork.DocumentTypes.Get((int)id);
            if (documentType == null)
            {
                return NotFound();
            }

            return View(documentType);
        }

        // POST: DocumentTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Name")] DocumentType documentType)
        {
            if (id != documentType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    unitOfWork.DocumentTypes.Update(documentType);
                    unitOfWork.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocumentTypeExists(documentType.Id))
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

            return View(documentType);
        }

        // GET: DocumentTypes/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var documentType = unitOfWork.DocumentTypes.Get((int)id);
            if (documentType == null)
            {
                return NotFound();
            }

            return View(documentType);
        }

        // POST: DocumentTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            unitOfWork.DocumentTypes.Delete(id);
            unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        private bool DocumentTypeExists(int id)
        {
            return unitOfWork.DocumentTypes.Get(id) != null;
        }
    }
}
