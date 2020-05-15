using System;

namespace ETicket.ApplicationServices.DTOs
{
    public class PriceListDto
    {
        public int Id { get; set; }

        public double Price { get; set; }

        public DateTime StartDate { get; set; }

        public int? AreaId { get; set; }
    }
}
