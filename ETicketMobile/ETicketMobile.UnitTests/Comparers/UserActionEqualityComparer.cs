using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ETicketMobile.Business.Model.UserAccount;

namespace ETicketMobile.UnitTests.Comparers
{
    public class UserActionEqualityComparer : EqualityComparer<UserAction>
    {
        public override bool Equals([AllowNull] UserAction x, [AllowNull] UserAction y)
        {
            return x.Name == y.Name
                && x.View == y.View;
        }

        public override int GetHashCode([DisallowNull] UserAction userAction)
        {
            return userAction.GetHashCode();
        }
    }
}