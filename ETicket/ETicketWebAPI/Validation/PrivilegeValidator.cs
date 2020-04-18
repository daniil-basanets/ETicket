using ETicket.DataAccess.Domain.Entities;
using FluentValidation;

namespace ETicket.WebAPI.Validation
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
                .NotNull()
                .InclusiveBetween(0M, 1M).WithMessage("{PropertyName} should be from {From} to {To}");
        }
    }
}