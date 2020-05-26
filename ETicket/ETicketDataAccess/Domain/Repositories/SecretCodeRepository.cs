using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ETicket.DataAccess.Domain.Repositories
{
    public class SecretCodeRepository : ISecretCodeRepository
    {
        private readonly ETicketDataContext context;

        public SecretCodeRepository(ETicketDataContext context)
        {
            this.context = context;
        }

        public async Task<SecretCode> GetAsync(string code, string email)
        {
            return await context.SecretCodes.FirstOrDefaultAsync(c => c.Code == code && c.Email == email);
        }

        public void RemoveRange(string email)
        {
            context.SecretCodes.RemoveRange(context.SecretCodes.Where(x => x.Email == email));
        }

        public void Add(SecretCode code)
        {
            context.SecretCodes.Add(code);
        }

        public int Count(string email)
        {
            return context.SecretCodes.Count(x => x.Email == email);
        }
    }
}
