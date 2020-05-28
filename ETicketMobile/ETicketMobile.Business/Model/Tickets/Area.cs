using System.Windows.Input;

namespace ETicketMobile.Business.Model.Tickets
{
    public class Area
    {
        public int Id { get; set; }

        public ICommand Selected { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}