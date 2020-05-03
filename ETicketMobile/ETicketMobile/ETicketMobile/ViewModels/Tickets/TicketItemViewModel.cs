using System.ComponentModel;
using System.Windows.Input;
using ETicketMobile.WebAccess;
using Xamarin.Forms;

namespace ETicketMobile.ViewModels.Tickets
{
    public class TicketItemViewModel : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public int id;

        public string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public decimal price;
        public decimal Price
        {
            get { return price; }
            set
            {
                price = value;
                OnPropertyChanged(nameof(Price));
            }
        }

        public int amount;
        public int Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                OnPropertyChanged(nameof(Amount));
            }
        }

        public ICommand AddTicket { get; }

        public ICommand RemoveTicket { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public TicketItemViewModel()
        {
            AddTicket = new Command(() => Amount++);
            RemoveTicket = new Command(() => Amount--);
        }

        public TicketItemViewModel(TicketDto dto)
            : this()
        {
            id = dto.Id;
            name = dto.Name;
            price = dto.Price;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
