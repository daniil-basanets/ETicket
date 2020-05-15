using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ETicket.DataAccess.Domain.Entities
{
    public class TicketArea
    {
        public Guid TicketId { get; set; }

        [ForeignKey("TicketId")]
        public Ticket Ticket { get; set; }

        public int AreaId { get; set; }

        [ForeignKey("AreaId")]
        public Area Area { get; set; }
    }
}
