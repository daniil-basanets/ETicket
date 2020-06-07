using System.Windows.Input;
using ETicketMobile.Business.Model.UserAccount;

namespace ETicketMobile.ViewModels.Settings
{
    public class LocalizationItemViewModel
    {
        #region Properties

        public string Language { get; set; }

        public bool IsChoosed { get; set; }

        public string Culture { get; set; }

        public ICommand SelectCommand { get; set; }

        #endregion

        public LocalizationItemViewModel(Localization localization)
        {
            Language = localization.Language;
            IsChoosed = localization.IsChoosed;
            Culture = localization.Culture;
        }
    }
}