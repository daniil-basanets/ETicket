using System.Globalization;

namespace ETicketMobile.Business.Model.UserAccount
{
    public class CultureHelper
    {
        public static CultureInfo GetCulture(Data.Entities.Localization localization)
        {
            return (localization == null) ? CultureInfo.CurrentCulture : new CultureInfo(localization.Culture);
        }
    }
}