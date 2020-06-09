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
                .NotEmpty().WithMessage("{PropertyName} is empty");

            RuleFor(d=>d.Number)
                .NotEmpty().WithMessage("{PropertyName} is empty")
                .Length(1, 50).WithMessage("Length {TotalLength} of {PropertyName} is invalid");

            RuleFor(d => d.IsValid)
                .NotNull().WithMessage("{PropertyName} is empty");
            
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