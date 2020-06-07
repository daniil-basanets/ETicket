using System;

namespace ETicketMobile.WebAccess.Network.Endpoints
{
    public static class TransactionsEndpoint
    {
        public static Uri GetTransactionsByEmail = new Uri("/api/TransactionHistory/transactions", UriKind.Relative);
    }
}