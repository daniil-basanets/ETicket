using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ETicketMobile.Business.Model.Tickets;

namespace ETicketMobile.UnitTests.Comparers
{
    public class TicketsEqualityComparer : EqualityComparer<Ticket>
    {
        public override bool Equals([AllowNull] Ticket x, [AllowNull] Ticket y)
        {
            return x.TicketType == y.TicketType
                && x.ReferenceNumber == y.ReferenceNumber
                && Enumerable.SequenceEqual(x.TicketAreas, y.TicketAreas)
                && x.CreatedAt == y.CreatedAt
                && x.ActivatedAt == y.ActivatedAt
                && x.ExpiredAt == y.ExpiredAt;
        }

        public override int GetHashCode([DisallowNull] Ticket ticket)
        {
            return ticket.GetHashCode();
        }
    }
}