using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;

namespace ETicket.ApplicationServices.Services
{
    public class SecretCodeService : ISecretCodeService
    {
        private readonly IUnitOfWork uow;

        public SecretCodeService(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public void Add(SecretCode code)
        {
            uow.SecretCodes.Add(code);
            uow.Save();
        }

        public int Count(string email)
        {
            return uow.SecretCodes.Count(email);
        }

        public async Task<SecretCode> Get(string code, string email)
        {
            return await uow.SecretCodes.Get(code, email);
        }

        public void RemoveRange(string email)
        {
            uow.SecretCodes.RemoveRange(email);
            uow.Save();
        }
    }
}
