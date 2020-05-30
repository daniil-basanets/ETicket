using System;

namespace ETicketMobile.WebAccess.Network.Endpoints
{
    public static class AuthorizeEndpoint
    {
        public static Uri Login = new Uri("/api/authentication/token");

        public static Uri Registration = new Uri("/api/authentication/registration");

        public static Uri CheckUserExists = new Uri("/api/authentication/check-user");

        public static Uri RefreshToken = new Uri("/api/authentication/refresh-token");

        public static Uri CheckCode = new Uri("/api/authentication/check-code");

        public static Uri RequestActivationCode = new Uri("/api/authentication/send-code");

        public static Uri ResetPassword = new Uri("/api/authentication/reset-password");
    }
}