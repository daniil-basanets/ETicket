using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ETicket.DataAccess.Domain.Entities;

namespace ETicket.DataAccess.Domain.Interfaces
{
    public interface ISecretCodeRepository
    {
        public Task<SecretCode> GetAsync(string code, string email);
        public void RemoveRange(string email);
        public void Add(SecretCode code);
        public int Count(string email);
    }
}
