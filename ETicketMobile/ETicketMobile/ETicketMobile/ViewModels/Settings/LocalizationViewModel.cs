using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Input;
using ETicketMobile.Business.Mapping;
using ETicketMobile.Business.Model.UserAccount;
using ETicketMobile.DataAccess.LocalAPI.Interfaces;
using ETicketMobile.Resources;
using ETicketMobile.UserInterface.Localization.Interfaces;
using Prism.Navigation;
using Xamarin.Forms;

namespace ETicketMobile.ViewModels.Settings
{
    public class LocalizationViewModel : ViewModelBase
    {
        private readonly ILocalApi localApi;
        private readonly ILocalize localize;

        private IEnumerable<Localization> languages;

        private ICommand chooseLanguage;

        public ICommand ChooseLanguage => chooseLanguage
            ?? (chooseLanguage = new Command<Localization>(OnChooseLanguage));

        public IEnumerable<Localization> Languages
        {
            get => languages;
            set => SetProperty(ref languages, value);
        }

        public LocalizationViewModel(INavigationService navigationService, ILocalApi localApi, ILocalize localize) 
            : base(navigationService)
        {
            this.localApi = localApi
                ?? throw new ArgumentNullException(nameof(localApi));

            this.localize = localize
                ?? throw new ArgumentNullException(nameof(localize));
        }

        public override void OnAppearing()
        {
            Languages = new List<Localization>
            {
                new Localization { Language = "Українська", Country = "Ukraine", Culture = "uk-UA" },
                new Localization { Language = "Русский", Country = "Russia", Culture = "ru-RU" },
                new Localization { Language = "English", Country = "USA", Culture = "en-US" },
            };
        }

        private void OnChooseLanguage(Localization localization)
        {
            var currentCulture = new CultureInfo(localization.Culture);

            localize.CurrentCulture = currentCulture;
            AppResource.Culture = currentCulture;

            var localizationEntity = AutoMapperConfiguration.Mapper.Map<Data.Entities.Localization>(localization);

            localApi.AddAsync(localizationEntity);
        }
    }
}