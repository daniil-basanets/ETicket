namespace ETicket.ApplicationServices.DTOs
{
    public class TransportDto
    {
        public long Id { get; set; }

        public string Number { get; set; }

        public int? CarrierId { get; set; }

        public int? RouteId { get; set; }
    }
}
