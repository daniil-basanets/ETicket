using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETicket.ApplicationServices.DTOs;
using ETicket.DataAccess.Domain.Entities;

namespace ETicket.ApplicationServices.Services.Interfaces
{
    public interface IUserService
    {
        public IEnumerable<UserDto> GetUsers();
        public UserDto GetUserById(Guid id);

        public void CreateUser(UserDto user);

        public void CreateUserWithDocument(DocumentDto document, UserDto user);

        public void SendMessage(Guid id, string message);

        public void Update(UserDto user);

        public User GetByEmail(string email);

        public void Delete(Guid id);
    }
}
