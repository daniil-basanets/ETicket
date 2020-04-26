using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETicket.DataAccess.Domain.Repositories
{
    public class UserRepository : IRepository<User, Guid>
    {
        private readonly ETicketDataContext context;

        public UserRepository(ETicketDataContext context)
        {
            this.context = context;
        }

        public IQueryable<User> GetAll()
        {
            return context.ETUsers.Include(u => u.Document).Include(u => u.Privilege);
        }

        public User Get(Guid id)
        {
            var user = context.ETUsers
                .Include(u => u.Document)
                .Include(u => u.Privilege)
                .FirstOrDefault(m => m.Id == id);
            return user;
        }

        public void Create(User user)
        {
            context.ETUsers.Add(user);
        }

        public void Update(User user)
        {
            context.Update(user);
        }

        public void Delete(Guid id)
        {
            var user = context.ETUsers.Find(id);

            if (user != null)
            {
                context.ETUsers.Remove(user);
            }
        }

        public bool UserExists(Guid id)
        {
            return context.ETUsers.Any(e => e.Id == id);
        }
    }
}
