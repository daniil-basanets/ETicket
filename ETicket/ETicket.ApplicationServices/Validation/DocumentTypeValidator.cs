using ETicket.ApplicationServices.DTOs;
using FluentValidation;

namespace ETicket.ApplicationServices.Validation
{
    public class DocumentTypeValidator : AbstractValidator<DocumentTypeDto>
    {
        public DocumentTypeValidator()
        {
            RuleFor(d => d.Name)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} is empty")
                .Length(2, 50).WithMessage("Length {TotalLength} of {PropertyName} is invalid")
                .Must(n => n.Trim().Length >= 2); 
        }
    }
}