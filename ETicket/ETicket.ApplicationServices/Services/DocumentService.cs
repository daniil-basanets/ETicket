using ETicket.ApplicationServices.DTOs;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ETicket.ApplicationServices.Services.Interfaces;

namespace ETicket.ApplicationServices.Services
{
    public class DocumentService : IDocumentService
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
            Save();
        }

        public IQueryable<Document> Read()
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
            Save();
        }

        public void Delete(Guid id)
        {
            unitOfWork.Documents.Delete(id);
            Save();
        }

        public bool Exists(Guid id)
        {
            return unitOfWork.Documents.Get(id) != null;
        }

        public void Save()
        {
            unitOfWork.Save();
        }
    }
}
