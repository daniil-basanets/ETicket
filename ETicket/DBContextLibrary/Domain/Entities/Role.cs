﻿using System.ComponentModel.DataAnnotations;

namespace DBContextLibrary.Domain.Entities
{
    public class Role
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}