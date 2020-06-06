using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ETicketMobile.WebAccess.DTO;

namespace ETicketMobile.UnitTests.Comparers
{
    public class AreasDtoEqualityComparer : EqualityComparer<AreaDto>
    {
        public override bool Equals([AllowNull] AreaDto x, [AllowNull] AreaDto y)
        {
            return x.Id == y.Id
                && x.Name == y.Name
                && x.Description == y.Description;
        }

        public override int GetHashCode([DisallowNull] AreaDto area)
        {
            return area.GetHashCode();
        }
    }
}