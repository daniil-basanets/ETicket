using ETicket.ApplicationServices.DTOs;
using FluentValidation;

namespace ETicket.ApplicationServices.Validation
{
    public class CarrierValidator : AbstractValidator<CarrierDto>
    {
        public CarrierValidator()
        {
            RuleFor(t => t.Name)
               .NotEmpty();

            RuleFor(t => t.Address)
                .NotEmpty();
        }
    }
}
