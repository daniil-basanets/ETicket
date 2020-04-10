using System;
using DBContextLibrary.Domain.Entities;
using FluentValidation;

namespace ETicket.Validation
{
    public class TransactionHistoryValidator : AbstractValidator<TransactionHistory>
    {
        public TransactionHistoryValidator()
        {
            RuleFor(t => t.Date)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .LessThanOrEqualTo(DateTime.Now);

            RuleFor(t => t.Count)
                .GreaterThan(0);

            RuleFor(t => t.TicketTypeId)
                .NotEmpty();

            RuleFor(t => t.TicketType)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .SetValidator(new TicketTypeValidator());

            RuleFor(t => t.TotalPrice)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                .GreaterThan(decimal.Zero);
        }
    }
}