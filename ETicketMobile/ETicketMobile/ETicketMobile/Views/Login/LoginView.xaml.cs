using Prism.Navigation;
using Xamarin.Forms;

namespace ETicketMobile.Views.Login
{
    public partial class LoginView : ContentPage, INavigationAware
    {
        public LoginView()
        {
            InitializeComponent();
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            //Navigation.RemovePage(this);
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
        }
    }
}