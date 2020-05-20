using System.Linq;
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
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            var pages = Navigation.NavigationStack
                .Where(p =>
                    p.Title == "Email"
                 || p.Title == "Confirm"
                 || p.Title == "Reset password"
                 || p.Title == "Confirm Email"
                 || p.Title == "Main menu")
                .ToList();

            foreach (var page in pages)
            {
                Navigation.RemovePage(page);
            }
        }
    }
}