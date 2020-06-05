using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.ApplicationServices.Validation;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;

namespace ETicket.ApplicationServices.Services
{
    public class SecretCodeService : ISecretCodeService
    {
        private readonly IUnitOfWork uow;
        private readonly SecretCodeValidator secretCodeValidator;

        public SecretCodeService(IUnitOfWork uow)
        {
            this.uow = uow;
            secretCodeValidator = new SecretCodeValidator();
        }

        public void Add(SecretCode code)
        {
            if (!secretCodeValidator.Validate(code).IsValid)
            {
                throw new ArgumentException(secretCodeValidator.Validate(code).Errors.First().ErrorMessage);
            }

            uow.SecretCodes.Add(code);
            uow.Save();
        }

        public int Count(string email)
        {
            return uow.SecretCodes.Count(email);
        }

        public async Task<SecretCode> Get(string code, string email)
        {
            return await uow.SecretCodes.GetAsync(code, email);
        }

        public void RemoveRange(string email)
        {
            uow.SecretCodes.RemoveRange(email);
            uow.Save();
        }
    }
}