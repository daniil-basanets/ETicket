using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ETicketMobile.Business.Model.Tickets;

namespace ETicketMobile.UnitTests.Comparers
{
    public class TicketTypesEqualityComparer : EqualityComparer<TicketType>
    {
        public override bool Equals([AllowNull] TicketType x, [AllowNull] TicketType y)
        {
            return x.Id == y.Id
                && x.Name == y.Name
                && x.Coefficient == y.Coefficient
                && x.DurationHours == y.DurationHours;
        }

        public override int GetHashCode([DisallowNull] TicketType ticketType)
        {
            return ticketType.GetHashCode();
        }
    }
}