//TODO 

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using DBContextLibrary.Domain;
//using DBContextLibrary.Domain.Entities;

//namespace ETicketAdmin.Controllers
//{
//    public class DocumentsController : Controller
//    {
//        private readonly ETicketData eTicketData;
//        private readonly ETicketDataContext _context;

//        public DocumentsController(ETicketDataContext context)
//        {
//            _context = context;
//            eTicketData = new ETicketData(_context);
//        }

//        public IActionResult Index()
//        {
//            return View(eTicketData.Documents.GetAll());
//        }

//        public IActionResult Details(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var document = eTicketData.Documents.Get((int)id);
//            if (document == null)
//            {
//                return NotFound();
//            }

//            return View(document);
//        }

//        public IActionResult Create()
//        {
//            return View();
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public IActionResult Create([Bind("Id,DocumentType,Number,ExpirationDate,IsValid")] Document document)
//        {
//            if (ModelState.IsValid)
//            {
//                eTicketData.Documents.Create(document);
//                eTicketData.Save();
//                return RedirectToAction(nameof(Index));
//            }
//            return View(document);
//        }

//        public IActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var document = eTicketData.Documents.Get((int)id);
//            if (document == null)
//            {
//                return NotFound();
//            }
//            return View(document);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public IActionResult Edit(Guid id, [Bind("Id,DocumentType,Number,ExpirationDate,IsValid")] Document document)
//        {
//            if (id != document.Id)
//            {
//                return NotFound();
//            }

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    eTicketData.Documents.Update(document);
//                    eTicketData.Save();
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!DocumentExists(document.Id))
//                    {
//                        return NotFound();
//                    }
//                    else
//                    {
//                        throw;
//                    }
//                }
//                return RedirectToAction(nameof(Index));
//            }
//            return View(document);
//        }

//        public IActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var document = eTicketData.Documents.Get((int)id);
//            if (document == null)
//            {
//                return NotFound();
//            }

//            return View(document);
//        }

//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public IActionResult DeleteConfirmed(int id)
//        {
//            eTicketData.Documents.Delete(id);
//            eTicketData.Save();
//            return RedirectToAction(nameof(Index));
//        }

//        private bool DocumentExists(Guid id)
//        {
//            return _context.Documents.Any(d => d.Id == id);
//        }

//    }
//}