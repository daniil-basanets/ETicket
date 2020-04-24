using System;
using System.Collections.Generic;
using System.Text;

using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;

namespace ETicket.ApplicationServices.Services.DocumentTypes
{
    public class DocumentTypesService : IDocumentTypesService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly MapperService mapper;

        public DocumentTypesService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            mapper = new MapperService();
        }

        public IEnumerable<DocumentType> GetAll()
        {
            return unitOfWork.DocumentTypes.GetAll();
        }

        public DocumentType Get(int id)
        {
            return unitOfWork.DocumentTypes.Get(id);
        }

        public void Create(DocumentTypeDto documentTypeDto)
        {
            var documentType = mapper.Map<DocumentTypeDto, DocumentType>(documentTypeDto);

            unitOfWork.DocumentTypes.Create(documentType);
            Save();
        }

        public void Update(DocumentTypeDto documentTypeDto)
        {
            var documentType = mapper.Map<DocumentTypeDto, DocumentType>(documentTypeDto);

            unitOfWork.DocumentTypes.Create(documentType);
            Save();
        }

        public void Delete(int id)
        {
            unitOfWork.DocumentTypes.Delete(id);
            Save();
        }

        public bool Exists(int id)
        {
            return unitOfWork.DocumentTypes.Get(id) != null;
        }

        private void Save()
        {
            unitOfWork.Save();
        }
    }
}
