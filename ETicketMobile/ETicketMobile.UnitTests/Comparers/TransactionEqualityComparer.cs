using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ETicketMobile.Business.Model.Transactions;

namespace ETicketMobile.UnitTests.Comparers
{
    public class TransactionEqualityComparer : EqualityComparer<Transaction>
    {
        public override bool Equals([AllowNull] Transaction x, [AllowNull] Transaction y)
        {
            return x.ReferenceNumber == y.ReferenceNumber
                && x.TotalPrice == y.TotalPrice
                && x.Date == y.Date;
        }

        public override int GetHashCode([DisallowNull] Transaction transaction)
        {
            return transaction.GetHashCode();
        }
    }
}