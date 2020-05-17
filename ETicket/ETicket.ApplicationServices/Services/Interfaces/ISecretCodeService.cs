using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ETicket.DataAccess.Domain.Entities;

namespace ETicket.ApplicationServices.Services.Interfaces
{
    public interface ISecretCodeService
    {
        public Task<SecretCode> Get(string code, string email);

        public void RemoveRange(string email);

        public void Add(SecretCode code);

        public int Count(string email);
    }
}
