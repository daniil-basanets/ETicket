using ETicket.ApplicationServices.DTOs;
using ETicket.DataAccess.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETicket.ApplicationServices.Services.Interfaces
{
    public interface IDocumentService
    {
        public IQueryable<Document> Read();
        public Document Read(Guid id);

        public void Create(DocumentDto documentTypeDto);

        public void Update(DocumentDto documentTypeDto);

        public void Delete(Guid id);

        public bool Exists(Guid id);
    }
}
