using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ETicket.DataAccess.Domain.Entities
{
    public class Area
    {
        public int Id { get; set; }

        [Required] 
        [MaxLength(50)] 
        public string Name { get; set; }

        [MaxLength(250)] 
        public string Description { get; set; }
        
        public ICollection<Station> Stations { get; set; }

        public ICollection<TicketArea> TicketArea { get; set; }
    }
}