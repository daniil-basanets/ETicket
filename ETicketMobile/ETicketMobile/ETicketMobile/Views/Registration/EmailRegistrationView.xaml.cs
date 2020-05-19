using System.Linq;
using Prism.Navigation;
using Xamarin.Forms;

namespace ETicketMobile.Views.Registration
{
    public partial class EmailRegistrationView : ContentPage, INavigationAware
    {
        public EmailRegistrationView()
        {
            InitializeComponent();
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            var page = Navigation.NavigationStack.Where(p => p.Title == "Login").FirstOrDefault();

            if (page != null)
                Navigation.RemovePage(page);
        }
    }
}