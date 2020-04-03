using System;
using System.Collections.Generic;
using ETicket.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ETicket.Domain.Entities
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
            context.Privilege.Add(privelege);
        }

        public void Delete(int id)
        {
            Privilege deleted = context.Privilege.Find(id);
            if (deleted != null)
            {
                context.Privilege.Remove(deleted);
            }
        }

        public Privilege Get(int id)
        {
            return context.Privilege.Find(id);
        }

        public IEnumerable<Privilege> GetAll()
        {
            return context.Privilege;
        }

        public void Update(Privilege privilege)
        {
            context.Entry(privilege).State = EntityState.Modified;
        }
    }
}
