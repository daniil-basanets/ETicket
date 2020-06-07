using System;

namespace ETicketMobile.Business.Exceptions
{
    public class WebException : Exception
    {
        public WebException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}