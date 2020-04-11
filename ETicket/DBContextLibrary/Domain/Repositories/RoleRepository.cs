using DBContextLibrary.Domain.Entities;
using DBContextLibrary.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DBContextLibrary.Domain.Repositories
{
    public class RoleRepository : IRepository<Role, int>
    {
        private readonly ETicketDataContext db;

        public RoleRepository(ETicketDataContext context)
        {
            db = context;
        }

        public void Create(Role item)
        {
            db.ETRoles.Add(item);
        }

        public void Delete(int id)
        {
            Role order = db.ETRoles.Find(id);
            if (order != null)
            {
                db.ETRoles.Remove(order);
            }
        }

        public Role Get(int id)
        {
            return db.ETRoles.Find(id);
        }

        public IQueryable<Role> GetAll()
        {
            return db.ETRoles;
        }

        public void Update(Role item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
