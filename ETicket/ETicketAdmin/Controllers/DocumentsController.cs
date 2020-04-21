using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ETicket.Admin.Extensions;
using ETicket.Admin.Models.DataTables;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;
using ETicketAdmin.DTOs;
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
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public DocumentsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public IActionResult Index()
        {
            ViewData["DocumentTypeId"] = new SelectList(unitOfWork.DocumentTypes.GetAll(), "Id", "Name");

            return View();
        }

        [HttpPost]
        public IActionResult GetCurrentPage(DataTableParameters dataTableParameters)
        {
            var drawStep = int.Parse(Request.Form["draw"]);

            var countRecords = unitOfWork
                    .Documents
                    .GetAll()
                    .AsNoTracking()
                    .Count();

            IQueryable<Document> documents = unitOfWork
                    .Documents
                    .GetAll()
                    .AsNoTracking()
                    .Include(t => t.DocumentType);

            SortDataTable(ref documents, dataTableParameters.Order);
            SearchInDataTable(ref documents, dataTableParameters.Search.Value);

            var countFiltered = documents.Count();

            documents = documents
                    .Skip(dataTableParameters.Start)
                    .Take(dataTableParameters.Length);

            return GetCurrentPage(documents, drawStep, countRecords, countFiltered);
        }

        private void SearchInDataTable(
            ref IQueryable<Document> users,
            string searchString
        )
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                users = users.ApplySearchBy(
                    t =>
                    t.DocumentType.Name.StartsWith(searchString)
                     || t.Number.StartsWith(searchString)
                     || t.ExpirationDate.ToString().StartsWith(searchString)
                     || "Yes".StartsWith(searchString) ? t.IsValid == true : false
                     || "No".StartsWith(searchString) ? t.IsValid == false : false
                     );
            }
        }

        private void SortDataTable(
            ref IQueryable<Document> documents,
            List<DataOrder> orders
        )
        {
            foreach (var order in orders)
            {
                documents = order.Column switch
                {
                    0 => documents.ApplySortBy(t => t.DocumentType.Name, order.Dir),
                    1 => documents.ApplySortBy(t => t.Number, order.Dir),
                    2 => documents.ApplySortBy(t => t.ExpirationDate, order.Dir),
                    3 => documents.ApplySortBy(t => t.IsValid, order.Dir),
                    _ => documents.ApplySortBy(t => t.ExpirationDate, "desc")
                };
            }
        }

        private JsonResult GetCurrentPage(
            IQueryable<Document> documents,
            int drawStep,
            int countRecords,
            int countFiltered
        )
        {
            return Json(new
            {
                draw = ++drawStep,
                recordsTotal = countRecords,
                recordsFiltered = countFiltered,
                data = documents
            });
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
        public IActionResult Create(DocumentDto documentDto)
        {//TODO remove Bind (base controller)
            if (ModelState.IsValid)
            {
                var document = mapper.Map<Document>(documentDto);

                document.Id = Guid.NewGuid();
                unitOfWork.Documents.Create(document);  //TODO move business logic to Services in another project(each Service has own folder)
                unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }

            ViewData["DocumentTypeId"] = new SelectList(unitOfWork.DocumentTypes.GetAll(), "Id", "Name", documentDto.DocumentTypeId);

            return View(documentDto);
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
                    var document = mapper.Map<Document>(documentDto);

                    unitOfWork.Documents.Update(document);
                    unitOfWork.Save();
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

            ViewData["DocumentTypeId"] = new SelectList(unitOfWork.DocumentTypes.GetAll(), "Id", "Name", documentDto.DocumentTypeId);
            return View(documentDto);
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
