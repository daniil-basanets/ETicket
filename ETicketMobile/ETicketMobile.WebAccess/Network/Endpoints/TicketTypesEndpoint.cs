using System;

namespace ETicketMobile.WebAccess.Network.Endpoints
{
    public static class TicketTypesEndpoint
    {
        public static Uri GetTicketTypes = new Uri("/api/tickettypes");

        public static Uri GetTicketPrice = new Uri("/api/payments/ticketprice");
    }
}