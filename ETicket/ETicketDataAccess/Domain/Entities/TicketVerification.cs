using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ETicket.DataAccess.Domain.Entities
{
    public class TicketVerification
    {
        [Key]
        public Guid Id { get; set; }

        public Guid TicketId { get; set; }

        [Required]
        [ForeignKey("TicketId")]
        [DisplayName("Ticket")]
        public Ticket Ticket { get; set; }

        [Required]
        [DisplayName("Verification date")]
        [DataType(DataType.DateTime)]
        public DateTime VerificationUTCDate { get; set; }

        public int StationId { get; set; }

        [Required]
        [ForeignKey("StationId")]
        [DisplayName("Station")]
        public Station Station { get; set; }

        public int TransportId { get; set; }

        [Required]
        [ForeignKey("TransportId")]
        [DisplayName("Transport")]
        public Transport Transport { get; set; }

        [Required]
        [DisplayName("Verified")]
        public bool IsVerified { get; set; }
    }
}
