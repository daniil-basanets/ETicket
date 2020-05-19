using System.Linq;
using Prism.Navigation;
using Xamarin.Forms;

namespace ETicketMobile.Views.Payment
{
    public partial class TransactionCompletedView : ContentPage, INavigationAware
    {
        public TransactionCompletedView()
        {
            InitializeComponent();
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            var pages = Navigation.NavigationStack
                .Where(p =>
                    p.Title == "Tickets"
                 || p.Title == "Areas"
                 || p.Title == "Payment")
                .ToList();

            foreach (var page in pages)
            {
                Navigation.RemovePage(page);
            }
        }
    }
}