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
            
            RuleFor(d => d.ExpirationDate)
                .Must(BeValidDate).WithMessage("Invalid {PropertyName}");
        }
        
        private bool BeValidDate(DateTime? date)
        {
            if (date == null) return true;
    
            var result = DateTime.Compare(date.Value.Date, DateTime.UtcNow.Date);

            return result >= 0;
        }
    }
}