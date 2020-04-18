using System;
using ETicket.DataAccess.Domain.Entities;
using FluentValidation;

namespace ETicket.WebAPI.Validation
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