using System;

namespace ETicketMobile.Business.Exceptions
{

    public class EmailActivationException : Exception
    {
        public EmailActivationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}