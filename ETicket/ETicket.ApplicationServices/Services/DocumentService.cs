using ETicket.ApplicationServices.DTOs;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;
using System;
using System.Collections.Generic;
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

        public void Create(DocumentDto documentDto)
        {
            var document = mapper.Map<DocumentDto, Document>(documentDto);
            unitOfWork.Documents.Create(document);
            unitOfWork.Save();
        }

        public IEnumerable<DocumentDto> GetDocuments()
        {
            return mapper.Map<IQueryable<Document>, IEnumerable<DocumentDto>>(unitOfWork.Documents.GetAll()).ToList();
        }

        public Document GetDocumentById(Guid id)
        {
            return unitOfWork.Documents.Get(id);
        }

        public void Update(DocumentDto documentDto)
        {
            var document = mapper.Map<DocumentDto, Document>(documentDto);
            unitOfWork.Documents.Update(document);
            unitOfWork.Save();
        }

        public void Delete(Guid id)
        {
            unitOfWork.Documents.Delete(id);
            unitOfWork.Save();
        }

        public bool Exists(Guid id)
        {
            return unitOfWork.Documents.Get(id) != null;
        }
    }
}

