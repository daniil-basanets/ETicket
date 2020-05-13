namespace ETicket.ApplicationServices.DTOs
{
    public class RouteDto
    {
        public int Id { get; set; }

        public string Number { get; set; }

        public int FirstStationId { get; set; }

        public int LastStationId { get; set; }
    }
}
