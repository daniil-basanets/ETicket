using System;
using System.Collections.Generic;
using System.Text;

using ETicket.ApplicationServices.DTOs;
using ETicket.DataAccess.Domain.Entities;

namespace ETicket.ApplicationServices.Services.Interfaces
{
    public interface IDocumentTypesService
    {
        public IEnumerable<DocumentType> GetAll();
        public DocumentType Get(int id);

        public void Create(DocumentTypeDto documentTypeDto);

        public void Update(DocumentTypeDto documentTypeDto);

        public void Delete(int id);

        public bool Exists(int id);
    }
}
