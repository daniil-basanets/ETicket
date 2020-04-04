using DBContextLibrary.Domain.Entities;
using DBContextLibrary.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBContextLibrary.Domain.Repositories
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

        public void Create(User user)
        {
            context.Users.Add(user);
        }

        public void Update(User user)
        {
            context.Entry(user).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            var user = context.Users.Find(id);

            if (user != null)
            {
                context.Users.Remove(user);
            }
        }
    }
}
