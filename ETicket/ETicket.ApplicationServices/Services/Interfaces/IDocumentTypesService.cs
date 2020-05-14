using System.Collections.Generic;
using ETicket.ApplicationServices.DTOs;
using ETicket.DataAccess.Domain.Entities;

namespace ETicket.ApplicationServices.Services.Interfaces
{
    public interface IDocumentTypesService
    {
        public IEnumerable<DocumentType> GetDocumentTypes();
        public DocumentType GetDocumentTypeById(int id);

        public void Create(DocumentTypeDto documentTypeDto);

        public void Update(DocumentTypeDto documentTypeDto);

        public void Delete(int id);
    }
}
