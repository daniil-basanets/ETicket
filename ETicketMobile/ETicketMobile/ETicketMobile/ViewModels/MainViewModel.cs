using Prism.Navigation;

namespace ETicketMobile.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Main Page";
        }
    }
}