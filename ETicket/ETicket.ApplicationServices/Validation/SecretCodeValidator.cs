using System;
using System.Collections.Generic;
using System.Text;
using ETicket.DataAccess.Domain.Entities;
using FluentValidation;

namespace ETicket.ApplicationServices.Validation
{
    public class SecretCodeValidator : AbstractValidator<SecretCode>
    {
        public SecretCodeValidator()
        {
            RuleFor(t => t.Email)
                 .Cascade(CascadeMode.StopOnFirstFailure)
                 .NotEmpty().WithMessage("{PropertyName} is empty");

            RuleFor(t => t.Code)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} is empty");
        }
    }
}