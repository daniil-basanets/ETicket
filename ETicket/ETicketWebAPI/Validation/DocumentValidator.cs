using System;
using ETicketDataAccess.Domain.Entities;
using FluentValidation;

namespace ETicketWebAPI.Validation
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