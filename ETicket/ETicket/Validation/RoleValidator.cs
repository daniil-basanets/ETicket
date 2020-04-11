using DBContextLibrary.Domain.Entities;
using FluentValidation;

namespace ETicket.Validation
{
    public class RoleValidator : AbstractValidator<Role>
    {
        public RoleValidator()
        {
            RuleFor(r => r.Name)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} is empty")
                .Length(2, 50).WithMessage("Lenght {TotalLength} of {PropertyName} is Invalid");
        }
    }
}