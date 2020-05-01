namespace ETicket.DataAccess.Domain.Entities
{
    public class RouteStation
    {
        public int RouteId { get; set; }

        public Route Route { get; set; }

        public int StationId { get; set; }

        public Station Station { get; set; }
    }
}
