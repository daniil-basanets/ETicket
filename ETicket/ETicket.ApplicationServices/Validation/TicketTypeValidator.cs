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
                .Length(2, 50).WithMessage("Length {TotalLength} of {PropertyName} is invalid. It should be from 2 to 50 symbols")
                .Must(BeAValidName);

            RuleFor(t => t.IsPersonal)
                .NotNull();

            RuleFor(t => t.DurationHours)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} is empty")
                .InclusiveBetween((uint)1,uint.MaxValue).WithMessage("Value should be from {From} to {To}");

            RuleFor(t => t.Coefficient)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                .GreaterThanOrEqualTo(decimal.Zero).WithMessage("{PropertyName} should be greater than {ComparisonValue}");
        }
        
        private bool BeAValidName(string name)
        {
            name = name.Replace(" ", "");
            return name.Length >= 2;
        }
    }
}