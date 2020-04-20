using AutoMapper;
using ETicket.Admin.Models.DataTables;
using ETicket.Admin.Services;
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
            CRUD cRUD = new CRUD(unitOfWork, mapper);
            return View(cRUD.Read<DocumentType>(typeof(DocumentType)));
        }

        // GET: DocumentTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        //// POST: DocumentTypes/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Create(DocumentTypeDto documentTypeDto)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        Add(documentTypeDto);
        //        Save();
        //        return RedirectToAction(nameof(Index));
        //    }

        //    return View(documentTypeDto);
        //}
     

        //// GET: DocumentTypes/Edit/5
        //public IActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var documentType = Read((int)id);
        //    if (documentType == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(documentType);
        //}


        //// POST: DocumentTypes/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Edit(int id, DocumentTypeDto documentTypeDto)
        //{
        //    if (id != documentTypeDto.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            Update(documentTypeDto);
        //            Save();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!DocumentTypeExists(documentTypeDto.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }

        //        return RedirectToAction(nameof(Index));
        //    }

        //    return View(documentTypeDto);
        //}
      
        //// GET: DocumentTypes/Delete/5
        //public IActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var documentType = Read((int)id);
        //    if (documentType == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(documentType);
        //}

        //// POST: DocumentTypes/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public IActionResult DeleteConfirmed(int id)
        //{
        //    Delete(id);
        //    Save();

        //    return RedirectToAction(nameof(Index));
        //}
       
        //private bool DocumentTypeExists(int id)
        //{
        //    return unitOfWork.DocumentTypes.Get(id) != null;
        //}

        #region CRUD service

        #endregion
    }
}
