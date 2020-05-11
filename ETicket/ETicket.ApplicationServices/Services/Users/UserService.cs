using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services.Users.Interfaces;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;
using ETicket.ApplicationServices.Services.DataTable.Interfaces;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace ETicket.ApplicationServices.Services.Users
{
    public class UserService : IUserService, IDataTablePagingService<User>
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

        public IEnumerable<User> GetAll()
        {
            return uow.Users.GetAll().ToList();
        }

        public User GetById(Guid id)
        {
            return uow.Users.Get(id);
        }

        public void SendMessage(Guid id, string message)
        {
            var user = GetById(id);

            mailService.SendEmail(user.Email, message);
        }

        public void Update(UserDto userDto)
        {
            var user = mapper.Map<UserDto, User>(userDto);
            uow.Users.Update(user);
            uow.Save();
        }

        public bool Exists(Guid id)
        {
            return uow.Users.UserExists(id);
        }

        public IDictionary<string, Expression<Func<User, object>>> GetSortExpressions()
        {
            return new Dictionary<string, Expression<Func<User, object>>>
            {
                { "firstName", (t => t.FirstName) },
                { "lastName", (t => t.LastName) },
                { "dateOfBirth", (t => t.DateOfBirth) },
                { "privilege", (t => t.Privilege.Name) },
                { "document", (t => t.Document.Number) }
            };
        }

        public Expression<Func<User, bool>> GetSingleFilterExpression(string columnName, string filterValue)
        {
            return columnName switch
            {
                "firstName" => (t => t.FirstName.StartsWith(filterValue)),
                "lastName" => (t => t.LastName.StartsWith(filterValue)),
                "dateOfBirth" => (t => t.DateOfBirth.ToString().Contains(filterValue)),
                "privilege" => (t => t.Privilege.Name == filterValue),
                "document" => (t => t.Document.Number.StartsWith(filterValue)),
                _ => (t => true)
            };
        }

        public IList<Expression<Func<User, bool>>> GetFilterExpressions(string[] columnNames, string[] filterValues)
        {
            var result = new List<Expression<Func<User, bool>>>();

            for (int i = 0; i < columnNames.Length; i++)
            {
                result.Add(GetSingleFilterExpression(columnNames[i], filterValues[i]));
            }

            return result;
        }

        public IList<Expression<Func<User, bool>>> GetGlobalSearchExpressions(string searchValue)
        {
            return new List<Expression<Func<User, bool>>>
            {
                (t => t.FirstName.StartsWith(searchValue)),
                (t => t.LastName.StartsWith(searchValue)),
                (t => t.DateOfBirth.ToString().Contains(searchValue)),
                (t => t.Privilege.Name.StartsWith(searchValue)),
                (t => t.Document.Number.StartsWith(searchValue))
            };
        }

        IQueryable<User> IDataTablePagingService<User>.GetAll()
        {
            return uow.Users
                    .GetAll()
                    .Include(t => t.Privilege)
                    .Include(t => t.Document);
        }
    }
}
