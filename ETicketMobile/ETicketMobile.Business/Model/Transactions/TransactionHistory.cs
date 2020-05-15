using System;

namespace ETicketMobile.Business.Model.Transactions
{
    public class Transaction
    {
        public string ReferenceNumber { get; set; }

        public decimal TotalPrice { get; set; }

        public DateTime Date { get; set; }
    }
}