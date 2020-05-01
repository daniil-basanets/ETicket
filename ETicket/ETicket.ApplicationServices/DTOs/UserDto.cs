using System;

namespace ETicket.ApplicationServices.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public DateTime DateOfBirth { get; set; }

        public int? PrivilegeId { get; set; }

        public Guid? DocumentId { get; set; }
    }
}