using DBContextLibrary.Domain.Entities;
using DBContextLibrary.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBContextLibrary.Domain.Repositories
{
    public class DocumentRepository : IRepository<Document, Guid>
    {
        private readonly ETicketDataContext db;

        public DocumentRepository(ETicketDataContext context)
        {
            db = context;
        }

        public void Create(Document item)
        {
            db.Documents.Add(item);
        }

        public void Delete(Guid id)
        {
            Document document = db.Documents.Find(id);
            if (document != null)
            {
                db.Documents.Remove(document);
            }
        }

        public Document Get(Guid id)
        {
            return db.Documents.Find(id);
        }

        public IQueryable<Document> GetAll()
        {
            return db.Documents;
        }

        public void Update(Document item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
