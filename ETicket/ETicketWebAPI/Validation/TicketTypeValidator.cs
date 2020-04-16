using System;
using ETicket.DataAccess.Domain.Entities;
using FluentValidation;

namespace ETicket.WebAPI.Validation
{
    public class TicketTypeValidator : AbstractValidator<TicketType>
    {
        public TicketTypeValidator()
        {
            RuleFor(t => t.TypeName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} is empty")
                .Length(2, 50).WithMessage("Lenght {TotalLength} of {PropertyName} is Invalid");

            RuleFor(t => t.IsPersonal)
                .NotNull();

            RuleFor(t => t.DurationHours)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} is empty")
                .InclusiveBetween((uint)1,uint.MaxValue).WithMessage("Value should be from {From} to {To}");

            RuleFor(t => t.Price)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                .GreaterThan(decimal.Zero).WithMessage("{PropertyName} should be greater than {ComparisonValue}")
                .Must(BeAValidPrice).WithMessage("{PropertyName} is Invalid");
        }
        
        private bool BeAValidPrice(decimal price)
        {
            var temp = price.ToString().Split('.', ',');
            
            return temp.Length == 1 || temp[1].Length <= 2;
        }
    }
}