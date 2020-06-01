using System;

namespace ETicketMobile.WebAccess.Network.Endpoints
{
    public static class AuthorizeEndpoint
    {
        public static Uri Login = new Uri("/api/authentication/token", UriKind.Relative);

        public static Uri Registration = new Uri("/api/authentication/registration", UriKind.Relative);

        public static Uri CheckUserExists = new Uri("/api/authentication/check-user", UriKind.Relative);

        public static Uri RefreshToken = new Uri("/api/authentication/refresh-token", UriKind.Relative);

        public static Uri CheckCode = new Uri("/api/authentication/check-code", UriKind.Relative);

        public static Uri RequestActivationCode = new Uri("/api/authentication/send-code", UriKind.Relative);

        public static Uri ResetPassword = new Uri("/api/authentication/reset-password", UriKind.Relative);
    }
}