using System;

namespace ETicketMobile.WebAccess.Network.Endpoints
{
    public static class AuthorizeEndpoint
    {
        public static Uri Login = new Uri("/api/Authentication/login");

        public static Uri Registration = new Uri("/api/Authentication/registration");

        public static Uri CheckEmail = new Uri("/api/Authentication/checkEmail");

        public static Uri RefreshToken = new Uri("/api/Authentication/RefreshUserToken");

        public static Uri CheckCode = new Uri("/api/Authentication/checkCode");

        public static Uri RequestActivationCode = new Uri("/api/Authentication/sendCode");

        public static Uri ResetPassword = new Uri("/api/Authentication/resetPassword");
    }
}