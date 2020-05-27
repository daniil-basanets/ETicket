using ETicket.ApplicationServices.DTOs;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using ETicket.Admin.Models.DataTables;
using ETicket.ApplicationServices.Services.DataTable;
using ETicket.ApplicationServices.Services.DataTable.Interfaces;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.ApplicationServices.Services.PagingServices;
using ETicket.ApplicationServices.Services.PagingServices.Models;

namespace ETicket.ApplicationServices.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly MapperService mapper;
        private readonly IDataTableService<Document> dataTableService;

        public DocumentService(IUnitOfWork uow)
        {
            unitOfWork = uow;
            mapper = new MapperService();
            var dataTablePagingService = new DocumentPagingService(unitOfWork);
            dataTableService = new DataTableService<Document>(dataTablePagingService);
        }

        public void Create(DocumentDto documentDto)
        {
            var document = mapper.Map<DocumentDto, Document>(documentDto);
            unitOfWork.Documents.Create(document);
            unitOfWork.Save();
        }

        public IEnumerable<DocumentDto> GetDocuments()
        {
            var documents = unitOfWork.Documents.GetAll();
            
            return mapper.Map<IQueryable<Document>, IEnumerable<DocumentDto>>(documents).ToList();
        }

        public DocumentDto GetDocumentById(Guid id)
        {
            return mapper.Map<Document, DocumentDto>(unitOfWork.Documents.Get(id));
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

        public DataTablePage<DocumentDto> GetDocumentPage(DataTablePagingInfo pagingInfo)
        {
            var documentsPage = dataTableService.GetDataTablePage(pagingInfo);

            return mapper.Map<DataTablePage<Document>, DataTablePage<DocumentDto>>(documentsPage);
        }
    }
}

