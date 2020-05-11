using ETicket.ApplicationServices.DTOs;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ETicket.ApplicationServices.Services.DataTable.Interfaces;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace ETicket.ApplicationServices.Services
{
    public class DocumentService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly MapperService mapper;

        public DocumentService(IUnitOfWork uow)
        {
            unitOfWork = uow;

            mapper = new MapperService();
        }

        public void Create(DocumentDto document)
        {
            var documentService = mapper.Map<DocumentDto, Document>(document);
            unitOfWork.Documents.Create(documentService);
        }

        public IQueryable Read()
        {
            return unitOfWork.Documents.GetAll();
        }

        public Document Read(Guid id)
        {
            return unitOfWork.Documents.Get(id);
        }

        public void Update(DocumentDto document)
        {
            var documentSerice = mapper.Map<DocumentDto, Document>(document);
            unitOfWork.Documents.Update(documentSerice);
        }

        public void Delete(Guid id)
        {
            unitOfWork.Documents.Delete(id);
        }

        public void Save()
        {
            unitOfWork.Save();
        }
    }
}
