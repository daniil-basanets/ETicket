using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ETicketMobile.ViewModels.Tickets;

namespace ETicketMobile.UnitTests.Comparers
{
    public class AreasViewModelEqualityComparer : EqualityComparer<AreaViewModel>
    {
        public override bool Equals([AllowNull] AreaViewModel x, [AllowNull] AreaViewModel y)
        {
            return x.Id == y.Id
                && x.Name == y.Name
                && x.Description == y.Description;
        }

        public override int GetHashCode([DisallowNull] AreaViewModel areaViewModel)
        {
            return areaViewModel.GetHashCode();
        }
    }
}