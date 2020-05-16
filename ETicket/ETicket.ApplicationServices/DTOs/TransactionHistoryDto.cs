using System;

namespace ETicket.ApplicationServices.DTOs
{
    public class TransactionHistoryDto
    {
        public Guid Id { get; set; }

        public string ReferenceNumber { get; set; }

        public decimal TotalPrice { get; set; }

        public DateTime Date { get; set; }
    }
}