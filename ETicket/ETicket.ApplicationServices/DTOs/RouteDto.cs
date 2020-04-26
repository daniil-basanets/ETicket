using System;
using System.Collections.Generic;
using System.Text;

namespace ETicket.ApplicationServices.DTOs
{
    public class RouteDto
    {
        public Guid Id { get; set; }

        public string Number { get; set; }

        public int FirstStationId { get; set; }

        public int LastStationId { get; set; }
    }
}
