using System;
using System.ComponentModel;

namespace ETicket.ApplicationServices.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }

        [DisplayName("First name")]
        public string FirstName { get; set; }

        [DisplayName("Last name")]
        public string LastName { get; set; }

        [DisplayName("Phone number")]
        public string Phone { get; set; }

        public string Email { get; set; }

        [DisplayName("Date of birth")]
        public DateTime DateOfBirth { get; set; }

        [DisplayName("Privilege")]
        public int? PrivilegeId { get; set; }

        [DisplayName("Document")]
        public Guid? DocumentId { get; set; }

        [DisplayName("Privilege")]
        public string PrivilegeName { get; set; }

        [DisplayName("Document")]
        public string DocumentNumber { get; set; }
    }
}