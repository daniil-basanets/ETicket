using System;

namespace ETicketMobile.WebAccess.Network
{
    public static class TicketsEndpoint
    {
        public static Uri Get = new Uri("http://192.168.1.102:50887/api/TicketTypes");
        public static Uri Post = new Uri("http://192.168.1.102:50887/api/Payments/Buy");

        public static Uri GetTicketPrice = new Uri("http://192.168.1.102:50887/api/Payments/GetTicketPrice");

        public static Uri Login = new Uri("http://192.168.1.102:50887/api/Authentication/login");
        public static Uri Registration = new Uri("http://192.168.1.102:50887/api/Authentication/registration");

        public static Uri CheckEmail = new Uri("http://192.168.1.102:50887/api/Authentication/checkEmail");

        public static Uri BuyTicket = new Uri("http://192.168.1.102:50887/api/payments/buy");

        public static Uri RefreshToken = new Uri("http://192.168.1.102:50887/api/Authentication/RefreshUserToken");

        public static Uri ConfirmEmail = new Uri("http://192.168.1.102:50887/api/Authentication/confirmEmail");

        public static Uri RequestActivationCode = new Uri("http://192.168.1.102:50887/api/Authentication/sendCode");

        public static Uri ResetPassword = new Uri("http://192.168.1.102:50887/api/Authentication/resetPassword");
    }
}