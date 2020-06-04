using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.ApplicationServices.Validation;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;

namespace ETicket.ApplicationServices.Services.DocumentTypes
{
    public class DocumentTypesService : IDocumentTypesService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly MapperService mapper;
        private readonly DocumentTypeValidator documentTypeValidator;

        public DocumentTypesService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            mapper = new MapperService();
            documentTypeValidator = new DocumentTypeValidator();
        }

        public IEnumerable<DocumentTypeDto> GetDocumentTypes()
        {
            var documentTypes = unitOfWork.DocumentTypes.GetAll();
            
            return mapper.Map<IQueryable<DocumentType>, IEnumerable<DocumentTypeDto>>(documentTypes).ToList();
        }

        public DocumentTypeDto GetDocumentTypeById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "id should be greater than zero");
            }

            var documentType = unitOfWork.DocumentTypes.Get(id);

            return mapper.Map<DocumentType, DocumentTypeDto>(documentType);
        }

        public void Create(DocumentTypeDto documentTypeDto)
        {
            if (!documentTypeValidator.Validate(documentTypeDto).IsValid)
            {
                throw new ArgumentException(documentTypeValidator.Validate(documentTypeDto).Errors.First().ErrorMessage);
            }

            var documentType = mapper.Map<DocumentTypeDto, DocumentType>(documentTypeDto);

            unitOfWork.DocumentTypes.Create(documentType);
            unitOfWork.Save();
        }

        public void Update(DocumentTypeDto documentTypeDto)
        {
            if (!documentTypeValidator.Validate(documentTypeDto).IsValid)
            {
                throw new ArgumentException(documentTypeValidator.Validate(documentTypeDto).Errors.First().ErrorMessage);
            }

            var documentType = mapper.Map<DocumentTypeDto, DocumentType>(documentTypeDto);

            unitOfWork.DocumentTypes.Create(documentType);
            unitOfWork.Save();
        }

        public void Delete(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "id should be greater than zero");
            }

            unitOfWork.DocumentTypes.Delete(id);
            unitOfWork.Save();
        }
    }
}
