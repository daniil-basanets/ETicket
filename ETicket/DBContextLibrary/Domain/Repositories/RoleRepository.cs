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
            db.Roles.Add(item);
        }

        public void Delete(int id)
        {
            Role order = db.Roles.Find(id);
            if (order != null)
            {
                db.Roles.Remove(order);
            }
        }

        public Role Get(int id)
        {
            return db.Roles.Find(id);
        }

        public IQueryable<Role> GetAll()
        {
            return db.Roles;
        }

        public void Update(Role item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
