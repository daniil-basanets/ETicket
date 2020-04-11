using DBContextLibrary.Domain.Entities;
using FluentValidation;

namespace ETicket.Validation
{
    public class DocumentTypeValidator : AbstractValidator<DocumentType>
    {
        public DocumentTypeValidator()
        {
            RuleFor(d => d.Name)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} is empty")
                .Length(2, 50).WithMessage("Lenght {TotalLength} of {PropertyName} is Invalid");
        }
    }
}