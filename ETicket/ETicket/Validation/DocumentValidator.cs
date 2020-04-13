using System;
using DBContextLibrary.Domain.Entities;
using FluentValidation;

namespace ETicket.Validation
{
    public class DocumentValidator : AbstractValidator<Document>
    {
        public DocumentValidator()
        {
            //RuleFor(d => d.DocumentType)
            //    //.Cascade(CascadeMode.StopOnFirstFailure)
            //    .NotEmpty();
            //    //.SetValidator(new DocumentTypeValidator());

            //RuleFor(d => d.Number)
            //    .NotEmpty();

            RuleFor(d => d.ExpirationDate)
                .GreaterThan(DateTime.Now);

            //RuleFor(d => d.IsValid)
            //    .NotNull();

        }
    }
}