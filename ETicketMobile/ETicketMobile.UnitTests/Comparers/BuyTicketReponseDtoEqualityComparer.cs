using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ETicketMobile.WebAccess.DTO;

namespace ETicketMobile.UnitTests.Comparers
{
    public class BuyTicketReponseDtoEqualityComparer : EqualityComparer<BuyTicketResponseDto>
    {
        public override bool Equals([AllowNull] BuyTicketResponseDto x, [AllowNull] BuyTicketResponseDto y)
        {
            return x.PayResult == y.PayResult
                && x.TotalPrice == y.TotalPrice
                && x.BoughtAt == y.BoughtAt;
        }

        public override int GetHashCode([DisallowNull] BuyTicketResponseDto response)
        {
            return response.GetHashCode();
        }
    }
}