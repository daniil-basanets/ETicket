using System;
using System.ComponentModel;

namespace ETicket.ApplicationServices.DTOs
{
    public class DocumentDto
    {
        public Guid Id { get; set; }

        [DisplayName("Document Type")]
        public int DocumentTypeId { get; set; }

        public string Number { get; set; }

        [DisplayName("Expiration Date")]
        public DateTime? ExpirationDate { get; set; }

        [DisplayName("Validity")]
        public bool IsValid { get; set; }
    }
}