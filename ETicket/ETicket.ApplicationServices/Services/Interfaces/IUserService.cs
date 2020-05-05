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
        public IEnumerable<User> GetUsers();
        public User GetUserById(Guid id);
        public UserDto GetUserDtoById(Guid id);

        public void CreateUser(UserDto user);

        public void CreateUserWithDocument(DocumentDto document, UserDto user);

        public void SendMessage(Guid id, string message);

        public void Update(UserDto user);

        public void Delete(Guid id);

        public bool Exists(Guid id);
    }
}
