using DBContextLibrary.Domain.Entities;
using FluentValidation;

namespace ETicket.Validation
{
    public class PrivilegeValidator : AbstractValidator<Privilege>
    {
        public PrivilegeValidator()
        {
            RuleFor(p => p.Name)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} is empty")
                .Length(2, 50).WithMessage("Lenght {TotalLength} of {PropertyName} is Invalid");

            RuleFor(p => p.Coefficient)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .InclusiveBetween(0.0f, 1.0f).WithMessage("{PropertyName} should be from {From} to {To}");
        }
    }
}