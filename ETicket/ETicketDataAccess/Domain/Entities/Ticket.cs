using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ETicket.DataAccess.Domain.Entities
{
    public class Ticket
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        public int TicketTypeId { get; set; }

        [ForeignKey("TicketTypeId")]
        [DisplayName("Ticket type")]
        public TicketType TicketType { get; set; }

        [Required]
        [DisplayName("Created")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedUTCDate { get; set; }

        [DisplayName("Activated")]
        [DataType(DataType.DateTime)]
        public DateTime? ActivatedUTCDate { get; set; }

        [DisplayName("Expiration")]
        [DataType(DataType.DateTime)]
        public DateTime? ExpirationUTCDate { get; set; }

        public Guid? UserId { get; set; }

        [ForeignKey("UserId")]
        [DisplayName("User")]
        public User User { get; set; }

        [Required]
        public Guid TransactionHistoryId { get; set; }

        [DisplayName("Transaction")]
        [ForeignKey("TransactionHistoryId")]
        public TransactionHistory TransactionHistory { get; set; }

        public ICollection<TicketArea>  TicketArea { get; set; }
    }
}
