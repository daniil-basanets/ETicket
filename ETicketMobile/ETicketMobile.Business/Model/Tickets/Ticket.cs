namespace ETicketMobile.Business.Model.Tickets
{
    public class Ticket
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int DurationHours { get; set; }

        public int Amount { get; set; }

        public decimal Coefficient { get; set; }
    }
}