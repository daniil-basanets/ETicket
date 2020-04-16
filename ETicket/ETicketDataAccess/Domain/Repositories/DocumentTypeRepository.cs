using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETicket.DataAccess.Domain.Repositories
{
    public class DocumentTypeRepository : IRepository<DocumentType, int>
    {
        private readonly ETicketDataContext db;

        public DocumentTypeRepository(ETicketDataContext context)
        {
            db = context;
        }

        public void Create(DocumentType item)
        {
            db.DocumentTypes.Add(item);
        }

        public void Delete(int id)
        {
            DocumentType documentType = db.DocumentTypes.Find(id);
            if (documentType != null)
            {
                db.DocumentTypes.Remove(documentType);
            }
        }

        public DocumentType Get(int id)
        {
            return db.DocumentTypes.Find(id);
        }

        public IQueryable<DocumentType> GetAll()
        {
            return db.DocumentTypes;
        }

        public void Update(DocumentType item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
