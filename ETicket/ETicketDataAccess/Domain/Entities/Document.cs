using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ETicket.DataAccess.Domain.Entities
{
    public class Document
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [DisplayName("Document Type")]
        public int DocumentTypeId { get; set; }

        [ForeignKey("DocumentTypeId")]
        [DisplayName("Document Type")]
        public DocumentType DocumentType { get; set; }

        [Required]
        [MaxLength(50)]
        public string Number { get; set; }

        [DisplayName("Expiration Date")]
        public DateTime? ExpirationDate { get; set; }

        [DisplayName("Validity")]
        [Required]
        public bool IsValid { get; set; }
    }
}
