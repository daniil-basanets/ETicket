using ETicket.ApplicationServices.DTOs;
using FluentValidation;

namespace ETicket.ApplicationServices.Validation
{
    public class PrivilegeValidator : AbstractValidator<PrivilegeDto>
    {
        public PrivilegeValidator()
        {
            RuleFor(p => p.Name)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} is empty")
                .Length(2, 50).WithMessage("Lenght {TotalLength} of {PropertyName} is Invalid");

            RuleFor(p => p.Coefficient)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                .InclusiveBetween(0M, 1M).WithMessage("{PropertyName} should be from {From} to {To}");
        }
    }
}