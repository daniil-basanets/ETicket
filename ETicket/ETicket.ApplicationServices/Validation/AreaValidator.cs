using ETicket.ApplicationServices.DTOs;
using FluentValidation;

namespace ETicket.ApplicationServices.Validation
{
    public class AreaValidator : AbstractValidator<AreaDto>
    {
        private const int MaxLengthAreaName = 50;
        private const int MaxLengthAreaDescription = 250;
        
        public AreaValidator()
        {
            RuleFor(a => a.Name)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .MaximumLength(MaxLengthAreaName).WithMessage($"Area name is invalid, it should be up to {MaxLengthAreaName} symbols");

            RuleFor(a => a.Description)
                .MaximumLength(MaxLengthAreaDescription).WithMessage($"Description is invalid, it should be up to {MaxLengthAreaDescription} symbols");
        }
    }
}