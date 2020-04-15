using System;
using System.Linq;
using DBContextLibrary.Domain.Entities;
using FluentValidation;

namespace ETicket.Validation
{
    public class TransactionHistoryValidator : AbstractValidator<TransactionHistory>
    {
        public TransactionHistoryValidator()
        {
            RuleFor(t => t.Count)
                .GreaterThan(0);

            RuleFor(t => t.TicketTypeId)
                .NotEmpty();

            RuleFor(t => t.TotalPrice)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                .GreaterThan(decimal.Zero);

            RuleFor(t => t.ReferenceNumber)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .Length(13)
                .Must(t=>t.All(char.IsNumber));
        }
    }
}