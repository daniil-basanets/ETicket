using System.Collections.Generic;
using System.Linq;
using ETicket.ApplicationServices.DTOs;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;

namespace ETicket.ApplicationServices.Services.PrivilegeService
{
    class PrivilegeService
    {
        #region private members

        private readonly IUnitOfWork uow;

        private readonly MapperService mapper;

        #endregion

        public PrivilegeService(IUnitOfWork uow)
        {
            this.uow = uow;
            mapper = new MapperService();
        }

        IEnumerable<Privilege>GetAll()
        {
            return uow.Privileges.GetAll().ToList();
        }
        public Privilege Get(int id)
        {
            return uow.Privileges.Get(id);
        }

        public void Create(PrivilegeDto privilegeDto)
        {
            var privilege = mapper.Map<PrivilegeDto, Privilege>(privilegeDto);
            uow.Privileges.Create(privilege);
            uow.Save();
        }

        public void Update(PrivilegeDto privilegeDto)
        {
            var privilege = mapper.Map<PrivilegeDto, Privilege>(privilegeDto);
            uow.Privileges.Update(privilege);
            uow.Save();
        }

        public void Delete(int id)
        {
            uow.Privileges.Delete(id);
            uow.Save();
        }

        public bool Exists(int id)
        {
            return uow.Privileges.Get(id) != null;
        }
    }
}
