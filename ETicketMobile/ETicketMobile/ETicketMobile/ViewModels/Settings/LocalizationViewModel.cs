using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
        #region Fields

        private IEnumerable<LocalizationItemViewModel> localizations;

        private readonly ILocalApi localApi;
        private readonly ILocalize localize;

        #endregion

        #region Properties

        public IEnumerable<LocalizationItemViewModel> Localizations
        {
            get => localizations;
            set => SetProperty(ref localizations, value);
        }

        #endregion

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
            Localizations = new List<LocalizationItemViewModel>
            {
                new LocalizationItemViewModel(new Localization { Language = "Українська", Culture = "uk-UA" }),
                new LocalizationItemViewModel(new Localization { Language = "Русский", Culture = "ru-RU" }),
                new LocalizationItemViewModel(new Localization { Language = "English", Culture = "en-US" }),
            };

            var lang = Localizations.Where(c => new CultureInfo(c.Culture).Equals(localize.CurrentCulture)).FirstOrDefault();
            SetLocalization(lang.Language);

            var selectHandler = new Command<string>(
                language =>
                    {
                        SetLocalization(language);
                    });

            Localizations.ToList().ForEach(x => x.SelectCommand = selectHandler);
        }

        private async void OnLanguageSelected(Localization localization)
        {
            var currentCulture = new CultureInfo(localization.Culture);

            localize.CurrentCulture = currentCulture;
            AppResource.Culture = currentCulture;

            var localizationEntity = AutoMapperConfiguration.Mapper.Map<Data.Entities.Localization>(localization);

            await localApi.AddAsync(localizationEntity);
        }

        private void SetLocalization(string language)
        {
            foreach (var localization in Localizations)
            {
                if (localization.Language == language)
                {
                    localization.IsChoosed = true;

                    OnLanguageSelected(GetLocalization(localization));
                }
            }
        }

        private Localization GetLocalization(LocalizationItemViewModel localizationItem)
        {
            return new Localization
            {
                Culture = localizationItem.Culture,
                Language = localizationItem.Language,
                IsChoosed = localizationItem.IsChoosed
            };
        }
    }
}