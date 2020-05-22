using System;

namespace ETicketMobile.WebAccess.Network
{
    public static class TransactionsEndpoint
    {
        public static Uri GetTransactionsByEmail = new Uri("/api/TransactionHistory/transactions");
    }
}