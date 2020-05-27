using System;
using System.ComponentModel;

namespace ETicket.ApplicationServices.DTOs
{
    public class DocumentDto
    {
        public Guid Id { get; set; }

        [DisplayName("Document type")]
        public int DocumentTypeId { get; set; }
        
        [DisplayName("Document type")]
        public string DocumentTypeName { get; set; }

        public string Number { get; set; }

        [DisplayName("Expiration date")]
        public DateTime? ExpirationDate { get; set; }

        [DisplayName("Validity")]
        public bool IsValid { get; set; }
    }
}