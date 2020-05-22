using ETicket.ApplicationServices.DTOs;
using FluentValidation;

namespace ETicket.ApplicationServices.Validation
{
    public class TicketValidator : AbstractValidator<TicketDto>
    {
        public TicketValidator()
        {

            RuleFor(t => t.TicketTypeId)
                .NotEmpty();

            RuleFor(t => t.TransactionHistoryId)
                .NotEmpty();

            RuleFor(t => t.SelectedAreaIds)
                .NotEmpty()
                .WithMessage("{PropertyName} is required");
        }
    }
}