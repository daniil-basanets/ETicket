using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using ETicketMobile.Business.Model.UserAccount;
using Xamarin.Forms;

namespace ETicketMobile.ViewModels.Settings
{
    public class LocalizationItemViewModel
    {
        public string Language { get; set; }

        public bool IsChoosed { get; set; }

        public string Culture { get; set; }

        public ICommand SelectCommand { get; set; }

        public LocalizationItemViewModel(Localization localization)
        {
            Language = localization.Language;
            IsChoosed = localization.IsChoosed;
            Culture = localization.Culture;
        }
    }
}
