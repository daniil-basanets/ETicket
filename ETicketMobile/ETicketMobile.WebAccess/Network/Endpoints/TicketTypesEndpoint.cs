using System;

namespace ETicketMobile.WebAccess.Network.Endpoints
{
    public static class TicketTypesEndpoint
    {
        public static Uri GetTicketTypes = new Uri("/api/ticket-types", UriKind.Relative);
    }
}