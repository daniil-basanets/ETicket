using ETicket.ApplicationServices.DTOs;
using FluentValidation;

namespace ETicket.ApplicationServices.Validation
{
    public class TicketTypeValidator : AbstractValidator<TicketTypeDto>
    {
        public TicketTypeValidator()
        {
            RuleFor(t => t.TypeName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} is empty")
                .Length(2, 50).WithMessage("Length {TotalLength} of {PropertyName} is invalid")
                .Must(n => n.Trim().Length >= 2);

            RuleFor(t => t.IsPersonal)
                .NotNull().WithMessage("{PropertyName} should not be null");

            RuleFor(t => t.DurationHours)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} is empty")
                .InclusiveBetween((uint)1,uint.MaxValue).WithMessage("Value should be from {From} to {To}");

            RuleFor(t => t.Coefficient)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                .GreaterThanOrEqualTo(decimal.Zero).WithMessage("{PropertyName} should be greater or equal than {ComparisonValue}");
        }
    }
}