using ETicket.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETicket.Domain.Entities
{
    public class UserRepository : IRepository<User>
    {
        private readonly ETicketDataContext context;

        public UserRepository(ETicketDataContext context)
        {
            this.context = context;
        }

        public IEnumerable<User> GetAll()
        {
            return context.Users;
        }

        public User Get(int id)
        {
            return context.Users.Find(id);
        }

        public void Create(User ticketType)
        {
            context.Users.Add(ticketType);
        }

        public void Update(User ticketType)
        {
            context.Entry(ticketType).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            var ticketType = context.Users.Find(id);

            if (ticketType != null)
            {
                context.Users.Remove(ticketType);
            }
        }
    }
}
