using ETicket.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETicket.Domain.Entities
{
    public class DocumentRepository : IRepository<Document>
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

        public void Delete(int id)
        {
            Document document = db.Documents.Find(id);
            if (document != null)
            {
                db.Documents.Remove(document);
            }
        }

        public Document Get(int id)
        {
            return db.Documents.Find(id);
        }

        public IEnumerable<Document> GetAll()
        {
            return db.Documents;
        }

        public void Update(Document item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
