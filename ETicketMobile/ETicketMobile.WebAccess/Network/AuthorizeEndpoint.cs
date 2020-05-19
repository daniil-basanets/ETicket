using System;

namespace ETicketMobile.WebAccess.Network
{
    public static class AuthorizeEndpoint
    {
        public static Uri Login = new Uri("http://192.168.1.102:50887/api/Authentication/login");
        public static Uri Registration = new Uri("http://192.168.1.102:50887/api/Authentication/registration");

        public static Uri CheckEmail = new Uri("http://192.168.1.102:50887/api/Authentication/checkEmail");

        public static Uri RefreshToken = new Uri("http://192.168.1.102:50887/api/Authentication/RefreshUserToken");

        public static Uri CheckCode = new Uri("http://192.168.1.102:50887/api/Authentication/checkCode");

        public static Uri RequestActivationCode = new Uri("http://192.168.1.102:50887/api/Authentication/sendCode");

        public static Uri ResetPassword = new Uri("http://192.168.1.102:50887/api/Authentication/resetPassword");
    }
}