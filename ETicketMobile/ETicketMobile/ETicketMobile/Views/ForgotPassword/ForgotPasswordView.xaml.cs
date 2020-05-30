using System.Linq;
using Prism.Navigation;
using Xamarin.Forms;

namespace ETicketMobile.Views.ForgotPassword
{
    public partial class ForgotPasswordView : ContentPage, INavigationAware
    {
        public ForgotPasswordView()
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