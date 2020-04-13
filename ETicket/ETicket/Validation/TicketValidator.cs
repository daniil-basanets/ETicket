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
                .NotEmpty();

            RuleFor(t => t.TransactionHistoryId)
                .NotEmpty();
        }
    }
}