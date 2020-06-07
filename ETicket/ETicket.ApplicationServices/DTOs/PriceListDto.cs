using System;
using System.ComponentModel;

namespace ETicket.ApplicationServices.DTOs
{
    public class PriceListDto
    {
        public int Id { get; set; }

        public double Price { get; set; }

        [DisplayName("Start date")]
        public DateTime StartDate { get; set; }

        [DisplayName("Area")]
        public int AreaId { get; set; }

        [DisplayName("Area")]
        public string AreaName { get; set; }
    }
}
