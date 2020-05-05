using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ETicket.DataAccess.Domain.Entities
{
    public class TransactionHistory
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [DisplayName("Reference Number")]
        public string ReferenceNumber { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Range(0.0, double.MaxValue)]
        [DisplayName("Total Price")]
        public decimal TotalPrice { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}