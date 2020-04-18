using System;

namespace ETicketAdmin.DTOs
{
    public class TransactionHistoryDto
    {
        public Guid Id { get; set; }

        public string ReferenceNumber { get; set; }

        public decimal TotalPrice { get; set; }

        public DateTime Date { get; set; }

        public int TicketTypeId { get; set; }

        public int Count { get; set; }
    }
}