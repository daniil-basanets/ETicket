using System;
using System.Collections.Generic;
using System.Linq;
using ETicket.ApplicationServices.DTOs;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;

namespace ETicket.ApplicationServices.Services
{
    public class DatabaseServices
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly MapperService mapper;

        public DatabaseServices(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;

            mapper = new MapperService();
        }

        public void Create<T>(T item)
        {
            switch (item)
            {
                case var type when type.GetType() == typeof(DocumentTypeDto):
                    var documentType = mapper.Map<DocumentTypeDto, DocumentType>(item as DocumentTypeDto);
                    unitOfWork.DocumentTypes.Create(documentType);
                    break;
                case var type when type.GetType() == typeof(DocumentDto):
                    var document = mapper.Map<DocumentDto, Document>(item as DocumentDto);
                    document.Id = Guid.NewGuid();
                    unitOfWork.Documents.Create(document);
                    break;
            }
        }

        public IQueryable Read<T>()
        {
            IQueryable data = typeof(T) switch
            {
                var type when type == typeof(DocumentType) => unitOfWork.DocumentTypes.GetAll(),
                var type when type == typeof(Document) => unitOfWork.Documents.GetAll(),
                _ => new List<T>().AsQueryable()
            };

            return data;
        }

        public T Read<T>(Guid id)
        {
            T item = default;

            switch (typeof(T))
            {
                case var type when type == typeof(Document):
                    var document = unitOfWork.Documents.Get(id);
                    item = (T)Convert.ChangeType(document, typeof(T));
                    break;
            }

            return item;
        }

        public T Read<T>(int id)
        {
            T item = default;

            switch (typeof(T))
            {
                case var type when type == typeof(DocumentType):
                    var documentType = unitOfWork.DocumentTypes.Get(id);
                    item = (T)Convert.ChangeType(documentType, typeof(T));
                    break;
            }

            return item;
        }

        public void Update<T>(T dto)
        {
            switch (typeof(T))
            {
                case var type when type == typeof(DocumentTypeDto):
                    var documentType = mapper.Map<DocumentTypeDto, DocumentType>(dto as DocumentTypeDto);
                    unitOfWork.DocumentTypes.Update(documentType);
                    break;
                case var type when type == typeof(DocumentDto):
                    var document = mapper.Map<DocumentDto, Document>(dto as DocumentDto);
                    unitOfWork.Documents.Update(document);
                    break;
            }
        }

        public void Delete<T>(int id)
        {
            switch (typeof(T))
            {
                case var type when type == typeof(DocumentType):
                    unitOfWork.DocumentTypes.Delete(id);
                    break;
            }
        }

        public void Delete<T>(Guid id)
        {
            switch (typeof(T))
            {
                case var type when type == typeof(Document):
                    unitOfWork.Documents.Delete(id);
                    break;
            }
        }

        public void Save() => unitOfWork.Save();
    }
}