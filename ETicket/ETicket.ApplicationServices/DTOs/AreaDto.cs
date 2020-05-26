using System.Collections.Generic;
using System.ComponentModel;

namespace ETicket.ApplicationServices.DTOs
{
    public class AreaDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        
        [DisplayName("Stations")]
        public IEnumerable<StationDto> Stations { get; set; }
    }
}