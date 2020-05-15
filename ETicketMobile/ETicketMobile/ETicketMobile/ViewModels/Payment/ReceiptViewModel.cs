using System;
using Prism.Navigation;

namespace ETicketMobile.ViewModels.Payment
{
    public class ReceiptViewModel : ViewModelBase
    {
        #region Fields

        private readonly INavigationService navigationService;

        private DateTime boughtAt;
        private string description;
        private string durationHours;
        private string area;
        private decimal price;

        #endregion

        #region Properties

        public DateTime BoughtAt
        {
            get => boughtAt;
            set => SetProperty(ref boughtAt, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public string DurationHours
        {
            get => durationHours;
            set => SetProperty(ref durationHours, value);
        }

        public string Area
        {
            get => area;
            set => SetProperty(ref area, value);
        }

        public decimal Price
        {
            get => price;
            set => SetProperty(ref price, value);
        }

        #endregion

        public ReceiptViewModel(INavigationService navigationService) 
            : base(navigationService)
        {
            this.navigationService = navigationService
                ?? throw new ArgumentNullException(nameof(navigationService));
        }

        public override void OnNavigatedTo(INavigationParameters navigationParameters)
        {
            BoughtAt = navigationParameters.GetValue<DateTime>("boughtAt");
            Description = navigationParameters.GetValue<string>("name");
            DurationHours = navigationParameters.GetValue<string>("durationHours");
            Price = navigationParameters.GetValue<decimal>("totalPrice");
        }
    }
}