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

        public void OnNavigatedFrom(INavigationParameters parameters) { }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            if (!(Navigation.NavigationStack[0] == this))
            {
                Navigation.RemovePage(Navigation.NavigationStack.First());
            }
        }
    }
}