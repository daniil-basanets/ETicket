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
    public class DocumentService : IDataTablePagingService<Document>
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

        public IDictionary<string, Expression<Func<Document, object>>> GetSortExpressions()
        {
            return new Dictionary<string, Expression<Func<Document, object>>>
            {
                { "documentType", (t => t.DocumentType.Name) },
                { "number", (t => t.Number) },
                { "expirationDate", (t => t.ExpirationDate) },
                { "isValid", (t => t.IsValid) }          
            };
        }

        public Expression<Func<Document, bool>> GetSingleFilterExpression(string columnName, string filterValue)
        {
            return columnName switch
            {
                "documentType" => (t => t.DocumentType.Name.StartsWith(filterValue)),
                "number" => (t => t.Number.StartsWith(filterValue)),
                "expirationDate" => (t => t.ExpirationDate.ToString().Contains(filterValue)),
                "isValid" => (t => t.IsValid.ToString() == filterValue),
                _ => (t => true)
            };
        }

        public IList<Expression<Func<Document, bool>>> GetFilterExpressions(string[] columnNames, string[] filterValues)
        {
            var result = new List<Expression<Func<Document, bool>>>();

            for (int i = 0; i < columnNames.Length; i++)
            {
                result.Add(GetSingleFilterExpression(columnNames[i], filterValues[i]));
            }

            return result;
        }

        public IList<Expression<Func<Document, bool>>> GetGlobalSearchExpressions(string searchValue)
        {
            return new List<Expression<Func<Document, bool>>>
            {
                (t => t.Number.StartsWith(searchValue)),
                (t => t.DocumentType.Name.StartsWith(searchValue)),
                (t => t.ExpirationDate.ToString().Contains(searchValue)),
            };
        }

        public IQueryable<Document> GetAll()
        {
            return unitOfWork.Documents
                .GetAll()
                .Include(t => t.DocumentType);
        }
    }
}
