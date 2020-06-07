using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ETicket.ApplicationServices.DTOs;

namespace ETicket.ApplicationServicesTests.Comparers
{
    public class TransactionHistoryDtoEqualityComparer : EqualityComparer<TransactionHistoryDto>
    {
        public override bool Equals([AllowNull] TransactionHistoryDto x, [AllowNull] TransactionHistoryDto y)
        {
            return x.Id == y.Id
                && x.ReferenceNumber == y.ReferenceNumber
                && x.TotalPrice == y.TotalPrice
                && x.Date == y.Date;
        }

        public override int GetHashCode([DisallowNull] TransactionHistoryDto obj)
        {
            return obj.GetHashCode();
        }
    }
}