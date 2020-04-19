using AutoMapper;
using ETicket.Admin.Models.DataTables;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;
using ETicketAdmin.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ETicket.Admin.Controllers
{
    [Authorize(Roles = "Admin, SuperUser")]
    public class DocumentTypesController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public DocumentTypesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        // GET: DocumentTypes
        public IActionResult Index()
        {
            return View();
            //return View(unitOfWork.DocumentTypes.GetAll());
        }

        static int drawI = 0;
        [HttpPost]
        public IActionResult GetPage(DataTableParameters dataTableParameters)
        {
            var documentTypes = unitOfWork.DocumentTypes.GetAll();
            //int totalCount = 
            drawI++;

            //var temp = dataTableParameters.Order[0].Dir.ToString();
            documentTypes = SortByColumn(documentTypes, dataTableParameters);
            //documentTypes.SortByColumn();

            var data = documentTypes.Where(x => x.Name.Contains(dataTableParameters.Search.Value + ""))
                    .Skip(dataTableParameters.Start * dataTableParameters.Length)
                    .Take(dataTableParameters.Length);

            return Json(new
            {
                draw = drawI,
                recordsTotal = documentTypes.CountAsync().Result,
                recordsFiltered = documentTypes.CountAsync().Result,
                data
            });
        }

        //public static void SortByColumn(this IQueryable<DocumentType> array, DataTableParameters dataTableParameters)
        //{
        //    //IOrderedQueryable<DocumentType> result = array.OrderBy(x => x.Name);

        //    foreach (DataOrder order in dataTableParameters.Order)
        //    {
        //        if (order.Column == 0)
        //        {
        //            if (order.Dir.ToLower() == "desc")
        //            {
        //                array = array.OrderByDescending(x => x.Name);
        //            }
        //            else if (order.Dir.ToLower() == "asc")
        //            {
        //                array = array.OrderBy(x => x.Name);
        //            }
        //        }
        //    }

        //    //return result;
        //}

        public IQueryable<DocumentType> SortByColumn(IQueryable<DocumentType> array, DataTableParameters dataTableParameters)
        {
            IOrderedQueryable<DocumentType> result = array.OrderBy(x => x.Name);

            foreach (DataOrder order in dataTableParameters.Order)
            {
                if (order.Column == 0)
                {
                    if (order.Dir.ToLower() == "desc")
                    {
                        result = array.OrderByDescending(x => x.Name);
                    }
                    else if (order.Dir.ToLower() == "asc")
                    {
                        result = array.OrderBy(x => x.Name);
                    }
                }
            }

            return result;
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
        public IActionResult Create(DocumentTypeDto documentTypeDto)
        {
            if (ModelState.IsValid)
            {
                var documentType = mapper.Map<DocumentType>(documentTypeDto);

                unitOfWork.DocumentTypes.Create(documentType);
                unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }

            return View(documentTypeDto);
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
                    var documentType = mapper.Map<DocumentType>(documentTypeDto);

                    unitOfWork.DocumentTypes.Update(documentType);
                    unitOfWork.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocumentTypeExists(documentTypeDto.Id))
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
