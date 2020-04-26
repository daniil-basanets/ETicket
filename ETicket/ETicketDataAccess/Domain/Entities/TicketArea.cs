using System;
using System.Collections.Generic;
using System.Text;

namespace ETicket.DataAccess.Domain.Entities
{
    public class TicketArea
    {
        public Guid TicketId { get; set; }

        public Ticket Ticket { get; set; }

        public int AreaId { get; set; }

        public Area Area { get; set; }
    }
}
