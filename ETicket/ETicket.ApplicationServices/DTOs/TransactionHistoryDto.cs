using System;
using System.ComponentModel;

namespace ETicket.ApplicationServices.DTOs
{
    public class TransactionHistoryDto
    {
        public Guid Id { get; set; }

        [DisplayName("Reference number")]
        public string ReferenceNumber { get; set; }

        [DisplayName("Price")]
        public decimal TotalPrice { get; set; }

        public DateTime Date { get; set; }
    }
}