using System;
using System.Collections.Generic;
using System.Linq;
using DBContextLibrary.Domain.Entities;
using DBContextLibrary.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DBContextLibrary.Domain.Repositories
{
	public class PrivilegeRepository: IRepository<Privilege>
	{
        #region

        private readonly ETicketDataContext context;

        #endregion

        public PrivilegeRepository(ETicketDataContext context)
        {
            this.context = context;
        }

        public void Create(Privilege privelege)
        {
            context.Privileges.Add(privelege);
        }

        public void Delete(int id)
        {
            Privilege deleted = context.Privileges.Find(id);
            if (deleted != null)
            {
                context.Privileges.Remove(deleted);
            }
        }

        public Privilege Get(int id)
        {
            return context.Privileges.Find(id);
        }

        public IQueryable<Privilege> GetAll()
        {
            return context.Privileges;
        }

        public void Update(Privilege privilege)
        {
            context.Entry(privilege).State = EntityState.Modified;
        }
    }
}
