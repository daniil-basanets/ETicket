using System;
using ETicket.ApplicationServices.DTOs;
using FluentValidation;

namespace ETicket.ApplicationServices.Validation
{
    public class DocumentValidator : AbstractValidator<DocumentDto>
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