using System.Collections.Generic;
using ETicket.ApplicationServices.DTOs;
using ETicket.DataAccess.Domain.Entities;

namespace ETicket.ApplicationServices.Services.Interfaces
{
    public interface IPrivilegeService
    {
        public IEnumerable<Privilege> GetAll();
        public Privilege Get(int id);
        public void Create(PrivilegeDto privilegeDto);
        public void Update(PrivilegeDto privilegeDto);
        public void Delete(int id);
        public bool Exists(int id);
    }
}
