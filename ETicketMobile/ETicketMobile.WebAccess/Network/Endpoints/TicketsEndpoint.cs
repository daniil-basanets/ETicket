using System;

namespace ETicketMobile.WebAccess.Network.Endpoints
{
    public static class TicketsEndpoint
    {
        public static Uri GetTicketTypes = new Uri("/api/tickettypes");

        public static Uri GetTicketPrice = new Uri("/api/payments/ticketprice");

        public static Uri BuyTicket = new Uri("/api/payments/buy");
    }
}