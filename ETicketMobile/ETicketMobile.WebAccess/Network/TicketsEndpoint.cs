using System;

namespace ETicketMobile.WebAccess.Network
{
    public static class TicketsEndpoint
    {
        public static Uri Get = new Uri("http://192.168.1.102:50887/api/TicketTypes");
        public static Uri Post = new Uri("http://192.168.1.102:50887/api/Payments/Buy");

        public static Uri Pay = new Uri("https://www.liqpay.ua/ru/checkout/card/i51542363582");

        public static Uri Login = new Uri("http://192.168.1.102:50887/api/Authentication/login");
        public static Uri Registration = new Uri("http://192.168.1.102:50887/api/Authentication/registration");

        public static Uri CheckUserExsists = new Uri("http://192.168.1.102:50887/api/Authentication/checkEmail");

        public static Uri BuyTicket = new Uri("http://192.168.1.102:50887/api/payments/buy");

        public static Uri RefreshToken = new Uri("http://192.168.1.102:50887/api/Authentication/RefreshUserToken");

        public static Uri ConfirmEmail = new Uri("http://192.168.1.102:50887/api/Authentication/confirmEmail");

        public static Uri RequestActivationCode = new Uri("http://192.168.1.102:50887/api/Authentication/sendCode");

        public static Uri ChangePassword = new Uri("http://192.168.1.102:50887/api/Authentication/changePassword");
    }
}