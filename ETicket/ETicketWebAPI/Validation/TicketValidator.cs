using System;
using ETicketDataAccess.Domain.Entities;
using FluentValidation;

namespace ETicketWebAPI.Validation
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