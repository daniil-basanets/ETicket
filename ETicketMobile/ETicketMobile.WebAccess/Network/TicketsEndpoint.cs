using System;

namespace ETicketMobile.WebAccess.Network
{
    public static class TicketsEndpoint
    {
        public static Uri GetTicketTypes = new Uri("http://192.168.1.102:50887/api/tickettypes");

        public static Uri GetTicketPrice = new Uri("http://192.168.1.102:50887/api/payments/ticketprice");

        public static Uri BuyTicket = new Uri("http://192.168.1.102:50887/api/payments/buy");
    }
}