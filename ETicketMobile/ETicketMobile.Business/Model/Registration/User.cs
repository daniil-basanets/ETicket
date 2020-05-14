using System;

namespace ETicketMobile.Business.Model.Registration
{
    public class User
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}