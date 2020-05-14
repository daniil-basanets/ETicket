using System;
using System.Collections.Generic;
using ETicket.ApplicationServices.DTOs;
using ETicket.DataAccess.Domain.Entities;

namespace ETicket.ApplicationServices.Services.Interfaces
{
    public interface IDocumentService
    {

        public IEnumerable<Document> GetDocuments();
        public Document GetDocumentById(Guid Id);
        public void Create(DocumentDto documentTypeDto);
        public void Update(DocumentDto documentTypeDto);
        public void Delete(Guid id);
    }
}