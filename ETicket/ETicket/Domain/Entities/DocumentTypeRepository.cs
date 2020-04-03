using ETicket.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETicket.Domain.Entities
{
    public class DocumentTypeRepository : IRepository<DocumentType>
    {
        private readonly ETicketDataContext db;

        public DocumentTypeRepository(ETicketDataContext context)
        {
            db = context;
        }

        public void Create(DocumentType item)
        {
            db.DocumentType.Add(item);
        }

        public void Delete(int id)
        {
            DocumentType documentType = db.DocumentType.Find(id);
            if (documentType != null)
            {
                db.DocumentType.Remove(documentType);
            }
        }

        public DocumentType Get(int id)
        {
            return db.DocumentType.Find(id);
        }

        public IEnumerable<DocumentType> GetAll()
        {
            return db.DocumentType;
        }

        public void Update(DocumentType item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
