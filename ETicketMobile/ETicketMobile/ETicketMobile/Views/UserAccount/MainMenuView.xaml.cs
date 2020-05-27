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

        public void OnNavigatedFrom(INavigationParameters parameters) { }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            if (!(Navigation.NavigationStack[0] == this))
            {
                Navigation.NavigationStack
                        .Take(Navigation.NavigationStack.Count - 1)
                        .ToList()
                        .ForEach(page => Navigation.RemovePage(page));
            }
        }
    }
}