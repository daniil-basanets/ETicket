using System.Globalization;

namespace ETicketMobile.UserInterface.Localization.Interfaces
{
    public interface ILocalize
    {
        CultureInfo CurrentCulture { get; set; }
    }
}