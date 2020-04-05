using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DBContextLibrary.Domain.Entities
{
    public class Document
    {
        [Key]
        public Guid Id { get; set; }
      
        public int DocumentTypeId { get; set; }

        [ForeignKey("DocumentTypeId")]
        public DocumentType DocumentType { get; set; }

        [Required]
        [MaxLength(50)]
        public string Number { get; set; }

        [Required]
        public DateTime? ExpirationDate { get; set; }

        [Required]
        public bool IsValid { get; set; }
    }
}
