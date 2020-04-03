using System;
using System.ComponentModel.DataAnnotations;


namespace ETicket.Domain.Entities
{
	public class Privilege
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[MaxLength(50)]
		public string Name { get; set; }

		[Required]
		public float Coefficient { get; set; }
	}
}
