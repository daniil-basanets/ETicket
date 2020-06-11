using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ETicketMobile.ViewModels.Settings;

namespace ETicketMobile.UnitTests.Comparers
{
    public class LocalizationItemViewModelEqualityComparer : EqualityComparer<LocalizationItemViewModel>
    {
        public override bool Equals([AllowNull] LocalizationItemViewModel x, [AllowNull] LocalizationItemViewModel y)
        {
            return x.Culture == y.Culture
                && x.Language == y.Language
                && x.IsChoosed == y.IsChoosed;
        }

        public override int GetHashCode([DisallowNull] LocalizationItemViewModel localization)
        {
            return localization.GetHashCode();
        }
    }
}