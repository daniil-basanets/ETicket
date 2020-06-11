using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ETicketMobile.WebAccess.DTO;

namespace ETicketMobile.UnitTests.Comparers
{
    public class GetTicketPriceResponseDtoEqualityComparer : EqualityComparer<GetTicketPriceResponseDto>
    {
        public override bool Equals([AllowNull] GetTicketPriceResponseDto x, [AllowNull] GetTicketPriceResponseDto y)
        {
            return x.TotalPrice == y.TotalPrice;
        }

        public override int GetHashCode([DisallowNull] GetTicketPriceResponseDto response)
        {
            return response.GetHashCode();
        }
    }
}