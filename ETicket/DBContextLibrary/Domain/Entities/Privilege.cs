using System;
using System.ComponentModel.DataAnnotations;


namespace DBContextLibrary.Domain.Entities
{
	public class Privilege
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[MaxLength(50)]
		public string Name { get; set; }

		[Required]
		[Range(0d, 1)]
		public float Coefficient { get; set; }
	}
}
