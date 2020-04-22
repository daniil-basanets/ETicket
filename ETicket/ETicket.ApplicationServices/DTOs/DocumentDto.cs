using System;

namespace ETicket.ApplicationServices.DTOs
{
    public class DocumentDto
    {
        public Guid Id { get; set; }

        public int DocumentTypeId { get; set; }

        public string Number { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public bool IsValid { get; set; }
    }
}