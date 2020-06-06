using System;

namespace ETicketMobile.WebAccess.Network.Endpoints
{
    public static class TicketsEndpoint
    {
        public static Uri GetTickets = new Uri("/api/users/tickets", UriKind.Relative);

        public static Uri GetTicketPrice = new Uri("/api/payments/ticketprice", UriKind.Relative);

        public static Uri BuyTicket = new Uri("/api/payments/buy", UriKind.Relative);
    }
}