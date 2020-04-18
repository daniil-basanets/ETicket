using System;
using ETicket.DataAccess.Domain.Entities;
using FluentValidation;

namespace ETicket.WebAPI.Validation
{
    public class DocumentValidator : AbstractValidator<Document>
    {
        public DocumentValidator()
        {
            RuleFor(d => d.DocumentTypeId)
                .NotEmpty();

            RuleFor(d=>d.Number)
                .NotEmpty();

            RuleFor(d => d.IsValid)
                .NotNull();
        }
    }
}