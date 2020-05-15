using ETicket.ApplicationServices.DTOs;
using FluentValidation;

namespace ETicket.WebAPI.Validation
{
    public class TicketTypeValidator : AbstractValidator<TicketTypeDto>
    {
        public TicketTypeValidator()
        {
            RuleFor(t => t.TypeName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} is empty")
                .Length(2, 50).WithMessage("Length {TotalLength} of {PropertyName} is Invalid");

            RuleFor(t => t.IsPersonal)
                .NotNull();

            RuleFor(t => t.DurationHours)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} is empty")
                .InclusiveBetween((uint)1,uint.MaxValue).WithMessage("Value should be from {From} to {To}");

            RuleFor(t => t.Coefficient)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                .GreaterThan(decimal.Zero).WithMessage("{PropertyName} should be greater than {ComparisonValue}");
        }
    }
}