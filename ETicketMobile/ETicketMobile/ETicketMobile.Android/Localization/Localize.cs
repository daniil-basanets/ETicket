using System.Globalization;
using ETicketMobile.UserInterface.Localization.Interfaces;
using Java.Util;
using Xamarin.Forms;

[assembly: Dependency(typeof(ETicketMobile.Droid.Localization.Localize))]
namespace ETicketMobile.Droid.Localization
{
    public class Localize : ILocalize
    {
        #region Fields

        private CultureInfo currentCulture;

        #endregion

        #region Properties

        public CultureInfo CurrentCulture
        {
            get
            {
                if (currentCulture == null)
                    currentCulture = GetSystemCultureInfo();

                return currentCulture;
            }
            set
            {
                currentCulture = value;
            }
        }

        #endregion

        private CultureInfo GetSystemCultureInfo()
        {
            var androidLocale = Locale.Default;
            var netLanguage = androidLocale.ToString().Replace("_", "-");

            return new CultureInfo(netLanguage);
        }
    }
}