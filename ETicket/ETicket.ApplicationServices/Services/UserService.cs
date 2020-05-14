using System;
using System.Collections.Generic;
using System.Linq;
using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;
using ETicket.ApplicationServices.Services.DataTable.Interfaces;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace ETicket.ApplicationServices.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork uow;
        private readonly IMailService mailService;
        private readonly MapperService mapper;

        public UserService(IUnitOfWork uow, IMailService mailService)
        {
            this.uow = uow;
            mapper = new MapperService();
            this.mailService = mailService;
        }

        public void CreateUser(UserDto userDto)
        {
            var user = mapper.Map<UserDto, User>(userDto);
            user.Id = Guid.NewGuid();
            uow.Users.Create(user);
            uow.Save();
        }

        public void CreateUserWithDocument(DocumentDto documentDto, UserDto userDto)
        {
            var document = mapper.Map<DocumentDto, Document>(documentDto);
            document.Id = Guid.NewGuid();
            uow.Documents.Create(document);

            var user = mapper.Map<UserDto, User>(userDto);
            user.DocumentId = document.Id;
            uow.Users.Create(user);
            uow.Save();
        }

        public void Delete(Guid id)
        {
            uow.Users.Delete(id);
            uow.Save();
        }

        public IEnumerable<User> GetUsers()
        {
            return uow.Users.GetAll().ToList();
        }

        public UserDto GetUserById(Guid id)
        {
            var user = uow.Users.Get(id);
            return mapper.Map<User, UserDto>(user);
        }

        public User GetByEmail(string email)
        {
            return uow.Users.GetByEmail(email);
        }

        public void SendMessage(Guid id, string message)
        {
            var user = GetUserById(id);

            mailService.SendEmail(user.Email, message, "Reminders");
        }

        public void Update(UserDto userDto)
        {
            var user = mapper.Map<UserDto, User>(userDto);
            uow.Users.Update(user);
            uow.Save();
        }
    }
}
