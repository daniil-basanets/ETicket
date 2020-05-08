using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using ETicketMobile.Business.Model.Tickets;
using ETicketMobile.Views.Tickets;
using Prism.Navigation;
using Xamarin.Forms;

namespace ETicketMobile.ViewModels.Tickets
{
    public class AreasViewModel : ViewModelBase
    {
        #region Fields

        private readonly INavigationService navigationService;
        private INavigationParameters navigationParameters;

        private ICommand buy;

        private IEnumerable<Area> areas;

        #endregion

        #region Properties

        public ICommand Buy => buy
            ?? (buy = new Command(OnBuy));

        public IEnumerable<Area> Areas
        {
            get => areas;
            set => SetProperty(ref areas, value);
        }

        #endregion

        public AreasViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            this.navigationService = navigationService
                ?? throw new ArgumentNullException(nameof(navigationService));
        }

        public override void OnNavigatedTo(INavigationParameters navigationParameters)
        {
            this.navigationParameters = navigationParameters;

            //TODO get areas from the server

            Areas = new List<Area>
            {
                new Area { Name = "A", Description = "AllCity" },
                new Area { Name = "P", Description = "Pridneprovsk" },
                new Area { Name = "S", Description = "Slobozganske" }
            };
        }

        private void OnBuy(object obj)
        {
            var selectedAreas = Areas.Where(a => a.Selected);

            if (!IsValid(selectedAreas.Count()))
                return;

            navigationParameters.Add("area", selectedAreas);
            navigationService.NavigateAsync(nameof(BuyTicketView), navigationParameters);
        }

        private bool IsValid(int count)
        {
            if (!TickedChoosed(count))
            {
                // TODO Alert Message

                return false;
            }

            return true;
        }

        private bool TickedChoosed(int count)
        {
            return count != 0;
        }
    }
}