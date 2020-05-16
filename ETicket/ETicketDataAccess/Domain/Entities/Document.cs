using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ETicket.DataAccess.Domain.Entities
{
    public class Document
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [DisplayName("Document type")]
        public int DocumentTypeId { get; set; }

        [ForeignKey("DocumentTypeId")]
        [DisplayName("Document type")]
        public DocumentType DocumentType { get; set; }

        [Required]
        [MaxLength(50)]
        public string Number { get; set; }

        [DisplayName("Expiration date")]
        public DateTime? ExpirationDate { get; set; }

        [Required]
        [DisplayName("Validity")]
        public bool IsValid { get; set; }
    }
}
