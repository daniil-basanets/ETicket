using Prism.Navigation;
using Xamarin.Forms;

namespace ETicketMobile.Views.ForgotPassword
{
    public partial class ConfirmForgotPasswordView : ContentPage, INavigationAware
    {
        public ConfirmForgotPasswordView()
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
