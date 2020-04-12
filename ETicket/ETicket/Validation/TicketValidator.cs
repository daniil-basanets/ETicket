using System;
using DBContextLibrary.Domain.Entities;
using FluentValidation;

namespace ETicket.Validation
{
    public class TicketValidator : AbstractValidator<Ticket>
    {
        public TicketValidator()
        {
            RuleFor(t => t.TicketTypeId)
                .NotNull();
            
            RuleFor(t => t.TicketType)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .SetValidator(new TicketTypeValidator());

            RuleFor(t => t.TransactionHistoryId)
                .NotNull();

            RuleFor(t => t.TransactionHistory)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .SetValidator(new TransactionHistoryValidator());

            RuleFor(t => t.User)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().When(t => t.UserId != null)
                .SetValidator(new UserValidator());

            RuleFor(t => t.CreatedUTCDate)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .LessThanOrEqualTo(DateTime.UtcNow);

            RuleFor(t => t.ActivatedUTCDate)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .GreaterThan(t=>t.CreatedUTCDate)
                .LessThanOrEqualTo(DateTime.UtcNow);

            RuleFor(t => t.ExpirationUTCDate)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .GreaterThan(t => t.CreatedUTCDate)
                .LessThanOrEqualTo(DateTime.UtcNow);
            
            
        }
    }
}