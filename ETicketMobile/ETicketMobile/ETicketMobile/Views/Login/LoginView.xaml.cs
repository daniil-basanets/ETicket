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

        public void OnNavigatedFrom(INavigationParameters parameters) { }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            if (!Navigation.NavigationStack.Any(p => p.GetType() == typeof(Page)))
            {
                Navigation.NavigationStack
                        .Take(Navigation.NavigationStack.Count - 1)
                        .ToList()
                        .ForEach(page => Navigation.RemovePage(page));
            }
        }
    }
}