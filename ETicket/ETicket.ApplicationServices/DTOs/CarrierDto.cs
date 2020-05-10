using System.ComponentModel;

namespace ETicket.ApplicationServices.DTOs
{
    public class CarrierDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [DisplayName("Phone number")]
        public string Phone { get; set; }

        public string Address { get; set; }

        [DisplayName("International Bank Account Number (IBAN)")]
        public string IBAN { get; set; }
    }
}
