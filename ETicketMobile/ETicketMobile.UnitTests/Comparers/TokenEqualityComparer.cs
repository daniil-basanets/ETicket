using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ETicketMobile.Data.Entities;

namespace ETicketMobile.UnitTests.Comparers
{
    public class TokenEqualityComparer : EqualityComparer<Token>
    {
        public override bool Equals([AllowNull] Token x, [AllowNull] Token y)
        {
            return x.AcessJwtToken == y.AcessJwtToken
                && x.RefreshJwtToken == y.RefreshJwtToken;
        }

        public override int GetHashCode([DisallowNull] Token token)
        {
            return token.GetHashCode();
        }
    }
}