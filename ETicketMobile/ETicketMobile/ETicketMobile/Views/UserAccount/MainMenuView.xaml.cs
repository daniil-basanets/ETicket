using System.Linq;
using Prism.Navigation;
using Xamarin.Forms;

namespace ETicketMobile.Views.UserActions
{
    public partial class MainMenuView : TabbedPage, INavigatedAware
    {
        public MainMenuView()
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
                    p.Title == "Login" 
                 || p.Title == "Birthday"
                 || p.Title == "Email"
                 || p.Title == "Name"
                 || p.Title == "Password"
                 || p.Title == "Phone"
                 || p.Title == "Confirm"
                 || p.Title == "Confirm Email"
                 || p.Title == "Complete")
                .ToList();

            foreach (var page in pages)
            {
                Navigation.RemovePage(page);
            }
        }
    }
}