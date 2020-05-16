using System;
using System.ComponentModel.DataAnnotations;

namespace ETicket.DataAccess.Domain.Entities
{
    public class TransactionHistory
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string ReferenceNumber { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Range(0.0, double.MaxValue)]
        public decimal TotalPrice { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}